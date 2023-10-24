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
    public static class DateGreaterThanFilterFactory
    {
        private static int _counter = 0;
        public static async Task<DateGreaterThanFilter> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            return new DateGreaterThanFilter
            {
                DateGreaterThanFilterID = _counter,
                Code = Guid.NewGuid(),
                DayCount = 0,
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                //ENDSET
            };
        }
        public static async Task<DateGreaterThanFilter> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            //ENDSET
            DateGreaterThanFilter result =  new DateGreaterThanFilter
            {
                DateGreaterThanFilterID = _counter,
                Code = Guid.NewGuid(),
                DayCount = 0,
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                //ENDSET
            };
            DateGreaterThanFilterManager dateGreaterThanFilterManager = new DateGreaterThanFilterManager(context);
            result = await dateGreaterThanFilterManager.AddAsync(result);
            return result;
        }
    }
}
