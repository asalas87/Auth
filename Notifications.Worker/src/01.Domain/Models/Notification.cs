using System;
using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; private set; }               // PK (o usa un ValueObject NotificationId)
        public Guid? RelatedDocumentId { get; private set; } // FK opcional al documento
        public string RecipientEmail { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public bool Sent { get; private set; } = false;

        public NotificationType Type { get; private set; }
        public NotificationStatus Status { get; private set; }
        public int Attempts { get; private set; }
        public DateTime? LastAttemptAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public DateTime? SentAt { get; private set; }
        public string? ExternalId { get; private set; } // opciona: id del proveedor/servicio

        // ctor para EF
        private Notification() { }

        public Notification(Guid? relatedDocumentId, string recipientEmail, string subject, string body, bool sent, NotificationType type)
        {
            Id = Guid.NewGuid();
            RelatedDocumentId = relatedDocumentId;
            RecipientEmail = recipientEmail ?? throw new ArgumentNullException(nameof(recipientEmail));
            Subject = subject ?? string.Empty;
            Body = body ?? string.Empty;
            Sent = sent;
            Type = type;
            Status = NotificationStatus.Pending;
            Attempts = 0;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkSent(string? externalId = null)
        {
            Status = NotificationStatus.Sent;
            Sent = true;
            SentAt = DateTime.UtcNow;
            LastAttemptAt = DateTime.UtcNow;
            ExternalId = externalId;
            Attempts++;
        }

        public void MarkFailed()
        {
            Status = NotificationStatus.Failed;
            LastAttemptAt = DateTime.UtcNow;
            Attempts++;
        }

        public void MarkProcessing()
        {
            Status = NotificationStatus.Processing;
            LastAttemptAt = DateTime.UtcNow;
        }

        public void ResetForRetry()
        {
            Status = NotificationStatus.Pending;
        }
    }
}
