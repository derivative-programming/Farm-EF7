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
    internal abstract partial class PacProvider : System.Configuration.Provider.ProviderBase
    {
        static PacProvider _instance;
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
            PacProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            PacProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static PacProvider Instance
        {
            get
            {
                PacProvider tempInstance;
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
        static PacProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            PacProviderConfiguration config = PacProviderConfiguration.GetConfig();
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
        static PacProvider LoadProvider()
        {
            PacProviderConfiguration config = PacProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static PacProvider LoadProvider(PacProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            PacProvider _instanceLoader;
            cacheKey = "FS.Farm.PacProvider::" + providerName;
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
                _instanceLoader = (PacProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider PacProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(PacProvider.Type);
                    _instanceLoader = (PacProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = PacProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //PacProvider.Attributes.Add("connectionString", cString);
                    if (PacProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        PacProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        PacProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(PacProvider.Name, PacProvider.Attributes);
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
        #region Pacs
        public abstract IDataReader GetPacList(SessionContext context);
        public abstract Task<IDataReader> GetPacListAsync(SessionContext context);
        public abstract Guid GetPacCode(SessionContext context, int pacID);
        public abstract Task<Guid> GetPacCodeAsync(SessionContext context, int pacID);
        public abstract int GetPacID(SessionContext context, System.Guid code);
        public abstract Task<int> GetPacIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetPac(SessionContext context, int pacID);
        public abstract Task<IDataReader> GetPacAsync(SessionContext context, int pacID);
        public abstract IDataReader GetDirtyPac(SessionContext context, int pacID);
        public abstract Task<IDataReader> GetDirtyPacAsync(SessionContext context, int pacID);
        public abstract IDataReader GetPac(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetPacAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyPac(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyPacAsync(SessionContext context, System.Guid code);
        public abstract void PacDelete(SessionContext context, int pacID);
        public abstract Task PacDeleteAsync(SessionContext context, int pacID);
        public abstract void PacCleanupTesting(SessionContext context);
        public abstract void PacCleanupChildObjectTesting(SessionContext context);
        public abstract int PacGetCount(SessionContext context);
        public abstract Task<int> PacGetCountAsync(SessionContext context);
        public abstract int PacGetMaxID(SessionContext context);
        public abstract Task<int> PacGetMaxIDAsync(SessionContext context);
        public abstract int PacInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            System.Guid code);
        public abstract Task<int> PacInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            System.Guid code);
        public abstract void PacUpdate(
            SessionContext context,
            int pacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task PacUpdateAsync(
            SessionContext context,
            int pacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchPacs(
            SessionContext context,
            bool searchByPacID, int pacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchPacsAsync(
            SessionContext context,
            bool searchByPacID, int pacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByCode, System.Guid code
            );
        public abstract int PacBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract Task<int> PacBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract int PacBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract Task<int> PacBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract int PacBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract Task<int> PacBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList);
        public abstract bool SupportsTransactions();
        #endregion

    }
}
