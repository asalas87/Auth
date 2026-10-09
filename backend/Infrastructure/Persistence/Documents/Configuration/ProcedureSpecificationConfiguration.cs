using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;

public class ProcedureSpecificationConfiguration : IEntityTypeConfiguration<ProcedureSpecification>
{
    public void Configure(EntityTypeBuilder<ProcedureSpecification> builder)
    {
        builder.ToTable("ProcedureSpecifications", "DOC");
        builder.Property(document => document.Id)
            .HasConversion(id => id.Value, value => new DocumentFileId(value))
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();
        builder.Property(document => document.ProcedureNumber).IsRequired().HasMaxLength(30);
        builder.Property(document => document.StandardCode).IsRequired().HasMaxLength(100);
    }
}