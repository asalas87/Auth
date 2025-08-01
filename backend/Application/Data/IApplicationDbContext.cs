using Domain.Sales.Entities;
using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Security.Entities;
using Domain.Partners.Entities;

namespace Application.Data;
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<User> Users { get; }
    DbSet<DocumentFile> DocumentFiles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Role> Roles { get; }
    DbSet<Company> Companies { get; }
    DbSet<Certificate> Certificates { get; }
    DbSet<GeneralDocument> GeneralDocuments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
