
using Domain.Partners.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Partners.Configuration;
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies", "PAR");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(companyId => companyId.Value, value => new CompanyId(value))
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();
        builder.Property(c => c.Name).HasMaxLength(50);
        builder.Property(c => c.CuitCuil).HasConversion(cuit => cuit.Value, value => Cuit.Create(value)!).HasMaxLength(13);
        builder.HasMany(c => c.Users)
            .WithOne(u => u.Company)
            .HasForeignKey("CompanyId");
        builder.Property(c => c.IsActive);
    }
}
