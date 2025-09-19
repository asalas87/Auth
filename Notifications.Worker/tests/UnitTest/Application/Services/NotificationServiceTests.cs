using System;
using System.Threading.Tasks;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Domain.Enums;
using Notifications.Worker.UnitTests.Fakes;
using Xunit;

namespace Notifications.Worker.UnitTests.Application.Services
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task CreateAndSendAsync_Should_MarkAsSent_When_EmailSucceeds()
        {
            var repo = new InMemoryNotificationRepository();
            var emailSender = new FakeEmailSender();
            var service = new NotificationService(repo, emailSender);

            await service.CreateAndSendAsync(
                Guid.NewGuid(),
                "test@csingenieria.com.ar",
                "Documento subido",
                "Su documento fue cargado correctamente",
                NotificationType.DocumentUploaded);

            Assert.Single(repo.Items);
            Assert.Equal(NotificationStatus.Sent, repo.Items[0].Status);
        }

        [Fact]
        public async Task CreateAndSendAsync_Should_MarkAsFailed_When_EmailFails()
        {
            var repo = new InMemoryNotificationRepository();
            var emailSender = new FakeEmailSender { ShouldFail = true };
            var service = new NotificationService(repo, emailSender);

            await Assert.ThrowsAsync<Exception>(() =>
                service.CreateAndSendAsync(
                    Guid.NewGuid(),
                    "test@csingenieria.com.ar",
                    "Documento subido",
                    "Su documento fue cargado correctamente",
                    NotificationType.DocumentUploaded));

            Assert.Single(repo.Items);
            Assert.Equal(NotificationStatus.Failed, repo.Items[0].Status);
        }
    }
}
