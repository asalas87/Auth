namespace Application.Interfaces;

public interface INotificationTemplateService
{
    (string Subject, string Body) GenerateUserActivationEmail(string userEmail, string activationLink);
}
