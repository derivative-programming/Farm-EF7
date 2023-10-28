using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace FS.Farm.Providers
{
    internal abstract partial class PlantProvider : System.Configuration.Provider.ProviderBase
    {

        //GENINCLUDESTART
        //GENLOOPPropStart
        //GENIF[isQueryByAvailable=true]Start 
        public abstract IDataReader GetPlantList_QueryByGENVALPropName(
            string GENVALCamelPropName,
           SessionContext context);

        public abstract Task<IDataReader> GetPlantList_QueryByGENVALPropNameAsync(
            string GENVALCamelPropName,
           SessionContext context);
        //GENIF[isQueryByAvailable=true]End
        //GENLOOPPropEnd 
        //GENINCLUDEEND


    }
}