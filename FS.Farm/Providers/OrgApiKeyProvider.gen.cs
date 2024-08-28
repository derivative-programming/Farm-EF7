using System;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Configuration.Provider;
using System.Data;
using System.Configuration;
using FS.Common.Objects;
using System.Threading.Tasks;
using FS.Base.Providers;
using FS.Common.Diagnostics.Loggers;
namespace FS.Farm.Providers
{
    internal abstract partial class OrgApiKeyProvider : System.Configuration.Provider.ProviderBase
    {
        static OrgApiKeyProvider _instance;
        static object padLock = new object();
        private static string _providerName = string.Empty;
        private static bool _cacheAll = false;
        private static bool _cacheIndividual = false;
        private static int _cacheLifetimeInMinutes = 60;
        private static int _maxCacheCount = 100;
        private static bool _lazyInsert = false;
        private static bool _lazyUpdate = false;
        private static bool _lazyDelete = false;
        private static bool _raiseEventOnInsert = false;
        private static bool _raiseEventOnUpdate = false;
        private static bool _raiseEventOnDelete = false;
        private static bool _isMirrorActive = false;
        private static void VerifyProviderIsLoaded()
        {
            string testAccess = Instance.Description;
        }
        public static void RefreshDataProviderInstance()
        {
            OrgApiKeyProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrgApiKeyProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static OrgApiKeyProvider Instance
        {
            get
            {
                OrgApiKeyProvider tempInstance;
                if (_instance == null)
                {
                    lock (padLock)
                    {
                        tempInstance = LoadProvider();
                        _instance = tempInstance;
                    }
                }
                return _instance;
            }
        }
        public static string ProviderName
        {
            get
            {
                VerifyProviderIsLoaded();
                return _providerName;
            }
        }
        public static bool CacheAll
        {
            get
            {
                VerifyProviderIsLoaded();
                return _cacheAll;
            }
        }
        public static bool CacheIndividual
        {
            get
            {
                VerifyProviderIsLoaded();
                return _cacheIndividual;
            }
        }
        public static int CacheLifetimeInMinutes
        {
            get
            {
                VerifyProviderIsLoaded();
                return _cacheLifetimeInMinutes;
            }
        }
        public static int MaxCacheCount
        {
            get
            {
                VerifyProviderIsLoaded();
                return _maxCacheCount;
            }
        }
        public static bool LazyInsert
        {
            get
            {
                VerifyProviderIsLoaded();
                return _lazyInsert;
            }
        }
        public static bool LazyUpdate
        {
            get
            {
                VerifyProviderIsLoaded();
                return _lazyUpdate;
            }
        }
        public static bool LazyDelete
        {
            get
            {
                VerifyProviderIsLoaded();
                return _lazyDelete;
            }
        }
        public static bool RaiseEventOnInsert
        {
            get
            {
                VerifyProviderIsLoaded();
                return _raiseEventOnInsert;
            }
        }
        public static bool RaiseEventOnUpdate
        {
            get
            {
                VerifyProviderIsLoaded();
                return _raiseEventOnUpdate;
            }
        }
        public static bool RaiseEventOnDelete
        {
            get
            {
                VerifyProviderIsLoaded();
                return _raiseEventOnDelete;
            }
        }
        public static bool IsMirrorActive
        {
            get
            {
                VerifyProviderIsLoaded();
                return _isMirrorActive;
            }
        }
        static OrgApiKeyProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrgApiKeyProviderConfiguration config = OrgApiKeyProviderConfiguration.GetConfig();
            string providerName = config.DefaultProvider;
            switch (dataProviderType)
            {
                case DataProviderType.File:
                    providerName = "File";
                    break;
                case DataProviderType.Firebase:
                    providerName = "Firebase";
                    break;
                case DataProviderType.MongoDB:
                    providerName = "Mongo";
                    break;
                case DataProviderType.MySql:
                    providerName = "MySql";
                    break;
                case DataProviderType.Postgres:
                    providerName = "Pg";
                    break;
                case DataProviderType.Redis:
                    providerName = "Redis";
                    break;
                case DataProviderType.SqlServer:
                    providerName = "Sql";
                    break;
                default:
                    break;
            }
            return LoadProvider(config, providerName);
        }
        static OrgApiKeyProvider LoadProvider()
        {
            OrgApiKeyProviderConfiguration config = OrgApiKeyProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static OrgApiKeyProvider LoadProvider(OrgApiKeyProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            OrgApiKeyProvider _instanceLoader;
            cacheKey = "FS.Farm.OrgApiKeyProvider::" + providerName;
            _providerName = cacheKey;
            _cacheAll = config.CacheAll;
            _cacheIndividual = config.CacheIndividual;
            _cacheLifetimeInMinutes = config.CacheLifetimeInMinutes;
            _maxCacheCount = config.MaxCacheCount;
            _lazyInsert = config.LazyInsert;
            _lazyUpdate = config.LazyUpdate;
            _lazyDelete = config.LazyDelete;
            _raiseEventOnInsert = config.RaiseEventOnInsert;
            _raiseEventOnUpdate = config.RaiseEventOnUpdate;
            _raiseEventOnDelete = config.RaiseEventOnDelete;
            _isMirrorActive = config.IsMirrorActive;
            object oProvider = AppDomain.CurrentDomain.GetData(cacheKey);
            if (oProvider != null)
            {
                _instanceLoader = (OrgApiKeyProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider OrgApiKeyProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(OrgApiKeyProvider.Type);
                    _instanceLoader = (OrgApiKeyProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = OrgApiKeyProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //OrgApiKeyProvider.Attributes.Add("connectionString", cString);
                    if (OrgApiKeyProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        OrgApiKeyProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        OrgApiKeyProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(OrgApiKeyProvider.Name, OrgApiKeyProvider.Attributes);
                    //pop it into the cache to keep out site from running into the ground :)
                    AppDomain.CurrentDomain.SetData(cacheKey, _instanceLoader);
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to load provider", e);
                }
            }
            return _instanceLoader;
        }
        protected void Log(System.Exception ex)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ex);
        }
        protected async Task LogAsync(SessionContext sessionContext, System.Exception ex)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ex);
        }
        protected void Log(string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ApplicationLogEntrySeverities.Information_HighDetail, _providerName + "::" + message);
        }
        protected async Task LogAsync(SessionContext sessionContext, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ApplicationLogEntrySeverities.Information_HighDetail, _providerName + "::" + message);
        }
        protected void Log(ApplicationLogEntrySeverities severity, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(severity, _providerName + "::" + message);
        }
        protected async Task LogAsync(SessionContext sessionContext, ApplicationLogEntrySeverities severity, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, severity, _providerName + "::" + message);
        }
        #region OrgApiKeys
        public abstract IDataReader GetOrgApiKeyList(SessionContext context);
        public abstract Task<IDataReader> GetOrgApiKeyListAsync(SessionContext context);
        public abstract Guid GetOrgApiKeyCode(SessionContext context, int orgApiKeyID);
        public abstract Task<Guid> GetOrgApiKeyCodeAsync(SessionContext context, int orgApiKeyID);
        public abstract int GetOrgApiKeyID(SessionContext context, System.Guid code);
        public abstract Task<int> GetOrgApiKeyIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetOrgApiKey(SessionContext context, int orgApiKeyID);
        public abstract Task<IDataReader> GetOrgApiKeyAsync(SessionContext context, int orgApiKeyID);
        public abstract IDataReader GetDirtyOrgApiKey(SessionContext context, int orgApiKeyID);
        public abstract Task<IDataReader> GetDirtyOrgApiKeyAsync(SessionContext context, int orgApiKeyID);
        public abstract IDataReader GetOrgApiKey(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetOrgApiKeyAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyOrgApiKey(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyOrgApiKeyAsync(SessionContext context, System.Guid code);
        public abstract void OrgApiKeyDelete(SessionContext context, int orgApiKeyID);
        public abstract Task OrgApiKeyDeleteAsync(SessionContext context, int orgApiKeyID);
        public abstract void OrgApiKeyCleanupTesting(SessionContext context);
        public abstract void OrgApiKeyCleanupChildObjectTesting(SessionContext context);
        public abstract int OrgApiKeyGetCount(SessionContext context);
        public abstract Task<int> OrgApiKeyGetCountAsync(SessionContext context);
        public abstract int OrgApiKeyGetMaxID(SessionContext context);
        public abstract Task<int> OrgApiKeyGetMaxIDAsync(SessionContext context);
        public abstract int OrgApiKeyInsert(
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
            System.Guid code);
        public abstract Task<int> OrgApiKeyInsertAsync(
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
            System.Guid code);
        public abstract void OrgApiKeyUpdate(
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
            System.Guid code);
        public abstract Task OrgApiKeyUpdateAsync(
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
            System.Guid code);
        public abstract IDataReader SearchOrgApiKeys(
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
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchOrgApiKeysAsync(
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
            bool searchByCode, System.Guid code
            );
        public abstract int OrgApiKeyBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract Task<int> OrgApiKeyBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract int OrgApiKeyBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract Task<int> OrgApiKeyBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract int OrgApiKeyBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract Task<int> OrgApiKeyBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgApiKey> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetOrgApiKeyList_FetchByOrganizationID(
            int organizationID,
           SessionContext context);
        public abstract IDataReader GetOrgApiKeyList_FetchByOrgCustomerID(
            int orgCustomerID,
           SessionContext context);
        public abstract Task<IDataReader> GetOrgApiKeyList_FetchByOrganizationIDAsync(
            int organizationID,
           SessionContext context);
        public abstract Task<IDataReader> GetOrgApiKeyList_FetchByOrgCustomerIDAsync(
            int orgCustomerID,
           SessionContext context);

    }
}
