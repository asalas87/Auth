using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications.Worker;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Domain.Interfaces;
using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Infrastructure.Messaging;
using Notifications.Worker.Infrastructure.Persistence;
using Notifications.Worker.Infrastructure.Persistence.Repositories;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // 1. ConnectionString desde appsettings.json
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        // 2. EF Core DbContext
        services.AddDbContext<NotificationsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.Configure<SmtpSettings>(
            context.Configuration.GetSection("Smtp"));

        // 4. Email sender
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        // 5. Notification service
        services.AddScoped<INotificationService, NotificationService>();
        // 6. Worker (background service)
        //builder.Services.AddHostedService<Worker>(); // RabbitMQ
        services.AddHostedService<SqlNotificationWorker>(); // SQL
    })
    .Build()
    .Run();
