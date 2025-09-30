using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions;

public static class SchemaExtensions
{
    public static void EnsureSchemas(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var schemas = new[] { "SEC", "DOC", "SAL", "PAR", "NOT" };

        foreach (var schema in schemas)
        {
            db.Database.ExecuteSqlRaw($"IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = '{schema}') EXEC('CREATE SCHEMA [{schema}]');");
        }
    }
}
