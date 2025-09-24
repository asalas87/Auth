using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Application.Services;

public interface INotificationService
{
    Task CreateAndSendAsync(Guid? documentId, string email, string subject, string body, NotificationType type);
}

