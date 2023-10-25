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
            public String FlavorName = String.Empty;
            public Boolean IsDeleteAllowed = false;
            public Boolean IsEditAllowed = false;
            public String OtherFlavor = String.Empty;
            public Int64 SomeBigIntVal = 0;
            public Boolean SomeBitVal = false;
            public DateTime SomeDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            public Decimal SomeDecimalVal = 0;
            public String SomeEmailAddress = String.Empty;
            public Double SomeFloatVal = 0;
            public Int32 SomeIntVal = 0;
            public Decimal SomeMoneyVal = 0;
            public String SomeNVarCharVal = String.Empty;
            public String SomePhoneNumber = String.Empty;
            public String SomeTextVal = String.Empty;
            public Guid SomeUniqueidentifierVal = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public DateTime SomeUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            public String SomeVarCharVal = String.Empty;
            public String PhoneNumConditionalOnIsEditable = String.Empty;
            public String NVarCharAsUrl = String.Empty;
            public Guid UpdateButtonTextLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid RandomPropertyUpdatesLinkPlantCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid BackToDashboardLinkTacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
        }
    }
}
