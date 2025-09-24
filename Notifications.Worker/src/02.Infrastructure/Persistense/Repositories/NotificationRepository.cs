using Microsoft.EntityFrameworkCore;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Domain.Models;

namespace Notifications.Worker.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationsDbContext _context;

        public NotificationRepository(NotificationsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IReadOnlyCollection<Notification>> GetPendingAsync(int maxAttempts = 5, DateTime? before = null)
        {
            var query = _context.Notifications
                .AsNoTracking()
                .Where(n => n.Status == Domain.Enums.NotificationStatus.Pending && n.Attempts < maxAttempts);

            if (before.HasValue)
                query = query.Where(n => n.CreatedAt <= before.Value);

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
