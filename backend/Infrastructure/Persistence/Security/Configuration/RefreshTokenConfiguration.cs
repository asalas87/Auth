using Domain.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Security;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", "SEC");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
               .IsRequired();

        builder.Property(rt => rt.ExpiresOn)
               .IsRequired();

        builder.Property(rt => rt.CreatedOn)
               .IsRequired();

        builder.Property(rt => rt.RevokedOn);

        builder.Property(rt => rt.UserId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();
    }
}
