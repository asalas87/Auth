using Application;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
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
var origins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>();

if (origins == null || origins.Length == 0)
    throw new InvalidOperationException("CORS origins not configured properly.");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("Faltan configuraciones JWT en appsettings o variables de entorno.");
}

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// -------------------------
// Middleware
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations(); // esto est� bien aqu�, se ejecuta solo en desarrollo
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
