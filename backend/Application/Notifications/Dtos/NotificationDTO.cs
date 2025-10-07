namespace Application.Notifications.DTOs;
public class NotificationDTO
{
    public Guid Id { get; init; }
    public string RecipientEmail { get; init; } = null!;
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}
