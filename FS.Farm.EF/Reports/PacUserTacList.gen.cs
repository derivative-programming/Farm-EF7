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
    public partial class PacUserTacList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserTacList(FarmDbContext dbContext)
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
        public async Task<List<PacUserTacListDTO>> GetAsync(
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
            var reports = await query.Select(x => MapPacUserTacListDTO(x)).ToListAsync();
            return reports;
        }
        public List<PacUserTacListDTO> Get(
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
            var reports = query.Select(x => MapPacUserTacListDTO(x)).ToList();
            return reports;
        }
        public class PacUserTacListDTO
        {
            private Guid _tacCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _tacDescription = String.Empty;
            private Int32 _tacDisplayOrder = 0;
            private Boolean _tacIsActive = false;
            private String _tacLookupEnumName = String.Empty;
            private String _tacName = String.Empty;
            private String _pacName = String.Empty;
            public Guid TacCode
            {
                get { return _tacCode; }
                set { _tacCode = value; }
            }
            public String TacDescription
            {
                get { return _tacDescription; }
                set { _tacDescription = value; }
            }
            public Int32 TacDisplayOrder
            {
                get { return _tacDisplayOrder; }
                set { _tacDisplayOrder = value; }
            }
            public Boolean TacIsActive
            {
                get { return _tacIsActive; }
                set { _tacIsActive = value; }
            }
            public String TacLookupEnumName
            {
                get { return _tacLookupEnumName; }
                set { _tacLookupEnumName = value; }
            }
            public String TacName
            {
                get { return _tacName; }
                set { _tacName = value; }
            }
            public String PacName
            {
                get { return _pacName; }
                set { _pacName = value; }
            }
        }
    }
}
