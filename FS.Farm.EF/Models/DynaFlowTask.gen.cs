using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class DynaFlowTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DynaFlowTaskID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public DateTime? CompletedUTCDateTime { get; set; }
        public int? DependencyDynaFlowTaskID { get; set; }
        public string Description { get; set; }
        public int? DynaFlowID { get; set; }
        public Guid? DynaFlowSubjectCode { get; set; }
        public int? DynaFlowTaskTypeID { get; set; }
        public bool? IsCanceled { get; set; }
        public bool? IsCancelRequested { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsParallelRunAllowed { get; set; }
        public bool? IsRunTaskDebugRequired { get; set; }
        public bool? IsStarted { get; set; }
        public bool? IsSuccessful { get; set; }
        public int? MaxRetryCount { get; set; }
        public DateTime? MinStartUTCDateTime { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string ProcessorIdentifier { get; set; }
        public DateTime? RequestedUTCDateTime { get; set; }
        public string ResultValue { get; set; }
        public int? RetryCount { get; set; }
        public DateTime? StartedUTCDateTime { get; set; }
        public Guid DynaFlowCodePeek { get; set; }//DynaFlowID // not mapped

        public Guid DynaFlowTaskTypeCodePeek { get; set; }//DynaFlowTaskTypeID // not mapped
        //public DynaFlow DynaFlow { get; set; }  //DynaFlowID
        //public DynaFlowTaskType DynaFlowTaskType { get; set; } //DynaFlowTaskTypeID
    }
}
