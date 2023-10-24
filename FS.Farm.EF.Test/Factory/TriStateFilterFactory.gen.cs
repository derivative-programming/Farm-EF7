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
    public static class TriStateFilterFactory
    {
        private static int _counter = 0;
        public static async Task<TriStateFilter> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            return new TriStateFilter
            {
                TriStateFilterID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                StateIntValue = 0,
                //ENDSET
            };
        }
        public static async Task<TriStateFilter> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            TriStateFilter result =  new TriStateFilter
            {
                TriStateFilterID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                StateIntValue = 0,
                //ENDSET
            };
            TriStateFilterManager triStateFilterManager = new TriStateFilterManager(context);
            result = await triStateFilterManager.AddAsync(result);
            return result;
        }
    }
}
