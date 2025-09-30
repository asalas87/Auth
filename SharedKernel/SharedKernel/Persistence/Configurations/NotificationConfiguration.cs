using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Entities;

namespace SharedKernel.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications", schema: "NOT");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.DocumentId);

        builder.Property(n => n.Subject)
            .HasMaxLength(500);

        builder.Property(n => n.Body)
            .HasMaxLength(4000);

        builder.Property(n => n.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.Property(n => n.ExpirationDate);

        // ValueObject Email → almacenamos como string
        builder.Property(n => n.RecipientEmail)
            .HasConversion<string>()
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.CreatedAt);
        builder.HasIndex(n => n.ExpirationDate);
    }
}
