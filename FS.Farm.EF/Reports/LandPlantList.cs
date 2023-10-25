using FS.Common.Configuration;
using FS.Common.Objects;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FS.Farm.EF.Reports
{
    public partial class LandPlantList
    {
        private readonly FarmDbContext _dbContext;
        public LandPlantList(FarmDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<int> GetCountAsync( 
           Guid? flavorCode,
           Int32? someIntVal,
           Int64? someBigIntVal,
           Double? someFloatVal,
           Boolean? someBitVal,
           Boolean? isEditAllowed,
           Boolean? isDeleteAllowed,
           Decimal? someDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someMoneyVal,
           String someNVarCharVal,
           String someVarCharVal,
           String someTextVal,
           String somePhoneNumber,
           String someEmailAddress,
           System.Guid userID,
           System.Guid contextCode)
        {  
            var query = BuildQuery(); 

            query = ApplyFilters(query,
                flavorCode,
                someIntVal,
                someBigIntVal,
                someFloatVal,
                someBitVal,
                isEditAllowed,
                isDeleteAllowed,
                someDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someMoneyVal,
                someNVarCharVal,
                someVarCharVal,
                someTextVal,
                somePhoneNumber,
                someEmailAddress,
                userID,
                contextCode);

            return await query.CountAsync();
        }

        public int GetCount(
           Guid? flavorCode,
           Int32? someIntVal,
           Int64? someBigIntVal,
           Double? someFloatVal,
           Boolean? someBitVal,
           Boolean? isEditAllowed,
           Boolean? isDeleteAllowed,
           Decimal? someDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someMoneyVal,
           String someNVarCharVal,
           String someVarCharVal,
           String someTextVal,
           String somePhoneNumber,
           String someEmailAddress,
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorCode,
                someIntVal,
                someBigIntVal,
                someFloatVal,
                someBitVal,
                isEditAllowed,
                isDeleteAllowed,
                someDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someMoneyVal,
                someNVarCharVal,
                someVarCharVal,
                someTextVal,
                somePhoneNumber,
                someEmailAddress,
                userID,
                contextCode);

            return query.Count();
        }

        public async Task<List<LandPlantListDTO>> GetAsync(
           Guid? flavorCode,
           Int32? someIntVal,
           Int64? someBigIntVal,
           Double? someFloatVal,
           Boolean? someBitVal,
           Boolean? isEditAllowed,
           Boolean? isDeleteAllowed,
           Decimal? someDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someMoneyVal,
           String someNVarCharVal,
           String someVarCharVal,
           String someTextVal,
           String somePhoneNumber,
           String someEmailAddress,
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        { 

            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorCode,
                someIntVal,
                someBigIntVal,
                someFloatVal,
                someBitVal,
                isEditAllowed,
                isDeleteAllowed,
                someDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someMoneyVal,
                someNVarCharVal,
                someVarCharVal,
                someTextVal,
                somePhoneNumber,
                someEmailAddress,
                userID,
                contextCode);

            if (!string.IsNullOrEmpty(orderByColumnName))
            {
                if (orderByDescending)
                {
                    query = query.OrderByDescending(p => Microsoft.EntityFrameworkCore.EF.Property<object>(p, orderByColumnName));
                }
                else
                {
                    query = query.OrderBy(p => Microsoft.EntityFrameworkCore.EF.Property<object>(p, orderByColumnName));
                }
            }

            // Applying pagination
            query = query.Skip((pageNumber - 1) * itemCountPerPage).Take(itemCountPerPage);
             
            var reports = await query.Select(x => MapLandPlantListDTO(x)).ToListAsync(); 

            return reports;
        }


        public List<LandPlantListDTO> Get(
           Guid? flavorCode,
           Int32? someIntVal,
           Int64? someBigIntVal,
           Double? someFloatVal,
           Boolean? someBitVal,
           Boolean? isEditAllowed,
           Boolean? isDeleteAllowed,
           Decimal? someDecimalVal,
           DateTime? someMinUTCDateTimeVal,
           DateTime? someMinDateVal,
           Decimal? someMoneyVal,
           String someNVarCharVal,
           String someVarCharVal,
           String someTextVal,
           String somePhoneNumber,
           String someEmailAddress,
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {

            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorCode,
                someIntVal,
                someBigIntVal,
                someFloatVal,
                someBitVal,
                isEditAllowed,
                isDeleteAllowed,
                someDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someMoneyVal,
                someNVarCharVal,
                someVarCharVal,
                someTextVal,
                somePhoneNumber,
                someEmailAddress,
                userID,
                contextCode);

            if (!string.IsNullOrEmpty(orderByColumnName))
            {
                if (orderByDescending)
                {
                    query = query.OrderByDescending(p => Microsoft.EntityFrameworkCore.EF.Property<object>(p, orderByColumnName));
                }
                else
                {
                    query = query.OrderBy(p => Microsoft.EntityFrameworkCore.EF.Property<object>(p, orderByColumnName));
                }
            }

            // Applying pagination
            query = query.Skip((pageNumber - 1) * itemCountPerPage).Take(itemCountPerPage);

            var reports = query.Select(x => MapLandPlantListDTO(x)).ToList();

            return reports;
        }

        public class LandPlantListDTO
        { 
            public Guid PlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Int32 SomeIntVal = 0;
            public Int64 SomeBigIntVal = 0;
            public Boolean SomeBitVal = false;
            public Boolean IsEditAllowed = false;
            public Boolean IsDeleteAllowed = false;
            public Double SomeFloatVal = 0;
            public Decimal SomeDecimalVal = 0;
            public DateTime SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            public DateTime SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            public Decimal SomeMoneyVal = 0;
            public String SomeNVarCharVal = String.Empty;
            public String SomeVarCharVal = String.Empty;
            public String SomeTextVal = String.Empty;
            public String SomePhoneNumber = String.Empty;
            public String SomeEmailAddress = String.Empty;
            public String FlavorName = String.Empty;
            public Guid FlavorCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Int32 SomeIntConditionalOnDeletable = 0;
            public String NVarCharAsUrl = String.Empty;
            public Guid UpdateLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid DeleteAsyncButtonLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid DetailsLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
             
        }
    }
}

 