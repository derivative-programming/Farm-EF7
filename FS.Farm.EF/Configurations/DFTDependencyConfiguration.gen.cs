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
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
