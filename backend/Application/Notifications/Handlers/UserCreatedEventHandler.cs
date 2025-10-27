using Application.Interfaces;
using Domain.Primitives;
using Domain.Security.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Notifications.Handlers;

public class UserCreatedEventHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork,
    IActivationTokenService activationTokenService,
    INotificationTemplateService notificationTemplateService,
    IConfiguration configuration) : INotificationHandler<UserCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IActivationTokenService _activationTokenService = activationTokenService;
    private readonly INotificationTemplateService _templateService = notificationTemplateService;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var token = _activationTokenService.GenerateActivationToken(notification.UserId, notification.Email);
        var frontendUrl = _configuration["Cors:AllowedOrigins"]!.Last().ToString();
        var activationLink = $"{frontendUrl}/security/auth?token={token}";
        var (subject, body) = _templateService.GenerateUserActivationEmail(notification.Email, activationLink);

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
