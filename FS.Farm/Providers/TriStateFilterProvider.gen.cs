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
    internal abstract partial class TriStateFilterProvider : System.Configuration.Provider.ProviderBase
    {
        static TriStateFilterProvider _instance;
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
            TriStateFilterProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            TriStateFilterProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static TriStateFilterProvider Instance
        {
            get
            {
                TriStateFilterProvider tempInstance;
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
        static TriStateFilterProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            TriStateFilterProviderConfiguration config = TriStateFilterProviderConfiguration.GetConfig();
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
        static TriStateFilterProvider LoadProvider()
        {
            TriStateFilterProviderConfiguration config = TriStateFilterProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static TriStateFilterProvider LoadProvider(TriStateFilterProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            TriStateFilterProvider _instanceLoader;
            cacheKey = "FS.Farm.TriStateFilterProvider::" + providerName;
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
                _instanceLoader = (TriStateFilterProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider TriStateFilterProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(TriStateFilterProvider.Type);
                    _instanceLoader = (TriStateFilterProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = TriStateFilterProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //TriStateFilterProvider.Attributes.Add("connectionString", cString);
                    if (TriStateFilterProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        TriStateFilterProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        TriStateFilterProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(TriStateFilterProvider.Name, TriStateFilterProvider.Attributes);
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
        #region TriStateFilters
        public abstract IDataReader GetTriStateFilterList(SessionContext context);
        public abstract Task<IDataReader> GetTriStateFilterListAsync(SessionContext context);
        public abstract Guid GetTriStateFilterCode(SessionContext context, int triStateFilterID);
        public abstract Task<Guid> GetTriStateFilterCodeAsync(SessionContext context, int triStateFilterID);
        public abstract int GetTriStateFilterID(SessionContext context, System.Guid code);
        public abstract Task<int> GetTriStateFilterIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetTriStateFilter(SessionContext context, int triStateFilterID);
        public abstract Task<IDataReader> GetTriStateFilterAsync(SessionContext context, int triStateFilterID);
        public abstract IDataReader GetDirtyTriStateFilter(SessionContext context, int triStateFilterID);
        public abstract Task<IDataReader> GetDirtyTriStateFilterAsync(SessionContext context, int triStateFilterID);
        public abstract IDataReader GetTriStateFilter(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetTriStateFilterAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyTriStateFilter(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyTriStateFilterAsync(SessionContext context, System.Guid code);
        public abstract void TriStateFilterDelete(SessionContext context, int triStateFilterID);
        public abstract Task TriStateFilterDeleteAsync(SessionContext context, int triStateFilterID);
        public abstract void TriStateFilterCleanupTesting(SessionContext context);
        public abstract void TriStateFilterCleanupChildObjectTesting(SessionContext context);
        public abstract int TriStateFilterGetCount(SessionContext context);
        public abstract Task<int> TriStateFilterGetCountAsync(SessionContext context);
        public abstract int TriStateFilterGetMaxID(SessionContext context);
        public abstract Task<int> TriStateFilterGetMaxIDAsync(SessionContext context);
        public abstract int TriStateFilterInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            System.Guid code);
        public abstract Task<int> TriStateFilterInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            System.Guid code);
        public abstract void TriStateFilterUpdate(
            SessionContext context,
            int triStateFilterID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task TriStateFilterUpdateAsync(
            SessionContext context,
            int triStateFilterID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchTriStateFilters(
            SessionContext context,
            bool searchByTriStateFilterID, int triStateFilterID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByStateIntValue, Int32 stateIntValue,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchTriStateFiltersAsync(
            SessionContext context,
            bool searchByTriStateFilterID, int triStateFilterID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByStateIntValue, Int32 stateIntValue,
            bool searchByCode, System.Guid code
            );
        public abstract int TriStateFilterBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract Task<int> TriStateFilterBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract int TriStateFilterBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract Task<int> TriStateFilterBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract int TriStateFilterBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract Task<int> TriStateFilterBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetTriStateFilterList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetTriStateFilterList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
