using Application.Interfaces;
using Application.Security.Services;
using Infrastructure.Services;
using Web.API.Middlewares;

namespace Web.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAutoMapper(typeof(DependencyInjection));

            return services;
        }
    }
}
