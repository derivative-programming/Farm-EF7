using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
         public void Configure(EntityTypeBuilder<Plant> builder)
        { 

            builder.ToTable(ToSnakeCase("Plant"));


            builder.HasOne<Land>() //LandID
                .WithMany()
                .HasForeignKey(p => p.LandID); 
            builder.HasOne<Flavor>() //FlvrForeignKeyID
                .WithMany()
                .HasForeignKey(p => p.FlvrForeignKeyID);
            //Boolean isDeleteAllowed,
            //Boolean isEditAllowed, 
            //String otherFlavor,
            //Int64 someBigIntVal,
            //Boolean someBitVal,  
            builder.Property(p => p.SomeDecimalVal)
              .HasColumnType("decimal(18,6)")
              .HasPrecision(18, 6);
            //String someEmailAddress,
            //Double someFloatVal,
            //Int32 someIntVal, 
            builder.Property(p => p.SomeMoneyVal)
              .HasColumnType("money");
            //String someNVarCharVal,
            //String somePhoneNumber,
            //String someTextVal,
            //Guid someUniqueidentifierVal, 
            //SomeUTCDateTimeVal
            //String someVarCharVal,
            // someDateVal, 
            //ENDSET

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(Plant.LastChangeCode)));


            builder.Ignore(p => p.FlvrForeignKeyCodePeek); //FlvrForeignKeyID
            builder.Ignore(p => p.LandCodePeek); //LandID
            //ENDSET

            builder.Property<DateTime>("insert_utc_date_time");
            builder.Property<DateTime>("last_updated_utc_date_time");

            // Loop through all the properties to set snake_case column names
            foreach (var property in builder.Metadata.GetProperties())
            {
                builder.Property(property.Name).HasColumnName(ToSnakeCase(property.Name));
            }
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}