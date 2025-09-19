using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Worker.Domain.Models;
using Notifications.Worker.Domain.Enums;

namespace Notifications.Worker.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.RecipientEmail)
                   .IsRequired()
                   .HasMaxLength(255);

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
        }
    }
}
