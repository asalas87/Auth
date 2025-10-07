using Application.Notifications.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Notifications.Querys;

public record GetPendingNotificationsQuery() : IRequest<ErrorOr<List<NotificationDTO>>>;
