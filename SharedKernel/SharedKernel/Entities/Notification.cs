using SharedKernel.Enums;
using SharedKernel.ValueObjects;

namespace SharedKernel.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid? DocumentId { get; private set; }
    public string RecipientEmail { get; private set; } = null!;
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public int RetryCount { get; private set; } = 0;

    // Constructor privado para EF
    private Notification() { }

    public Notification(
        string recipientEmail,
        Guid? documentId,
        string subject,
        string body,
        NotificationType type,
        DateTime? expirationDate = null)
    {
        DocumentId = documentId;
        RecipientEmail = recipientEmail;
        Subject = subject;
        Body = body;
        Type = type;
        Status = NotificationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        ExpirationDate = expirationDate;
    }

    // Métodos de cambio de estado
    public void MarkProcessing() => Status = NotificationStatus.Processing;
    public void MarkSent() =>  Status = NotificationStatus.Sent;
    public void MarkFailed() => Status = NotificationStatus.Failed;
    public void MarkPending() => Status = NotificationStatus.Pending;
    public void IncrementRetryCount() => RetryCount++;
}
