using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Infrastructure.Persistence.Notifications.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _db;

    public NotificationRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _db.Notifications.AddAsync(notification, cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetPendingAsync(int batchSize = 50, CancellationToken cancellationToken = default)
    {
        return await _db.Notifications
            .Where(n => n.Status == NotificationStatus.Pending)
            .OrderBy(n => n.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _db.Notifications.Update(notification);
        return Task.CompletedTask;
    }

    Task<List<Notification>> INotificationRepository.GetPendingAsync(int batchSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
