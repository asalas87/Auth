using Application.Common.Interfaces;
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
            int batchDays = 10;
            int count = 0;

            var query = new GetExpiringQuery(batchDays);
            var expiringDocs = await _mediator.Send(query);

            foreach (var doc in expiringDocs.Value)
            {
                var alreadyExists = await _mediator.Send(new IsAlreadySentQuery(doc.DocumentId), cancellationToken);

                if (alreadyExists.Value)
                    continue;

                var notification = new CreateNotificationCommand(
                    DocumentId: doc.DocumentId,
                    RecipientEmail: string.Join(",", doc.AssignedToEmails),
                    Subject: "Documento próximo a vencer",
                    Body: $"El documento {doc.Name} vence el {doc.ExpirationDate:dd/MM/yyyy}",
                    Type: NotificationType.DocumentExpiring,
                    Status: NotificationStatus.Pending,
                    CreatedAt: DateTime.UtcNow,
                    ExpirationDate: doc.ExpirationDate
                );

                await _mediator.Send(notification, cancellationToken);
                count++;
            }

            return count;
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
