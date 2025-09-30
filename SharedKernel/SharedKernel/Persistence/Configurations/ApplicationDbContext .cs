using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using SharedKernel.Persistence.Configurations;

namespace Web.API.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        }
    }
}
