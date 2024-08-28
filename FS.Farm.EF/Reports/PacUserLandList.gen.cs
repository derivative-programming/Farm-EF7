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

        public async Task<List<PacUserLandListDTO>> GetAsync(

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

            var reports = await query.Select(x => MapPacUserLandListDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacUserLandListDTO> Get(

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

            var reports = query.Select(x => MapPacUserLandListDTO(x)).ToList();

            return reports;
        }

        public class PacUserLandListDTO
        {
            private Guid _landCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _landDescription = String.Empty;
            private Int32 _landDisplayOrder = 0;
            private Boolean _landIsActive = false;
            private String _landLookupEnumName = String.Empty;
            private String _landName = String.Empty;
            private String _pacName = String.Empty;
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
            public Int32 LandDisplayOrder
            {
                get { return _landDisplayOrder; }
                set { _landDisplayOrder = value; }
            }
            public Boolean LandIsActive
            {
                get { return _landIsActive; }
                set { _landIsActive = value; }
            }
            public String LandLookupEnumName
            {
                get { return _landLookupEnumName; }
                set { _landLookupEnumName = value; }
            }
            public String LandName
            {
                get { return _landName; }
                set { _landName = value; }
            }
            public String PacName
            {
                get { return _pacName; }
                set { _pacName = value; }
            }

        }
    }
}

