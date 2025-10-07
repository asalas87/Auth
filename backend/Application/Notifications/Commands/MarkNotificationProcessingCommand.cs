using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationSent;

public record MarkNotificationProcessingCommand(Guid NotificationId) : IRequest<ErrorOr<bool>>;
