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
    public partial class PacUserFlavorList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserFlavorList(FarmDbContext dbContext)
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
        public async Task<List<PacUserFlavorListDTO>> GetAsync(
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
            var reports = await query.Select(x => MapPacUserFlavorListDTO(x)).ToListAsync();
            return reports;
        }
        public List<PacUserFlavorListDTO> Get(
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
            var reports = query.Select(x => MapPacUserFlavorListDTO(x)).ToList();
            return reports;
        }
        public class PacUserFlavorListDTO
        {
            private Guid _flavorCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _flavorDescription = String.Empty;
            private Int32 _flavorDisplayOrder = 0;
            private Boolean _flavorIsActive = false;
            private String _flavorLookupEnumName = String.Empty;
            private String _flavorName = String.Empty;
            private String _pacName = String.Empty;
            public Guid FlavorCode
            {
                get { return _flavorCode; }
                set { _flavorCode = value; }
            }
            public String FlavorDescription
            {
                get { return _flavorDescription; }
                set { _flavorDescription = value; }
            }
            public Int32 FlavorDisplayOrder
            {
                get { return _flavorDisplayOrder; }
                set { _flavorDisplayOrder = value; }
            }
            public Boolean FlavorIsActive
            {
                get { return _flavorIsActive; }
                set { _flavorIsActive = value; }
            }
            public String FlavorLookupEnumName
            {
                get { return _flavorLookupEnumName; }
                set { _flavorLookupEnumName = value; }
            }
            public String FlavorName
            {
                get { return _flavorName; }
                set { _flavorName = value; }
            }
            public String PacName
            {
                get { return _pacName; }
                set { _pacName = value; }
            }
        }
    }
}
