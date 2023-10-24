using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
namespace FS.Farm.EF.Configurations
{
    public class OrgCustomerConfiguration : IEntityTypeConfiguration<OrgCustomer>
    {
         public void Configure(EntityTypeBuilder<OrgCustomer> builder)
        {
            builder.HasOne<Customer>() //CustomerID
                .WithMany()
                .HasForeignKey(p => p.CustomerID);
            builder.HasOne<Organization>() //OrganizationID
                .WithMany()
                .HasForeignKey(p => p.OrganizationID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
            builder.Ignore(p => p.CustomerCodePeek); //CustomerID
            builder.Ignore(p => p.OrganizationCodePeek); //OrganizationID
            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        }
    }
}
