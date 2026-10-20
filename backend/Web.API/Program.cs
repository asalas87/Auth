using System.IdentityModel.Tokens.Jwt;
using Application;
using Infrastructure;
using Serilog;
using Web.Api.Jobs;
using Web.API;
using Web.API.Extensions;

try
{
    var options = new WebApplicationOptions { WebRootPath = "wwwroot" };
    var builder = WebApplication.CreateBuilder(options);

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

    builder.AddSerilogLogging();

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    builder.Services
        .AddPresentation()
        .AddInfrastructure(builder.Configuration)
        .AddSecurityServices(builder.Configuration)
        .AddApplication(builder.Configuration)
        .AddCorsPolicy(builder.Configuration)
        .AddJwtAuthentication(builder.Configuration)
        .AddDataProtectionKeys(builder.Configuration)
        .AddInvalidModelStateMiddlewares()
        .AddHostedService<NotificationsJob>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.ConfigurePipeline();
    app.ApplyDatabaseSetup();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
