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
    public partial class PacConfigDynaFlowTaskRetryRunList
    {
        private readonly FarmDbContext _dbContext;
        public PacConfigDynaFlowTaskRetryRunList(FarmDbContext dbContext)
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

        public async Task<List<PacConfigDynaFlowTaskRetryRunListDTO>> GetAsync(

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

            var reports = await query.Select(x => MapPacConfigDynaFlowTaskRetryRunListDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacConfigDynaFlowTaskRetryRunListDTO> Get(

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

            var reports = query.Select(x => MapPacConfigDynaFlowTaskRetryRunListDTO(x)).ToList();

            return reports;
        }

        public class PacConfigDynaFlowTaskRetryRunListDTO
        {
            private Guid _dynaFlowTaskCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid DynaFlowTaskCode
            {
                get { return _dynaFlowTaskCode; }
                set { _dynaFlowTaskCode = value; }
            }

        }
    }
}

