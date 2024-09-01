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

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            ErrorLogManager errorLogManager = new ErrorLogManager(context);
            var errorLog = errorLogManager.GetByCode(code);

            result = PacFactory.GetCodeLineage(context, errorLog.PacCodePeek); //PacID
                                                                                //FlvrForeignKeyID

            result.Add("ErrorLogCode", errorLog.Code.Value.ToString());

            return result;
        }

        public static async Task<ErrorLog> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
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
            };
        }

        public static ErrorLog Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID
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
            };
        }
        public static async Task<ErrorLog> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
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
            };

            ErrorLogManager errorLogManager = new ErrorLogManager(context);
            result = await errorLogManager.AddAsync(result);
            return result;
        }

        public static ErrorLog CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID
            ErrorLog result = new ErrorLog
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
            };

            ErrorLogManager errorLogManager = new ErrorLogManager(context);
            result = errorLogManager.Add(result);
            return result;
        }

    }
}
