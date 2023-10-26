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
    internal abstract partial class FlavorProvider : System.Configuration.Provider.ProviderBase
    {
        static FlavorProvider _instance;
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
            FlavorProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            FlavorProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static FlavorProvider Instance
        {
            get
            {
                FlavorProvider tempInstance;
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
        static FlavorProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            FlavorProviderConfiguration config = FlavorProviderConfiguration.GetConfig();
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
        static FlavorProvider LoadProvider()
        {
            FlavorProviderConfiguration config = FlavorProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static FlavorProvider LoadProvider(FlavorProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            FlavorProvider _instanceLoader;
            cacheKey = "FS.Farm.FlavorProvider::" + providerName;
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
                _instanceLoader = (FlavorProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider FlavorProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(FlavorProvider.Type);
                    _instanceLoader = (FlavorProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = FlavorProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //FlavorProvider.Attributes.Add("connectionString", cString);
                    if (FlavorProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        FlavorProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        FlavorProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(FlavorProvider.Name, FlavorProvider.Attributes);
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
        #region Flavors
        public abstract IDataReader GetFlavorList(SessionContext context);
        public abstract Task<IDataReader> GetFlavorListAsync(SessionContext context);
        public abstract Guid GetFlavorCode(SessionContext context, int flavorID);
        public abstract Task<Guid> GetFlavorCodeAsync(SessionContext context, int flavorID);
        public abstract int GetFlavorID(SessionContext context, System.Guid code);
        public abstract Task<int> GetFlavorIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetFlavor(SessionContext context, int flavorID);
        public abstract Task<IDataReader> GetFlavorAsync(SessionContext context, int flavorID);
        public abstract IDataReader GetDirtyFlavor(SessionContext context, int flavorID);
        public abstract Task<IDataReader> GetDirtyFlavorAsync(SessionContext context, int flavorID);
        public abstract IDataReader GetFlavor(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetFlavorAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyFlavor(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyFlavorAsync(SessionContext context, System.Guid code);
        public abstract void FlavorDelete(SessionContext context, int flavorID);
        public abstract Task FlavorDeleteAsync(SessionContext context, int flavorID);
        public abstract void FlavorCleanupTesting(SessionContext context);
        public abstract void FlavorCleanupChildObjectTesting(SessionContext context);
        public abstract int FlavorGetCount(SessionContext context);
        public abstract Task<int> FlavorGetCountAsync(SessionContext context);
        public abstract int FlavorGetMaxID(SessionContext context);
        public abstract Task<int> FlavorGetMaxIDAsync(SessionContext context);
        public abstract int FlavorInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> FlavorInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void FlavorUpdate(
            SessionContext context,
            int flavorID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task FlavorUpdateAsync(
            SessionContext context,
            int flavorID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchFlavors(
            SessionContext context,
            bool searchByFlavorID, int flavorID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchFlavorsAsync(
            SessionContext context,
            bool searchByFlavorID, int flavorID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int FlavorBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract Task<int> FlavorBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract int FlavorBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract Task<int> FlavorBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract int FlavorBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract Task<int> FlavorBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetFlavorList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetFlavorList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);
    }
}
