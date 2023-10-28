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
    public partial class PacUserRoleList
    {
        private readonly FarmDbContext _dbContext;
        public PacUserRoleList(FarmDbContext dbContext)
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
        public async Task<List<PacUserRoleListDTO>> GetAsync(
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
            var reports = await query.Select(x => MapPacUserRoleListDTO(x)).ToListAsync();
            return reports;
        }
        public List<PacUserRoleListDTO> Get(
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
            var reports = query.Select(x => MapPacUserRoleListDTO(x)).ToList();
            return reports;
        }
        public class PacUserRoleListDTO
        {
            private Guid _roleCode = Guid.Parse("00000000-0000-0000-0000-000000000000");
            private String _roleDescription = String.Empty;
            private Int32 _roleDisplayOrder = 0;
            private Boolean _roleIsActive = false;
            private String _roleLookupEnumName = String.Empty;
            private String _roleName = String.Empty;
            private String _pacName = String.Empty;
            public Guid RoleCode
            {
                get { return _roleCode; }
                set { _roleCode = value; }
            }
            public String RoleDescription
            {
                get { return _roleDescription; }
                set { _roleDescription = value; }
            }
            public Int32 RoleDisplayOrder
            {
                get { return _roleDisplayOrder; }
                set { _roleDisplayOrder = value; }
            }
            public Boolean RoleIsActive
            {
                get { return _roleIsActive; }
                set { _roleIsActive = value; }
            }
            public String RoleLookupEnumName
            {
                get { return _roleLookupEnumName; }
                set { _roleLookupEnumName = value; }
            }
            public String RoleName
            {
                get { return _roleName; }
                set { _roleName = value; }
            }
            public String PacName
            {
                get { return _pacName; }
                set { _pacName = value; }
            }
        }
    }
}
