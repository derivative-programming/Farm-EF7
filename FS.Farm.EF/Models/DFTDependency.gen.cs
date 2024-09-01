using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class DFTDependency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DFTDependencyID { get; set; }

        public Guid? Code { get; set; }

        public Guid? LastChangeCode { get; set; }
        public int? DependencyDFTaskID { get; set; }
        public int? DynaFlowTaskID { get; set; }
        public bool? IsPlaceholder { get; set; }
        public Guid DynaFlowTaskCodePeek { get; set; }//DynaFlowTaskID // not mapped

        //public DynaFlowTask DynaFlowTask { get; set; }  //DynaFlowTaskID
    }
}
