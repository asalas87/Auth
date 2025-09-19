using Microsoft.EntityFrameworkCore;
using Notifications.Worker.Domain.Enums;
using Notifications.Worker.Domain.Models;
using Notifications.Worker.Infrastructure.Persistence;
using Notifications.Worker.Infrastructure.Persistence.Repositories;

namespace Notifications.Worker.IntegrationTests.Persistence
{
    public class NotificationRepositoryTests
    {
        private NotificationsDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<NotificationsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new NotificationsDbContext(options);
        }

        [Fact]
        public async Task Should_Add_And_Retrieve_Notification()
        {
            using var db = CreateInMemoryDb();
            var repo = new NotificationRepository(db);

            var notification = new Notification(
                Guid.NewGuid(),
                "cliente@ejemplo.com",
                "Asunto",
                "Mensaje",
                false,
                NotificationType.DocumentUploaded
            );

            await repo.AddAsync(notification);

            var fetched = await repo.GetByIdAsync(notification.Id);

            Assert.NotNull(fetched);
            Assert.Equal("cliente@ejemplo.com", fetched?.RecipientEmail);
        }
    }
}
