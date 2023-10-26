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
    internal abstract partial class LandProvider : System.Configuration.Provider.ProviderBase
    {
        static LandProvider _instance;
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
            LandProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            LandProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static LandProvider Instance
        {
            get
            {
                LandProvider tempInstance;
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
        static LandProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            LandProviderConfiguration config = LandProviderConfiguration.GetConfig();
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
        static LandProvider LoadProvider()
        {
            LandProviderConfiguration config = LandProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static LandProvider LoadProvider(LandProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            LandProvider _instanceLoader;
            cacheKey = "FS.Farm.LandProvider::" + providerName;
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
                _instanceLoader = (LandProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider LandProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(LandProvider.Type);
                    _instanceLoader = (LandProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = LandProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //LandProvider.Attributes.Add("connectionString", cString);
                    if (LandProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        LandProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        LandProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(LandProvider.Name, LandProvider.Attributes);
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
        #region Lands
        public abstract IDataReader GetLandList(SessionContext context);
        public abstract Task<IDataReader> GetLandListAsync(SessionContext context);
        public abstract Guid GetLandCode(SessionContext context, int landID);
        public abstract Task<Guid> GetLandCodeAsync(SessionContext context, int landID);
        public abstract int GetLandID(SessionContext context, System.Guid code);
        public abstract Task<int> GetLandIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetLand(SessionContext context, int landID);
        public abstract Task<IDataReader> GetLandAsync(SessionContext context, int landID);
        public abstract IDataReader GetDirtyLand(SessionContext context, int landID);
        public abstract Task<IDataReader> GetDirtyLandAsync(SessionContext context, int landID);
        public abstract IDataReader GetLand(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetLandAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyLand(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyLandAsync(SessionContext context, System.Guid code);
        public abstract void LandDelete(SessionContext context, int landID);
        public abstract Task LandDeleteAsync(SessionContext context, int landID);
        public abstract void LandCleanupTesting(SessionContext context);
        public abstract void LandCleanupChildObjectTesting(SessionContext context);
        public abstract int LandGetCount(SessionContext context);
        public abstract Task<int> LandGetCountAsync(SessionContext context);
        public abstract int LandGetMaxID(SessionContext context);
        public abstract Task<int> LandGetMaxIDAsync(SessionContext context);
        public abstract int LandInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> LandInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void LandUpdate(
            SessionContext context,
            int landID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task LandUpdateAsync(
            SessionContext context,
            int landID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchLands(
            SessionContext context,
            bool searchByLandID, int landID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchLandsAsync(
            SessionContext context,
            bool searchByLandID, int landID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int LandBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract Task<int> LandBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract int LandBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract Task<int> LandBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract int LandBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract Task<int> LandBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetLandList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetLandList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);
    }
}
