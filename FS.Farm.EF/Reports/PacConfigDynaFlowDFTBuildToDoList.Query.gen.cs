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
    public partial class PacConfigDynaFlowDFTBuildToDoList
    {

        private IQueryable<QueryDTO> BuildQuery()
        {
            return from pac in _dbContext.PacSet.AsNoTracking()

				from dyna_flow in _dbContext.DynaFlowSet.AsNoTracking().Where(x => x.PacID == pac.PacID)  //child obj

				from dyna_flowdyna_flow_type in _dbContext.DynaFlowTypeSet.AsNoTracking().Where(x => x.DynaFlowTypeID == dyna_flow.DynaFlowTypeID).DefaultIfEmpty() //child obj lookup

                select new QueryDTO
                {
                    pac = pac,

				dyna_flow = dyna_flow,

				dyna_flowdyna_flow_type = dyna_flowdyna_flow_type,

                };
        }

        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,
           Guid? isBuildTaskDebugRequiredTriStateFilterCode,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.pac.Code == contextCode);

            if (isBuildTaskDebugRequiredTriStateFilterCode != null && isBuildTaskDebugRequiredTriStateFilterCode != Guid.Empty) query = query.Where(x => x.dyna_flow.IsBuildTaskDebugRequired == isBuildTaskDebugRequiredTriStateFilterCode);

            return query;
        }

        private static PacConfigDynaFlowDFTBuildToDoListDTO MapPacConfigDynaFlowDFTBuildToDoListDTO(QueryDTO data)
        {
            return new PacConfigDynaFlowDFTBuildToDoListDTO
            {

				DynaFlowTypeName = data.dyna_flowdyna_flow_type.Name,

				Description = data.dyna_flow.Description,

					RequestedUTCDateTime = data.dyna_flow.RequestedUTCDateTime.Value,

					IsBuildTaskDebugRequired = data.dyna_flow.IsBuildTaskDebugRequired.Value,

					IsStarted = data.dyna_flow.IsStarted.Value,

					StartedUTCDateTime = data.dyna_flow.StartedUTCDateTime.Value,

					IsCompleted = data.dyna_flow.IsCompleted.Value,

					CompletedUTCDateTime = data.dyna_flow.CompletedUTCDateTime.Value,

					IsSuccessful = data.dyna_flow.IsSuccessful.Value,

					DynaFlowCode = data.dyna_flow.Code.Value,

            };
        }

        private class QueryDTO
        {
            public Pac pac { get; set; }

            public DynaFlow dyna_flow { get; set; }

            public DynaFlowType dyna_flowdyna_flow_type { get; set; }

        }
    }
}

/*
 *

		--TriStateFilter IsBuildTaskDebugRequiredTriStateFilterCode
		DECLARE @IsBuildTaskDebugRequiredTriStateFilterCode_TriStateFilterValue int = -1
		select @IsBuildTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = StateIntValue from TriStateFilter where code = @i_uidIsBuildTaskDebugRequiredTriStateFilterCode

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlowDynaFlowType.Name as DynaFlowTypeName,

			DynaFlow.Description as Description,

			DynaFlow.RequestedUTCDateTime as RequestedUTCDateTime,

			DynaFlow.IsBuildTaskDebugRequired as IsBuildTaskDebugRequired,

			DynaFlow.IsStarted as IsStarted,

			DynaFlow.StartedUTCDateTime as StartedUTCDateTime,

			DynaFlow.IsCompleted as IsCompleted,

			DynaFlow.CompletedUTCDateTime as CompletedUTCDateTime,

			DynaFlow.IsSuccessful as IsSuccessful,

			DynaFlow.Code as DynaFlowCode,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'DynaFlowTypeName' THEN DynaFlowDynaFlowType.Name  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'Description' THEN DynaFlow.Description  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'RequestedUTCDateTime' THEN DynaFlow.RequestedUTCDateTime  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsBuildTaskDebugRequired' THEN DynaFlow.IsBuildTaskDebugRequired  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'StartedUTCDateTime' THEN DynaFlow.StartedUTCDateTime  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'CompletedUTCDateTime' THEN DynaFlow.CompletedUTCDateTime  END ASC,

					CASE WHEN @i_bOrderByDescending = 0 and @i_strOrderByColumnName = 'IsSuccessful' THEN DynaFlow.IsSuccessful  END ASC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'DynaFlowTypeName' THEN DynaFlowDynaFlowType.Name  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'Description' THEN DynaFlow.Description  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'RequestedUTCDateTime' THEN DynaFlow.RequestedUTCDateTime  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsBuildTaskDebugRequired' THEN DynaFlow.IsBuildTaskDebugRequired  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'StartedUTCDateTime' THEN DynaFlow.StartedUTCDateTime  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'CompletedUTCDateTime' THEN DynaFlow.CompletedUTCDateTime  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'IsSuccessful' THEN DynaFlow.IsSuccessful  END DESC,

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_DynaFlow DynaFlow on Pac.PacID = DynaFlow.PacID		 --child obj

			left join vw_FS_Farm_DynaFlowType DynaFlowDynaFlowType on DynaFlow.DynaFlowTypeID = DynaFlowDynaFlowType.DynaFlowTypeID --child obj lookup prop

		where
			 (Pac.code = @i_uidContextCode
			   )

				--TriStateFilter IsBuildTaskDebugRequiredTriStateFilterCode @IsBuildTaskDebugRequiredTriStateFilterCode_TriStateFilterValue
			and (@i_uidIsBuildTaskDebugRequiredTriStateFilterCode is null or @i_uidIsBuildTaskDebugRequiredTriStateFilterCode = '00000000-0000-0000-0000-000000000000' or @IsBuildTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = -1 or @IsBuildTaskDebugRequiredTriStateFilterCode_TriStateFilterValue = DynaFlow.IsBuildTaskDebugRequired)

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
