using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;
public class RenovationConfiguration : IEntityTypeConfiguration<Renovation>
{
    public void Configure(EntityTypeBuilder<Renovation> builder)
    {
        builder.ToTable("Renovations", "DOC");
        builder.Property(c => c.Id)
            .HasConversion(companyId => companyId.Value, value => new DocumentFileId(value))
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Validity).IsRequired();
        builder.Property(c => c.CertificateNumber).IsRequired().HasMaxLength(30);
        builder.Property(c => c.EmployerFullName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.StandardCode).IsRequired().HasMaxLength(30);
    }
}
