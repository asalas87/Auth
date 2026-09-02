using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Security.Entities;
using Domain.Security.Enums;

namespace Infrastructure.Persistence.Configurations.Security;
public class UserActivationTokenConfiguration : IEntityTypeConfiguration<UserActivationToken>
{
    public void Configure(EntityTypeBuilder<UserActivationToken> builder)
    {
        builder.ToTable("UserActivationToken", "SEC");
        builder.HasKey(c => c.Id);
        builder.Property(r => r.UserId)
                .HasConversion(id => id.Value, value => new UserId(value))
                .IsRequired();

        builder.Property(r => r.Used)
               .IsRequired();

        builder.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(50);

        builder.Property(r => r.ExpiresAt)
                .IsRequired();

        builder.Property(r => r.Purpose)
                .HasConversion<int>()
                .IsRequired();
    }
}

