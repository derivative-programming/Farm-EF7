using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class DynaFlowConfiguration : IEntityTypeConfiguration<DynaFlow>
    {
         public void Configure(EntityTypeBuilder<DynaFlow> builder)
        {

            builder.ToTable(ToSnakeCase("DynaFlow"));
            //CompletedUTCDateTime
            //Int32 dependencyDynaFlowID,
            //String description,
            builder.HasOne<DynaFlowType>() //DynaFlowTypeID
                .WithMany()
                .HasForeignKey(p => p.DynaFlowTypeID);
            //Boolean isBuildTaskDebugRequired,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isPaused,
            //Boolean isResubmitted,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Boolean isTaskCreationStarted,
            //Boolean isTasksCreated,
            //MinStartUTCDateTime
            builder.HasOne<Pac>() //PacID
                .WithMany()
                .HasForeignKey(p => p.PacID);
            //String param1,
            //Int32 parentDynaFlowID,
            //Int32 priorityLevel,
            //RequestedUTCDateTime
            //String resultValue,
            //Int32 rootDynaFlowID,
            //StartedUTCDateTime
            //Guid subjectCode,
            //String taskCreationProcessorIdentifier,

            bool isDBColumnIndexed = false;
            //CompletedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CompletedUTCDateTime);
            }
            //Int32 dependencyDynaFlowID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DependencyDynaFlowID);
            }
            //String description,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Description);
            }
            //DynaFlowTypeID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.DynaFlowTypeID);
            }
            //Boolean isBuildTaskDebugRequired,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsBuildTaskDebugRequired);
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
            //Boolean isPaused,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsPaused);
            }
            //Boolean isResubmitted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsResubmitted);
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
            //Boolean isTaskCreationStarted,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsTaskCreationStarted);
            }
            //Boolean isTasksCreated,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsTasksCreated);
            }
            //MinStartUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.MinStartUTCDateTime);
            }
            //PacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PacID);
            }
            //String param1,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Param1);
            }
            //Int32 parentDynaFlowID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ParentDynaFlowID);
            }
            //Int32 priorityLevel,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.PriorityLevel);
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
            //Int32 rootDynaFlowID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.RootDynaFlowID);
            }
            //StartedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.StartedUTCDateTime);
            }
            //Guid subjectCode,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.SubjectCode);
            }
            //String taskCreationProcessorIdentifier,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.TaskCreationProcessorIdentifier);
            }

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(DynaFlow.LastChangeCode)));
            builder.Ignore(p => p.DynaFlowTypeCodePeek); //DynaFlowTypeID
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
