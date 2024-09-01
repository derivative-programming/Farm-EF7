using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
         public void Configure(EntityTypeBuilder<Organization> builder)
        {

            builder.ToTable(ToSnakeCase("Organization"));
            //String name,
            builder.HasOne<Tac>() //TacID
                .WithMany()
                .HasForeignKey(p => p.TacID);
            bool isDBColumnIndexed = false;
            //String name,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Name);
            }
            //TacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.TacID);
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
