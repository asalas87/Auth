using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Domain.Models;

namespace Notifications.Worker.UnitTests.Fakes
{
    public class InMemoryNotificationRepository : INotificationRepository
    {
        public List<Notification> Items { get; } = new();

        public Task AddAsync(Notification notification)
        {
            Items.Add(notification);
            return Task.CompletedTask;
        }

        public Task<Notification?> GetByIdAsync(Guid id)
        {
            var notification = Items.FirstOrDefault(n => n.Id == id);
            return Task.FromResult(notification);
        }

        public Task<IReadOnlyCollection<Notification>> GetPendingAsync(int maxAttempts = 5, DateTime? before = null)
        {
            var pending = Items
                .Where(n =>
                    !n.Sent &&
                    n.Attempts < maxAttempts &&
                    (before == null || n.CreatedAt <= before))
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyCollection<Notification>)pending);
        }

        public Task UpdateAsync(Notification notification)
        {
            // Como es in-memory, el objeto ya está en la lista.
            // Si querés podés forzar reemplazo por Id:
            var index = Items.FindIndex(n => n.Id == notification.Id);
            if (index >= 0)
                Items[index] = notification;

            return Task.CompletedTask;
        }

        // Métodos extra que tu interfaz pida
        public Task DeleteAsync(Guid id)
        {
            Items.RemoveAll(n => n.Id == id);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Notification>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyCollection<Notification>)Items.AsReadOnly());
        }
    }
}
