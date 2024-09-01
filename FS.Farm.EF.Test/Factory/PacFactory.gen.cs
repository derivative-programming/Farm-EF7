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
    public static class PacFactory
    {
        private static int _counter = 0;

        public static async Task<Pac> CreateAsync(FarmDbContext context)
        {
            _counter++;

            return new Pac
            {
                PacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
            };
        }

        public static Pac Create(FarmDbContext context)
        {
            _counter++;

            return new Pac
            {
                PacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
            };
        }
        public static async Task<Pac> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;

            Pac result =  new Pac
            {
                PacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
            };

            PacManager pacManager = new PacManager(context);
            result = await pacManager.AddAsync(result);
            return result;
        }

        public static Pac CreateAndSave(FarmDbContext context)
        {
            _counter++;

            Pac result = new Pac
            {
                PacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
            };

            PacManager pacManager = new PacManager(context);
            result = pacManager.Add(result);
            return result;
        }

    }
}
