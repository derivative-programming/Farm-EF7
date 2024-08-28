using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class DFTDependencyConfiguration : IEntityTypeConfiguration<DFTDependency>
    {
         public void Configure(EntityTypeBuilder<DFTDependency> builder)
        {

            builder.ToTable(ToSnakeCase("DFTDependency"));
            //Int32 dependencyDFTaskID,
            builder.HasOne<DynaFlowTask>() //DynaFlowTaskID
                .WithMany()
                .HasForeignKey(p => p.DynaFlowTaskID);
            //Boolean isPlaceholder,

            bool isDBColumnIndexed = false;
            //Int32 dependencyDFTaskID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DependencyDFTaskID);
            }
            //DynaFlowTaskID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowTaskID);
            }
            //Boolean isPlaceholder,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsPlaceholder);
            }

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(DFTDependency.LastChangeCode)));
            builder.Ignore(p => p.DynaFlowTaskCodePeek); //DynaFlowTaskID

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
