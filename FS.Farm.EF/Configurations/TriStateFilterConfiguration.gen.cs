using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
namespace FS.Farm.EF.Configurations
{
    public class TriStateFilterConfiguration : IEntityTypeConfiguration<TriStateFilter>
    {
         public void Configure(EntityTypeBuilder<TriStateFilter> builder)
        {
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
            builder.Ignore(p => p.PacCodePeek); //PacID
            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        }
    }
}
