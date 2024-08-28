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
    public partial class PacConfigDynaFlowRetryTaskBuildList
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

           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.pac.Code == contextCode);

            return query;
        }

        private static PacConfigDynaFlowRetryTaskBuildListDTO MapPacConfigDynaFlowRetryTaskBuildListDTO(QueryDTO data)
        {
            return new PacConfigDynaFlowRetryTaskBuildListDTO
            {

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

	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)

			DynaFlow.Code as DynaFlowCode,

			ROW_NUMBER() OVER(
				ORDER BY

					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC

				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Pac  Pac  --owner obj

			  join vw_FS_Farm_DynaFlow DynaFlow on Pac.PacID = DynaFlow.PacID		 --child obj

			left join vw_FS_Farm_DynaFlowType DynaFlowDynaFlowType on DynaFlow.DynaFlowTypeID = DynaFlowDynaFlowType.DynaFlowTypeID --child obj lookup prop

		where
			 (Pac.code = @i_uidContextCode
			 And DynaFlow.IsStarted = 1 and DynaFlow.IsSuccessful = 0 and DynaFlow.IsCanceled = 0 and DynaFlow.IsCancelRequested = 0 and DynaFlow.IsTasksCreated = 0 and DynaFlow.IsTaskCreationStarted = 1 and DynaFlow.StartedUTCDateTime < dateadd(HOUR,-2,getutcdate())  )

	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)

		  */
