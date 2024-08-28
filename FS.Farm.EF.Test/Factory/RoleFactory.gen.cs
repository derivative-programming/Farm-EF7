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
    public static class RoleFactory
    {
        private static int _counter = 0;

        public static async Task<Role> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            return new Role
            {
                RoleID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                            };
        }

        public static Role Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID

            return new Role
            {
                RoleID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                            };
        }
        public static async Task<Role> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            Role result =  new Role
            {
                RoleID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                            };

            RoleManager roleManager = new RoleManager(context);
            result = await roleManager.AddAsync(result);
            return result;
        }

        public static Role CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID

            Role result = new Role
            {
                RoleID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                            };

            RoleManager roleManager = new RoleManager(context);
            result = roleManager.Add(result);
            return result;
        }

    }
}
