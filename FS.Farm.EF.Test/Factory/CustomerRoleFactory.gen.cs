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
    public static class CustomerRoleFactory
    {
        private static int _counter = 0;

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            CustomerRoleManager customerRoleManager = new CustomerRoleManager(context);
            var customerRole = customerRoleManager.GetByCode(code);

            result = CustomerFactory.GetCodeLineage(context, customerRole.CustomerCodePeek); //CustomerID
                                                                                //FlvrForeignKeyID

            result.Add("CustomerRoleCode", customerRole.Code.Value.ToString());

            return result;
        }

        public static async Task<CustomerRole> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context); //CustomerID
            var role = await RoleFactory.CreateAndSaveAsync(context);//RoleID
            return new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
            };
        }

        public static CustomerRole Create(FarmDbContext context)
        {
            _counter++;
            var customer = CustomerFactory.CreateAndSave(context); //CustomerID
            var role = RoleFactory.CreateAndSave(context);//RoleID
            return new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
            };
        }
        public static async Task<CustomerRole> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context); //CustomerID
            var role = await RoleFactory.CreateAndSaveAsync(context);//RoleID
            CustomerRole result =  new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
            };

            CustomerRoleManager customerRoleManager = new CustomerRoleManager(context);
            result = await customerRoleManager.AddAsync(result);
            return result;
        }

        public static CustomerRole CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var customer =   CustomerFactory.CreateAndSave(context); //CustomerID
            var role =   RoleFactory.CreateAndSave(context);//RoleID
            CustomerRole result = new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
            };

            CustomerRoleManager customerRoleManager = new CustomerRoleManager(context);
            result = customerRoleManager.Add(result);
            return result;
        }

    }
}
