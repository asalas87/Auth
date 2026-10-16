using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Web.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        public static WebApplication ApplyDatabaseSetup(this WebApplication app)
        {
            var configuration = app.Services.GetRequiredService<IConfiguration>();
            bool applyMigrations = configuration.GetValue<bool>("Database:ApplyMigrations");

            if (applyMigrations)
            {
                app.ApplyMigrations();
            }

            app.EnsureSchemas();
            app.SeedData();

            return app;
        }
    }
}
