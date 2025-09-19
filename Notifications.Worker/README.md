# 📧 Notifications.Worker

Microservicio en .NET 9 para gestión y envío de notificaciones (por email).  
Soporta arquitectura orientada a eventos con **RabbitMQ** y está preparado para crecer con diferentes estrategias de mensajería.

---

## 🚀 Tecnologías
- [.NET 9](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) (con InMemory y SQL Server)
- [RabbitMQ](https://www.rabbitmq.com/) (mensajería asíncrona)
- [xUnit](https://xunit.net/) (tests unitarios e integración)
- [SMTP](https://learn.microsoft.com/dotnet/api/system.net.mail.smtpclient) para envío de correos

---

## 📂 Estructura
- **Domain** → Entidades y lógica de negocio (`Notification`, enums, interfaces).
- **Infrastructure** → Email (`SmtpEmailSender`), Mensajería (`RabbitMqConsumer`), Persistencia (EF Core).
- **Application** → Servicios de aplicación (ej. `NotificationService`).
- **Worker** → HostedService que escucha eventos y dispara notificaciones.
- **Tests** → Unitarios e integraciones (SMTP, RabbitMQ, EF Core).

---

## ⚙️ Configuración

### `appsettings.json`
Ejemplo mínimo:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "RabbitMQ": {
    "Host": "localhost",
    "User": "guest",
    "Pass": "guest",
    "Exchange": "documents"
  },
  "Smtp": {
    "Host": "smtp.tuservidor.com",
    "Port": 587,
    "User": "notificaciones@csingenieria.com.ar",
    "Pass": "tu-password"
  }
}
