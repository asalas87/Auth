using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;
public class EmailService(IConfiguration config) : IEmailService
{
    private readonly IConfiguration _config = config;

    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpSection = _config.GetSection("Smtp");

        var host = smtpSection["Host"] ?? throw new InvalidOperationException("SMTP Host not configured");
        var port = int.Parse(smtpSection["Port"] ?? throw new InvalidOperationException("SMTP Port not configured"));
        var user = smtpSection["User"] ?? throw new InvalidOperationException("SMTP User not configured");
        var pass = smtpSection["Pass"] ?? throw new InvalidOperationException("SMTP Pass not configured");
        var from = smtpSection["From"] ?? throw new InvalidOperationException("SMTP From not configured");
        var enableSsl = bool.Parse(smtpSection["EnableSsl"] ?? "true");

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = enableSsl
        };

        var recipientEmail = GetRecipientEmail(to);

        var overrideRecipients = _config.GetValue<bool>("Notifications:OverrideRecipients");

        if (overrideRecipients)
        {
            subject = $"[TEST] {subject} ({to})";
        }

        var mailMessage = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(recipientEmail);
        await client.SendMailAsync(mailMessage);
    }

    private string GetRecipientEmail(string originalRecipient)
    {
        var overrideRecipients =
            _config.GetValue<bool>(
                "Notifications:OverrideRecipients");

        if (!overrideRecipients)
        {
            return originalRecipient;
        }

        var testRecipient = _config["Notifications:TestRecipient"];

        if (string.IsNullOrWhiteSpace(testRecipient))
        {
            throw new InvalidOperationException("Notifications:TestRecipient no está configurado.");
        }

        return testRecipient;
    }
}
