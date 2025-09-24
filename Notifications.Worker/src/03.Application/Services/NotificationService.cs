using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Domain.Models;
using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly IEmailSender _emailSender;

    public NotificationService(INotificationRepository repo, IEmailSender emailSender)
    {
        _repo = repo;
        _emailSender = emailSender;
    }

    public async Task CreateAndSendAsync(Guid? documentId, string email, string subject, string body, NotificationType type)
    {
        var notification = new Notification(documentId, email, subject, body, false, type);
        await _repo.AddAsync(notification);

        // try to send immediately (worker can do this too)
        try
        {
            notification.MarkProcessing();
            await _repo.UpdateAsync(notification);

            await _emailSender.SendAsync(email, subject, body);

            notification.MarkSent();
            await _repo.UpdateAsync(notification);
        }
        catch
        {
            notification.MarkFailed();
            await _repo.UpdateAsync(notification);
            throw;
        }
    }

    // Worker polling: GetPendingAsync -> try send -> update status / retries
}
