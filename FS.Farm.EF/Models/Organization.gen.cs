using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FS.Farm.EF.Models
{
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganizationID { get; set; }
        public Guid? Code { get; set; }
        public Guid? LastChangeCode { get; set; }
        public string Name { get; set; }
        public int? TacID { get; set; }
        public Guid TacCodePeek { get; set; }//TacID // not mapped
        //public Tac Tac { get; set; }  //TacID
    }
}
