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
    .AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
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
    app.ApplyMigrations(); // esto está bien aquí, se ejecuta solo en desarrollo
}

app.UseExceptionHandler("/error"); // manejo global

app.UseHttpsRedirection();         // redirección a HTTPS
app.UseStaticFiles();              // wwwroot

app.UseRouting();                  // importante antes de CORS

app.UseCors("AllowReactApp");      // cors antes de auth

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>(); // si necesita usuario, poner después de auth

app.MapControllers();
app.MapGet("/health", () => Results.Ok("Healthy"));

await app.RunAsync();
