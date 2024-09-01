using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class OrgCustomerConfiguration : IEntityTypeConfiguration<OrgCustomer>
    {
         public void Configure(EntityTypeBuilder<OrgCustomer> builder)
        {

            builder.ToTable(ToSnakeCase("OrgCustomer"));
            builder.HasOne<Customer>() //CustomerID
                .WithMany()
                .HasForeignKey(p => p.CustomerID);
            //String email,
            builder.HasOne<Organization>() //OrganizationID
                .WithMany()
                .HasForeignKey(p => p.OrganizationID);
            bool isDBColumnIndexed = false;
            //CustomerID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CustomerID);
            }
            //String email,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Email);
            }
            //OrganizationID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.OrganizationID);
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
