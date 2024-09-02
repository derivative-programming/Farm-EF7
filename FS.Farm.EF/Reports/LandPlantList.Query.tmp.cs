using FS.Common.Configuration;
using FS.Common.Objects;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.EF.Reports
{
    public partial class LandPlantList
    {
         
        private IQueryable<QueryDTO> BuildQuery()
        {
            //TODO handle full heirarchy, including target child if applicable.  Handle all lookups on each as well
            return from plant in _dbContext.PlantSet.AsNoTracking()
                   from flavor in _dbContext.FlavorSet.AsNoTracking().Where(x => x.FlavorID == plant.FlvrForeignKeyID).DefaultIfEmpty()
                   from land in _dbContext.LandSet.AsNoTracking().Where(x => x.LandID == plant.LandID).DefaultIfEmpty()
                   from pac in _dbContext.PacSet.AsNoTracking().Where(x => x.PacID == land.PacID).DefaultIfEmpty()

            select new QueryDTO
            {
                Plant = plant,
                Flavor = flavor,
                Land = land,
                Pac = pac,
            };
        }


        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,
           Guid? flavorFilterCode,
           Int32? someFilterIntVal,
           Int64? someFilterBigIntVal,
           Double? someFilterFloatVal,
           Boolean? someFilterBitVal,
           Boolean? isFilterEditAllowed,
           Boolean? isFilterDeleteAllowed,
           Decimal? someFilterDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someFilterMoneyVal,
           String someFilterNVarCharVal,
           String someFilterVarCharVal,
           String someFilterTextVal,
           String someFilterPhoneNumber,
           String someFilterEmailAddress,
           Guid? someFilterUniqueIdentifier,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.Land.Code == contextCode);
            if (flavorFilterCode != null && flavorFilterCode != Guid.Empty) query = query.Where(x => x.Flavor.Code == flavorFilterCode);
            if (someFilterIntVal != null) query = query.Where(x => x.Plant.SomeIntVal == someFilterIntVal);
            if (someFilterBigIntVal != null) query = query.Where(x => x.Plant.SomeBigIntVal == someFilterBigIntVal);
            if (someFilterFloatVal != null) query = query.Where(x => x.Plant.SomeFloatVal == someFilterFloatVal);
            if (someFilterBitVal != null) query = query.Where(x => x.Plant.SomeBitVal == someFilterBitVal);
            if (isFilterEditAllowed != null) query = query.Where(x => x.Plant.IsEditAllowed == isFilterEditAllowed);
            if (isFilterDeleteAllowed != null) query = query.Where(x => x.Plant.IsDeleteAllowed == isFilterDeleteAllowed);
            if (someFilterDecimalVal != null && someFilterDecimalVal != null) query = query.Where(x => x.Plant.SomeDecimalVal == someFilterDecimalVal);
            if (someMinUTCDateTimeVal != null && someMinUTCDateTimeVal != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.Plant.SomeUTCDateTimeVal >= someMinUTCDateTimeVal);
            if (someMinDateVal != null && someMinDateVal != (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) query = query.Where(x => x.Plant.SomeDateVal >= someMinDateVal);
            if (someFilterMoneyVal != null) query = query.Where(x => x.Plant.SomeMoneyVal == someFilterMoneyVal);
            if (!string.IsNullOrEmpty(someFilterNVarCharVal)) query = query.Where(x => x.Plant.SomeNVarCharVal.Contains(someFilterNVarCharVal));
            if (!string.IsNullOrEmpty(someFilterVarCharVal)) query = query.Where(x => x.Plant.SomeVarCharVal.Contains(someFilterVarCharVal));
            if (!string.IsNullOrEmpty(someFilterTextVal)) query = query.Where(x => x.Plant.SomeTextVal.Contains(someFilterTextVal));
            if (!string.IsNullOrEmpty(someFilterPhoneNumber)) query = query.Where(x => x.Plant.SomePhoneNumber.Contains(someFilterPhoneNumber));
            if (!string.IsNullOrEmpty(someFilterEmailAddress)) query = query.Where(x => x.Plant.SomeEmailAddress.Contains(someFilterEmailAddress));

            return query;
        }


        private static LandPlantListDTO MapLandPlantListDTO(QueryDTO data)
        {
            return new LandPlantListDTO
            {
                PlantCode = data.Plant.Code.HasValue ? data.Plant.Code.Value : Guid.Empty,
                SomeIntVal = data.Plant.SomeIntVal.HasValue ? data.Plant.SomeIntVal.Value : 0,
                SomeBigIntVal = data.Plant.SomeBigIntVal.HasValue ? data.Plant.SomeBigIntVal.Value : 0L,
                SomeBitVal = data.Plant.SomeBitVal.HasValue ? data.Plant.SomeBitVal.Value : false,
                IsEditAllowed = data.Plant.IsEditAllowed.HasValue ? data.Plant.IsEditAllowed.Value : false,
                IsDeleteAllowed = data.Plant.IsDeleteAllowed.HasValue ? data.Plant.IsDeleteAllowed.Value : false,
                SomeFloatVal = data.Plant.SomeFloatVal.HasValue ? (double)data.Plant.SomeFloatVal.Value : 0.0,
                SomeDecimalVal = data.Plant.SomeDecimalVal.HasValue ? data.Plant.SomeDecimalVal.Value : 0m,
                SomeUTCDateTimeVal = data.Plant.SomeUTCDateTimeVal.HasValue ? data.Plant.SomeUTCDateTimeVal.Value : (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeDateVal = data.Plant.SomeDateVal.HasValue ? data.Plant.SomeDateVal.Value : (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SomeMoneyVal = data.Plant.SomeMoneyVal.HasValue ? data.Plant.SomeMoneyVal.Value : 0m,
                SomeNVarCharVal = data.Plant.SomeNVarCharVal != null ? data.Plant.SomeNVarCharVal : String.Empty,
                SomeVarCharVal = data.Plant.SomeVarCharVal != null ? data.Plant.SomeVarCharVal : String.Empty,
                SomeTextVal = data.Plant.SomeTextVal != null ? data.Plant.SomeTextVal : String.Empty,
                SomePhoneNumber = data.Plant.SomePhoneNumber != null ? data.Plant.SomePhoneNumber : String.Empty,
                SomeEmailAddress = data.Plant.SomeEmailAddress != null ? data.Plant.SomeEmailAddress : String.Empty,
                FlavorName = data.Flavor.Name != null ? data.Flavor.Name : String.Empty,
                FlavorCode = data.Flavor.Code.HasValue ? data.Flavor.Code.Value : Guid.Empty,
                SomeIntConditionalOnDeletable = data.Plant.SomeIntVal.HasValue ? data.Plant.SomeIntVal.Value : 0,
                NVarCharAsUrl = data.Plant.SomeNVarCharVal != null ? data.Plant.SomeNVarCharVal : String.Empty,
                UpdateLinkPlantCode = data.Plant.Code.HasValue ? data.Plant.Code.Value : Guid.Empty,
                DeleteAsyncButtonLinkPlantCode = data.Plant.Code.HasValue ? data.Plant.Code.Value : Guid.Empty,
                DetailsLinkPlantCode = data.Plant.Code.HasValue ? data.Plant.Code.Value : Guid.Empty,
                TestFileDownloadLinkPacCode = data.Pac.Code.HasValue ? data.Pac.Code.Value : Guid.Empty,
                TestConditionalFileDownloadLinkPacCode = data.Pac.Code.HasValue ? data.Pac.Code.Value : Guid.Empty,
                TestAsyncFlowReqLinkPacCode = data.Pac.Code.HasValue ? data.Pac.Code.Value : Guid.Empty,
                TestConditionalAsyncFlowReqLinkPacCode = data.Pac.Code.HasValue ? data.Pac.Code.Value : Guid.Empty,
                ConditionalBtnExampleLinkPlantCode = data.Plant.Code.HasValue ? data.Plant.Code.Value : Guid.Empty,
            };
        }


        private class QueryDTO
        {
            public Plant Plant { get; set; }
            public Flavor Flavor { get; set; }
            public Land Land { get; set; }
            public Pac Pac { get; set; }
        }
    }
}

 