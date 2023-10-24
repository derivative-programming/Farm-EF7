using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FS.Farm.EF.Models
{
    public class OrgCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrgCustomerID { get; set; }
        public Guid? Code { get; set; }
        public Guid? LastChangeCode { get; set; }
        public int? CustomerID { get; set; }
        public string Email { get; set; }
        public int? OrganizationID { get; set; }
        public Guid CustomerCodePeek { get; set; }//CustomerID // not mapped
        public Guid OrganizationCodePeek { get; set; }//OrganizationID // not mapped
        //public Customer Customer { get; set; } //CustomerID
        //public Organization Organization { get; set; }  //OrganizationID
    }
}
