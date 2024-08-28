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
                            };
        }

        public static TriStateFilter Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID

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
                            };
        }
        public static async Task<TriStateFilter> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

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
                            };

            TriStateFilterManager triStateFilterManager = new TriStateFilterManager(context);
            result = await triStateFilterManager.AddAsync(result);
            return result;
        }

        public static TriStateFilter CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID

            TriStateFilter result = new TriStateFilter
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
                            };

            TriStateFilterManager triStateFilterManager = new TriStateFilterManager(context);
            result = triStateFilterManager.Add(result);
            return result;
        }

    }
}
