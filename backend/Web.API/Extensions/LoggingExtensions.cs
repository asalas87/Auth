using Serilog;
using Serilog.Debugging;

namespace Web.API.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
        Directory.CreateDirectory(logDirectory);

        SelfLog.Enable(msg => Console.Error.WriteLine($"[SERILOG] {msg}"));

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(logDirectory, "bootstrap-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)
            .CreateBootstrapLogger();

        Log.Information("Starting application");

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    Path.Combine(logDirectory, "log-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true);
        });

        return builder;
    }
}
