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
    public partial class TacFarmDashboard
    {
        private readonly FarmDbContext _dbContext;
        public TacFarmDashboard(FarmDbContext dbContext)
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

        public async Task<List<TacFarmDashboardDTO>> GetAsync(

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

            var reports = await query.Select(x => MapTacFarmDashboardDTO(x)).ToListAsync();

            return reports;
        }

        public List<TacFarmDashboardDTO> Get(

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

            var reports = query.Select(x => MapTacFarmDashboardDTO(x)).ToList();

            return reports;
        }

        public class TacFarmDashboardDTO
        {
            private Guid _fieldOnePlantListLinkLandCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _conditionalBtnExampleLinkLandCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Boolean _isConditionalBtnAvailable = false;
            private Guid _testFileDownloadLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testConditionalFileDownloadLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testAsyncFlowReqLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private Guid _testConditionalAsyncFlowReqLinkPacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            public Guid FieldOnePlantListLinkLandCode
            {
                get { return _fieldOnePlantListLinkLandCode; }
                set { _fieldOnePlantListLinkLandCode = value; }
            }
            public Guid ConditionalBtnExampleLinkLandCode
            {
                get { return _conditionalBtnExampleLinkLandCode; }
                set { _conditionalBtnExampleLinkLandCode = value; }
            }
            public Boolean IsConditionalBtnAvailable
            {
                get { return _isConditionalBtnAvailable; }
                set { _isConditionalBtnAvailable = value; }
            }
            public Guid TestFileDownloadLinkPacCode
            {
                get { return _testFileDownloadLinkPacCode; }
                set { _testFileDownloadLinkPacCode = value; }
            }
            public Guid TestConditionalFileDownloadLinkPacCode
            {
                get { return _testConditionalFileDownloadLinkPacCode; }
                set { _testConditionalFileDownloadLinkPacCode = value; }
            }
            public Guid TestAsyncFlowReqLinkPacCode
            {
                get { return _testAsyncFlowReqLinkPacCode; }
                set { _testAsyncFlowReqLinkPacCode = value; }
            }
            public Guid TestConditionalAsyncFlowReqLinkPacCode
            {
                get { return _testConditionalAsyncFlowReqLinkPacCode; }
                set { _testConditionalAsyncFlowReqLinkPacCode = value; }
            }

        }
    }
}

