using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class DFMaintenance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DFMaintenanceID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public bool? IsPaused { get; set; }
        public bool? IsScheduledDFProcessRequestCompleted { get; set; }
        public bool? IsScheduledDFProcessRequestStarted { get; set; }
        public DateTime? LastScheduledDFProcessRequestUTCDateTime { get; set; }
        public DateTime? NextScheduledDFProcessRequestUTCDateTime { get; set; }
        public int? PacID { get; set; }
        public string PausedByUsername { get; set; }
        public DateTime? PausedUTCDateTime { get; set; }
        public string ScheduledDFProcessRequestProcessorIdentifier { get; set; }
        public Guid PacCodePeek { get; set; }//PacID // not mapped
                //public Pac Pac { get; set; }  //PacID
    }
}
