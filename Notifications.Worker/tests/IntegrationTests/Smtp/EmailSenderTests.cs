using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Worker.Application;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Infrastructure;
using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Infrastructure.Persistence;

namespace Notifications.Worker.IntegrationTests.Smtp;

public class EmailSenderTests
{
    private readonly ServiceProvider _provider;

    public EmailSenderTests()
    {
        // 1. Ruta del Worker
        var projectDir = Path.GetFullPath("../../../../../src/04.Worker");

        // 2. Configuración desde appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(projectDir)
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
            .Build();

        // 3. Construir ServiceCollection
        var services = new ServiceCollection();

        // 3a. Registrar IConfiguration para usar en otros servicios
        services.AddSingleton<IConfiguration>(config);

        // 3b. DbContext en memoria (si NotificationService depende de EF)
        services.AddDbContext<NotificationsDbContext>(options =>
            options.UseInMemoryDatabase("NotificationsTestDb"));

        // 3c. Vincular SmtpSettings usando IOptions
        services.Configure<SmtpSettings>(config.GetSection("Smtp"));

        // 3d. Registrar Application & Infrastructure
        services.AddApplication(config);      // INotificationService
        services.AddInfrastructure(config);   // IEmailSender + INotificationRepository

        // 4. Construir ServiceProvider
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Should_Send_Email_Using_Smtp()
    {
        // Resolver el servicio usando la interfaz, no la implementación concreta
        var service = _provider.GetRequiredService<INotificationService>();

        await service.CreateAndSendAsync(
            null,
            "asalas87.ar@gmail.com",
            "📬 Prueba SMTP desde Notifications.Worker",
            "Este es un correo de prueba de integración.",
            Domain.Enums.NotificationType.DocumentUploaded
        );

        Assert.True(true);
    }
}
