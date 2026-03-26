using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;
public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates", "DOC");

        builder.Property(c => c.Id)
            .HasConversion(companyId => companyId.Value, value => new DocumentFileId(value))
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();
        builder.Property(c => c.Validity).IsRequired();
        builder.Property(c => c.CertificateNumber).IsRequired().HasMaxLength(30);
        builder.Property(c => c.EmployerFullName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.StandardCode).IsRequired().HasMaxLength(100);
    }
}
