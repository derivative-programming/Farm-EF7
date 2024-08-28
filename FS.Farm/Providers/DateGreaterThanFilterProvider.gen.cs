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
    internal abstract partial class DateGreaterThanFilterProvider : System.Configuration.Provider.ProviderBase
    {
        static DateGreaterThanFilterProvider _instance;
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
            DateGreaterThanFilterProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DateGreaterThanFilterProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DateGreaterThanFilterProvider Instance
        {
            get
            {
                DateGreaterThanFilterProvider tempInstance;
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
        static DateGreaterThanFilterProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DateGreaterThanFilterProviderConfiguration config = DateGreaterThanFilterProviderConfiguration.GetConfig();
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
        static DateGreaterThanFilterProvider LoadProvider()
        {
            DateGreaterThanFilterProviderConfiguration config = DateGreaterThanFilterProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DateGreaterThanFilterProvider LoadProvider(DateGreaterThanFilterProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DateGreaterThanFilterProvider _instanceLoader;
            cacheKey = "FS.Farm.DateGreaterThanFilterProvider::" + providerName;
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
                _instanceLoader = (DateGreaterThanFilterProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DateGreaterThanFilterProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DateGreaterThanFilterProvider.Type);
                    _instanceLoader = (DateGreaterThanFilterProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DateGreaterThanFilterProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DateGreaterThanFilterProvider.Attributes.Add("connectionString", cString);
                    if (DateGreaterThanFilterProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DateGreaterThanFilterProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DateGreaterThanFilterProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DateGreaterThanFilterProvider.Name, DateGreaterThanFilterProvider.Attributes);
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
        #region DateGreaterThanFilters
        public abstract IDataReader GetDateGreaterThanFilterList(SessionContext context);
        public abstract Task<IDataReader> GetDateGreaterThanFilterListAsync(SessionContext context);
        public abstract Guid GetDateGreaterThanFilterCode(SessionContext context, int dateGreaterThanFilterID);
        public abstract Task<Guid> GetDateGreaterThanFilterCodeAsync(SessionContext context, int dateGreaterThanFilterID);
        public abstract int GetDateGreaterThanFilterID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDateGreaterThanFilterIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDateGreaterThanFilter(SessionContext context, int dateGreaterThanFilterID);
        public abstract Task<IDataReader> GetDateGreaterThanFilterAsync(SessionContext context, int dateGreaterThanFilterID);
        public abstract IDataReader GetDirtyDateGreaterThanFilter(SessionContext context, int dateGreaterThanFilterID);
        public abstract Task<IDataReader> GetDirtyDateGreaterThanFilterAsync(SessionContext context, int dateGreaterThanFilterID);
        public abstract IDataReader GetDateGreaterThanFilter(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDateGreaterThanFilterAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDateGreaterThanFilter(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDateGreaterThanFilterAsync(SessionContext context, System.Guid code);
        public abstract void DateGreaterThanFilterDelete(SessionContext context, int dateGreaterThanFilterID);
        public abstract Task DateGreaterThanFilterDeleteAsync(SessionContext context, int dateGreaterThanFilterID);
        public abstract void DateGreaterThanFilterCleanupTesting(SessionContext context);
        public abstract void DateGreaterThanFilterCleanupChildObjectTesting(SessionContext context);
        public abstract int DateGreaterThanFilterGetCount(SessionContext context);
        public abstract Task<int> DateGreaterThanFilterGetCountAsync(SessionContext context);
        public abstract int DateGreaterThanFilterGetMaxID(SessionContext context);
        public abstract Task<int> DateGreaterThanFilterGetMaxIDAsync(SessionContext context);
        public abstract int DateGreaterThanFilterInsert(
            SessionContext context,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> DateGreaterThanFilterInsertAsync(
            SessionContext context,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void DateGreaterThanFilterUpdate(
            SessionContext context,
            int dateGreaterThanFilterID,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DateGreaterThanFilterUpdateAsync(
            SessionContext context,
            int dateGreaterThanFilterID,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDateGreaterThanFilters(
            SessionContext context,
            bool searchByDateGreaterThanFilterID, int dateGreaterThanFilterID,
            bool searchByDayCount, Int32 dayCount,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDateGreaterThanFiltersAsync(
            SessionContext context,
            bool searchByDateGreaterThanFilterID, int dateGreaterThanFilterID,
            bool searchByDayCount, Int32 dayCount,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int DateGreaterThanFilterBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract Task<int> DateGreaterThanFilterBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract int DateGreaterThanFilterBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract Task<int> DateGreaterThanFilterBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract int DateGreaterThanFilterBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract Task<int> DateGreaterThanFilterBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDateGreaterThanFilterList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDateGreaterThanFilterList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
