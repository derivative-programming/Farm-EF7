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
    public partial class PacUserTriStateFilterList
    {
        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()
				from tri_state_filter in _dbContext.TriStateFilterSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj
                select new QueryDTO
                {
                    pac = pac,
				tri_state_filter = tri_state_filter,
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
        private static PacUserTriStateFilterListDTO MapPacUserTriStateFilterListDTO(QueryDTO data)
        {
            return new PacUserTriStateFilterListDTO
            {
					TriStateFilterCode = data.tri_state_filter.Code.Value,
				TriStateFilterDescription = data.tri_state_filter.Description,
					TriStateFilterDisplayOrder = data.tri_state_filter.DisplayOrder.Value,
					TriStateFilterIsActive = data.tri_state_filter.IsActive.Value,
				TriStateFilterLookupEnumName = data.tri_state_filter.LookupEnumName,
				TriStateFilterName = data.tri_state_filter.Name,
					TriStateFilterStateIntValue = data.tri_state_filter.StateIntValue.Value,
            };
        }
        private class QueryDTO
        {
            public Pac pac { get; set; }
            public TriStateFilter tri_state_filter { get; set; }
        }
    }
}
/*
 *
	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)
			TriStateFilter.Code as TriStateFilterCode,
			TriStateFilter.Description as TriStateFilterDescription,
			TriStateFilter.DisplayOrder as TriStateFilterDisplayOrder,
			TriStateFilter.IsActive as TriStateFilterIsActive,
			TriStateFilter.LookupEnumName as TriStateFilterLookupEnumName,
			TriStateFilter.Name as TriStateFilterName,
			TriStateFilter.StateIntValue as TriStateFilterStateIntValue,
			ROW_NUMBER() OVER(
				ORDER BY
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterDescription' THEN TriStateFilter.Description  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterDisplayOrder' THEN TriStateFilter.DisplayOrder  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterIsActive' THEN TriStateFilter.IsActive  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterLookupEnumName' THEN TriStateFilter.LookupEnumName  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterName' THEN TriStateFilter.Name  END ASC,
					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'TriStateFilterStateIntValue' THEN TriStateFilter.StateIntValue  END ASC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterDescription' THEN TriStateFilter.Description  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterDisplayOrder' THEN TriStateFilter.DisplayOrder  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterIsActive' THEN TriStateFilter.IsActive  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterLookupEnumName' THEN TriStateFilter.LookupEnumName  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterName' THEN TriStateFilter.Name  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'TriStateFilterStateIntValue' THEN TriStateFilter.StateIntValue  END DESC,
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC
				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj
			  join vw_FS_Farm_TriStateFilter TriStateFilter on Pac.PacID = TriStateFilter.PacID		 --child obj
		where
			 (Pac.code = @i_uidContextCode
			   )
	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)
		  */
