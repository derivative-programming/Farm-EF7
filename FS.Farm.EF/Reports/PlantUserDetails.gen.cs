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
    public partial class PlantUserDetails
    {
        private readonly FarmDbContext _dbContext;
        public PlantUserDetails(FarmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetCountAsync(
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();
            query = ApplyFilters(query,
                userID,
                contextCode);
            return await query.CountAsync();
        }
        public int GetCount(
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();
            query = ApplyFilters(query,
                userID,
                contextCode);
            return query.Count();
        }
        public async Task<List<PlantUserDetailsDTO>> GetAsync(
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {
            var query = BuildQuery();
            query = ApplyFilters(query,
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
            var reports = await query.Select(x => MapPlantUserDetailsDTO(x)).ToListAsync();
            return reports;
        }
        public List<PlantUserDetailsDTO> Get(
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {
            var query = BuildQuery();
            query = ApplyFilters(query,
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
            var reports = query.Select(x => MapPlantUserDetailsDTO(x)).ToList();
            return reports;
        }
        public class PlantUserDetailsDTO
        {
            private String _flavorName = String.Empty;
            private Boolean _isDeleteAllowed = false;
            private Boolean _isEditAllowed = false;
            private String _otherFlavor = String.Empty;
            private Int64 _someBigIntVal = 0;
            private Boolean _someBitVal = false;
            private DateTime _someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private Decimal _someDecimalVal = 0;
            private String _someEmailAddress = String.Empty;
            private Double _someFloatVal = 0;
            private Int32 _someIntVal = 0;
            private Decimal _someMoneyVal = 0;
            private String _someNVarCharVal = String.Empty;
            private String _somePhoneNumber = String.Empty;
            private String _someTextVal = String.Empty;
            private Guid _someUniqueidentifierVal = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private DateTime _someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private String _someVarCharVal = String.Empty;
            private String _phoneNumConditionalOnIsEditable = String.Empty;
            private String _nVarCharAsUrl = String.Empty;
            private Guid _updateButtonTextLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _randomPropertyUpdatesLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _backToDashboardLinkTacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public String FlavorName
            {
                get { return _flavorName; }
                set { _flavorName = value; }
            }
            public Boolean IsDeleteAllowed
            {
                get { return _isDeleteAllowed; }
                set { _isDeleteAllowed = value; }
            }
            public Boolean IsEditAllowed
            {
                get { return _isEditAllowed; }
                set { _isEditAllowed = value; }
            }
            public String OtherFlavor
            {
                get { return _otherFlavor; }
                set { _otherFlavor = value; }
            }
            public Int64 SomeBigIntVal
            {
                get { return _someBigIntVal; }
                set { _someBigIntVal = value; }
            }
            public Boolean SomeBitVal
            {
                get { return _someBitVal; }
                set { _someBitVal = value; }
            }
            public DateTime SomeDateVal
            {
                get { return _someDateVal; }
                set { _someDateVal = value; }
            }
            public Decimal SomeDecimalVal
            {
                get { return _someDecimalVal; }
                set { _someDecimalVal = value; }
            }
            public String SomeEmailAddress
            {
                get { return _someEmailAddress; }
                set { _someEmailAddress = value; }
            }
            public Double SomeFloatVal
            {
                get { return _someFloatVal; }
                set { _someFloatVal = value; }
            }
            public Int32 SomeIntVal
            {
                get { return _someIntVal; }
                set { _someIntVal = value; }
            }
            public Decimal SomeMoneyVal
            {
                get { return _someMoneyVal; }
                set { _someMoneyVal = value; }
            }
            public String SomeNVarCharVal
            {
                get { return _someNVarCharVal; }
                set { _someNVarCharVal = value; }
            }
            public String SomePhoneNumber
            {
                get { return _somePhoneNumber; }
                set { _somePhoneNumber = value; }
            }
            public String SomeTextVal
            {
                get { return _someTextVal; }
                set { _someTextVal = value; }
            }
            public Guid SomeUniqueidentifierVal
            {
                get { return _someUniqueidentifierVal; }
                set { _someUniqueidentifierVal = value; }
            }
            public DateTime SomeUTCDateTimeVal
            {
                get { return _someUTCDateTimeVal; }
                set { _someUTCDateTimeVal = value; }
            }
            public String SomeVarCharVal
            {
                get { return _someVarCharVal; }
                set { _someVarCharVal = value; }
            }
            public String PhoneNumConditionalOnIsEditable
            {
                get { return _phoneNumConditionalOnIsEditable; }
                set { _phoneNumConditionalOnIsEditable = value; }
            }
            public String NVarCharAsUrl
            {
                get { return _nVarCharAsUrl; }
                set { _nVarCharAsUrl = value; }
            }
            public Guid UpdateButtonTextLinkPlantCode
            {
                get { return _updateButtonTextLinkPlantCode; }
                set { _updateButtonTextLinkPlantCode = value; }
            }
            public Guid RandomPropertyUpdatesLinkPlantCode
            {
                get { return _randomPropertyUpdatesLinkPlantCode; }
                set { _randomPropertyUpdatesLinkPlantCode = value; }
            }
            public Guid BackToDashboardLinkTacCode
            {
                get { return _backToDashboardLinkTacCode; }
                set { _backToDashboardLinkTacCode = value; }
            }
        }
    }
}
