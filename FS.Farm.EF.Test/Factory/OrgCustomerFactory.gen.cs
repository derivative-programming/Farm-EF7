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
    public static class OrgCustomerFactory
    {
        private static int _counter = 0;
        public static async Task<OrgCustomer> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context);//CustomerID
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            //ENDSET
            return new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
                //ENDSET
            };
        }
        public static async Task<OrgCustomer> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context);//CustomerID
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            //ENDSET
            OrgCustomer result =  new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
                //ENDSET
            };
            OrgCustomerManager orgCustomerManager = new OrgCustomerManager(context);
            result = await orgCustomerManager.AddAsync(result);
            return result;
        }
    }
}
