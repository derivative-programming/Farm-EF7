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

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            OrgCustomerManager orgCustomerManager = new OrgCustomerManager(context);
            var orgCustomer = orgCustomerManager.GetByCode(code);

            result = OrganizationFactory.GetCodeLineage(context, orgCustomer.OrganizationCodePeek); //OrganizationID
                                                                                //FlvrForeignKeyID

            result.Add("OrgCustomerCode", orgCustomer.Code.Value.ToString());

            return result;
        }

        public static async Task<OrgCustomer> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context);//CustomerID
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            return new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
            };
        }

        public static OrgCustomer Create(FarmDbContext context)
        {
            _counter++;
            var customer = CustomerFactory.CreateAndSave(context);//CustomerID
            var organization = OrganizationFactory.CreateAndSave(context); //OrganizationID
            return new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
            };
        }
        public static async Task<OrgCustomer> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context);//CustomerID
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            OrgCustomer result =  new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
            };

            OrgCustomerManager orgCustomerManager = new OrgCustomerManager(context);
            result = await orgCustomerManager.AddAsync(result);
            return result;
        }

        public static OrgCustomer CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var customer =   CustomerFactory.CreateAndSave(context);//CustomerID
            var organization =   OrganizationFactory.CreateAndSave(context); //OrganizationID
            OrgCustomer result = new OrgCustomer
            {
                OrgCustomerID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                Email = String.Empty,
                OrganizationID = organization.OrganizationID,
            };

            OrgCustomerManager orgCustomerManager = new OrgCustomerManager(context);
            result = orgCustomerManager.Add(result);
            return result;
        }

    }
}
