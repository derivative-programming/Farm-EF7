using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class DynaFlowTaskConfiguration : IEntityTypeConfiguration<DynaFlowTask>
    {
         public void Configure(EntityTypeBuilder<DynaFlowTask> builder)
        {

            builder.ToTable(ToSnakeCase("DynaFlowTask"));
            //CompletedUTCDateTime
            //Int32 dependencyDynaFlowTaskID,
            //String description,
            builder.HasOne<DynaFlow>() //DynaFlowID
                .WithMany()
                .HasForeignKey(p => p.DynaFlowID);
            //Guid dynaFlowSubjectCode,
            builder.HasOne<DynaFlowTaskType>() //DynaFlowTaskTypeID
                .WithMany()
                .HasForeignKey(p => p.DynaFlowTaskTypeID);
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isParallelRunAllowed,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Int32 maxRetryCount,
            //MinStartUTCDateTime
            //String param1,
            //String param2,
            //String processorIdentifier,
            //RequestedUTCDateTime
            //String resultValue,
            //Int32 retryCount,
            //StartedUTCDateTime

            bool isDBColumnIndexed = false;
            //CompletedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CompletedUTCDateTime);
            }
            //Int32 dependencyDynaFlowTaskID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DependencyDynaFlowTaskID);
            }
            //String description,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Description);
            }
            //DynaFlowID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowID);
            }
            //Guid dynaFlowSubjectCode,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowSubjectCode);
            }
            //DynaFlowTaskTypeID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowTaskTypeID);
            }
            //Boolean isCanceled,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsCanceled);
            }
            //Boolean isCancelRequested,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsCancelRequested);
            }
            //Boolean isCompleted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsCompleted);
            }
            //Boolean isParallelRunAllowed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsParallelRunAllowed);
            }
            //Boolean isRunTaskDebugRequired,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsRunTaskDebugRequired);
            }
            //Boolean isStarted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsStarted);
            }
            //Boolean isSuccessful,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsSuccessful);
            }
            //Int32 maxRetryCount,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.MaxRetryCount);
            }
            //MinStartUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.MinStartUTCDateTime);
            }
            //String param1,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Param1);
            }
            //String param2,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Param2);
            }
            //String processorIdentifier,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ProcessorIdentifier);
            }
            //RequestedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.RequestedUTCDateTime);
            }
            //String resultValue,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ResultValue);
            }
            //Int32 retryCount,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.RetryCount);
            }
            //StartedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.StartedUTCDateTime);
            }

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(DynaFlowTask.LastChangeCode)));
            builder.Ignore(p => p.DynaFlowCodePeek); //DynaFlowID
            builder.Ignore(p => p.DynaFlowTaskTypeCodePeek); //DynaFlowTaskTypeID

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
