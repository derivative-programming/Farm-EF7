using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;
namespace FS.Farm.EF.Configurations
{
    public partial class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
    {
         public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.ToTable(ToSnakeCase("ErrorLog"));
            //Guid browserCode,
            //Guid contextCode,
            //CreatedUTCDateTime
            //String description,
            //Boolean isClientSideError,
            //Boolean isResolved,
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            //String url,
            bool isDBColumnIndexed = false;
            //Guid browserCode,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.BrowserCode);
            }
            //Guid contextCode,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ContextCode);
            }
            //CreatedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CreatedUTCDateTime);
            }
            //String description,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Description);
            }
            //Boolean isClientSideError,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsClientSideError);
            }
            //Boolean isResolved,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsResolved);
            }
            //PacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PacID);
            }
            //String url,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Url);
            }
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(ErrorLog.LastChangeCode)));
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
