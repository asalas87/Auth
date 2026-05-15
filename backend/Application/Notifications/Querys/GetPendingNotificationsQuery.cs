using Application.Notifications.DTOs;
using ErrorOr;
using MediatR;
using SharedKernel.Enums;

namespace Application.Notifications.Querys;

public record GetPendingNotificationsQuery(List<NotificationType>? Types) : IRequest<ErrorOr<List<NotificationDTO>>>;
