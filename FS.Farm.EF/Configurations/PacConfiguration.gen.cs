using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
namespace FS.Farm.EF.Configurations
{
    public class PacConfiguration : IEntityTypeConfiguration<Pac>
    {
         public void Configure(EntityTypeBuilder<Pac> builder)
        {
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        }
    }
}
