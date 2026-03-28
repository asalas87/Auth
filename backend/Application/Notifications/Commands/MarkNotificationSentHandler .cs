using Application.Notifications.Commands.MarkNotificationFailed;
using Application.Notifications.Commands.MarkNotificationSent;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using SharedKernel.Entities;
using SharedKernel.Interfaces;

public class MarkNotificationSentHandler : IRequestHandler<MarkNotificationSentCommand, ErrorOr<bool>>, IRequestHandler<MarkNotificationFailedCommand, ErrorOr<bool>>, IRequestHandler<MarkNotificationProcessingCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationSentHandler(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
    {
        _unitOfWork = unitOfWork;
        _notificationRepository = notificationRepository;
    }

    public async Task<ErrorOr<bool>> Handle(MarkNotificationSentCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetByListAsync(request.NotificationIds);
        if (notifications.Count == 0)
            return Error.NotFound("Notifications.NotFound", "Notifications not found");

        notifications.ForEach(n => n.MarkSent());

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<ErrorOr<bool>> Handle(MarkNotificationFailedCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetByListAsync(request.NotificationIds);
        if (notifications.Count == 0)
            return Error.NotFound("Notifications.NotFound", "Notifications not found");

        foreach (var notification in notifications)
        {
            if (notification.RetryCount > 3)
                notification.MarkFailed();
            else
            {
                notification.IncrementRetryCount();
                notification.MarkPending();
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<ErrorOr<bool>> Handle(MarkNotificationProcessingCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetByListAsync(request.NotificationIds);
        if (notifications.Count == 0)
            return Error.NotFound("Notifications.NotFound", "Notifications not found");

        notifications.ForEach(n => n.MarkProcessing());

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
