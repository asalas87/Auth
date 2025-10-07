using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationSent;

public record MarkNotificationSentCommand(Guid NotificationId) : IRequest<ErrorOr<bool>>;
