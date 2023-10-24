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
    public partial class PlantUserDetails
    {
        private IQueryable<QueryDTO> BuildQuery()
        {
            return from plant in _dbContext.PlantSet.AsNoTracking()
				from plantflavor in _dbContext.FlavorSet.AsNoTracking().Where(x => x.FlavorID == plant.FlvrForeignKeyID).DefaultIfEmpty()   // fk lookup prop
				from land  in _dbContext.LandSet.AsNoTracking().Where(x => x.LandID == plant.LandID).DefaultIfEmpty() // up obj tree
				from pac  in _dbContext.PacSet.AsNoTracking().Where(x => x.PacID == land.PacID).DefaultIfEmpty() // up obj tree
                   from tac in _dbContext.TacSet.AsNoTracking().Where(x => x.PacID == pac.PacID).DefaultIfEmpty()  
                   select new QueryDTO
                {
                    plant = plant,
				plantflavor = plantflavor,
				land = land,
				pac = pac,
                       tac = tac,
                   };
        }
        private IQueryable<QueryDTO> ApplyFilters(
            IQueryable<QueryDTO> query,
           System.Guid userID,
           System.Guid contextCode
           )
        {
            if (contextCode != Guid.Empty) query = query.Where(x => x.plant.Code == contextCode);
            return query;
        }
        private static PlantUserDetailsDTO MapPlantUserDetailsDTO(QueryDTO data)
        {
            return new PlantUserDetailsDTO
            {
				FlavorName = data.plantflavor.Name,
					IsDeleteAllowed = data.plant.IsDeleteAllowed.Value,
					IsEditAllowed = data.plant.IsEditAllowed.Value,
				OtherFlavor = data.plant.OtherFlavor,
					SomeBigIntVal = data.plant.SomeBigIntVal.Value,
					SomeBitVal = data.plant.SomeBitVal.Value,
					SomeDateVal = data.plant.SomeDateVal.Value,
					SomeDecimalVal = data.plant.SomeDecimalVal.Value,
				SomeEmailAddress = data.plant.SomeEmailAddress,
					SomeFloatVal = data.plant.SomeFloatVal.Value,
					SomeIntVal = data.plant.SomeIntVal.Value,
					SomeMoneyVal = data.plant.SomeMoneyVal.Value,
				SomeNVarCharVal = data.plant.SomeNVarCharVal,
				SomePhoneNumber = data.plant.SomePhoneNumber,
				SomeTextVal = data.plant.SomeTextVal,
					SomeUniqueidentifierVal = data.plant.SomeUniqueidentifierVal.Value,
					SomeUTCDateTimeVal = data.plant.SomeUTCDateTimeVal.Value,
				SomeVarCharVal = data.plant.SomeVarCharVal,
				PhoneNumConditionalOnIsEditable = data.plant.SomePhoneNumber,
				NVarCharAsUrl = data.plant.SomeNVarCharVal,
					UpdateButtonTextLinkPlantCode = data.plant.Code.Value,
					RandomPropertyUpdatesLinkPlantCode = data.plant.Code.Value,
					BackToDashboardLinkTacCode = data.tac.Code.Value,
            };
        }
        private class QueryDTO
        {
            public Plant plant { get; set; }
            public Flavor plantflavor { get; set; }
            public Land land { get; set; }
            public Pac pac { get; set; }
            public Tac tac { get; set; }
        }
    }
}
/*
 *
	SELECT * FROM
	(
		SELECT  Top  (@i_iPageNumber * @i_iItemCountPerPage)
			PlantFlavor.Name as FlavorName,
			Plant.IsDeleteAllowed as IsDeleteAllowed,
			Plant.IsEditAllowed as IsEditAllowed,
			Plant.OtherFlavor as OtherFlavor,
			Plant.SomeBigIntVal as SomeBigIntVal,
			Plant.SomeBitVal as SomeBitVal,
			Plant.SomeDateVal as SomeDateVal,
			Plant.SomeDecimalVal as SomeDecimalVal,
			Plant.SomeEmailAddress as SomeEmailAddress,
			Plant.SomeFloatVal as SomeFloatVal,
			Plant.SomeIntVal as SomeIntVal,
			Plant.SomeMoneyVal as SomeMoneyVal,
			Plant.SomeNVarCharVal as SomeNVarCharVal,
			Plant.SomePhoneNumber as SomePhoneNumber,
			Plant.SomeTextVal as SomeTextVal,
			Plant.SomeUniqueidentifierVal as SomeUniqueidentifierVal,
			Plant.SomeUTCDateTimeVal as SomeUTCDateTimeVal,
			Plant.SomeVarCharVal as SomeVarCharVal,
			Plant.SomePhoneNumber as PhoneNumConditionalOnIsEditable,
			Plant.SomeNVarCharVal as NVarCharAsUrl,
			Plant.Code as UpdateButtonTextLinkPlantCode,
			Plant.Code as RandomPropertyUpdatesLinkPlantCode,
			Tac.Code as BackToDashboardLinkTacCode,
			ROW_NUMBER() OVER(
				ORDER BY
					CASE WHEN @i_bOrderByDescending = 1 and @i_strOrderByColumnName = 'placeholder' THEN ''  END DESC
				) AS ROWNUMBER
		  -- select *
		from
		 	vw_FS_Farm_Plant  Plant  --owner obj
			left join vw_FS_Farm_Flavor PlantFlavor on Plant.FlvrForeignKeyID = PlantFlavor.FlavorID   -- fk prop
			left join vw_FS_Farm_Land Land on Land.LandID = Plant.LandID  -- up obj tree
			left join vw_FS_Farm_Pac Pac on Pac.PacID = Land.PacID  -- up obj tree
		where
			 (Plant.code = @i_uidContextCode
			   )
	) AS TBL
	WHERE
		ROWNUMBER BETWEEN ((@i_iPageNumber - 1) * @i_iItemCountPerPage + 1) AND (@i_iPageNumber * @i_iItemCountPerPage)
		  */
