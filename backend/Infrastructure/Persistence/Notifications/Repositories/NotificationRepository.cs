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

    public async Task<bool> IsAlreadySentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var documentSent = await _db.Notifications
            .Where(n => n.DocumentId == id && n.Type != NotificationType.DocumentUploaded)
            .SingleOrDefaultAsync();
        return documentSent != null;
    }

    public async Task<List<Notification>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Notifications
            .Where(n => n.Status == NotificationStatus.Pending)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _db.Notifications.Update(notification);
        return Task.CompletedTask;
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }
}
