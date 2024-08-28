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

        public List<PacUserDateGreaterThanFilterListDTO> Get(

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

            var reports = query.Select(x => MapPacUserDateGreaterThanFilterListDTO(x)).ToList();

            return reports;
        }

        public class PacUserDateGreaterThanFilterListDTO
        {
            private Guid _dateGreaterThanFilterCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Int32 _dateGreaterThanFilterDayCount = 0;
            private String _dateGreaterThanFilterDescription = String.Empty;
            private Int32 _dateGreaterThanFilterDisplayOrder = 0;
            private Boolean _dateGreaterThanFilterIsActive = false;
            private String _dateGreaterThanFilterLookupEnumName = String.Empty;
            private String _dateGreaterThanFilterName = String.Empty;
            public Guid DateGreaterThanFilterCode
            {
                get { return _dateGreaterThanFilterCode; }
                set { _dateGreaterThanFilterCode = value; }
            }
            public Int32 DateGreaterThanFilterDayCount
            {
                get { return _dateGreaterThanFilterDayCount; }
                set { _dateGreaterThanFilterDayCount = value; }
            }
            public String DateGreaterThanFilterDescription
            {
                get { return _dateGreaterThanFilterDescription; }
                set { _dateGreaterThanFilterDescription = value; }
            }
            public Int32 DateGreaterThanFilterDisplayOrder
            {
                get { return _dateGreaterThanFilterDisplayOrder; }
                set { _dateGreaterThanFilterDisplayOrder = value; }
            }
            public Boolean DateGreaterThanFilterIsActive
            {
                get { return _dateGreaterThanFilterIsActive; }
                set { _dateGreaterThanFilterIsActive = value; }
            }
            public String DateGreaterThanFilterLookupEnumName
            {
                get { return _dateGreaterThanFilterLookupEnumName; }
                set { _dateGreaterThanFilterLookupEnumName = value; }
            }
            public String DateGreaterThanFilterName
            {
                get { return _dateGreaterThanFilterName; }
                set { _dateGreaterThanFilterName = value; }
            }

        }
    }
}

