using Application.Common.Interfaces;
using Application.Interfaces;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Events;
using Domain.Security.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    IEmailService emailService,
    IConfiguration configuration) : INotificationHandler<UserCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUserActivationTokenRepository _userActivationTokenRepository = userActivationTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly INotificationTemplateService _templateService = notificationTemplateService;
    private readonly IEmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        UserActivationToken userActivationToken = UserActivationToken.Create(new UserId(notification.UserId), TimeSpan.FromDays(1));

        var frontendUrl = _configuration["Frontend:BaseUrl"] ?? "https://app.csingenieria.com.ar";
        var activationLink = $"{frontendUrl}/activate?token={userActivationToken.Token}";
        var (subject, body) = _templateService.GenerateUserActivationEmail(notification.Email, activationLink);

        var notif = new Notification(
            recipientEmail: notification.Email,
            documentId: null,
            companyId: notification.CompanyId,
            subject: subject,
            body: body,
            type: NotificationType.UserCreated,
            expirationDate: DateTime.UtcNow.AddDays(1)
        );

        try
        {
            await _emailService.SendAsync(
                notification.Email,
                subject,
                body);

            notif.MarkSent();
        }
        catch (Exception ex)
        {
            notif.MarkFailed();
        }

        await _userActivationTokenRepository.AddAsync(userActivationToken, cancellationToken);
        await _notificationRepository.AddAsync(notif, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
