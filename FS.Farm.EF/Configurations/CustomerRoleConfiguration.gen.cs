using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class CustomerRoleConfiguration : IEntityTypeConfiguration<CustomerRole>
    {
         public void Configure(EntityTypeBuilder<CustomerRole> builder)
        {

            builder.ToTable(ToSnakeCase("CustomerRole"));
            builder.HasOne<Customer>() //CustomerID
                .WithMany()
                .HasForeignKey(p => p.CustomerID);
            //Boolean isPlaceholder,
            //Boolean placeholder,
            builder.HasOne<Role>() //RoleID
                .WithMany()
                .HasForeignKey(p => p.RoleID);
            bool isDBColumnIndexed = false;
            //CustomerID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.CustomerID);
            }
            //Boolean isPlaceholder,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsPlaceholder);
            }
            //Boolean placeholder,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Placeholder);
            }
            //RoleID
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.RoleID);
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
