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
    public partial class PacUserDateGreaterThanFilterList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserDateGreaterThanFilterList(FarmDbContext dbContext)
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
        public async Task<List<PacUserDateGreaterThanFilterListDTO>> GetAsync(
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
            var reports = await query.Select(x => MapPacUserDateGreaterThanFilterListDTO(x)).ToListAsync();
            return reports;
        }
        public class PacUserDateGreaterThanFilterListDTO
        {
            public Guid DateGreaterThanFilterCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Int32 DateGreaterThanFilterDayCount = 0;
            public String DateGreaterThanFilterDescription = String.Empty;
            public Int32 DateGreaterThanFilterDisplayOrder = 0;
            public Boolean DateGreaterThanFilterIsActive = false;
            public String DateGreaterThanFilterLookupEnumName = String.Empty;
            public String DateGreaterThanFilterName = String.Empty;
        }
    }
}
