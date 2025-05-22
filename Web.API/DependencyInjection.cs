using Application.Common.Interfaces;
using Application.Documents.Services;
using Application.Interfaces;
using Application.Security.Services;
using Domain.Documents.Interfaces;
using Infrastructure.Common.Services;
using Infrastructure.Documents.Services;
using Infrastructure.Persistence.Documents.Repositories;
using Infrastructure.Services;
using Web.API.Middlewares;

namespace Web.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5173") // URL del frontend
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Si usas cookies o autenticación
                    });
            });
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7277;  // Asegúrate de usar el puerto correcto
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();


            services.AddAutoMapper(typeof(DependencyInjection));

            return services;
        }
    }
}
