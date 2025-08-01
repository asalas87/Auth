using System.IdentityModel.Tokens.Jwt;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Extensions;
using Web.API;
using Web.API.Extensions;
using Web.API.Middlewares;

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

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
    .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// -------------------------
// Middleware
// -------------------------
app.EnsureSchemas();
app.SeedData();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseExceptionHandler("/error"); // manejo global

app.UseHttpsRedirection();         // redirección a HTTPS
app.UseStaticFiles();              // wwwroot

app.UseRouting();                  // importante antes de CORS

app.UseCors("AllowReactApp");      // cors antes de auth

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>(); // si necesita usuario, poner despu�s de auth

app.MapControllers();
app.MapGet("/health", () => Results.Ok("Healthy"));

await app.RunAsync();
