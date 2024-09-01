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
    public static class TacFactory
    {
        private static int _counter = 0;

        public static async Task<Tac> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            return new Tac
            {
                TacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }

        public static Tac Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID
            return new Tac
            {
                TacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }
        public static async Task<Tac> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            Tac result =  new Tac
            {
                TacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };

            TacManager tacManager = new TacManager(context);
            result = await tacManager.AddAsync(result);
            return result;
        }

        public static Tac CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID
            Tac result = new Tac
            {
                TacID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
            };

            TacManager tacManager = new TacManager(context);
            result = tacManager.Add(result);
            return result;
        }

    }
}
