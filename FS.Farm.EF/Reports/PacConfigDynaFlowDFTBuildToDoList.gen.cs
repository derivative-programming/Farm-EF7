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
    public partial class PacConfigDynaFlowDFTBuildToDoList
    {
        private readonly FarmDbContext _dbContext;
        public PacConfigDynaFlowDFTBuildToDoList(FarmDbContext dbContext)
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

        public async Task<List<PacConfigDynaFlowDFTBuildToDoListDTO>> GetAsync(
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

            var reports = await query.Select(x => MapPacConfigDynaFlowDFTBuildToDoListDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacConfigDynaFlowDFTBuildToDoListDTO> Get(
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

            var reports = query.Select(x => MapPacConfigDynaFlowDFTBuildToDoListDTO(x)).ToList();

            return reports;
        }

        public class PacConfigDynaFlowDFTBuildToDoListDTO
        {
            private String _dynaFlowTypeName = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private String _description = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private DateTime _requestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someConditionalUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private Boolean _isBuildTaskDebugRequired = false;
            private Boolean _someConditionalBitVal = false;
            private Boolean _isStarted = false;
            private Boolean _someConditionalBitVal = false;
            private DateTime _startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someConditionalUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private Boolean _isCompleted = false;
            private Boolean _someConditionalBitVal = false;
            private DateTime _completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private DateTime _someConditionalUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private Boolean _isSuccessful = false;
            private Boolean _someConditionalBitVal = false;
            private Guid _dynaFlowCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Boolean _isImageUrlAvailable = false;
            private String _someImageUrlVal = String.Empty;
            private String _someConditionalImageUrlVal = String.Empty;
            public String DynaFlowTypeName
            {
                get { return _dynaFlowTypeName; }
                set { _dynaFlowTypeName = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public String Description
            {
                get { return _description; }
                set { _description = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public DateTime RequestedUTCDateTime
            {
                get { return _requestedUTCDateTime; }
                set { _requestedUTCDateTime = value; }
            }
            public DateTime SomeConditionalUTCDateTimeVal
            {
                get { return _someConditionalUTCDateTimeVal; }
                set { _someConditionalUTCDateTimeVal = value; }
            }
            public Boolean IsBuildTaskDebugRequired
            {
                get { return _isBuildTaskDebugRequired; }
                set { _isBuildTaskDebugRequired = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public Boolean IsStarted
            {
                get { return _isStarted; }
                set { _isStarted = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public DateTime StartedUTCDateTime
            {
                get { return _startedUTCDateTime; }
                set { _startedUTCDateTime = value; }
            }
            public DateTime SomeConditionalUTCDateTimeVal
            {
                get { return _someConditionalUTCDateTimeVal; }
                set { _someConditionalUTCDateTimeVal = value; }
            }
            public Boolean IsCompleted
            {
                get { return _isCompleted; }
                set { _isCompleted = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public DateTime CompletedUTCDateTime
            {
                get { return _completedUTCDateTime; }
                set { _completedUTCDateTime = value; }
            }
            public DateTime SomeConditionalUTCDateTimeVal
            {
                get { return _someConditionalUTCDateTimeVal; }
                set { _someConditionalUTCDateTimeVal = value; }
            }
            public Boolean IsSuccessful
            {
                get { return _isSuccessful; }
                set { _isSuccessful = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public Guid DynaFlowCode
            {
                get { return _dynaFlowCode; }
                set { _dynaFlowCode = value; }
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

