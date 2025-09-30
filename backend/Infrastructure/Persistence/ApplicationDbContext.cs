using Application.Data;
using Domain.Documents.Entities;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Sales.Entities;
using Domain.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<DocumentFile> DocumentFiles => Set<DocumentFile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<GeneralDocument> GeneralDocuments => Set<GeneralDocument>();
    public DbSet<Notification> Notifications => Set<Notification>();

    public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        _publisher = null!;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SharedKernel.Persistence.Configurations.NotificationConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .SelectMany(e => e.GetDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);
        if (_publisher is not null)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
        return result;
    }
}
