﻿using Application.Data;
using Application.Interfaces;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using Domain.Sales.Customers;
using Domain.Security.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Documents.Repositories;
using Infrastructure.Persistence.Sales.Repositories;
using Infrastructure.Persistence.Security.Repositories;
using Infrastructure.Security;
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentFileRepository, DocumentFileRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}
