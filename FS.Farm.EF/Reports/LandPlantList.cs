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
           System.Guid userID,
           System.Guid contextCode)
        {  
            var query = BuildQuery(); 

            query = ApplyFilters(query,
                flavorFilterCode,
                someFilterIntVal,
                someFilterBigIntVal,
                someFilterFloatVal,
                someFilterBitVal,
                isFilterEditAllowed,
                isFilterDeleteAllowed,
                someFilterDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someFilterMoneyVal,
                someFilterNVarCharVal,
                someFilterVarCharVal,
                someFilterTextVal,
                someFilterPhoneNumber,
                someFilterEmailAddress,
                userID,
                contextCode);

            return await query.CountAsync();
        }

        public int GetCount(
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
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorFilterCode,
                someFilterIntVal,
                someFilterBigIntVal,
                someFilterFloatVal,
                someFilterBitVal,
                isFilterEditAllowed,
                isFilterDeleteAllowed,
                someFilterDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someFilterMoneyVal,
                someFilterNVarCharVal,
                someFilterVarCharVal,
                someFilterTextVal,
                someFilterPhoneNumber,
                someFilterEmailAddress,
                userID,
                contextCode);

            return query.Count();
        }

        public async Task<List<LandPlantListDTO>> GetAsync(
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
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        { 

            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorFilterCode,
                someFilterIntVal,
                someFilterBigIntVal,
                someFilterFloatVal,
                someFilterBitVal,
                isFilterEditAllowed,
                isFilterDeleteAllowed,
                someFilterDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someFilterMoneyVal,
                someFilterNVarCharVal,
                someFilterVarCharVal,
                someFilterTextVal,
                someFilterPhoneNumber,
                someFilterEmailAddress,
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
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {

            var query = BuildQuery();

            query = ApplyFilters(query,
                flavorFilterCode,
                someFilterIntVal,
                someFilterBigIntVal,
                someFilterFloatVal,
                someFilterBitVal,
                isFilterEditAllowed,
                isFilterDeleteAllowed,
                someFilterDecimalVal,
                someMinUTCDateTimeVal,
                someMinDateVal,
                someFilterMoneyVal,
                someFilterNVarCharVal,
                someFilterVarCharVal,
                someFilterTextVal,
                someFilterPhoneNumber,
                someFilterEmailAddress,
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
            private Guid _plantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Int32 _someIntVal = 0;
            private Int32 _someConditionalIntVal = 0;
            private Int64 _someBigIntVal = 0;
            private Int64 _someConditionalBigIntVal = 0;
            private Boolean _someBitVal = false;
            private Boolean _someConditionalBitVal = false;
            private Boolean _isEditAllowed = false;
            private Boolean _isDeleteAllowed = false;
            private Double _someFloatVal = 0;
            private Double _someConditionalFloatVal = 0;
            private Decimal _someDecimalVal = 0;
            private Decimal _someConditionalDecimalVal = 0;
            private DateTime _someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someConditionalUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someConditionalDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private Decimal _someMoneyVal = 0;
            private Decimal _someConditionalMoneyVal = 0;
            private String _someNVarCharVal = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private String _someVarCharVal = String.Empty;
            private String _someConditionalVarCharVal = String.Empty;
            private String _someTextVal = String.Empty;
            private String _someConditionalTextVal = String.Empty;
            private String _somePhoneNumber = String.Empty;
            private String _someConditionalPhoneNumber = String.Empty;
            private String _someEmailAddress = String.Empty;
            private String _someConditionalEmailAddress = String.Empty;
            private String _flavorName = String.Empty;
            private Guid _flavorCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Int32 _someIntConditionalOnDeletable = 0;
            private String _nVarCharAsUrl = String.Empty;
            private String _nVarCharConditionalAsUrl = String.Empty;
            private Guid _updateLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _deleteAsyncButtonLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _detailsLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testFileDownloadLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testConditionalFileDownloadLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testAsyncFlowReqLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testConditionalAsyncFlowReqLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _conditionalBtnExampleLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Boolean _isImageUrlAvailable = false;
            private String _someImageUrlVal = String.Empty;
            private String _someConditionalImageUrlVal = String.Empty;

            public Guid PlantCode
            {
                get { return _plantCode; }
                set { _plantCode = value; }
            }
            public Int32 SomeIntVal
            {
                get { return _someIntVal; }
                set { _someIntVal = value; }
            }
            public Int32 SomeConditionalIntVal
            {
                get { return _someConditionalIntVal; }
                set { _someConditionalIntVal = value; }
            }
            public Int64 SomeBigIntVal
            {
                get { return _someBigIntVal; }
                set { _someBigIntVal = value; }
            }
            public Int64 SomeConditionalBigIntVal
            {
                get { return _someConditionalBigIntVal; }
                set { _someConditionalBigIntVal = value; }
            }
            public Boolean SomeBitVal
            {
                get { return _someBitVal; }
                set { _someBitVal = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public Boolean IsEditAllowed
            {
                get { return _isEditAllowed; }
                set { _isEditAllowed = value; }
            }
            public Boolean IsDeleteAllowed
            {
                get { return _isDeleteAllowed; }
                set { _isDeleteAllowed = value; }
            }
            public Double SomeFloatVal
            {
                get { return _someFloatVal; }
                set { _someFloatVal = value; }
            }
            public Double SomeConditionalFloatVal
            {
                get { return _someConditionalFloatVal; }
                set { _someConditionalFloatVal = value; }
            }
            public Decimal SomeDecimalVal
            {
                get { return _someDecimalVal; }
                set { _someDecimalVal = value; }
            }
            public Decimal SomeConditionalDecimalVal
            {
                get { return _someConditionalDecimalVal; }
                set { _someConditionalDecimalVal = value; }
            }
            public DateTime SomeUTCDateTimeVal
            {
                get { return _someUTCDateTimeVal; }
                set { _someUTCDateTimeVal = value; }
            }
            public DateTime SomeConditionalUTCDateTimeVal
            {
                get { return _someConditionalUTCDateTimeVal; }
                set { _someConditionalUTCDateTimeVal = value; }
            }
            public DateTime SomeDateVal
            {
                get { return _someDateVal; }
                set { _someDateVal = value; }
            }
            public DateTime SomeConditionalDateVal
            {
                get { return _someConditionalDateVal; }
                set { _someConditionalDateVal = value; }
            }
            public Decimal SomeMoneyVal
            {
                get { return _someMoneyVal; }
                set { _someMoneyVal = value; }
            }
            public Decimal SomeConditionalMoneyVal
            {
                get { return _someConditionalMoneyVal; }
                set { _someConditionalMoneyVal = value; }
            }
            public String SomeNVarCharVal
            {
                get { return _someNVarCharVal; }
                set { _someNVarCharVal = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public String SomeVarCharVal
            {
                get { return _someVarCharVal; }
                set { _someVarCharVal = value; }
            }
            public String SomeConditionalVarCharVal
            {
                get { return _someConditionalVarCharVal; }
                set { _someConditionalVarCharVal = value; }
            }
            public String SomeTextVal
            {
                get { return _someTextVal; }
                set { _someTextVal = value; }
            }
            public String SomeConditionalTextVal
            {
                get { return _someConditionalTextVal; }
                set { _someConditionalTextVal = value; }
            }
            public String SomePhoneNumber
            {
                get { return _somePhoneNumber; }
                set { _somePhoneNumber = value; }
            }
            public String SomeConditionalPhoneNumber
            {
                get { return _someConditionalPhoneNumber; }
                set { _someConditionalPhoneNumber = value; }
            }
            public String SomeEmailAddress
            {
                get { return _someEmailAddress; }
                set { _someEmailAddress = value; }
            }
            public String SomeConditionalEmailAddress
            {
                get { return _someConditionalEmailAddress; }
                set { _someConditionalEmailAddress = value; }
            }
            public String FlavorName
            {
                get { return _flavorName; }
                set { _flavorName = value; }
            }
            public Guid FlavorCode
            {
                get { return _flavorCode; }
                set { _flavorCode = value; }
            }
            public Int32 SomeIntConditionalOnDeletable
            {
                get { return _someIntConditionalOnDeletable; }
                set { _someIntConditionalOnDeletable = value; }
            }
            public String NVarCharAsUrl
            {
                get { return _nVarCharAsUrl; }
                set { _nVarCharAsUrl = value; }
            }
            public String NVarCharConditionalAsUrl
            {
                get { return _nVarCharConditionalAsUrl; }
                set { _nVarCharConditionalAsUrl = value; }
            }
            public Guid UpdateLinkPlantCode
            {
                get { return _updateLinkPlantCode; }
                set { _updateLinkPlantCode = value; }
            }
            public Guid DeleteAsyncButtonLinkPlantCode
            {
                get { return _deleteAsyncButtonLinkPlantCode; }
                set { _deleteAsyncButtonLinkPlantCode = value; }
            }
            public Guid DetailsLinkPlantCode
            {
                get { return _detailsLinkPlantCode; }
                set { _detailsLinkPlantCode = value; }
            }
            public Guid TestFileDownloadLinkPacCode
            {
                get { return _testFileDownloadLinkPacCode; }
                set { _testFileDownloadLinkPacCode = value; }
            }
            public Guid TestConditionalFileDownloadLinkPacCode
            {
                get { return _testConditionalFileDownloadLinkPacCode; }
                set { _testConditionalFileDownloadLinkPacCode = value; }
            }
            public Guid TestAsyncFlowReqLinkPacCode
            {
                get { return _testAsyncFlowReqLinkPacCode; }
                set { _testAsyncFlowReqLinkPacCode = value; }
            }
            public Guid TestConditionalAsyncFlowReqLinkPacCode
            {
                get { return _testConditionalAsyncFlowReqLinkPacCode; }
                set { _testConditionalAsyncFlowReqLinkPacCode = value; }
            }
            public Guid ConditionalBtnExampleLinkPlantCode
            {
                get { return _conditionalBtnExampleLinkPlantCode; }
                set { _conditionalBtnExampleLinkPlantCode = value; }
            }
            public Boolean IsImageUrlAvailable
            {
                get { return _isImageUrlAvailable; }
                set { _isImageUrlAvailable = value; }
            }
            public String SomeImageUrlVal
            {
                get { return _someImageUrlVal; }
                set { _someImageUrlVal = value; }
            }
            public String SomeConditionalImageUrlVal
            {
                get { return _someConditionalImageUrlVal; }
                set { _someConditionalImageUrlVal = value; }
            }


        }
    }
}

 