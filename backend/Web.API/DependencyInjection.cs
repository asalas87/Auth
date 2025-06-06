using Application.Common.Interfaces;
using Application.Documents.Services;
using Application.Interfaces;
using Application.Security.Services;
using Infrastructure.Common.Services;
using Infrastructure.Documents.Services;
using Infrastructure.Security;
using Infrastructure.Services;
using Web.API.Middlewares;

namespace Web.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7277;
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

            services.AddAutoMapper(typeof(DependencyInjection));

            return services;
        }
    }
}
