using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Notifications.Worker.Infrastructure.Persistence;
using System.IO;

namespace Notifications.Worker.Infrastructure.Persistense
{
    public class NotificationsDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
    {
        public NotificationsDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NotificationsDbContext(optionsBuilder.Options);
        }
    }
}
