using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;
public class RenovationConfiguration : IEntityTypeConfiguration<Renovation>
{
    public void Configure(EntityTypeBuilder<Renovation> builder)
    {
        builder.ToTable("Renovations", "DOC");

        builder.Property(c => c.ValidFrom).IsRequired();
        builder.Property(c => c.CertificateNumber).IsRequired().HasMaxLength(200);
        builder.Property(c => c.EmployerName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(200);
    }
}
