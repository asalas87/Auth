using Application.Data;
using Domain.Primitives;
using Domain.Sales.Customers;
using Domain.Secutiry.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Sales.Repositories;
using Infrastructure.Persistence.Security.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistense(configuration);
            return services;
        }

        private static IServiceCollection AddPersistense(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer"), b => b.MigrationsAssembly("Web.API")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
