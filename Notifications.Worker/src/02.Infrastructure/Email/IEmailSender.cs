namespace Notifications.Worker.Infrastructure.Email;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}
