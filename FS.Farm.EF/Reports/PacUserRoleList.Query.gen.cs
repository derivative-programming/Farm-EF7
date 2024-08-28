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
    public partial class PacUserRoleList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from role in _dbContext.RoleSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

                select new QueryDTO
                {
                    pac = pac,

				role = role,

                };
        }

        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,

           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.pac.Code == contextCode);

            return query;
        }

        private static PacUserRoleListDTO MapPacUserRoleListDTO(QueryDTO data)
        {
            return new PacUserRoleListDTO
            {

					RoleCode = data.role.Code.Value,

				RoleDescription = data.role.Description,

					RoleDisplayOrder = data.role.DisplayOrder.Value,

					RoleIsActive = data.role.IsActive.Value,

				RoleLookupEnumName = data.role.LookupEnumName,

				RoleName = data.role.Name,

				PacName = data.pac.Name,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public Role role { get; set; }

        }
    }
}

/*
 *

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			Role.Code as RoleCode,

			Role.Description as RoleDescription,

			Role.DisplayOrder as RoleDisplayOrder,

			Role.IsActive as RoleIsActive,

			Role.LookupEnumName as RoleLookupEnumName,

			Role.Name as RoleName,

			Pac.Name as PacName,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RoleDescription' THEN Role.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RoleDisplayOrder' THEN Role.DisplayOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RoleIsActive' THEN Role.IsActive  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RoleLookupEnumName' THEN Role.LookupEnumName  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RoleName' THEN Role.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RoleDescription' THEN Role.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RoleDisplayOrder' THEN Role.DisplayOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RoleIsActive' THEN Role.IsActive  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RoleLookupEnumName' THEN Role.LookupEnumName  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RoleName' THEN Role.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_Role Role on Pac.PacID = Role.PacID		 --child obj

		where
			 (Pac.code = @i_uidContextCode
			   )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
