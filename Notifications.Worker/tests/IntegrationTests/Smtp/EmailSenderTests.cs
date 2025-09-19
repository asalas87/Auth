using Microsoft.Extensions.Options;
using Notifications.Worker.Infrastructure.Email;

namespace Notifications.Worker.IntegrationTests.Smtp;
public class EmailSenderTests
{
    [Fact(Skip = "Requires real SMTP config in appsettings.Test.json")]
    public async Task Should_Send_Email_Using_Smtp()
    {
        var settings = Options.Create(new SmtpSettings
        {
            Host = "smtp.tuservidor.com",
            Port = 587,
            User = "notificaciones@csingenieria.com.ar",
            Pass = "tu-password",
            EnableSsl = true
        });

        var sender = new SmtpEmailSender(settings);

        await sender.SendAsync(
            "destinatario@ejemplo.com",
            "Prueba integración SMTP",
            "Este es un correo de prueba"
        );
    }
}
