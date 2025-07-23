using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Security.Entities;

namespace Infrastructure.Persistence.Configurations.Security
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "SEC");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .ValueGeneratedNever();

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
