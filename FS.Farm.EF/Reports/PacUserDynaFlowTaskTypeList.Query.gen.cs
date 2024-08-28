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
    public partial class PacUserDynaFlowTaskTypeList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from dyna_flow_task_type in _dbContext.DynaFlowTaskTypeSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

                select new QueryDTO
                {
                    pac = pac,

				dyna_flow_task_type = dyna_flow_task_type,

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

        private static PacUserDynaFlowTaskTypeListDTO MapPacUserDynaFlowTaskTypeListDTO(QueryDTO data)
        {
            return new PacUserDynaFlowTaskTypeListDTO
            {

					DynaFlowTaskTypeCode = data.dyna_flow_task_type.Code.Value,

				DynaFlowTaskTypeDescription = data.dyna_flow_task_type.Description,

					DynaFlowTaskTypeDisplayOrder = data.dyna_flow_task_type.DisplayOrder.Value,

					DynaFlowTaskTypeIsActive = data.dyna_flow_task_type.IsActive.Value,

				DynaFlowTaskTypeLookupEnumName = data.dyna_flow_task_type.LookupEnumName,

					DynaFlowTaskTypeMaxRetryCount = data.dyna_flow_task_type.MaxRetryCount.Value,

				DynaFlowTaskTypeName = data.dyna_flow_task_type.Name,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public DynaFlowTaskType dyna_flow_task_type { get; set; }

        }
    }
}

/*
 *

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlowTaskType.Code as DynaFlowTaskTypeCode,

			DynaFlowTaskType.Description as DynaFlowTaskTypeDescription,

			DynaFlowTaskType.DisplayOrder as DynaFlowTaskTypeDisplayOrder,

			DynaFlowTaskType.IsActive as DynaFlowTaskTypeIsActive,

			DynaFlowTaskType.LookupEnumName as DynaFlowTaskTypeLookupEnumName,

			DynaFlowTaskType.MaxRetryCount as DynaFlowTaskTypeMaxRetryCount,

			DynaFlowTaskType.Name as DynaFlowTaskTypeName,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeDescription' THEN DynaFlowTaskType.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeDisplayOrder' THEN DynaFlowTaskType.DisplayOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeIsActive' THEN DynaFlowTaskType.IsActive  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeLookupEnumName' THEN DynaFlowTaskType.LookupEnumName  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeMaxRetryCount' THEN DynaFlowTaskType.MaxRetryCount  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTaskTypeName' THEN DynaFlowTaskType.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeDescription' THEN DynaFlowTaskType.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeDisplayOrder' THEN DynaFlowTaskType.DisplayOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeIsActive' THEN DynaFlowTaskType.IsActive  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeLookupEnumName' THEN DynaFlowTaskType.LookupEnumName  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeMaxRetryCount' THEN DynaFlowTaskType.MaxRetryCount  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTaskTypeName' THEN DynaFlowTaskType.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_DynaFlowTaskType DynaFlowTaskType on Pac.PacID = DynaFlowTaskType.PacID		 --child obj

		where
			 (Pac.code = @i_uidContextCode
			   )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
