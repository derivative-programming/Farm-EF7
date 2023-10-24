using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
namespace FS.Farm.EF.Configurations
{
    public class CustomerRoleConfiguration : IEntityTypeConfiguration<CustomerRole>
    {
         public void Configure(EntityTypeBuilder<CustomerRole> builder)
        {
            builder.HasOne<Customer>() //CustomerID
                .WithMany()
                .HasForeignKey(p => p.CustomerID);
            builder.HasOne<Role>() //RoleID
                .WithMany()
                .HasForeignKey(p => p.RoleID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
            builder.Ignore(p => p.CustomerCodePeek); //CustomerID
            builder.Ignore(p => p.RoleCodePeek); //RoleID
            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        }
    }
}
