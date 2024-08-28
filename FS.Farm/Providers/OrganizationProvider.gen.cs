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
    internal abstract partial class OrganizationProvider : System.Configuration.Provider.ProviderBase
    {
        static OrganizationProvider _instance;
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
            OrganizationProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrganizationProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static OrganizationProvider Instance
        {
            get
            {
                OrganizationProvider tempInstance;
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
        static OrganizationProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            OrganizationProviderConfiguration config = OrganizationProviderConfiguration.GetConfig();
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
        static OrganizationProvider LoadProvider()
        {
            OrganizationProviderConfiguration config = OrganizationProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static OrganizationProvider LoadProvider(OrganizationProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            OrganizationProvider _instanceLoader;
            cacheKey = "FS.Farm.OrganizationProvider::" + providerName;
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
                _instanceLoader = (OrganizationProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider OrganizationProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(OrganizationProvider.Type);
                    _instanceLoader = (OrganizationProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = OrganizationProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //OrganizationProvider.Attributes.Add("connectionString", cString);
                    if (OrganizationProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        OrganizationProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        OrganizationProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(OrganizationProvider.Name, OrganizationProvider.Attributes);
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
        #region Organizations
        public abstract IDataReader GetOrganizationList(SessionContext context);
        public abstract Task<IDataReader> GetOrganizationListAsync(SessionContext context);
        public abstract Guid GetOrganizationCode(SessionContext context, int organizationID);
        public abstract Task<Guid> GetOrganizationCodeAsync(SessionContext context, int organizationID);
        public abstract int GetOrganizationID(SessionContext context, System.Guid code);
        public abstract Task<int> GetOrganizationIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetOrganization(SessionContext context, int organizationID);
        public abstract Task<IDataReader> GetOrganizationAsync(SessionContext context, int organizationID);
        public abstract IDataReader GetDirtyOrganization(SessionContext context, int organizationID);
        public abstract Task<IDataReader> GetDirtyOrganizationAsync(SessionContext context, int organizationID);
        public abstract IDataReader GetOrganization(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetOrganizationAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyOrganization(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyOrganizationAsync(SessionContext context, System.Guid code);
        public abstract void OrganizationDelete(SessionContext context, int organizationID);
        public abstract Task OrganizationDeleteAsync(SessionContext context, int organizationID);
        public abstract void OrganizationCleanupTesting(SessionContext context);
        public abstract void OrganizationCleanupChildObjectTesting(SessionContext context);
        public abstract int OrganizationGetCount(SessionContext context);
        public abstract Task<int> OrganizationGetCountAsync(SessionContext context);
        public abstract int OrganizationGetMaxID(SessionContext context);
        public abstract Task<int> OrganizationGetMaxIDAsync(SessionContext context);
        public abstract int OrganizationInsert(
            SessionContext context,
            String name,
            Int32 tacID,
            System.Guid code);
        public abstract Task<int> OrganizationInsertAsync(
            SessionContext context,
            String name,
            Int32 tacID,
            System.Guid code);
        public abstract void OrganizationUpdate(
            SessionContext context,
            int organizationID,
            String name,
            Int32 tacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task OrganizationUpdateAsync(
            SessionContext context,
            int organizationID,
            String name,
            Int32 tacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchOrganizations(
            SessionContext context,
            bool searchByOrganizationID, int organizationID,
            bool searchByName, String name,
            bool searchByTacID, Int32 tacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchOrganizationsAsync(
            SessionContext context,
            bool searchByOrganizationID, int organizationID,
            bool searchByName, String name,
            bool searchByTacID, Int32 tacID,
            bool searchByCode, System.Guid code
            );
        public abstract int OrganizationBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract Task<int> OrganizationBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract int OrganizationBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract Task<int> OrganizationBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract int OrganizationBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract Task<int> OrganizationBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetOrganizationList_FetchByTacID(
            int tacID,
           SessionContext context);
        public abstract Task<IDataReader> GetOrganizationList_FetchByTacIDAsync(
            int tacID,
           SessionContext context);

    }
}
