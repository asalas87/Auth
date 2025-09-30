using SharedKernel.Entities;

namespace SharedKernel.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<List<Notification>> GetPendingAsync(int batchSize = 10, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
