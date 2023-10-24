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
    public static class CustomerFactory
    {
        private static int _counter = 0;
        public static async Task<Customer> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var tac = await TacFactory.CreateAndSaveAsync(context); //TacID
            return new Customer
            {
                CustomerID = _counter,
                Code = Guid.NewGuid(),
                ActiveOrganizationID = 0,
                Email = String.Empty,
                EmailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                FirstName = String.Empty,
                ForgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ForgotPasswordKeyValue = String.Empty,
                FSUserCodeValue = Guid.NewGuid(),
                IsActive = false,
                IsEmailAllowed = false,
                IsEmailConfirmed = false,
                IsEmailMarketingAllowed = false,
                IsLocked = false,
                IsMultipleOrganizationsAllowed = false,
                IsVerboseLoggingForced = false,
                LastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                LastName = String.Empty,
                Password = String.Empty,
                Phone = String.Empty,
                Province = String.Empty,
                RegistrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                TacID = tac.TacID,
                UTCOffsetInMinutes = 0,
                Zip = String.Empty,
            };
        }
        public static Customer Create(FarmDbContext context)
        {
            _counter++;
            var tac = TacFactory.CreateAndSave(context); //TacID
            return new Customer
            {
                CustomerID = _counter,
                Code = Guid.NewGuid(),
                ActiveOrganizationID = 0,
                Email = String.Empty,
                EmailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                FirstName = String.Empty,
                ForgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ForgotPasswordKeyValue = String.Empty,
                FSUserCodeValue = Guid.NewGuid(),
                IsActive = false,
                IsEmailAllowed = false,
                IsEmailConfirmed = false,
                IsEmailMarketingAllowed = false,
                IsLocked = false,
                IsMultipleOrganizationsAllowed = false,
                IsVerboseLoggingForced = false,
                LastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                LastName = String.Empty,
                Password = String.Empty,
                Phone = String.Empty,
                Province = String.Empty,
                RegistrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                TacID = tac.TacID,
                UTCOffsetInMinutes = 0,
                Zip = String.Empty,
            };
        }
        public static async Task<Customer> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var tac = await TacFactory.CreateAndSaveAsync(context); //TacID
            Customer result =  new Customer
            {
                CustomerID = _counter,
                Code = Guid.NewGuid(),
                ActiveOrganizationID = 0,
                Email = String.Empty,
                EmailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                FirstName = String.Empty,
                ForgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ForgotPasswordKeyValue = String.Empty,
                FSUserCodeValue = Guid.NewGuid(),
                IsActive = false,
                IsEmailAllowed = false,
                IsEmailConfirmed = false,
                IsEmailMarketingAllowed = false,
                IsLocked = false,
                IsMultipleOrganizationsAllowed = false,
                IsVerboseLoggingForced = false,
                LastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                LastName = String.Empty,
                Password = String.Empty,
                Phone = String.Empty,
                Province = String.Empty,
                RegistrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                TacID = tac.TacID,
                UTCOffsetInMinutes = 0,
                Zip = String.Empty,
            };
            CustomerManager customerManager = new CustomerManager(context);
            result = await customerManager.AddAsync(result);
            return result;
        }
        public static Customer CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var tac =   TacFactory.CreateAndSave(context); //TacID
            Customer result = new Customer
            {
                CustomerID = _counter,
                Code = Guid.NewGuid(),
                ActiveOrganizationID = 0,
                Email = String.Empty,
                EmailConfirmedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                FirstName = String.Empty,
                ForgotPasswordKeyExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ForgotPasswordKeyValue = String.Empty,
                FSUserCodeValue = Guid.NewGuid(),
                IsActive = false,
                IsEmailAllowed = false,
                IsEmailConfirmed = false,
                IsEmailMarketingAllowed = false,
                IsLocked = false,
                IsMultipleOrganizationsAllowed = false,
                IsVerboseLoggingForced = false,
                LastLoginUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                LastName = String.Empty,
                Password = String.Empty,
                Phone = String.Empty,
                Province = String.Empty,
                RegistrationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                TacID = tac.TacID,
                UTCOffsetInMinutes = 0,
                Zip = String.Empty,
            };
            CustomerManager customerManager = new CustomerManager(context);
            result = customerManager.Add(result);
            return result;
        }
    }
}
