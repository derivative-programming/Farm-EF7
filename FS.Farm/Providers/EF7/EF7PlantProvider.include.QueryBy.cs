using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace FS.Farm.Providers.EF7
{
    partial class EF7PlantProvider : FS.Farm.Providers.PlantProvider
    {

        //GENINCLUDESTART
        //GENLOOPPropStart
        //GENIF[isQueryByAvailable=true]Start

        public override async Task<IDataReader> GetPlantList_QueryByGENVALPropNameAsync(
            string GENVALCamelPropName,
           SessionContext context
            )
        {
            var procedureName = "GetPlantList_QueryByGENVALPropNameAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(await plantManager.GetByGENVALPropNameAsync(GENVALCamelPropName));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                var sException = "Error Executing FS_Farm_Plant_QueryByGENVALPropName: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }



        public override IDataReader GetPlantList_QueryByGENVALPropName(
            string GENVALCamelPropName,
           SessionContext context
            )
        {
            var procedureName = "GetPlantList_QueryByGENVALPropName";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(plantManager.GetByGENVALPropName(GENVALCamelPropName));

            }
            catch (Exception x)
            {
                Log(x);
                var sException = "Error Executing FS_Farm_Plant_QueryByGENVALPropName: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        //GENIF[isQueryByAvailable=true]End
        //GENLOOPPropEnd 
        //GENINCLUDEEND


    }
}