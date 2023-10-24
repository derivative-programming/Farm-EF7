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
    public partial class TacFarmDashboard
    {
        private IQueryable<QueryDTO> BuildQuery()
        {
            return from tac in _dbContext.TacSet.AsNoTracking()
				from pac  in _dbContext.PacSet.AsNoTracking().Where(x => x.PacID == tac.PacID).DefaultIfEmpty() // up obj tree
                   from land in _dbContext.LandSet.AsNoTracking().Where(x => x.PacID == pac.PacID) // up obj tree
                   select new QueryDTO
                {
                    tac = tac,
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
            if (contextCode != Guid.Empty) query = query.Where(x => x.tac.Code == contextCode);
            return query;
        }
        private static TacFarmDashboardDTO MapTacFarmDashboardDTO(QueryDTO data)
        {
            return new TacFarmDashboardDTO
            {
					FieldOnePlantListLinkLandCode = data.land.Code.Value,
					ConditionalBtnExampleLinkLandCode = data.land.Code.Value,
                    IsConditionalBtnAvailable = false, 
            };
        }
        private class QueryDTO
        {
            public Tac tac { get; set; }
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
			Land.Code as FieldOnePlantListLinkLandCode,
			Land.Code as ConditionalBtnExampleLinkLandCode,
			cast((case when 1=0 then 1 else 0 end) as bit) as IsConditionalBtnAvailable,
			ROW_NUMBER() OVER(
				ORDER BY
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC
				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Tac  Tac  --owner obj
			left join vw_FS_Farm_Pac Pac on Pac.PacID = Tac.PacID  -- up obj tree
		where
			 (Tac.code = @i_uidContextCode
			   )
	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)
		  */
