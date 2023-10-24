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
        public static async Task<CustomerRole> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context); //CustomerID
            var role = await RoleFactory.CreateAndSaveAsync(context);//RoleID
            //ENDSET
            return new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
                //ENDSET
            };
        }
        public static async Task<CustomerRole> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var customer = await CustomerFactory.CreateAndSaveAsync(context); //CustomerID
            var role = await RoleFactory.CreateAndSaveAsync(context);//RoleID
            //ENDSET
            CustomerRole result =  new CustomerRole
            {
                CustomerRoleID = _counter,
                Code = Guid.NewGuid(),
                CustomerID = customer.CustomerID,
                IsPlaceholder = false,
                Placeholder = false,
                RoleID = role.RoleID,
                //ENDSET
            };
            CustomerRoleManager customerRoleManager = new CustomerRoleManager(context);
            result = await customerRoleManager.AddAsync(result);
            return result;
        }
    }
}
