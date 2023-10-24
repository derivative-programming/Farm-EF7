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
    public partial class PacUserLandList
    {
        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()
				from land in _dbContext.LandSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj
                select new QueryDTO
                {
                    pac = pac,
				land = land,
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
        private static PacUserLandListDTO MapPacUserLandListDTO(QueryDTO data)
        {
            return new PacUserLandListDTO
            {
					LandCode = data.land.Code.Value,
				LandDescription = data.land.Description,
					LandDisplayOrder = data.land.DisplayOrder.Value,
					LandIsActive = data.land.IsActive.Value,
				LandLookupEnumName = data.land.LookupEnumName,
				LandName = data.land.Name,
				PacName = data.pac.Name,
            };
        }
        private class QueryDTO
        {
            public Pac pac { get; set; }
            public Land land { get; set; }
        }
    }
}
/*
 *
	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)
			Land.Code as LandCode,
			Land.Description as LandDescription,
			Land.DisplayOrder as LandDisplayOrder,
			Land.IsActive as LandIsActive,
			Land.LookupEnumName as LandLookupEnumName,
			Land.Name as LandName,
			Pac.Name as PacName,
			ROW_NUMBER() OVER(
				ORDER BY
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'LandDescription' THEN Land.Description  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'LandDisplayOrder' THEN Land.DisplayOrder  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'LandIsActive' THEN Land.IsActive  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'LandLookupEnumName' THEN Land.LookupEnumName  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'LandName' THEN Land.Name  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END ASC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'LandDescription' THEN Land.Description  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'LandDisplayOrder' THEN Land.DisplayOrder  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'LandIsActive' THEN Land.IsActive  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'LandLookupEnumName' THEN Land.LookupEnumName  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'LandName' THEN Land.Name  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC
				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj
			  join vw_FS_Farm_Land Land on Pac.PacID = Land.PacID		 --child obj
		where
			 (Pac.code = @i_uidContextCode
			   )
	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)
		  */
