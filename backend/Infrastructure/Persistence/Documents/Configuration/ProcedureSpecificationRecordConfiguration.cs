using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;

public class ProcedureSpecificationRecordConfiguration : IEntityTypeConfiguration<ProcedureSpecificationRecord>
{
    public void Configure(EntityTypeBuilder<ProcedureSpecificationRecord> builder)
    {
        builder.ToTable("ProcedureSpecificationRecords", "DOC");
        builder.Property(document => document.Id)
            .HasConversion(id => id.Value, value => new DocumentFileId(value))
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();
    }
}