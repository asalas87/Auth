using ErrorOr;
using MediatR;

namespace Application.Notifications.Commands.MarkNotificationSent;

public record MarkNotificationProcessingCommand(List<Guid> NotificationIds) : IRequest<ErrorOr<bool>>;
