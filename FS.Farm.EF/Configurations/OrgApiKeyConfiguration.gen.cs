using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;
namespace FS.Farm.EF.Configurations
{
    public partial class OrgApiKeyConfiguration : IEntityTypeConfiguration<OrgApiKey>
    {
         public void Configure(EntityTypeBuilder<OrgApiKey> builder)
        {
            builder.ToTable(ToSnakeCase("OrgApiKey"));
            //String apiKeyValue,
            //String createdBy,
            //CreatedUTCDateTime
            //ExpirationUTCDateTime
            //Boolean isActive,
            //Boolean isTempUserKey,
            //String name,
            builder.HasOne<Organization>() //OrganizationID
                .WithMany()
                .HasForeignKey(p => p.OrganizationID);
            builder.HasOne<OrgCustomer>() //OrgCustomerID
                .WithMany()
                .HasForeignKey(p => p.OrgCustomerID);
            bool isDBColumnIndexed = false;
            //String apiKeyValue,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ApiKeyValue);
            }
            //String createdBy,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CreatedBy);
            }
            //CreatedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CreatedUTCDateTime);
            }
            //ExpirationUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ExpirationUTCDateTime);
            }
            //Boolean isActive,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsActive);
            }
            //Boolean isTempUserKey,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsTempUserKey);
            }
            //String name,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Name);
            }
            //OrganizationID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.OrganizationID);
            }
            //OrgCustomerID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.OrgCustomerID);
            }
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(OrgApiKey.LastChangeCode)));
            builder.Ignore(p => p.OrganizationCodePeek); //OrganizationID
            builder.Ignore(p => p.OrgCustomerCodePeek); //OrgCustomerID
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
