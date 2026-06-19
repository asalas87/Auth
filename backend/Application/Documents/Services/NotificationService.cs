using Application.Common.Interfaces;
using Application.Documents.Management.GetAll;
using Application.Interfaces;
using Application.Notifications.Commands;
using Application.Notifications.Commands.MarkNotificationFailed;
using Application.Notifications.Commands.MarkNotificationSent;
using Application.Notifications.DTOs;
using Application.Notifications.Querys;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.Enums;

namespace Application.Documents.Services;

public class NotificationService : INotificationService
{
    private readonly ISender _mediator;
    private readonly IEmailService _emailService;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IConfiguration _configuration;

    public NotificationService(
        IEmailService emailService,
        ITemplateRenderer templateRenderer,
        IConfiguration configuration,
        ISender mediator)
    {
        _emailService = emailService;
        _templateRenderer = templateRenderer;
        _mediator = mediator;
        _configuration = configuration;

    }

    public async Task<int> CreateExpiringDocumentNotificationsAsync(
        CancellationToken cancellationToken = default)
    {
        var frontendUrl = _configuration["Application:FrontendUrl"] ?? "https://app.csingenieria.com.ar";
        const int batchDays = 30;

        int count = 0;

        var expiringDocs = await _mediator.Send(new GetExpiringDocumentsNotSendQuery(batchDays),  cancellationToken);

        var grouped = expiringDocs.Value.GroupBy(d => d.CompanyId);

        foreach (var group in grouped)
        {
            var recipientEmail = string.Join(",",
                group.SelectMany(d => d.AssignedToEmails)
                     .Distinct());

            var recipientName = string.Join(",", group.SelectMany(d => d.AssignedToNames).Distinct());

            foreach (var doc in group)
            {
                await _mediator.Send(
                    new CreateNotificationCommand(
                        DocumentId: doc.DocumentId,
                        CompanyId: doc.CompanyId,
                        RecipientEmail: recipientEmail,
                        RecipientName: recipientName,
                        Subject: "Documentos próximos a vencer",
                        Body: doc.Name,
                        Type: NotificationType.DocumentExpiring,
                        Status: NotificationStatus.Pending,
                        CreatedAt: DateTime.UtcNow,
                        ExpirationDate: doc.ExpirationDate),
                    cancellationToken);

                count++;
            }
        }

        return count;
    }

    public async Task<int> SendPendingNotificationsAsync(
        CancellationToken cancellationToken = default)
    {
        var pending = await _mediator.Send(
            new GetPendingNotificationsQuery(
                new()
                {
                    NotificationType.DocumentExpiring,
                    NotificationType.DocumentUploaded
                }),
            cancellationToken);

        int count = 0;

        var grouped = pending.Value.GroupBy(n => new { n.CompanyId, n.Type });

        foreach (var group in grouped)
        {
            var notifications = group.ToList();

            try
            {
                var ids = notifications
                    .Select(n => n.Id)
                    .ToList();

                var recipientEmail = notifications
                    .First()
                    .RecipientEmail;

                string subject;
                string body;

                switch (group.Key.Type)
                {
                    case NotificationType.DocumentUploaded:

                        subject = "Nuevos documentos disponibles";

                        body = await RenderDocumentUploadedAsync(
                            notifications);

                        break;

                    case NotificationType.DocumentExpiring:

                        subject = "Documentos próximos a vencer";

                        body = await RenderDocumentExpiringAsync(
                            notifications);

                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Tipo de notificación no soportado: {group.Key.Type}");
                }


                await _mediator.Send(new MarkNotificationProcessingCommand(ids), cancellationToken);

                await _emailService.SendAsync(recipientEmail, subject, body);

                await _mediator.Send(new MarkNotificationSentCommand(ids), cancellationToken);

                count += notifications.Count;
            }
            catch (Exception ex)
            {
                await _mediator.Send(
                    new MarkNotificationFailedCommand(
                        notifications.Select(n => n.Id).ToList(),
                        ex.Message),
                    cancellationToken);
            }
        }

        return count;
    }

    private async Task<string> RenderDocumentUploadedAsync(
        List<NotificationDTO> notifications)
    {
        var first = notifications.First();

        var table = CreateTable(notifications);

        return await _templateRenderer.RenderAsync(
            "NewDocument",
            new Dictionary<string, string>
            {
                ["customerName"] = first.RecipientName,
                ["documentsTable"] = table,
                ["title"] = "Nuevos documentos disponibles",
                ["portalLink"] = _configuration["Application:FrontendUrl"] ?? "https://app.csingenieria.com.ar"
            });
    }

    private async Task<string> RenderDocumentExpiringAsync(
        List<NotificationDTO> notifications)
    {
        var first = notifications.First();

        var table = CreateTable(notifications);

        return await _templateRenderer.RenderAsync(
            "ExpirationNotification",
            new Dictionary<string, string>
            {
                ["customerName"] = first.RecipientName,
                ["daysBeforeExpiration"] = "30",
                ["title"] = "Documentos próximos a vencer",
                ["documentsTable"] = table,
                ["portalLink"] = _configuration["Application:FrontendUrl"] ?? "https://app.csingenieria.com.ar"
            });
    }

    protected string CreateTable(List<NotificationDTO> notifications)
    {
        var rows = string.Join("",
            notifications.Select(n =>
                $"""
                <tr>
                    <td>{n.Body}</td>
                    <td>{n.ExpirationDate:dd/MM/yyyy}</td>
                </tr>
                """));
        return $"""
            <table border="1"
                   cellpadding="5"
                   cellspacing="0"
                   width="100%"
                   style="border-collapse: collapse;">
                <tr>
                    <th>Documento</th>
                    <th>Vencimiento</th>
                </tr>
                {rows}
            </table>
            """;
    }
}
