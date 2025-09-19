using Notifications.Worker.Domain.Events;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Application.Handlers;

public class DocumentUploadedHandler
{
    private readonly NotificationService _notificationService;

    public DocumentUploadedHandler(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(DocumentUploadedEvent message)
    {
        await _notificationService.CreateAndSendAsync(
            message.DocumentId,
            message.Email,
            "Nuevo documento disponible",
            $"Se ha subido un nuevo documento el {message.UploadedAt:d}",
            NotificationType.DocumentUploaded
        );
    }
}
