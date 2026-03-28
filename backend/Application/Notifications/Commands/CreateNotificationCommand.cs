using ErrorOr;
using MediatR;
using SharedKernel.Enums;

namespace Application.Notifications.Commands;
public record CreateNotificationCommand(
    Guid? DocumentId,
    Guid? CompanyId,
    string RecipientEmail,
    string Subject,
    string Body,
    NotificationType Type,
    NotificationStatus Status,
    DateTime CreatedAt,
    DateTime? ExpirationDate)
    : IRequest<ErrorOr<bool>>;
