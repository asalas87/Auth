using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Notifications.Worker.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;

    public SmtpEmailSender(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        using var smtp = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.User, _settings.Pass),
            EnableSsl = _settings.EnableSsl
        };

        var mail = new MailMessage(_settings.User, to, subject, body);
        await smtp.SendMailAsync(mail);
    }
}
