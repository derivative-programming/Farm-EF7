using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class DFMaintenanceConfiguration : IEntityTypeConfiguration<DFMaintenance>
    {
         public void Configure(EntityTypeBuilder<DFMaintenance> builder)
        {

            builder.ToTable(ToSnakeCase("DFMaintenance"));
            //Boolean isPaused,
            //Boolean isScheduledDFProcessRequestCompleted,
            //Boolean isScheduledDFProcessRequestStarted,
            //LastScheduledDFProcessRequestUTCDateTime
            //NextScheduledDFProcessRequestUTCDateTime
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            //String pausedByUsername,
            //PausedUTCDateTime
            //String scheduledDFProcessRequestProcessorIdentifier,
            bool isDBColumnIndexed = false;
            //Boolean isPaused,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsPaused);
            }
            //Boolean isScheduledDFProcessRequestCompleted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsScheduledDFProcessRequestCompleted);
            }
            //Boolean isScheduledDFProcessRequestStarted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsScheduledDFProcessRequestStarted);
            }
            //LastScheduledDFProcessRequestUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LastScheduledDFProcessRequestUTCDateTime);
            }
            //NextScheduledDFProcessRequestUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.NextScheduledDFProcessRequestUTCDateTime);
            }
            //PacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PacID);
            }
            //String pausedByUsername,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PausedByUsername);
            }
            //PausedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PausedUTCDateTime);
            }
            //String scheduledDFProcessRequestProcessorIdentifier,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ScheduledDFProcessRequestProcessorIdentifier);
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
