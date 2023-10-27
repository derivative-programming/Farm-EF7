using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
namespace FS.Farm.EF.Configurations
{
    public class OrgCustomerConfiguration : IEntityTypeConfiguration<OrgCustomer>
    {
         public void Configure(EntityTypeBuilder<OrgCustomer> builder)
        {
            builder.ToTable(ToSnakeCase("OrgCustomer"));
            builder.HasOne<Customer>() //CustomerID
                .WithMany()
                .HasForeignKey(p => p.CustomerID);
            builder.HasOne<Organization>() //OrganizationID
                .WithMany()
                .HasForeignKey(p => p.OrganizationID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(OrgCustomer.LastChangeCode)));
            builder.Ignore(p => p.CustomerCodePeek); //CustomerID
            builder.Ignore(p => p.OrganizationCodePeek); //OrganizationID
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
