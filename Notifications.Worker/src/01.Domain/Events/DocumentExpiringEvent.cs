namespace Notifications.Worker.Domain.Events;

public class DocumentExpiringEvent
{
    public Guid DocumentId { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
}
