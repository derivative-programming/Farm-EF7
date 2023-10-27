using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
namespace FS.Farm.EF.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
         public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(ToSnakeCase("Customer"));
            builder.HasOne<Tac>() //TacID
                .WithMany()
                .HasForeignKey(p => p.TacID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken()
                .HasColumnName(ToSnakeCase(nameof(Customer.LastChangeCode)));
            builder.Ignore(p => p.TacCodePeek); //TacID
            builder.Property<DateTime>("InsertUtcDateTime").HasColumnName(ToSnakeCase("InsertUtcDateTime"));
            builder.Property<DateTime>("LastUpdatedUtcDateTime").HasColumnName(ToSnakeCase("LastUpdatedUtcDateTime"));
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
