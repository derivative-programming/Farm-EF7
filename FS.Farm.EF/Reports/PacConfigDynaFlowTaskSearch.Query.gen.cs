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
    public partial class PacConfigDynaFlowTaskSearch
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
           Guid? startedDateGreaterThanFilterCode,
           String processorIdentifier,
           Guid? isStartedTriStateFilterCode,
           Guid? isCompletedTriStateFilterCode,
           Guid? isSuccessfulTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.pac.Code == contextCode);

            if (startedDateGreaterThanFilterCode != null && startedDateGreaterThanFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow_task.StartedUTCDateTime == startedDateGreaterThanFilterCode);

            if (!String.IsNullOrEmpty(processorIdentifier)) query = query.Where(x => x.dyna_flow_task.ProcessorIdentifier.Contains(processorIdentifier));

            if (isStartedTriStateFilterCode != null && isStartedTriStateFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow_task.IsStarted == isStartedTriStateFilterCode);

            if (isCompletedTriStateFilterCode != null && isCompletedTriStateFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow_task.IsCompleted == isCompletedTriStateFilterCode);

            if (isSuccessfulTriStateFilterCode != null && isSuccessfulTriStateFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow_task.IsSuccessful == isSuccessfulTriStateFilterCode);

            return query;
        }

        private static PacConfigDynaFlowTaskSearchDTO MapPacConfigDynaFlowTaskSearchDTO(QueryDTO data)
        {
            return new PacConfigDynaFlowTaskSearchDTO
            {

					StartedUTCDateTime = data.dyna_flow_task.StartedUTCDateTime.Value,

				ProcessorIdentifier = data.dyna_flow_task.ProcessorIdentifier,

					IsStarted = data.dyna_flow_task.IsStarted.Value,

					IsCompleted = data.dyna_flow_task.IsCompleted.Value,

					IsSuccessful = data.dyna_flow_task.IsSuccessful.Value,

					DynaFlowTaskCode = data.dyna_flow_task.Code.Value,

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

		--DateGreaterThanFilter StartedDateGreaterThanFilterCode
		DECLARE @StartedDateGreaterThanFilterCode_DateGreaterThanFilterIntValue int = -1
		select @StartedDateGreaterThanFilterCode_DateGreaterThanFilterIntValue = DayCount from DateGreaterThanFilter where code = @i_uidStartedDateGreaterThanFilterCode
		DECLARE @StartedDateGreaterThanFilterCode_DateGreaterThanFilterUtcDateTimeValue datetime = getutcdate()
		select @StartedDateGreaterThanFilterCode_DateGreaterThanFilterUtcDateTimeValue = dateadd(d,(-1 * @StartedDateGreaterThanFilterCode_DateGreaterThanFilterIntValue),getutcdate())

		--TriStateFilter IsStartedTriStateFilterCode
		DECLARE @IsStartedTriStateFilterCode_TriStateFilterValue int = -1
		select @IsStartedTriStateFilterCode_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_uidIsStartedTriStateFilterCode

		--TriStateFilter IsCompletedTriStateFilterCode
		DECLARE @IsCompletedTriStateFilterCode_TriStateFilterValue int = -1
		select @IsCompletedTriStateFilterCode_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_uidIsCompletedTriStateFilterCode

		--TriStateFilter IsSuccessfulTriStateFilterCode
		DECLARE @IsSuccessfulTriStateFilterCode_TriStateFilterValue int = -1
		select @IsSuccessfulTriStateFilterCode_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_uidIsSuccessfulTriStateFilterCode

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlowTask.StartedUTCDateTime as StartedUTCDateTime,

			DynaFlowTask.ProcessorIdentifier as ProcessorIdentifier,

			DynaFlowTask.IsStarted as IsStarted,

			DynaFlowTask.IsCompleted as IsCompleted,

			DynaFlowTask.IsSuccessful as IsSuccessful,

			DynaFlowTask.Code as DynaFlowTaskCode,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'StartedUTCDateTime' THEN DynaFlowTask.StartedUTCDateTime  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'ProcessorIdentifier' THEN DynaFlowTask.ProcessorIdentifier  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsStarted' THEN DynaFlowTask.IsStarted  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsCompleted' THEN DynaFlowTask.IsCompleted  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsSuccessful' THEN DynaFlowTask.IsSuccessful  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'StartedUTCDateTime' THEN DynaFlowTask.StartedUTCDateTime  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'ProcessorIdentifier' THEN DynaFlowTask.ProcessorIdentifier  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsStarted' THEN DynaFlowTask.IsStarted  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsCompleted' THEN DynaFlowTask.IsCompleted  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsSuccessful' THEN DynaFlowTask.IsSuccessful  END DESC,

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

				--DateGreaterThanFilter StartedDateGreaterThanFilterCode @StartedDateGreaterThanFilterCode_DateGreaterThanFilterUtcDateTimeValue
			and (@i_uidStartedDateGreaterThanFilterCode is null or @i_uidStartedDateGreaterThanFilterCode = '00000000-0000-0000-0000-000000000000' or @StartedDateGreaterThanFilterCode_DateGreaterThanFilterUtcDateTimeValue < DynaFlowTask.StartedUTCDateTime)

			and (@i_strProcessorIdentifier is null or @i_strProcessorIdentifier = '' or  DynaFlowTask.ProcessorIdentifier like '%' + @i_strProcessorIdentifier + '%')

				--TriStateFilter IsStartedTriStateFilterCode @IsStartedTriStateFilterCode_TriStateFilterValue
			and (@i_uidIsStartedTriStateFilterCode is null or @i_uidIsStartedTriStateFilterCode = '00000000-0000-0000-0000-000000000000' or @IsStartedTriStateFilterCode_TriStateFilterValue = -1 or @IsStartedTriStateFilterCode_TriStateFilterValue = DynaFlowTask.IsStarted)

				--TriStateFilter IsCompletedTriStateFilterCode @IsCompletedTriStateFilterCode_TriStateFilterValue
			and (@i_uidIsCompletedTriStateFilterCode is null or @i_uidIsCompletedTriStateFilterCode = '00000000-0000-0000-0000-000000000000' or @IsCompletedTriStateFilterCode_TriStateFilterValue = -1 or @IsCompletedTriStateFilterCode_TriStateFilterValue = DynaFlowTask.IsCompleted)

				--TriStateFilter IsSuccessfulTriStateFilterCode @IsSuccessfulTriStateFilterCode_TriStateFilterValue
			and (@i_uidIsSuccessfulTriStateFilterCode is null or @i_uidIsSuccessfulTriStateFilterCode = '00000000-0000-0000-0000-000000000000' or @IsSuccessfulTriStateFilterCode_TriStateFilterValue = -1 or @IsSuccessfulTriStateFilterCode_TriStateFilterValue = DynaFlowTask.IsSuccessful)

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
