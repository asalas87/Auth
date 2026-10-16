using SharedKernel.Enums;

namespace Application.Notifications.DTOs;

public class NotificationDTO
{
    public Guid Id { get; init; }

    public Guid CompanyId { get; init; }

    public string RecipientName { get; init; } = string.Empty;

    public string RecipientEmail { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;

    public NotificationType Type { get; init; }

    public DateTime? ExpirationDate { get; init; }
}
