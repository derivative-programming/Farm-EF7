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
    public partial class PacUserDateGreaterThanFilterList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from date_greater_than_filter in _dbContext.DateGreaterThanFilterSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

                select new QueryDTO
                {
                    pac = pac,

				date_greater_than_filter = date_greater_than_filter,

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

        private static PacUserDateGreaterThanFilterListDTO MapPacUserDateGreaterThanFilterListDTO(QueryDTO data)
        {
            return new PacUserDateGreaterThanFilterListDTO
            {

					DateGreaterThanFilterCode = data.date_greater_than_filter.Code.Value,

					DateGreaterThanFilterDayCount = data.date_greater_than_filter.DayCount.Value,

				DateGreaterThanFilterDescription = data.date_greater_than_filter.Description,

					DateGreaterThanFilterDisplayOrder = data.date_greater_than_filter.DisplayOrder.Value,

					DateGreaterThanFilterIsActive = data.date_greater_than_filter.IsActive.Value,

				DateGreaterThanFilterLookupEnumName = data.date_greater_than_filter.LookupEnumName,

				DateGreaterThanFilterName = data.date_greater_than_filter.Name,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public DateGreaterThanFilter date_greater_than_filter { get; set; }

        }
    }
}

/*
 *

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DateGreaterThanFilter.Code as DateGreaterThanFilterCode,

			DateGreaterThanFilter.DayCount as DateGreaterThanFilterDayCount,

			DateGreaterThanFilter.Description as DateGreaterThanFilterDescription,

			DateGreaterThanFilter.DisplayOrder as DateGreaterThanFilterDisplayOrder,

			DateGreaterThanFilter.IsActive as DateGreaterThanFilterIsActive,

			DateGreaterThanFilter.LookupEnumName as DateGreaterThanFilterLookupEnumName,

			DateGreaterThanFilter.Name as DateGreaterThanFilterName,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterDayCount' THEN DateGreaterThanFilter.DayCount  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterDescription' THEN DateGreaterThanFilter.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterDisplayOrder' THEN DateGreaterThanFilter.DisplayOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterIsActive' THEN DateGreaterThanFilter.IsActive  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterLookupEnumName' THEN DateGreaterThanFilter.LookupEnumName  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DateGreaterThanFilterName' THEN DateGreaterThanFilter.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterDayCount' THEN DateGreaterThanFilter.DayCount  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterDescription' THEN DateGreaterThanFilter.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterDisplayOrder' THEN DateGreaterThanFilter.DisplayOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterIsActive' THEN DateGreaterThanFilter.IsActive  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterLookupEnumName' THEN DateGreaterThanFilter.LookupEnumName  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DateGreaterThanFilterName' THEN DateGreaterThanFilter.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_DateGreaterThanFilter DateGreaterThanFilter on Pac.PacID = DateGreaterThanFilter.PacID		 --child obj

		where
			 (Pac.code = @i_uidContextCode
			   )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
