using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorLogID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public Guid? BrowserCode { get; set; }
        public Guid? ContextCode { get; set; }
        public DateTime? CreatedUTCDateTime { get; set; }
        public string Description { get; set; }
        public bool? IsClientSideError { get; set; }
        public bool? IsResolved { get; set; }
        public int? PacID { get; set; }
        public string Url { get; set; }
        public Guid PacCodePeek { get; set; }//PacID // not mapped
                //public Pac Pac { get; set; }  //PacID
    }
}
