using Domain.Security.Entities;
using Domain.ValueObjects;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions;

public static class ApplicationDbSeeder
{
    public static void SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!db.Roles.Any())
        {
            db.Roles.AddRange(Role.Admin);
            db.Roles.AddRange(Role.User);
            db.SaveChanges();
        }

        if (!db.Users.Any())
        {
            var adminRole = db.Roles.First(r => r.Name == "Admin");
            db.Users.Add(new User("admin", new PasswordHasher().HashPassword("admin123!"), Email.Create("admin@csingenieria.com.ar")!, adminRole, true));
            db.SaveChanges();
        }
    }
}
