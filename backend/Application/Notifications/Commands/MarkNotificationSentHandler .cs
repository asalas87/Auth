using Application.Notifications.Commands.MarkNotificationFailed;
using Application.Notifications.Commands.MarkNotificationSent;
using Domain.Primitives;
using ErrorOr;
using MediatR;
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
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification is null)
            return Error.NotFound("Notification.NotFound", "Notification not found");

        notification.MarkSent();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<ErrorOr<bool>> Handle(MarkNotificationFailedCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification is null)
            return Error.NotFound("Notification.NotFound", "Notification not found");

        if (notification.RetryCount > 3)
            notification.MarkFailed();
        else
        {
            notification.IncrementRetryCount();
            notification.MarkPending();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
    public async Task<ErrorOr<bool>> Handle(MarkNotificationProcessingCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification is null)
            return Error.NotFound("Notification.NotFound", "Notification not found");

        notification.MarkProcessing();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
