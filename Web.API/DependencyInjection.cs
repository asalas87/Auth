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
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(policy =>
            //                      {
            //                          policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            //                          .AllowAnyHeader()
            //                          .AllowAnyMethod();
            //                      });
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // URL del frontend
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Si usas cookies o autenticación
                    });
            });
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
