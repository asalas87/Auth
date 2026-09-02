using Application.Common.Interfaces;
using Application.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Enums;
using Domain.Security.Events;
using Domain.Security.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Notifications.Handlers;

public class PasswordResetRequestedEventHandler(
    INotificationRepository notificationRepository,
    IUserActivationTokenRepository userActivationTokenRepository,
    IUnitOfWork unitOfWork,
    ITemplateRenderer templateRenderer,
    IEmailService emailService,
    IConfiguration configuration) : INotificationHandler<PasswordResetRequestedEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUserActivationTokenRepository _userActivationTokenRepository = userActivationTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITemplateRenderer _templateRenderer = templateRenderer;
    private readonly IEmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(PasswordResetRequestedEvent notification, CancellationToken cancellationToken)
    {
        UserActivationToken userActivationToken = UserActivationToken.Create(
            new UserId(notification.UserId),
            TimeSpan.FromDays(1),
            TokenPurpose.PasswordReset);
        await _userActivationTokenRepository.AddAsync(userActivationToken, cancellationToken);

        await _userActivationTokenRepository.InvalidateOtherTokensAsync(new UserId(notification.UserId), userActivationToken.Token, cancellationToken);

        var frontendUrl = _configuration["Application:FrontendUrl"] ?? "https://app.csingenieria.com.ar";
        var resetLink = $"{frontendUrl}/reset-password?token={userActivationToken.Token}";
        var subject = "Reinicio de contraseña";

        var notif = new Notification(
            recipientEmail: notification.Email,
            recipientName: notification.Name,
            documentId: null,
            companyId: notification.CompanyId,
            subject: subject,
            body: string.Empty,
            type: NotificationType.PasswordReset,
            expirationDate: DateTime.UtcNow.AddDays(1)
        );

        try
            {

            var body = await _templateRenderer.RenderAsync("PasswordReset", new Dictionary<string, string> {
                    ["resetLink"] = resetLink,
                    ["userName"] = notification.Name
                });

            notif.UpdateBody(body);
            await _emailService.SendAsync(notification.Email, subject, body);

            notif.MarkSent();
        }
        catch (Exception)
        {
            //_logger.LogError(ex, "Error al enviar email de restablecimiento para {Email}", notification.Email);
            notif.MarkFailed();
        }
        await _notificationRepository.AddAsync(notif, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
