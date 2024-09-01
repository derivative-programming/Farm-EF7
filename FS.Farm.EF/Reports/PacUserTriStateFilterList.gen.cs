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
    public partial class PacUserTriStateFilterList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserTriStateFilterList(FarmDbContext dbContext)
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

        public async Task<List<PacUserTriStateFilterListDTO>> GetAsync(

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

            var reports = await query.Select(x => MapPacUserTriStateFilterListDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacUserTriStateFilterListDTO> Get(

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

            var reports = query.Select(x => MapPacUserTriStateFilterListDTO(x)).ToList();

            return reports;
        }

        public class PacUserTriStateFilterListDTO
        {
            private Guid _triStateFilterCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _triStateFilterDescription = String.Empty;
            private Int32 _triStateFilterDisplayOrder = 0;
            private Boolean _triStateFilterIsActive = false;
            private String _triStateFilterLookupEnumName = String.Empty;
            private String _triStateFilterName = String.Empty;
            private Int32 _triStateFilterStateIntValue = 0;
            public Guid TriStateFilterCode
            {
                get { return _triStateFilterCode; }
                set { _triStateFilterCode = value; }
            }
            public String TriStateFilterDescription
            {
                get { return _triStateFilterDescription; }
                set { _triStateFilterDescription = value; }
            }
            public Int32 TriStateFilterDisplayOrder
            {
                get { return _triStateFilterDisplayOrder; }
                set { _triStateFilterDisplayOrder = value; }
            }
            public Boolean TriStateFilterIsActive
            {
                get { return _triStateFilterIsActive; }
                set { _triStateFilterIsActive = value; }
            }
            public String TriStateFilterLookupEnumName
            {
                get { return _triStateFilterLookupEnumName; }
                set { _triStateFilterLookupEnumName = value; }
            }
            public String TriStateFilterName
            {
                get { return _triStateFilterName; }
                set { _triStateFilterName = value; }
            }
            public Int32 TriStateFilterStateIntValue
            {
                get { return _triStateFilterStateIntValue; }
                set { _triStateFilterStateIntValue = value; }
            }

        }
    }
}

