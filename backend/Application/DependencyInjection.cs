using Application.Common.Behaviors;
using Application.Controls.Services;
using Application.Documents.Services;
using Application.Security.Services;
using FluentValidation;
using Infrastructure.Documents.Services;
using Infrastructure.Security;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssembllyReference>();
            });

            services.AddScoped(typeof(IPipelineBehavior<,>) , typeof(ValidationBehavior<,>));
            services.AddAutoMapper(typeof(ApplicationAssembllyReference).Assembly);

            services.AddScoped<IControlService, ControlService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddValidatorsFromAssemblyContaining<ApplicationAssembllyReference>();

            return services;
        }
    }
}
