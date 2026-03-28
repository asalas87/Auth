using SharedKernel.Entities;

namespace SharedKernel.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<bool> IsAlreadySentAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task<List<Notification>> GetByListAsync(List<Guid> id, CancellationToken cancellationToken = default);
}
