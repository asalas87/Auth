using Application.Notifications.DTOs;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using SharedKernel.Interfaces;

namespace Application.Notifications.Querys;
public class GetPendingNotificationsQueryHandler : IRequestHandler<GetPendingNotificationsQuery, ErrorOr<List<NotificationDTO>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetPendingNotificationsQueryHandler(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<ErrorOr<List<NotificationDTO>>> Handle(GetPendingNotificationsQuery request, CancellationToken cancellationToken)
    {
        var pending = await _notificationRepository.GetPendingAsync(cancellationToken);
        return pending.Select(n => new NotificationDTO
        {
            Id = n.Id,
            RecipientEmail = n.RecipientEmail,
            Subject = n.Subject,
            Body = n.Body
        }).ToList();
    }
}

