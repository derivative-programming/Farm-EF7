using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.EF;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Models;
namespace FS.Farm.EF.Test.Factory
{
    public static class ErrorLogFactory
    {
        private static int _counter = 0;
        public static async Task<ErrorLog> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            return new ErrorLog
            {
                ErrorLogID = _counter,
                Code = Guid.NewGuid(),
                BrowserCode = Guid.NewGuid(),
                ContextCode = Guid.NewGuid(),
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Description = String.Empty,
                IsClientSideError = false,
                IsResolved = false,
                PacID = pac.PacID,
                Url = String.Empty,
                //ENDSET
            };
        }
        public static async Task<ErrorLog> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            ErrorLog result =  new ErrorLog
            {
                ErrorLogID = _counter,
                Code = Guid.NewGuid(),
                BrowserCode = Guid.NewGuid(),
                ContextCode = Guid.NewGuid(),
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Description = String.Empty,
                IsClientSideError = false,
                IsResolved = false,
                PacID = pac.PacID,
                Url = String.Empty,
                //ENDSET
            };
            ErrorLogManager errorLogManager = new ErrorLogManager(context);
            result = await errorLogManager.AddAsync(result);
            return result;
        }
    }
}
