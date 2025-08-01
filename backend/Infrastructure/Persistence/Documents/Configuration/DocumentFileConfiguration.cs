using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Documents.Entities;
using Domain.Security.Entities;
using Domain.Partners.Entities;

namespace Infrastructure.Persistence.Documents.Configuration;

public class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.ToTable("DocumentFiles", "DOC");

        builder.HasKey(c => c.Id);
        builder.Property(d => d.Id)
            .HasConversion(documentId => documentId.Value, value => new DocumentFileId(value));

        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Description).HasMaxLength(50);
        builder.Property(d => d.ExpirationDate);
        builder.Property(d => d.UploadDate).IsRequired();
        builder.Property(d => d.Path).IsRequired().HasMaxLength(500);

        builder.Property<UserId>("UploadedById")
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.HasOne(d => d.UploadedBy)
            .WithMany()
            .HasForeignKey("UploadedById")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<CompanyId?>("AssignedToId")
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value != null ? new CompanyId(value.Value) : null);

        builder.HasOne(d => d.AssignedTo)
            .WithMany()
            .HasForeignKey("AssignedToId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.DocumentType)
            .IsRequired();
    }
}
