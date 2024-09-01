using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.Farm.EF.Models
{
    public class Plant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlantID { get; set; }
         
        public Guid? Code { get; set; }
         
        public Guid? LastChangeCode { get; set; } 

        public bool? IsDeleteAllowed { get; set; }

        public bool? IsEditAllowed { get; set; }

        public int? LandID { get; set; }

        public string OtherFlavor { get; set; }

        public long? SomeBigIntVal { get; set; }

        public bool? SomeBitVal { get; set; }

        public DateTime? SomeDateVal { get; set; }

        public decimal? SomeDecimalVal { get; set; }

        public string SomeEmailAddress { get; set; }

        public float? SomeFloatVal { get; set; }

        public int? SomeIntVal { get; set; }

        public decimal? SomeMoneyVal { get; set; }

        public string SomeNVarCharVal { get; set; }

        public string SomePhoneNumber { get; set; }

        public string SomeTextVal { get; set; }

        public Guid? SomeUniqueidentifierVal { get; set; }

        public DateTime? SomeUTCDateTimeVal { get; set; }

        public string SomeVarCharVal { get; set; }

        public int? FlvrForeignKeyID { get; set; }

        public bool? IsImageUrlAvailable { get; set; }

        public string SomeImageUrlVal { get; set; }

//ENDSET 
        public Guid FlvrForeignKeyCodePeek { get; set; }//FlvrForeignKeyID // not mapped 
        public Guid LandCodePeek { get; set; }//LandID // not mapped

//ENDSET
        //public Land Land { get; set; }  //LandID
        //public Flavor FlvrForeignKey { get; set; } //FlvrForeignKeyID
    }
}