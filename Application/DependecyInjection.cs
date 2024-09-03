using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssembllyReference>();
            });

            services.AddScoped(typeof(IPipelineBehavior<,>) , typeof(ValidationBehavior<,>));
            services.AddAutoMapper(typeof(ApplicationAssembllyReference).Assembly);

            services.AddValidatorsFromAssemblyContaining<ApplicationAssembllyReference>();

            return services;
        }
    }
}
