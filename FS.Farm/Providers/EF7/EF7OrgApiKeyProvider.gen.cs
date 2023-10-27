using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using FS.Common.Objects;
using System.Threading.Tasks;
using FS.Farm.EF;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using FS.Farm.EF.Models;
using NetTopologySuite.Index.HPRtree;
using FS.Farm.Objects;
namespace FS.Farm.Providers.EF7
{
    partial class EF7OrgApiKeyProvider : FS.Farm.Providers.OrgApiKeyProvider
    {
        string _connectionString = "";
        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = configValue["connectionString"].ToString();
        }
        public override string Name
        {
            get
            {
                return null;
            }
        }
        #endregion
        #region OrgApiKey Methods
        public override int OrgApiKeyGetCount(
            SessionContext context )
        {
            string procedureName = "OrgApiKeyGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                iOut = orgApiKeyManager.GetTotalCount();
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> OrgApiKeyGetCountAsync(
            SessionContext context )
        {
            string procedureName = "OrgApiKeyGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                iOut = await orgApiKeyManager.GetTotalCountAsync();
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context,  x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int OrgApiKeyGetMaxID(
            SessionContext context)
        {
            string procedureName = "OrgApiKeyGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                iOut = orgApiKeyManager.GetMaxId().Value;
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> OrgApiKeyGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "OrgApiKeyGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                var maxId = await orgApiKeyManager.GetMaxIdAsync();
                iOut = maxId.Value;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int OrgApiKeyInsert(
            SessionContext context,
            String apiKeyValue,
            String createdBy,
            DateTime createdUTCDateTime,
            DateTime expirationUTCDateTime,
            Boolean isActive,
            Boolean isTempUserKey,
            String name,
            Int32 organizationID,
            Int32 orgCustomerID,
            System.Guid code)
        {
            string procedureName = "OrgApiKeyInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String apiKeyValue,
            //String createdBy,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(expirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 expirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Boolean isActive,
            //Boolean isTempUserKey,
            //String name,
            //Int32 organizationID,
            //Int32 orgCustomerID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                orgApiKey.Code = code;
                orgApiKey.LastChangeCode = Guid.NewGuid();
                orgApiKey.ApiKeyValue = apiKeyValue;
                orgApiKey.CreatedBy = createdBy;
                orgApiKey.CreatedUTCDateTime = createdUTCDateTime;
                orgApiKey.ExpirationUTCDateTime = expirationUTCDateTime;
                orgApiKey.IsActive = isActive;
                orgApiKey.IsTempUserKey = isTempUserKey;
                orgApiKey.Name = name;
                orgApiKey.OrganizationID = organizationID;
                orgApiKey.OrgCustomerID = orgCustomerID;
                orgApiKey = orgApiKeyManager.Add(orgApiKey);
                iOut = orgApiKey.OrgApiKeyID;
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> OrgApiKeyInsertAsync(
            SessionContext context,
            String apiKeyValue,
            String createdBy,
            DateTime createdUTCDateTime,
            DateTime expirationUTCDateTime,
            Boolean isActive,
            Boolean isTempUserKey,
            String name,
            Int32 organizationID,
            Int32 orgCustomerID,
            System.Guid code)
        {
            string procedureName = "OrgApiKeyInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String apiKeyValue,
            //String createdBy,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(expirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 expirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Boolean isActive,
            //Boolean isTempUserKey,
            //String name,
            //Int32 organizationID,
            //Int32 orgCustomerID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                orgApiKey.Code = code;
                orgApiKey.LastChangeCode = Guid.NewGuid();
                orgApiKey.ApiKeyValue = apiKeyValue;
                orgApiKey.CreatedBy = createdBy;
                orgApiKey.CreatedUTCDateTime = createdUTCDateTime;
                orgApiKey.ExpirationUTCDateTime = expirationUTCDateTime;
                orgApiKey.IsActive = isActive;
                orgApiKey.IsTempUserKey = isTempUserKey;
                orgApiKey.Name = name;
                orgApiKey.OrganizationID = organizationID;
                orgApiKey.OrgCustomerID = orgCustomerID;
                orgApiKey = await orgApiKeyManager.AddAsync(orgApiKey);
                iOut = orgApiKey.OrgApiKeyID;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override void OrgApiKeyUpdate(
            SessionContext context,
            int orgApiKeyID,
            String apiKeyValue,
            String createdBy,
            DateTime createdUTCDateTime,
            DateTime expirationUTCDateTime,
            Boolean isActive,
            Boolean isTempUserKey,
            String name,
            Int32 organizationID,
            Int32 orgCustomerID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "OrgApiKeyUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String apiKeyValue,
            //String createdBy,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(expirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 expirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Boolean isActive,
            //Boolean isTempUserKey,
            //String name,
            //Int32 organizationID,
            //Int32 orgCustomerID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                orgApiKey.Code = code;
                orgApiKey.ApiKeyValue = apiKeyValue;
                orgApiKey.CreatedBy = createdBy;
                orgApiKey.CreatedUTCDateTime = createdUTCDateTime;
                orgApiKey.ExpirationUTCDateTime = expirationUTCDateTime;
                orgApiKey.IsActive = isActive;
                orgApiKey.IsTempUserKey = isTempUserKey;
                orgApiKey.Name = name;
                orgApiKey.OrganizationID = organizationID;
                orgApiKey.OrgCustomerID = orgCustomerID;
                bool success = orgApiKeyManager.Update(orgApiKey);
                if (!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task OrgApiKeyUpdateAsync(
            SessionContext context,
            int orgApiKeyID,
            String apiKeyValue,
            String createdBy,
            DateTime createdUTCDateTime,
            DateTime expirationUTCDateTime,
            Boolean isActive,
            Boolean isTempUserKey,
            String name,
            Int32 organizationID,
            Int32 orgCustomerID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "OrgApiKeyUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String apiKeyValue,
            //String createdBy,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(expirationUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 expirationUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Boolean isActive,
            //Boolean isTempUserKey,
            //String name,
            //Int32 organizationID,
            //Int32 orgCustomerID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                orgApiKey.Code = code;
                orgApiKey.ApiKeyValue = apiKeyValue;
                orgApiKey.CreatedBy = createdBy;
                orgApiKey.CreatedUTCDateTime = createdUTCDateTime;
                orgApiKey.ExpirationUTCDateTime = expirationUTCDateTime;
                orgApiKey.IsActive = isActive;
                orgApiKey.IsTempUserKey = isTempUserKey;
                orgApiKey.Name = name;
                orgApiKey.OrganizationID = organizationID;
                orgApiKey.OrgCustomerID = orgCustomerID;
                bool success = await orgApiKeyManager.UpdateAsync(orgApiKey);
                if(!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override IDataReader SearchOrgApiKeys(
            SessionContext context,
            bool searchByOrgApiKeyID, int orgApiKeyID,
            bool searchByApiKeyValue, String apiKeyValue,
            bool searchByCreatedBy, String createdBy,
            bool searchByCreatedUTCDateTime, DateTime createdUTCDateTime,
            bool searchByExpirationUTCDateTime, DateTime expirationUTCDateTime,
            bool searchByIsActive, Boolean isActive,
            bool searchByIsTempUserKey, Boolean isTempUserKey,
            bool searchByName, String name,
            bool searchByOrganizationID, Int32 organizationID,
            bool searchByOrgCustomerID, Int32 orgCustomerID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrgApiKeys";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> SearchOrgApiKeysAsync(
                    SessionContext context,
                    bool searchByOrgApiKeyID, int orgApiKeyID,
                    bool searchByApiKeyValue, String apiKeyValue,
                    bool searchByCreatedBy, String createdBy,
                    bool searchByCreatedUTCDateTime, DateTime createdUTCDateTime,
                    bool searchByExpirationUTCDateTime, DateTime expirationUTCDateTime,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByIsTempUserKey, Boolean isTempUserKey,
                    bool searchByName, String name,
                    bool searchByOrganizationID, Int32 organizationID,
                    bool searchByOrgCustomerID, Int32 orgCustomerID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrgApiKeysAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetOrgApiKeyList(
            SessionContext context)
        {
            string procedureName = "GetOrgApiKeyList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(orgApiKeyManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetOrgApiKeyListAsync(
            SessionContext context)
        {
            string procedureName = "GetOrgApiKeyListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(await orgApiKeyManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override Guid GetOrgApiKeyCode(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetOrgApiKeyCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "OrgApiKey::" + orgApiKeyID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    Log(procedureName + "::Get From Cache");
                    Log(procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                var orgApiKey = orgApiKeyManager.GetById(orgApiKeyID);
                result = orgApiKey.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<Guid> GetOrgApiKeyCodeAsync(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetOrgApiKeyCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "OrgApiKey::" + orgApiKeyID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    await LogAsync(context, procedureName + "::Get From Cache");
                    await LogAsync(context, procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                var orgApiKey = await orgApiKeyManager.GetByIdAsync(orgApiKeyID);
                result = orgApiKey.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override IDataReader GetOrgApiKey(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetOrgApiKey";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = orgApiKeyManager.GetById(orgApiKeyID);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetOrgApiKeyAsync(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetOrgApiKeyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = await orgApiKeyManager.GetByIdAsync(orgApiKeyID);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyOrgApiKey(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetDirtyOrgApiKey";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = orgApiKeyManager.DirtyGetById(orgApiKeyID);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyOrgApiKeyAsync(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "GetDirtyOrgApiKeyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = await orgApiKeyManager.DirtyGetByIdAsync(orgApiKeyID);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetOrgApiKey(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrgApiKey";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = orgApiKeyManager.GetByCode(code);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetOrgApiKeyAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrgApiKeyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = await orgApiKeyManager.GetByCodeAsync(code);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyOrgApiKey(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyOrgApiKey";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = orgApiKeyManager.DirtyGetByCode(code);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyOrgApiKeyAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyOrgApiKeyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                var orgApiKey = await orgApiKeyManager.DirtyGetByCodeAsync(code);
                orgApiKeys.Add(orgApiKey);
                rdr = BuildDataReader(orgApiKeys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override int GetOrgApiKeyID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrgApiKeyID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                var orgApiKey = orgApiKeyManager.GetByCode(code);
                result = orgApiKey.OrgApiKeyID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<int> GetOrgApiKeyIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrgApiKeyIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                var orgApiKey = await orgApiKeyManager.GetByCodeAsync(code);
                result = orgApiKey.OrgApiKeyID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override int OrgApiKeyBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.Code = item.Code;
                    orgApiKey.LastChangeCode = Guid.NewGuid();
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKeys.Add(orgApiKey);
                }
                orgApiKeyManager.BulkInsert(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> OrgApiKeyBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.Code = item.Code;
                    orgApiKey.LastChangeCode = Guid.NewGuid();
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKeys.Add(orgApiKey);
                }
                await orgApiKeyManager.BulkInsertAsync(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int OrgApiKeyBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.OrgApiKeyID = item.OrgApiKeyID;
                    orgApiKey.Code = item.Code;
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKey.LastChangeCode = item.LastChangeCode;
                    orgApiKeys.Add(orgApiKey);
                }
                orgApiKeyManager.BulkUpdate(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> OrgApiKeyBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.OrgApiKeyID = item.OrgApiKeyID;
                    orgApiKey.Code = item.Code;
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKey.LastChangeCode = item.LastChangeCode;
                    orgApiKeys.Add(orgApiKey);
                }
                orgApiKeyManager.BulkUpdate(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int OrgApiKeyBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.OrgApiKeyID = item.OrgApiKeyID;
                    orgApiKey.Code = item.Code;
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKey.LastChangeCode = item.LastChangeCode;
                    orgApiKeys.Add(orgApiKey);
                }
                orgApiKeyManager.BulkDelete(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> OrgApiKeyBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList)
        {
            string procedureName = "OrgApiKeyBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                List<EF.Models.OrgApiKey> orgApiKeys = new List<EF.Models.OrgApiKey>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgApiKeyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.OrgApiKey item = dataList[i];
                    EF.Models.OrgApiKey orgApiKey = new EF.Models.OrgApiKey();
                    orgApiKey.OrgApiKeyID = item.OrgApiKeyID;
                    orgApiKey.Code = item.Code;
                    orgApiKey.ApiKeyValue = item.ApiKeyValue;
                    orgApiKey.CreatedBy = item.CreatedBy;
                    orgApiKey.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    orgApiKey.ExpirationUTCDateTime = item.ExpirationUTCDateTime;
                    orgApiKey.IsActive = item.IsActive;
                    orgApiKey.IsTempUserKey = item.IsTempUserKey;
                    orgApiKey.Name = item.Name;
                    orgApiKey.OrganizationID = item.OrganizationID;
                    orgApiKey.OrgCustomerID = item.OrgCustomerID;
                    orgApiKey.LastChangeCode = item.LastChangeCode;
                    orgApiKeys.Add(orgApiKey);
                }
                await orgApiKeyManager.BulkDeleteAsync(orgApiKeys);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgApiKey_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override void OrgApiKeyDelete(
            SessionContext context,
            int orgApiKeyID)
        {
            string procedureName = "OrgApiKeyDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                orgApiKeyManager.Delete(orgApiKeyID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_OrgApiKey_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task OrgApiKeyDeleteAsync(
           SessionContext context,
           int orgApiKeyID)
        {
            string procedureName = "OrgApiKeyDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgApiKeyID::" + orgApiKeyID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                await orgApiKeyManager.DeleteAsync(orgApiKeyID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_OrgApiKey_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void OrgApiKeyCleanupTesting(
            SessionContext context )
        {
            string procedureName = "OrgApiKeyCleanupTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                EF.CurrentRuntime.ClearTestObjects(dbContext);
            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override void OrgApiKeyCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "OrgApiKeyCleanupChildObjectTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                EF.CurrentRuntime.ClearTestChildObjects(dbContext);
            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestChildObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override bool SupportsTransactions()
        {
            return true;
        }
        #endregion
        #region Error Handling
        void HandleError( Exception x, string sprocName)
        {
            Log(x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsync(SessionContext context, Exception x, string sprocName)
        {
            await LogAsync(context, x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        #endregion
        public override IDataReader GetOrgApiKeyList_FetchByOrganizationID(
            int organizationID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgApiKeyList_FetchByOrganizationID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(orgApiKeyManager.GetByOrganizationID(organizationID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_FetchByOrganizationID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetOrgApiKeyList_FetchByOrgCustomerID(
            int orgCustomerID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgApiKeyList_FetchByOrgCustomerID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(orgApiKeyManager.GetByOrgCustomerID(orgCustomerID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgApiKey_FetchByOrgCustomerID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetOrgApiKeyList_FetchByOrganizationIDAsync(
            int organizationID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgApiKeyList_FetchByOrganizationIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(await orgApiKeyManager.GetByOrganizationIDAsync(organizationID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_FetchByOrganizationID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetOrgApiKeyList_FetchByOrgCustomerIDAsync(
            int orgCustomerID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgApiKeyList_FetchByOrgCustomerIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgApiKeyManager = new EF.Managers.OrgApiKeyManager(dbContext);
                rdr = BuildDataReader(await orgApiKeyManager.GetByOrgCustomerIDAsync(orgCustomerID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgApiKey_FetchByOrgCustomerID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        private async Task<EF.FarmDbContext> BuildDbContextAsync(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }
                dbContext = EF.FarmDbContextFactory.Create(connection);
                await dbContext.Database.UseTransactionAsync(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }
            return dbContext;
        }
        private EF.FarmDbContext BuildDbContext(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }
                dbContext = EF.FarmDbContextFactory.Create(connection);
                dbContext.Database.UseTransaction(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }
            return dbContext;
        }
        private IDataReader BuildDataReader(List<EF.Models.OrgApiKey> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.OrgApiKey).GetProperties())
            {
                Type columnType = prop.PropertyType;
                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }
                dataTable.Columns.Add(prop.Name, columnType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.OrgApiKey).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
