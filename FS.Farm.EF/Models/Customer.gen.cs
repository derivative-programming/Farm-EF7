using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public int? ActiveOrganizationID { get; set; }
        public string Email { get; set; }
        public DateTime? EmailConfirmedUTCDateTime { get; set; }
        public string FirstName { get; set; }
        public DateTime? ForgotPasswordKeyExpirationUTCDateTime { get; set; }
        public string ForgotPasswordKeyValue { get; set; }
        public Guid? FSUserCodeValue { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailAllowed { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsEmailMarketingAllowed { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsMultipleOrganizationsAllowed { get; set; }
        public bool? IsVerboseLoggingForced { get; set; }
        public DateTime? LastLoginUTCDateTime { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public DateTime? RegistrationUTCDateTime { get; set; }
        public int? TacID { get; set; }
        public int? UTCOffsetInMinutes { get; set; }
        public string Zip { get; set; }
        public Guid TacCodePeek { get; set; }//TacID // not mapped
                //public Tac Tac { get; set; }  //TacID
    }
}
