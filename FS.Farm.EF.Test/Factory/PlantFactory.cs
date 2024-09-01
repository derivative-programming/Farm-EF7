using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.EF;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Models;

namespace FS.Farm.EF.Test.Factory
{
    public static class PlantFactory
    {
        private static int _counter = 0;


        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            PlantManager plantManager = new PlantManager(context);
            var plant = plantManager.GetByCode(code);

//ENDSET
            result = LandFactory.GetCodeLineage(context, plant.LandCodePeek); //LandID
                                                                                //FlvrForeignKeyID
//ENDSET

            result.Add("PlantCode", plant.Code.Value.ToString());

            return result;
        }

        public static async Task<Plant> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var land = await LandFactory.CreateAndSaveAsync(context); //LandID
            var flvrForeignKey = await FlavorFactory.CreateAndSaveAsync(context);//FlvrForeignKeyID
//ENDSET

            return new Plant
            {
                PlantID = _counter,
                Code = Guid.NewGuid(),
                IsDeleteAllowed = false,
                IsEditAllowed = false,
                LastChangeCode = Guid.NewGuid(),
                OtherFlavor = String.Empty,
                SomeBigIntVal = 0,
                SomeBitVal = false,
                SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeDecimalVal = 0,
                SomeEmailAddress = String.Empty,
                SomeFloatVal = 0,
                SomeIntVal = 0,
                SomeMoneyVal = 0,
                SomeNVarCharVal = String.Empty,
                SomePhoneNumber = String.Empty,
                SomeTextVal = String.Empty,
                SomeUniqueidentifierVal = Guid.NewGuid(),
                SomeImageUrlVal = String.Empty,
                IsImageUrlAvailable = false,
                SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeVarCharVal = String.Empty,
                FlvrForeignKeyID = flvrForeignKey.FlavorID,
                LandID = land.LandID,
//ENDSET 
            };
        }

        public static Plant Create(FarmDbContext context)
        {
            _counter++;
            var land = LandFactory.CreateAndSave(context); //LandID
            var flvrForeignKey = FlavorFactory.CreateAndSave(context);//FlvrForeignKeyID
//ENDSET

            return new Plant
            {
                PlantID = _counter,
                Code = Guid.NewGuid(),
                IsDeleteAllowed = false,
                IsEditAllowed = false,
                LastChangeCode = Guid.NewGuid(),
                OtherFlavor = String.Empty,
                SomeBigIntVal = 0,
                SomeBitVal = false,
                SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeDecimalVal = 0,
                SomeEmailAddress = String.Empty,
                SomeFloatVal = 0,
                SomeIntVal = 0,
                SomeMoneyVal = 0,
                SomeNVarCharVal = String.Empty,
                SomePhoneNumber = String.Empty,
                SomeTextVal = String.Empty,
                SomeUniqueidentifierVal = Guid.NewGuid(),
                SomeImageUrlVal = String.Empty,
                IsImageUrlAvailable = false,
                SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeVarCharVal = String.Empty,
                FlvrForeignKeyID = flvrForeignKey.FlavorID,
                LandID = land.LandID,
//ENDSET 
            };
        }
        public static async Task<Plant> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var land = await LandFactory.CreateAndSaveAsync(context); //LandID
            var flvrForeignKey = await FlavorFactory.CreateAndSaveAsync(context);//FlvrForeignKeyID
//ENDSET

            Plant result =  new Plant
            {
                PlantID = _counter,
                Code = Guid.NewGuid(),
                IsDeleteAllowed = false,
                IsEditAllowed = false,
                LastChangeCode = Guid.NewGuid(),
                OtherFlavor = String.Empty,
                SomeBigIntVal = 0,
                SomeBitVal = false,
                SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeDecimalVal = 0,
                SomeEmailAddress = String.Empty,
                SomeFloatVal = 0,
                SomeIntVal = 0,
                SomeMoneyVal = 0,
                SomeNVarCharVal = String.Empty,
                SomePhoneNumber = String.Empty,
                SomeTextVal = String.Empty,
                SomeUniqueidentifierVal = Guid.NewGuid(),
                SomeImageUrlVal = String.Empty,
                IsImageUrlAvailable = false,
                SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeVarCharVal = String.Empty,
                FlvrForeignKeyID = flvrForeignKey.FlavorID,
                LandID = land.LandID,
//ENDSET 
            };

            PlantManager plantManager = new PlantManager(context);
            result = await plantManager.AddAsync(result);
            return result;
        }


        public static Plant CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var land =   LandFactory.CreateAndSave(context); //LandID
            var flvrForeignKey =   FlavorFactory.CreateAndSave(context);//FlvrForeignKeyID
//ENDSET

            Plant result = new Plant
            {
                PlantID = _counter,
                Code = Guid.NewGuid(),
                IsDeleteAllowed = false,
                IsEditAllowed = false,
                LastChangeCode = Guid.NewGuid(),
                OtherFlavor = String.Empty,
                SomeBigIntVal = 0,
                SomeBitVal = false,
                SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeDecimalVal = 0,
                SomeEmailAddress = String.Empty,
                SomeFloatVal = 0,
                SomeIntVal = 0,
                SomeMoneyVal = 0,
                SomeNVarCharVal = String.Empty,
                SomePhoneNumber = String.Empty,
                SomeTextVal = String.Empty,
                SomeUniqueidentifierVal = Guid.NewGuid(),
                SomeImageUrlVal = String.Empty,
                IsImageUrlAvailable = false,
                SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeVarCharVal = String.Empty,
                FlvrForeignKeyID = flvrForeignKey.FlavorID,
                LandID = land.LandID,
//ENDSET 
            };

            PlantManager plantManager = new PlantManager(context);
            result = plantManager.Add(result);
            return result;
        }

    }
}
