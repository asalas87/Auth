using Domain.Partners.Entities;
using Domain.Security.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Security.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "SEC");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(id => id.Value, value => new UserId(value))
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Email).HasConversion(email => email.Value, value => Email.Create(value)!).HasMaxLength(255);
            builder.Property(c => c.Password).HasMaxLength(60);
            builder.HasOne(u => u.Role)
                   .WithMany()
                   .HasForeignKey("RoleId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Active).HasDefaultValue(true);
        }
    }
}
