using Serilog;
using Web.API.Middlewares;

namespace Web.API.Extensions;

public static class PipelineExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseExceptionHandler();
        app.UseRouting();
        app.UseCors("AllowReactApp");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapGet("/health", () => Results.Ok("Healthy"));
        app.MapGet("/version", () =>
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString();
            return Results.Ok(new { version, environment = app.Environment.EnvironmentName });
        });

        Log.Information("Application started successfully");

        return app;
    }
}
