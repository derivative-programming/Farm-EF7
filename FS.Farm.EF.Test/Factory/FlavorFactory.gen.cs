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
    public static class FlavorFactory
    {
        private static int _counter = 0;
        public static async Task<Flavor> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            return new Flavor
            {
                FlavorID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }
        public static Flavor Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID
            return new Flavor
            {
                FlavorID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }
        public static async Task<Flavor> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            Flavor result =  new Flavor
            {
                FlavorID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
            FlavorManager flavorManager = new FlavorManager(context);
            result = await flavorManager.AddAsync(result);
            return result;
        }
        public static Flavor CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID
            Flavor result = new Flavor
            {
                FlavorID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
            FlavorManager flavorManager = new FlavorManager(context);
            result = flavorManager.Add(result);
            return result;
        }
    }
}
