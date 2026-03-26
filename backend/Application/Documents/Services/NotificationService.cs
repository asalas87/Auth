using Application.Common.Interfaces;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.GetAll;
using Application.Notifications.Commands;
using Application.Notifications.Commands.MarkNotificationFailed;
using Application.Notifications.Commands.MarkNotificationSent;
using Application.Notifications.Querys;
using AutoMapper;
using MediatR;
using SharedKernel.Enums;

namespace Application.Documents.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;
        private readonly IEmailService _emailService;

        public NotificationService(IEmailService emailService, IMapper mapper, ISender mediator)
        {
            _emailService = emailService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> CreateExpiringDocumentNotificationsAsync(CancellationToken cancellationToken = default)
        {
            int batchDays = 30;
            int count = 0;

            var query = new GetExpiringQuery(batchDays);
            var expiringDocs = await _mediator.Send(query);
            var grouped = expiringDocs.Value.GroupBy(d => string.Join(",", d.CompanyId));

            foreach (var group in grouped)
            {
                var emails = group
                    .SelectMany(d => d.AssignedToEmails)
                    .Distinct()
                    .ToList();

                var recipientEmail = string.Join(",", emails);
                var documents = group.ToList();

                // evitar duplicados (podés mejorar esto después)
                var alreadyExists = await _mediator.Send(
                    new IsAlreadySentForCompanyQuery(documents.First().CompanyId),
                    cancellationToken
                );

                if (alreadyExists.Value)
                    continue;

                var body = BuildGroupedEmailBody(documents);

                var notification = new CreateNotificationCommand(
                    DocumentId: documents.First().DocumentId,
                    CompanyId: documents.First().CompanyId,
                    RecipientEmail: recipientEmail,
                    Subject: "Documento/s próximo/s a vencer",
                    Body: body,
                    Type: NotificationType.DocumentExpiring,
                    Status: NotificationStatus.Pending,
                    CreatedAt: DateTime.UtcNow,
                    ExpirationDate: documents.Min(d => d.ExpirationDate)
                );

                await _mediator.Send(notification, cancellationToken);
                count++;
            }

            return count;
        }

        private string BuildGroupedEmailBody(List<ExpiringDocumentDTO> documents)
        {
            var rows = string.Join("", documents.Select(d => $@"
                <tr>
                    <td>{d.Name}</td>
                    <td>{d.ExpirationDate:dd/MM/yyyy}</td>
                </tr>"));

                    return $@"
            <h3>Documentos próximos a vencer</h3>
            <table border='1' cellpadding='5' cellspacing='0'>
                <tr>
                    <th>Documento</th>
                    <th>Vencimiento</th>
                </tr>
                {rows}
            </table>
            ";
        }

        public async Task<int> SendPendingNotificationsAsync(CancellationToken cancellationToken = default)
        {
            var pending = await _mediator.Send(new GetPendingNotificationsQuery(), cancellationToken);

            int count = 0;

            foreach (var notif in pending.Value)
            {
                try
                {
                    await _mediator.Send(new MarkNotificationProcessingCommand(notif.Id), cancellationToken);
                    await _emailService.SendAsync(notif.RecipientEmail, notif.Subject, notif.Body);
                    await _mediator.Send(new MarkNotificationSentCommand(notif.Id), cancellationToken);
                    count++;
                }
                catch
                {
                    await _mediator.Send(new MarkNotificationFailedCommand(notif.Id, "Error al enviar"), cancellationToken);
                }
            }

            return count;
        }
    }
}
