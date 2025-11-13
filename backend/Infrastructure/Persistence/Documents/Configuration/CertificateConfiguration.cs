using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;
public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates", "DOC");

        builder.Property(c => c.ValidFrom).IsRequired();
        builder.Property(c => c.CertificateNumber).IsRequired();
        builder.Property(c => c.EmployerName).IsRequired();
        builder.Property(c => c.Code).IsRequired();
    }
}
