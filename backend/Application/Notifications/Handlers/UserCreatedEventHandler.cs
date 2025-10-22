using Domain.Primitives;
using Domain.Security.Events;
using MediatR;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Notifications.Handlers;

public class UserCreatedEventHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<UserCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Podés personalizar el mail acá:
        var subject = "Activación de usuario";
        var body = $"Hola! Se ha creado tu cuenta. Email: {notification.Email}";

        var notif = new Notification(
            recipientEmail: notification.Email,
            documentId: null,
            subject: subject,
            body: body,
            type: NotificationType.UserCreated
        );

        await _notificationRepository.AddAsync(notif, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
