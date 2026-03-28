using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationFailed;

public record MarkNotificationFailedCommand(List<Guid> NotificationIds, string ErrorMessage) : IRequest<ErrorOr<bool>>;
