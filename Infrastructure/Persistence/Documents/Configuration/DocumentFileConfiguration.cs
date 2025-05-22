using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Documents.Entities;
using Domain.Secutiry.Entities;

namespace Infrastructure.Persistence.Documents.Configuration
{

    internal class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
    {
        public void Configure(EntityTypeBuilder<DocumentFile> builder)
        {
            builder.ToTable("DOC_DocumentFiles");

            builder.HasKey(c => c.Id);
            builder.Property(d => d.Id).HasConversion(documentId => documentId.Value, value => new DocumentFileId(value));
            builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
            builder.Property(d => d.Description).HasMaxLength(50);
            builder.Property(d => d.ExpirationDate);
            builder.Property(d => d.UploadDate).IsRequired();
            builder.Property(d => d.Path).IsRequired().HasMaxLength(50);

            builder.Property(d => d.UploadedById)
                .IsRequired()
                .HasConversion(id => id.Value, value => new UserId(value));

            builder.Property(d => d.AssignedToId)
                .IsRequired(false)
                .IsRequired()
                .HasConversion(id => id.Value, value => new UserId(value));

            builder.HasOne<User>() 
                .WithMany()
                .HasForeignKey("UploadedById")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey("AssignedToId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
