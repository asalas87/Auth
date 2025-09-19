using Notifications.Worker.Domain.Models;

namespace Notifications.Worker.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task<Notification?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Notification>> GetPendingAsync(int maxAttempts = 5, DateTime? before = null);
}
