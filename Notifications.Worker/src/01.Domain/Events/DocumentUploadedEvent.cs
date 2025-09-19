namespace Notifications.Worker.Domain.Events;

public class DocumentUploadedEvent
{
    public Guid DocumentId { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
