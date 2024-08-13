using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddValidatorsFromAssemblyContaining<ApplicationAssembllyReference>();

            return services;
        }
    }
}
