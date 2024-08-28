using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class Tac
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TacID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string LookupEnumName { get; set; }
        public string Name { get; set; }
        public int? PacID { get; set; }
        public Guid PacCodePeek { get; set; }//PacID // not mapped
                //public Pac Pac { get; set; }  //PacID
    }
}
