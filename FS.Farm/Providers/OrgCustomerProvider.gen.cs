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
    internal abstract partial class OrgCustomerProvider : System.Configuration.Provider.ProviderBase
    {
        static OrgCustomerProvider _instance;
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
            OrgCustomerProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrgCustomerProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static OrgCustomerProvider Instance
        {
            get
            {
                OrgCustomerProvider tempInstance;
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
        static OrgCustomerProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrgCustomerProviderConfiguration config = OrgCustomerProviderConfiguration.GetConfig();
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
        static OrgCustomerProvider LoadProvider()
        {
            OrgCustomerProviderConfiguration config = OrgCustomerProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static OrgCustomerProvider LoadProvider(OrgCustomerProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            OrgCustomerProvider _instanceLoader;
            cacheKey = "FS.Farm.OrgCustomerProvider::" + providerName;
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
                _instanceLoader = (OrgCustomerProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider OrgCustomerProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(OrgCustomerProvider.Type);
                    _instanceLoader = (OrgCustomerProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = OrgCustomerProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //OrgCustomerProvider.Attributes.Add("connectionString", cString);
                    if (OrgCustomerProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        OrgCustomerProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        OrgCustomerProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(OrgCustomerProvider.Name, OrgCustomerProvider.Attributes);
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
        #region OrgCustomers
        public abstract IDataReader GetOrgCustomerList(SessionContext context);
        public abstract Task<IDataReader> GetOrgCustomerListAsync(SessionContext context);
        public abstract Guid GetOrgCustomerCode(SessionContext context, int orgCustomerID);
        public abstract Task<Guid> GetOrgCustomerCodeAsync(SessionContext context, int orgCustomerID);
        public abstract int GetOrgCustomerID(SessionContext context, System.Guid code);
        public abstract Task<int> GetOrgCustomerIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetOrgCustomer(SessionContext context, int orgCustomerID);
        public abstract Task<IDataReader> GetOrgCustomerAsync(SessionContext context, int orgCustomerID);
        public abstract IDataReader GetDirtyOrgCustomer(SessionContext context, int orgCustomerID);
        public abstract Task<IDataReader> GetDirtyOrgCustomerAsync(SessionContext context, int orgCustomerID);
        public abstract IDataReader GetOrgCustomer(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetOrgCustomerAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyOrgCustomer(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyOrgCustomerAsync(SessionContext context, System.Guid code);
        public abstract void OrgCustomerDelete(SessionContext context, int orgCustomerID);
        public abstract Task OrgCustomerDeleteAsync(SessionContext context, int orgCustomerID);
        public abstract void OrgCustomerCleanupTesting(SessionContext context);
        public abstract void OrgCustomerCleanupChildObjectTesting(SessionContext context);
        public abstract int OrgCustomerGetCount(SessionContext context);
        public abstract Task<int> OrgCustomerGetCountAsync(SessionContext context);
        public abstract int OrgCustomerGetMaxID(SessionContext context);
        public abstract Task<int> OrgCustomerGetMaxIDAsync(SessionContext context);
        public abstract int OrgCustomerInsert(
            SessionContext context,
            Int32 customerID,
            String email,
            Int32 organizationID,
            System.Guid code);
        public abstract Task<int> OrgCustomerInsertAsync(
            SessionContext context,
            Int32 customerID,
            String email,
            Int32 organizationID,
            System.Guid code);
        public abstract void OrgCustomerUpdate(
            SessionContext context,
            int orgCustomerID,
            Int32 customerID,
            String email,
            Int32 organizationID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task OrgCustomerUpdateAsync(
            SessionContext context,
            int orgCustomerID,
            Int32 customerID,
            String email,
            Int32 organizationID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchOrgCustomers(
            SessionContext context,
            bool searchByOrgCustomerID, int orgCustomerID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByEmail, String email,
            bool searchByOrganizationID, Int32 organizationID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchOrgCustomersAsync(
            SessionContext context,
            bool searchByOrgCustomerID, int orgCustomerID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByEmail, String email,
            bool searchByOrganizationID, Int32 organizationID,
            bool searchByCode, System.Guid code
            );
        public abstract int OrgCustomerBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract Task<int> OrgCustomerBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract int OrgCustomerBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract Task<int> OrgCustomerBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract int OrgCustomerBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract Task<int> OrgCustomerBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetOrgCustomerList_FetchByCustomerID(
            int customerID,
           SessionContext context);
        public abstract IDataReader GetOrgCustomerList_FetchByOrganizationID(
            int organizationID,
           SessionContext context);
        public abstract Task<IDataReader> GetOrgCustomerList_FetchByCustomerIDAsync(
            int customerID,
           SessionContext context);
        public abstract Task<IDataReader> GetOrgCustomerList_FetchByOrganizationIDAsync(
            int organizationID,
           SessionContext context);

    }
}
