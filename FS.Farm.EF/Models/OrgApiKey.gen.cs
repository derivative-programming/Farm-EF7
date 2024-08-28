using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class OrgApiKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrgApiKeyID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public string ApiKeyValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedUTCDateTime { get; set; }
        public DateTime? ExpirationUTCDateTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTempUserKey { get; set; }
        public string Name { get; set; }
        public int? OrganizationID { get; set; }
        public int? OrgCustomerID { get; set; }
        public Guid OrganizationCodePeek { get; set; }//OrganizationID // not mapped
                public Guid OrgCustomerCodePeek { get; set; }//OrgCustomerID // not mapped
                //public Organization Organization { get; set; }  //OrganizationID
        //public OrgCustomer OrgCustomer { get; set; } //OrgCustomerID
    }
}
