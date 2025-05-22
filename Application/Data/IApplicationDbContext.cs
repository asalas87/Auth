using Domain.Sales.Entities;
using Domain.Secutiry.Entities;
using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<DocumentFile> DocumentFiles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
