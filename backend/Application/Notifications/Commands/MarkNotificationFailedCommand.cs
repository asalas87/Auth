using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationFailed;

public record MarkNotificationFailedCommand(Guid NotificationId, string ErrorMessage) : IRequest<ErrorOr<bool>>;
