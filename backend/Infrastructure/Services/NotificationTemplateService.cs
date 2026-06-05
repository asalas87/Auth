using Application.Interfaces;

namespace Infrastructure.Services;

public class NotificationTemplateService : INotificationTemplateService
{
    private readonly string _templatesPath;

    public NotificationTemplateService()
    {
        _templatesPath = Path.Combine(AppContext.BaseDirectory, "Notifications", "Templates");
    }

    [Obsolete]
    public (string Subject, string Body) GenerateUserActivationEmail(string userEmail, string activationLink)
    {
        var subject = "Activación de tu cuenta";

        var templatePath = Path.Combine(_templatesPath, "UserActivation.html");
        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"No se encontró la plantilla: {templatePath}");

        var body = File.ReadAllText(templatePath)
            .Replace("{{activationLink}}", activationLink);

        return (subject, body);
    }
}
