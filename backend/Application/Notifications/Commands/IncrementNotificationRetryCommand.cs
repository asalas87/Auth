using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.IncrementNotificationRetry;

public record IncrementNotificationRetryCommand(Guid NotificationId) : IRequest<ErrorOr<bool>>;
