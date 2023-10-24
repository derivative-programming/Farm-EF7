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
    public static class OrganizationFactory
    {
        private static int _counter = 0;
        public static async Task<Organization> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var tac = await TacFactory.CreateAndSaveAsync(context); //TacID
            //ENDSET
            return new Organization
            {
                OrganizationID = _counter,
                Code = Guid.NewGuid(),
                Name = String.Empty,
                TacID = tac.TacID,
                //ENDSET
            };
        }
        public static async Task<Organization> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var tac = await TacFactory.CreateAndSaveAsync(context); //TacID
            //ENDSET
            Organization result =  new Organization
            {
                OrganizationID = _counter,
                Code = Guid.NewGuid(),
                Name = String.Empty,
                TacID = tac.TacID,
                //ENDSET
            };
            OrganizationManager organizationManager = new OrganizationManager(context);
            result = await organizationManager.AddAsync(result);
            return result;
        }
    }
}
