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
    public partial class PacUserLandList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserLandList(FarmDbContext dbContext)
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

        public async Task<List<PacUserLandListDTO>> GetAsync(
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

            var reports = await query.Select(x => MapPacUserLandListDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacUserLandListDTO> Get(
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

            var reports = query.Select(x => MapPacUserLandListDTO(x)).ToList();

            return reports;
        }

        public class PacUserLandListDTO
        {
            private Guid _landCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _landDescription = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private Int32 _landDisplayOrder = 0;
            private Int32 _someConditionalIntVal = 0;
            private Boolean _landIsActive = false;
            private Boolean _someConditionalBitVal = false;
            private String _landLookupEnumName = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private String _landName = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private String _pacName = String.Empty;
            private String _someConditionalNVarCharVal = String.Empty;
            private Boolean _isImageUrlAvailable = false;
            private String _someImageUrlVal = String.Empty;
            private String _someConditionalImageUrlVal = String.Empty;
            public Guid LandCode
            {
                get { return _landCode; }
                set { _landCode = value; }
            }
            public String LandDescription
            {
                get { return _landDescription; }
                set { _landDescription = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public Int32 LandDisplayOrder
            {
                get { return _landDisplayOrder; }
                set { _landDisplayOrder = value; }
            }
            public Int32 SomeConditionalIntVal
            {
                get { return _someConditionalIntVal; }
                set { _someConditionalIntVal = value; }
            }
            public Boolean LandIsActive
            {
                get { return _landIsActive; }
                set { _landIsActive = value; }
            }
            public Boolean SomeConditionalBitVal
            {
                get { return _someConditionalBitVal; }
                set { _someConditionalBitVal = value; }
            }
            public String LandLookupEnumName
            {
                get { return _landLookupEnumName; }
                set { _landLookupEnumName = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public String LandName
            {
                get { return _landName; }
                set { _landName = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
            }
            public String PacName
            {
                get { return _pacName; }
                set { _pacName = value; }
            }
            public String SomeConditionalNVarCharVal
            {
                get { return _someConditionalNVarCharVal; }
                set { _someConditionalNVarCharVal = value; }
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

