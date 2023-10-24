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
    public static class OrgApiKeyFactory
    {
        private static int _counter = 0;
        public static async Task<OrgApiKey> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            var orgCustomer = await OrgCustomerFactory.CreateAndSaveAsync(context);//OrgCustomerID
            return new OrgApiKey
            {
                OrgApiKeyID = _counter,
                Code = Guid.NewGuid(),
                ApiKeyValue = String.Empty,
                CreatedBy = String.Empty,
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                IsActive = false,
                IsTempUserKey = false,
                Name = String.Empty,
                OrganizationID = organization.OrganizationID,
                OrgCustomerID = orgCustomer.OrgCustomerID,
            };
        }
        public static OrgApiKey Create(FarmDbContext context)
        {
            _counter++;
            var organization = OrganizationFactory.CreateAndSave(context); //OrganizationID
            var orgCustomer = OrgCustomerFactory.CreateAndSave(context);//OrgCustomerID
            return new OrgApiKey
            {
                OrgApiKeyID = _counter,
                Code = Guid.NewGuid(),
                ApiKeyValue = String.Empty,
                CreatedBy = String.Empty,
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                IsActive = false,
                IsTempUserKey = false,
                Name = String.Empty,
                OrganizationID = organization.OrganizationID,
                OrgCustomerID = orgCustomer.OrgCustomerID,
            };
        }
        public static async Task<OrgApiKey> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var organization = await OrganizationFactory.CreateAndSaveAsync(context); //OrganizationID
            var orgCustomer = await OrgCustomerFactory.CreateAndSaveAsync(context);//OrgCustomerID
            OrgApiKey result =  new OrgApiKey
            {
                OrgApiKeyID = _counter,
                Code = Guid.NewGuid(),
                ApiKeyValue = String.Empty,
                CreatedBy = String.Empty,
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                IsActive = false,
                IsTempUserKey = false,
                Name = String.Empty,
                OrganizationID = organization.OrganizationID,
                OrgCustomerID = orgCustomer.OrgCustomerID,
            };
            OrgApiKeyManager orgApiKeyManager = new OrgApiKeyManager(context);
            result = await orgApiKeyManager.AddAsync(result);
            return result;
        }
        public static OrgApiKey CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var organization =   OrganizationFactory.CreateAndSave(context); //OrganizationID
            var orgCustomer =   OrgCustomerFactory.CreateAndSave(context);//OrgCustomerID
            OrgApiKey result = new OrgApiKey
            {
                OrgApiKeyID = _counter,
                Code = Guid.NewGuid(),
                ApiKeyValue = String.Empty,
                CreatedBy = String.Empty,
                CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ExpirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                IsActive = false,
                IsTempUserKey = false,
                Name = String.Empty,
                OrganizationID = organization.OrganizationID,
                OrgCustomerID = orgCustomer.OrgCustomerID,
            };
            OrgApiKeyManager orgApiKeyManager = new OrgApiKeyManager(context);
            result = orgApiKeyManager.Add(result);
            return result;
        }
    }
}
