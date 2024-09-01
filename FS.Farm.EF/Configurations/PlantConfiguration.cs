using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class PlantConfiguration : IEntityTypeConfiguration<Plant>
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
            //Boolean isImageUrlAvailable 
            //String someImageUrlVal  
            //SomeUTCDateTimeVal
            //String someVarCharVal,
            // someDateVal, 
//ENDSET


            bool isDBColumnIndexed = false;
            //LandID 
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LandID);
            }
            //FlvrForeignKeyID 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.FlvrForeignKeyID);
            }
            //Boolean isDeleteAllowed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsDeleteAllowed);
            }
            //Boolean isEditAllowed, 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsEditAllowed);
            }
            //String otherFlavor,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.OtherFlavor);
            }
            //Int64 someBigIntVal,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeBigIntVal);
            }
            //Boolean someBitVal,  
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeBitVal);
            }
            //SomeDecimalVal  
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeDecimalVal);
            }
            //String someEmailAddress,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeEmailAddress);
            }
            //Double someFloatVal,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeFloatVal);
            }
            //Int32 someIntVal, 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeIntVal);
            }
            //SomeMoneyVal 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeMoneyVal);
            }
            //String someNVarCharVal,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeNVarCharVal);
            }
            //String somePhoneNumber,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomePhoneNumber);
            }
            //String someTextVal,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeTextVal);
            }
            //Guid someUniqueidentifierVal, 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeUniqueidentifierVal);
            }
            //SomeUTCDateTimeVal
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeUTCDateTimeVal);
            }
            //String someVarCharVal,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeVarCharVal);
            }
            // someDateVal, 
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SomeDateVal);
            }
            //Boolean isImageUrlAvailable 
            //String someImageUrlVal  
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