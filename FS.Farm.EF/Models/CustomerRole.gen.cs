using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FS.Farm.EF.Models
{
    public class CustomerRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerRoleID { get; set; }
        public Guid? Code { get; set; }
        public Guid? LastChangeCode { get; set; }
        public int? CustomerID { get; set; }
        public bool? IsPlaceholder { get; set; }
        public bool? Placeholder { get; set; }
        public int? RoleID { get; set; }
        public Guid CustomerCodePeek { get; set; }//CustomerID // not mapped
        public Guid RoleCodePeek { get; set; }//RoleID // not mapped
        //public Customer Customer { get; set; }  //CustomerID
        //public Role Role { get; set; } //RoleID
    }
}
