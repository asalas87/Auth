using System.IdentityModel.Tokens.Jwt;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Extensions;
using Serilog;
using Web.Api.Jobs;
using Web.API;
using Web.API.Extensions;
using Web.API.Middlewares;

try
{
    Log.Information("Starting application");

    var options = new WebApplicationOptions
    {
        WebRootPath = "wwwroot"
    };

    var builder = WebApplication.CreateBuilder(options);

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

    // -------------------------
    // Logging
    // -------------------------
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    // Limpieza de claims
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    // -------------------------
    // Servicios
    // -------------------------
    builder.Services
        .AddPresentation()
        .AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddCorsPolicy(builder.Configuration)
        .AddJwtAuthentication(builder.Configuration)
        .AddDataProtectionKeys(builder.Configuration)
        .AddInvalidModelStateMiddlewares();

    builder.Services.AddMemoryCache();
    builder.Services.AddSecurityServices(builder.Configuration);
    builder.Services.AddHostedService<NotificationsJob>();

    var app = builder.Build();

    // -------------------------
    // Logging HTTP
    // -------------------------
    app.UseSerilogRequestLogging();

    // -------------------------
    // Middleware
    // -------------------------
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var configuration = app.Services.GetRequiredService<IConfiguration>();
    bool applyMigrations = configuration.GetValue<bool>("Database:ApplyMigrations");

    if (applyMigrations)
    {
        app.ApplyMigrations();
    }

    app.EnsureSchemas();
    app.SeedData();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseRouting();

    app.UseCors("AllowReactApp");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapGet("/health", () => Results.Ok("Healthy"));
    app.MapGet("/version", () =>
    {
        var version = typeof(Program)
            .Assembly
            .GetName()
            .Version?
            .ToString();

        return Results.Ok(new
        {
            version,
            environment = app.Environment.EnvironmentName
        });
    });

    Log.Information("Application started successfully");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
