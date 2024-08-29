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
    public partial class PacConfigDynaFlowTaskRunToDoList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from dyna_flow in _dbContext.DynaFlowSet.AsNoTracking().Where(x => x.PacID == pac.PacID).DefaultIfEmpty() // up obj join tree

				from dyna_flowdyna_flow_type in _dbContext.DynaFlowTypeSet.AsNoTracking().Where(x => x.DynaFlowTypeID == dyna_flow.DynaFlowTypeID).DefaultIfEmpty() // join tree hild obj lookup prop

				from dyna_flow_task in _dbContext.DynaFlowTaskSet.AsNoTracking().Where(x => x.DynaFlowID == dyna_flow.DynaFlowID).DefaultIfEmpty() // up obj join tree

				from dyna_flow_taskdyna_flow_task_type in _dbContext.DynaFlowTaskTypeSet.AsNoTracking().Where(x => x.DynaFlowTaskTypeID == dyna_flow_task.DynaFlowTaskTypeID).DefaultIfEmpty() // join tree hild obj lookup prop

                select new QueryDTO
                {
                    pac = pac,

				dyna_flow = dyna_flow,

				dyna_flowdyna_flow_type = dyna_flowdyna_flow_type,

				dyna_flow_task = dyna_flow_task,

				dyna_flow_taskdyna_flow_task_type = dyna_flow_taskdyna_flow_task_type,

                };
        }

        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,
           Guid? isRunTaskDebugRequiredTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.pac.Code == contextCode);

            //if (isRunTaskDebugRequiredTriStateFilterCode != null && isRunTaskDebugRequiredTriStateFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow_task.IsRunTaskDebugRequired == isRunTaskDebugRequiredTriStateFilterCode);

            return query;
        }

        private static PacConfigDynaFlowTaskRunToDoListDTO MapPacConfigDynaFlowTaskRunToDoListDTO(QueryDTO data)
        {
            return new PacConfigDynaFlowTaskRunToDoListDTO
            {

					DynaFlowTaskCode = data.dyna_flow_task.Code.Value,

					IsRunTaskDebugRequired = data.dyna_flow_task.IsRunTaskDebugRequired.Value,

					//RunOrder = data.pac.RunOrder.Value,

					DynaFlowPriorityLevel = data.dyna_flow.PriorityLevel.Value,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public DynaFlow dyna_flow { get; set; }

            public DynaFlowType dyna_flowdyna_flow_type { get; set; }

            public DynaFlowTask dyna_flow_task { get; set; }

            public DynaFlowTaskType dyna_flow_taskdyna_flow_task_type { get; set; }

        }
    }
}

/*
 *

		--TriStateFilter IsRunTaskDebugRequiredTriStateFilterCode
		DECLARE @IsRunTaskDebugRequiredTriStateFilterCode_TriStateFilterValue int = -1
		select @IsRunTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_uidIsRunTaskDebugRequiredTriStateFilterCode

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlowTask.Code as DynaFlowTaskCode,

			DynaFlowTask.IsRunTaskDebugRequired as IsRunTaskDebugRequired,

			Pac.RunOrder as RunOrder,

			DynaFlow.PriorityLevel as DynaFlowPriorityLevel,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsRunTaskDebugRequired' THEN DynaFlowTask.IsRunTaskDebugRequired  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RunOrder' THEN Pac.RunOrder  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowPriorityLevel' THEN DynaFlow.PriorityLevel  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsRunTaskDebugRequired' THEN DynaFlowTask.IsRunTaskDebugRequired  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RunOrder' THEN Pac.RunOrder  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowPriorityLevel' THEN DynaFlow.PriorityLevel  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			--left join vw_FS_Farm_DynaFlowTask DynaFlowTask on 1=1

			left join vw_FS_Farm_DynaFlow DynaFlow on DynaFlow.PacID = Pac.PacID  -- up obj join tree

			left join vw_FS_Farm_DynaFlowType DynaFlowDynaFlowType on DynaFlow.DynaFlowTypeID = DynaFlowDynaFlowType.DynaFlowTypeID -- join tree hild obj lookup prop

			left join vw_FS_Farm_DynaFlowTask DynaFlowTask on DynaFlowTask.DynaFlowID = DynaFlow.DynaFlowID  -- up obj join tree

			left join vw_FS_Farm_DynaFlowTaskType DynaFlowTaskDynaFlowTaskType on DynaFlowTask.DynaFlowTaskTypeID = DynaFlowTaskDynaFlowTaskType.DynaFlowTaskTypeID -- join tree hild obj lookup prop

		where
			 (Pac.code = @i_uidContextCode
			   )

				--TriStateFilter IsRunTaskDebugRequiredTriStateFilterCode @IsRunTaskDebugRequiredTriStateFilterCode_TriStateFilterValue
			and (@i_uidIsRunTaskDebugRequiredTriStateFilterCode is null or @i_uidIsRunTaskDebugRequiredTriStateFilterCode = '00000000-0000-0000-0000-000000000000' or @IsRunTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = -1 or @IsRunTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = DynaFlowTask.IsRunTaskDebugRequired)

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
