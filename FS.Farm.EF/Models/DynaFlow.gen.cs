using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class DynaFlow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DynaFlowID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public DateTime? CompletedUTCDateTime { get; set; }
        public int? DependencyDynaFlowID { get; set; }
        public string Description { get; set; }
        public int? DynaFlowTypeID { get; set; }
        public bool? IsBuildTaskDebugRequired { get; set; }
        public bool? IsCanceled { get; set; }
        public bool? IsCancelRequested { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsPaused { get; set; }
        public bool? IsResubmitted { get; set; }
        public bool? IsRunTaskDebugRequired { get; set; }
        public bool? IsStarted { get; set; }
        public bool? IsSuccessful { get; set; }
        public bool? IsTaskCreationStarted { get; set; }
        public bool? IsTasksCreated { get; set; }
        public DateTime? MinStartUTCDateTime { get; set; }
        public int? PacID { get; set; }
        public string Param1 { get; set; }
        public int? ParentDynaFlowID { get; set; }
        public int? PriorityLevel { get; set; }
        public DateTime? RequestedUTCDateTime { get; set; }
        public string ResultValue { get; set; }
        public int? RootDynaFlowID { get; set; }
        public DateTime? StartedUTCDateTime { get; set; }
        public Guid? SubjectCode { get; set; }
        public string TaskCreationProcessorIdentifier { get; set; }
                public Guid DynaFlowTypeCodePeek { get; set; }//DynaFlowTypeID // not mapped
        public Guid PacCodePeek { get; set; }//PacID // not mapped
        //public DynaFlowType DynaFlowType { get; set; } //DynaFlowTypeID
                //public Pac Pac { get; set; }  //PacID
    }
}
