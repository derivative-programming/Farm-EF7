using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;
namespace FS.Farm.EF.Configurations
{
    public partial class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
         public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(ToSnakeCase("Role"));
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            bool isDBColumnIndexed = false;
            //String description,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Description);
            }
            //Int32 displayOrder,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DisplayOrder);
            }
            //Boolean isActive,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsActive);
            }
            //String lookupEnumName,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LookupEnumName);
            }
            //String name,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Name);
            }
            //PacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PacID);
            }
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(Role.LastChangeCode)));
            builder.Ignore(p => p.PacCodePeek); //PacID
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
