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
    public partial class PacConfigDynaFlowTaskSearch
    {
        private readonly FarmDbContext _dbContext;
        public PacConfigDynaFlowTaskSearch(FarmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetCountAsync(
           Guid? startedDateGreaterThanFilterCode,
           String processorIdentifier,
           Guid? isStartedTriStateFilterCode,
           Guid? isCompletedTriStateFilterCode,
           Guid? isSuccessfulTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();

            query = ApplyFilters(query,
                startedDateGreaterThanFilterCode,
                processorIdentifier,
                isStartedTriStateFilterCode,
                isCompletedTriStateFilterCode,
                isSuccessfulTriStateFilterCode,
                userID,
                contextCode);

            return await query.CountAsync();
        }

        public int GetCount(
           Guid? startedDateGreaterThanFilterCode,
           String processorIdentifier,
           Guid? isStartedTriStateFilterCode,
           Guid? isCompletedTriStateFilterCode,
           Guid? isSuccessfulTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode)
        {
            var query = BuildQuery();

            query = ApplyFilters(query,
                startedDateGreaterThanFilterCode,
                processorIdentifier,
                isStartedTriStateFilterCode,
                isCompletedTriStateFilterCode,
                isSuccessfulTriStateFilterCode,
                userID,
                contextCode);

            return query.Count();
        }

        public async Task<List<PacConfigDynaFlowTaskSearchDTO>> GetAsync(
           Guid? startedDateGreaterThanFilterCode,
           String processorIdentifier,
           Guid? isStartedTriStateFilterCode,
           Guid? isCompletedTriStateFilterCode,
           Guid? isSuccessfulTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {

            var query = BuildQuery();

            query = ApplyFilters(query,
                startedDateGreaterThanFilterCode,
                processorIdentifier,
                isStartedTriStateFilterCode,
                isCompletedTriStateFilterCode,
                isSuccessfulTriStateFilterCode,
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

            var reports = await query.Select(x => MapPacConfigDynaFlowTaskSearchDTO(x)).ToListAsync();

            return reports;
        }

        public List<PacConfigDynaFlowTaskSearchDTO> Get(
           Guid? startedDateGreaterThanFilterCode,
           String processorIdentifier,
           Guid? isStartedTriStateFilterCode,
           Guid? isCompletedTriStateFilterCode,
           Guid? isSuccessfulTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending)
        {

            var query = BuildQuery();

            query = ApplyFilters(query,
                startedDateGreaterThanFilterCode,
                processorIdentifier,
                isStartedTriStateFilterCode,
                isCompletedTriStateFilterCode,
                isSuccessfulTriStateFilterCode,
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

            var reports = query.Select(x => MapPacConfigDynaFlowTaskSearchDTO(x)).ToList();

            return reports;
        }

        public class PacConfigDynaFlowTaskSearchDTO
        {
            private DateTime _startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            private String _processorIdentifier = String.Empty;
            private Boolean _isStarted = false;
            private Boolean _isCompleted = false;
            private Boolean _isSuccessful = false;
            private Guid _dynaFlowTaskCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public DateTime StartedUTCDateTime
            {
                get { return _startedUTCDateTime; }
                set { _startedUTCDateTime = value; }
            }
            public String ProcessorIdentifier
            {
                get { return _processorIdentifier; }
                set { _processorIdentifier = value; }
            }
            public Boolean IsStarted
            {
                get { return _isStarted; }
                set { _isStarted = value; }
            }
            public Boolean IsCompleted
            {
                get { return _isCompleted; }
                set { _isCompleted = value; }
            }
            public Boolean IsSuccessful
            {
                get { return _isSuccessful; }
                set { _isSuccessful = value; }
            }
            public Guid DynaFlowTaskCode
            {
                get { return _dynaFlowTaskCode; }
                set { _dynaFlowTaskCode = value; }
            }

        }
    }
}

