using Domain.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Documents.Configuration;
public class GeneralDocumentConfiguration : IEntityTypeConfiguration<GeneralDocument>
{
    public void Configure(EntityTypeBuilder<GeneralDocument> builder)
    {
        builder.ToTable("GeneralDocuments", "DOC");
    }
}
