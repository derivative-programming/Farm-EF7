using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class PlantManager
	{

        //GENINCLUDESTART
        //GENLOOPPropStart
        //GENIF[isQueryByAvailable=true]Start
        public async Task<List<Plant>> GetByGENVALPropNameAsync(string GENVALCamelPropName) 
        {  
            var plantsWithCodes = await BuildQuery()
                                    .Where(x => x.PlantObj.GENVALPropName == GENVALCamelPropName)
                                    .ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }
         
         
        public List<Plant> GetByGENVALPropName(string GENVALCamelPropName)
        {
            var plantsWithCodes = BuildQuery()
                                    .Where(x => x.PlantObj.GENVALPropName == GENVALCamelPropName)
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }
        //GENIF[isQueryByAvailable=true]End
        //GENLOOPPropEnd 
        //GENINCLUDEEND


    }
}