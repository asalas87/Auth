using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Worker;
using Notifications.Worker.Application.Services;
using Notifications.Worker.Infrastructure.Email;
using Notifications.Worker.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

// ✅ Connection string desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Registrar DbContext
builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Configurar SMTP
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));

// ✅ Registrar servicios
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// ✅ Registrar el worker de RabbitMQ
builder.Services.AddHostedService<RabbitNotificationWorker>();

// ✅ Ejecutar
var host = builder.Build();
host.Run();
