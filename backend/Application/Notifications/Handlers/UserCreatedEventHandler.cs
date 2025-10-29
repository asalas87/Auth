using Application.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Events;
using Domain.Security.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Notifications.Handlers;

public class UserCreatedEventHandler(
    INotificationRepository notificationRepository,
    IUserActivationTokenRepository userActivationTokenRepository,
    IUnitOfWork unitOfWork,
    INotificationTemplateService notificationTemplateService,
    IConfiguration configuration) : INotificationHandler<UserCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUserActivationTokenRepository _userActivationTokenRepository = userActivationTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly INotificationTemplateService _templateService = notificationTemplateService;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        UserActivationToken userActivationToken = UserActivationToken.Create(new UserId(notification.UserId), TimeSpan.FromDays(1));
        var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        var frontendUrl = allowedOrigins?.FirstOrDefault() ?? "https://app.csingenieria.com.ar";
        var activationLink = $"{frontendUrl}/activate?token={userActivationToken.Token}";
        var (subject, body) = _templateService.GenerateUserActivationEmail(notification.Email, activationLink);

        var notif = new Notification(
            recipientEmail: notification.Email,
            documentId: null,
            subject: subject,
            body: body,
            type: NotificationType.UserCreated,
            expirationDate: DateTime.UtcNow.AddDays(1)
        );

        await _userActivationTokenRepository.AddAsync(userActivationToken, cancellationToken);
        await _notificationRepository.AddAsync(notif, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
