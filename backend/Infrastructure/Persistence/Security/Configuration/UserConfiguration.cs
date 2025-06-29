﻿using Domain.Security.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Security.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("SEC_Users");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(customerId => customerId.Value, value => new UserId(value));
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Email).HasConversion(email => email.Value, value => Email.Create(value)!).HasMaxLength(255);
            builder.Property(c => c.Password).HasMaxLength(60);
            builder.Property(c => c.Active);
            //builder.HasMany(u => u.RefreshTokens)
            //    .WithOne(r => r.User)
            //    .HasForeignKey(r => r.UserId)
            //    .IsRequired();
        }
    }
}
