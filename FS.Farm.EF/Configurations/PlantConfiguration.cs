using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;

namespace FS.Farm.EF.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
         public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.HasOne<Land>() //LandID
                .WithMany()
                .HasForeignKey(p => p.LandID);

            builder.HasOne<Flavor>() //FlvrForeignKeyID
                .WithMany()
                .HasForeignKey(p => p.FlvrForeignKeyID);

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
             
            //ENDSET

            builder.Ignore(p => p.FlvrForeignKeyCodePeek); //FlvrForeignKeyID
            builder.Ignore(p => p.LandCodePeek); //LandID
            //ENDSET

            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        } 
    }
}