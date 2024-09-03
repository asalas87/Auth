using Domain.Sales.Entities;
using Domain.Secutiry.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
