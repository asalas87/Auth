using System;
using Notifications.Worker.Domain.Enums;
using Notifications.Worker.Domain.Models;
using Xunit;

namespace Notifications.Worker.UnitTests.Domain
{
    public class NotificationTests
    {
        [Fact]
        public void Constructor_Should_InitializePropertiesCorrectly()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var email = "test@csingenieria.com.ar";
            var subject = "Nuevo documento";
            var body = "Su documento fue cargado con éxito.";

            // Act
            var notification = new Notification(documentId, email, subject, body, false, NotificationType.DocumentUploaded);

            // Assert
            Assert.NotEqual(Guid.Empty, notification.Id);
            Assert.Equal(documentId, notification.RelatedDocumentId);
            Assert.Equal(email, notification.RecipientEmail);
            Assert.Equal(subject, notification.Subject);
            Assert.Equal(body, notification.Body);
            Assert.False(notification.Sent);
            Assert.Equal(NotificationType.DocumentUploaded, notification.Type);
            Assert.Equal(NotificationStatus.Pending, notification.Status);
            Assert.Equal(0, notification.Attempts);
            Assert.True(notification.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void MarkSent_Should_UpdateStatusAndProperties()
        {
            // Arrange
            var notification = new Notification(null, "destino@test.com", "Asunto", "Cuerpo", false, NotificationType.DocumentExpiring);

            // Act
            notification.MarkSent("external-123");

            // Assert
            Assert.True(notification.Sent);
            Assert.Equal(NotificationStatus.Sent, notification.Status);
            Assert.NotNull(notification.SentAt);
            Assert.NotNull(notification.LastAttemptAt);
            Assert.Equal("external-123", notification.ExternalId);
            Assert.Equal(1, notification.Attempts);
        }

        [Fact]
        public void MarkFailed_Should_UpdateStatusAndIncrementAttempts()
        {
            // Arrange
            var notification = new Notification(null, "destino@test.com", "Asunto", "Cuerpo", false, NotificationType.DocumentExpiring);

            // Act
            notification.MarkFailed();

            // Assert
            Assert.Equal(NotificationStatus.Failed, notification.Status);
            Assert.NotNull(notification.LastAttemptAt);
            Assert.Equal(1, notification.Attempts);
        }

        [Fact]
        public void MarkProcessing_Should_UpdateStatusWithoutIncrementingAttempts()
        {
            // Arrange
            var notification = new Notification(null, "destino@test.com", "Asunto", "Cuerpo", false, NotificationType.DocumentUploaded);

            // Act
            notification.MarkProcessing();

            // Assert
            Assert.Equal(NotificationStatus.Processing, notification.Status);
            Assert.NotNull(notification.LastAttemptAt);
            Assert.Equal(0, notification.Attempts);
        }

        [Fact]
        public void ResetForRetry_Should_SetStatusBackToPending()
        {
            // Arrange
            var notification = new Notification(null, "destino@test.com", "Asunto", "Cuerpo", false, NotificationType.DocumentUploaded);
            notification.MarkFailed(); // set to Failed first

            // Act
            notification.ResetForRetry();

            // Assert
            Assert.Equal(NotificationStatus.Pending, notification.Status);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenRecipientEmailIsNull()
        {
            // Arrange
            Guid? relatedDocId = Guid.NewGuid();
            string? recipientEmail = null;
            string subject = "Asunto de prueba";
            string body = "Cuerpo de prueba";
            bool sent = false;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new Notification(
                    relatedDocId,
                    recipientEmail!,
                    subject,
                    body,
                    sent,
                    NotificationType.DocumentUploaded
                )
            );
        }
    }
}
