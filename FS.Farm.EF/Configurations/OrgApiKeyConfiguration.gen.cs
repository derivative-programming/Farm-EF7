using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
namespace FS.Farm.EF.Configurations
{
    public class OrgApiKeyConfiguration : IEntityTypeConfiguration<OrgApiKey>
    {
         public void Configure(EntityTypeBuilder<OrgApiKey> builder)
        {
            builder.HasOne<Organization>() //OrganizationID
                .WithMany()
                .HasForeignKey(p => p.OrganizationID);
            builder.HasOne<OrgCustomer>() //OrgCustomerID
                .WithMany()
                .HasForeignKey(p => p.OrgCustomerID);
            builder.HasIndex(p => p.Code)
                .IsUnique();
            builder.Property(p => p.LastChangeCode)
                .IsConcurrencyToken();
            builder.Ignore(p => p.OrganizationCodePeek); //OrganizationID
            //ENDSET
            builder.Ignore(p => p.OrgCustomerCodePeek); //OrgCustomerID
            //ENDSET
            builder.Property<DateTime>("InsertUtcDateTime");
            builder.Property<DateTime>("LastUpdatedUtcDateTime");
        }
    }
}
