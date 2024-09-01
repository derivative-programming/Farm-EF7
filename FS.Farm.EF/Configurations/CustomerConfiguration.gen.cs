using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Text.RegularExpressions;
using NetTopologySuite.Operation.Overlay;

namespace FS.Farm.EF.Configurations
{
    public partial class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
         public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.ToTable(ToSnakeCase("Customer"));
            //Int32 activeOrganizationID,
            //String email,
            //EmailConfirmedUTCDateTime
            //String firstName,
            //ForgotPasswordKeyExpirationUTCDateTime
            //String forgotPasswordKeyValue,
            //Guid fSUserCodeValue,
            //Boolean isActive,
            //Boolean isEmailAllowed,
            //Boolean isEmailConfirmed,
            //Boolean isEmailMarketingAllowed,
            //Boolean isLocked,
            //Boolean isMultipleOrganizationsAllowed,
            //Boolean isVerboseLoggingForced,
            //LastLoginUTCDateTime
            //String lastName,
            //String password,
            //String phone,
            //String province,
            //RegistrationUTCDateTime
            builder.HasOne<Tac>() //TacID
                .WithMany()
                .HasForeignKey(p => p.TacID);
            //Int32 uTCOffsetInMinutes,
            //String zip,
            bool isDBColumnIndexed = false;
            //Int32 activeOrganizationID,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ActiveOrganizationID);
            }
            //String email,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Email);
            }
            //EmailConfirmedUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.EmailConfirmedUTCDateTime);
            }
            //String firstName,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.FirstName);
            }
            //ForgotPasswordKeyExpirationUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ForgotPasswordKeyExpirationUTCDateTime);
            }
            //String forgotPasswordKeyValue,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.ForgotPasswordKeyValue);
            }
            //Guid fSUserCodeValue,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.FSUserCodeValue);
            }
            //Boolean isActive,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsActive);
            }
            //Boolean isEmailAllowed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsEmailAllowed);
            }
            //Boolean isEmailConfirmed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsEmailConfirmed);
            }
            //Boolean isEmailMarketingAllowed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsEmailMarketingAllowed);
            }
            //Boolean isLocked,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsLocked);
            }
            //Boolean isMultipleOrganizationsAllowed,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsMultipleOrganizationsAllowed);
            }
            //Boolean isVerboseLoggingForced,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.IsVerboseLoggingForced);
            }
            //LastLoginUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LastLoginUTCDateTime);
            }
            //String lastName,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.LastName);
            }
            //String password,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Password);
            }
            //String phone,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Phone);
            }
            //String province,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Province);
            }
            //RegistrationUTCDateTime
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.RegistrationUTCDateTime);
            }
            //TacID
            isDBColumnIndexed = false;
            if(isDBColumnIndexed)
            {
                builder.HasIndex(p => p.TacID);
            }
            //Int32 uTCOffsetInMinutes,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.UTCOffsetInMinutes);
            }
            //String zip,
            isDBColumnIndexed = false;
            if (isDBColumnIndexed)
            {
                builder.HasIndex(p => p.Zip);
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
