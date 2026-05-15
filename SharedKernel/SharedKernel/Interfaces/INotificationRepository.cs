using SharedKernel.Entities;
using SharedKernel.Enums;

namespace SharedKernel.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<bool> IsAlreadySentAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetPendingAsync(List<NotificationType>? types = null, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetByListAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
