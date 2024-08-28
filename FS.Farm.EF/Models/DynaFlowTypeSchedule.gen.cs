using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class DynaFlowTypeSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DynaFlowTypeScheduleID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public int? DynaFlowTypeID { get; set; }
        public int? FrequencyInHours { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUTCDateTime { get; set; }
        public DateTime? NextUTCDateTime { get; set; }
        public int? PacID { get; set; }
                public Guid DynaFlowTypeCodePeek { get; set; }//DynaFlowTypeID // not mapped
        public Guid PacCodePeek { get; set; }//PacID // not mapped
        //public DynaFlowType DynaFlowType { get; set; } //DynaFlowTypeID
                //public Pac Pac { get; set; }  //PacID
    }
}
