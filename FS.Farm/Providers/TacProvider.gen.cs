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
    internal abstract partial class TacProvider : System.Configuration.Provider.ProviderBase
    {
        static TacProvider _instance;
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
            TacProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            TacProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static TacProvider Instance
        {
            get
            {
                TacProvider tempInstance;
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
        static TacProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            TacProviderConfiguration config = TacProviderConfiguration.GetConfig();
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
        static TacProvider LoadProvider()
        {
            TacProviderConfiguration config = TacProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static TacProvider LoadProvider(TacProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            TacProvider _instanceLoader;
            cacheKey = "FS.Farm.TacProvider::" + providerName;
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
                _instanceLoader = (TacProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider TacProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(TacProvider.Type);
                    _instanceLoader = (TacProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = TacProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //TacProvider.Attributes.Add("connectionString", cString);
                    if (TacProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        TacProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        TacProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(TacProvider.Name, TacProvider.Attributes);
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
        #region Tacs
        public abstract IDataReader GetTacList(SessionContext context);
        public abstract Task<IDataReader> GetTacListAsync(SessionContext context);
        public abstract Guid GetTacCode(SessionContext context, int tacID);
        public abstract Task<Guid> GetTacCodeAsync(SessionContext context, int tacID);
        public abstract int GetTacID(SessionContext context, System.Guid code);
        public abstract Task<int> GetTacIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetTac(SessionContext context, int tacID);
        public abstract Task<IDataReader> GetTacAsync(SessionContext context, int tacID);
        public abstract IDataReader GetDirtyTac(SessionContext context, int tacID);
        public abstract Task<IDataReader> GetDirtyTacAsync(SessionContext context, int tacID);
        public abstract IDataReader GetTac(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetTacAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyTac(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyTacAsync(SessionContext context, System.Guid code);
        public abstract void TacDelete(SessionContext context, int tacID);
        public abstract Task TacDeleteAsync(SessionContext context, int tacID);
        public abstract void TacCleanupTesting(SessionContext context);
        public abstract void TacCleanupChildObjectTesting(SessionContext context);
        public abstract int TacGetCount(SessionContext context);
        public abstract Task<int> TacGetCountAsync(SessionContext context);
        public abstract int TacGetMaxID(SessionContext context);
        public abstract Task<int> TacGetMaxIDAsync(SessionContext context);
        public abstract int TacInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> TacInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void TacUpdate(
            SessionContext context,
            int tacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task TacUpdateAsync(
            SessionContext context,
            int tacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchTacs(
            SessionContext context,
            bool searchByTacID, int tacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchTacsAsync(
            SessionContext context,
            bool searchByTacID, int tacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int TacBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract Task<int> TacBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract int TacBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract Task<int> TacBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract int TacBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract Task<int> TacBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetTacList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetTacList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
