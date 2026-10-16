using Domain.Partners.Entities;
using Domain.Security.Entities;
using Domain.ValueObjects;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions;

public static class ApplicationDbSeeder
{
    public static void SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (!db.Roles.Any())
        {
            db.Roles.AddRange(Role.Admin);
            db.Roles.AddRange(Role.User);
            db.SaveChanges();
        }

        if (db.Users.Any())
            return;

        var adminEmail = config["Seed:AdminEmail"];
        var adminPassword = config["Seed:AdminPassword"];
        var companyName = config["Seed:CompanyName"];
        var companyCuit = config["Seed:CompanyCuit"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword) ||
            string.IsNullOrWhiteSpace(companyName) ||
            string.IsNullOrWhiteSpace(companyCuit))
        {
            throw new InvalidOperationException(
                "Seed aborted: seed credentials are not configured. " +
                "Set Seed:AdminEmail, Seed:AdminPassword, Seed:CompanyName, Seed:CompanyCuit " +
                "via configuration or environment variables.");
        }

        var adminRole = db.Roles.First(r => r.Name == "Admin");

        var company = new Company(companyName, Cuit.Create(companyCuit)!);
        db.Companies.Add(company);
        db.SaveChanges();

        var hashedPassword = new PasswordHasher().HashPassword(adminPassword);
        var email = Email.Create(adminEmail)!;

        db.Users.Add(new User("admin", hashedPassword, email, adminRole, company, true));
        db.SaveChanges();
    }
}