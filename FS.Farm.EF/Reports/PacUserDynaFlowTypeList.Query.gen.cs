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
    public partial class PacUserDynaFlowTypeList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from dyna_flow_type in _dbContext.DynaFlowTypeSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

                select new QueryDTO
                {
                    pac = pac,

				dyna_flow_type = dyna_flow_type,

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

        private static PacUserDynaFlowTypeListDTO MapPacUserDynaFlowTypeListDTO(QueryDTO data)
        {
            return new PacUserDynaFlowTypeListDTO
            {

					DynaFlowTypeCode = data.dyna_flow_type.Code.Value,

				DynaFlowTypeDescription = data.dyna_flow_type.Description,

					DynaFlowTypeDisplayOrder = data.dyna_flow_type.DisplayOrder.Value,

					DynaFlowTypeIsActive = data.dyna_flow_type.IsActive.Value,

				DynaFlowTypeLookupEnumName = data.dyna_flow_type.LookupEnumName,

				DynaFlowTypeName = data.dyna_flow_type.Name,

					DynaFlowTypePriorityLevel = data.dyna_flow_type.PriorityLevel.Value,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public DynaFlowType dyna_flow_type { get; set; }

        }
    }
}

/*
 *

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlowType.Code as DynaFlowTypeCode,

			DynaFlowType.Description as DynaFlowTypeDescription,

			DynaFlowType.DisplayOrder as DynaFlowTypeDisplayOrder,

			DynaFlowType.IsActive as DynaFlowTypeIsActive,

			DynaFlowType.LookupEnumName as DynaFlowTypeLookupEnumName,

			DynaFlowType.Name as DynaFlowTypeName,

			DynaFlowType.PriorityLevel as DynaFlowTypePriorityLevel,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeDescription' THEN DynaFlowType.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeDisplayOrder' THEN DynaFlowType.DisplayOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeIsActive' THEN DynaFlowType.IsActive  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeLookupEnumName' THEN DynaFlowType.LookupEnumName  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeName' THEN DynaFlowType.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypePriorityLevel' THEN DynaFlowType.PriorityLevel  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeDescription' THEN DynaFlowType.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeDisplayOrder' THEN DynaFlowType.DisplayOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeIsActive' THEN DynaFlowType.IsActive  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeLookupEnumName' THEN DynaFlowType.LookupEnumName  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeName' THEN DynaFlowType.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypePriorityLevel' THEN DynaFlowType.PriorityLevel  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_DynaFlowType DynaFlowType on Pac.PacID = DynaFlowType.PacID		 --child obj

		where
			 (Pac.code = @i_uidContextCode
			   )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
