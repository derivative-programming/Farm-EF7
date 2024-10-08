using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class DynaFlowTypeScheduleConfiguration : IEntityTypeConfiguration<DynaFlowTypeSchedule>
    {
         public void Configure(EntityTypeBuilder<DynaFlowTypeSchedule> builder)
        {

            builder.ToTable(ToSnakeCase("DynaFlowTypeSchedule"));
            builder.HasOne<DynaFlowType>() //DynaFlowTypeID
                .WithMany()
                .HasForeignKey(p => p.DynaFlowTypeID);
            //Int32 frequencyInHours,
            //Boolean isActive,
            //LastUTCDateTime
            //NextUTCDateTime
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            bool isDBColumnIndexed = false;
            //DynaFlowTypeID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowTypeID);
            }
            //Int32 frequencyInHours,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.FrequencyInHours);
            }
            //Boolean isActive,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsActive);
            }
            //LastUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LastUTCDateTime);
            }
            //NextUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.NextUTCDateTime);
            }
            //PacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PacID);
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
