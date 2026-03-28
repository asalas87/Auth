using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationSent;

public record MarkNotificationSentCommand(List<Guid> NotificationIds) : IRequest<ErrorOr<bool>>;
