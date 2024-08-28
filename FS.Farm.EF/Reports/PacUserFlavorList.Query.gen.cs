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
    public partial class PacUserFlavorList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from flavor in _dbContext.FlavorSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

                select new QueryDTO
                {
                    pac = pac,

				flavor = flavor,

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

        private static PacUserFlavorListDTO MapPacUserFlavorListDTO(QueryDTO data)
        {
            return new PacUserFlavorListDTO
            {

					FlavorCode = data.flavor.Code.Value,

				FlavorDescription = data.flavor.Description,

					FlavorDisplayOrder = data.flavor.DisplayOrder.Value,

					FlavorIsActive = data.flavor.IsActive.Value,

				FlavorLookupEnumName = data.flavor.LookupEnumName,

				FlavorName = data.flavor.Name,

				PacName = data.pac.Name,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public Flavor flavor { get; set; }

        }
    }
}

/*
 *

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			Flavor.Code as FlavorCode,

			Flavor.Description as FlavorDescription,

			Flavor.DisplayOrder as FlavorDisplayOrder,

			Flavor.IsActive as FlavorIsActive,

			Flavor.LookupEnumName as FlavorLookupEnumName,

			Flavor.Name as FlavorName,

			Pac.Name as PacName,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'FlavorDescription' THEN Flavor.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'FlavorDisplayOrder' THEN Flavor.DisplayOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'FlavorIsActive' THEN Flavor.IsActive  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'FlavorLookupEnumName' THEN Flavor.LookupEnumName  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'FlavorName' THEN Flavor.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'FlavorDescription' THEN Flavor.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'FlavorDisplayOrder' THEN Flavor.DisplayOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'FlavorIsActive' THEN Flavor.IsActive  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'FlavorLookupEnumName' THEN Flavor.LookupEnumName  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'FlavorName' THEN Flavor.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'PacName' THEN Pac.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_Flavor Flavor on Pac.PacID = Flavor.PacID		 --child obj

		where
			 (Pac.code = @i_uidContextCode
			   )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
