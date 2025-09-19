using Notifications.Worker.Domain.Events;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Application.Handlers;

public class DocumentExpiringHandler
{
    private readonly NotificationService _notificationService;

    public DocumentExpiringHandler(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(DocumentExpiringEvent message)
    {
        await _notificationService.CreateAndSendAsync(
            message.DocumentId,
            message.Email,
            "Documento por expirar",
            $"El siguiente documento esta por expirar {message.ExpirationDate:d}",
            NotificationType.DocumentExpiring
        );
    }
}
