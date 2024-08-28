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
    internal abstract partial class DFMaintenanceProvider : System.Configuration.Provider.ProviderBase
    {
        static DFMaintenanceProvider _instance;
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
            DFMaintenanceProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DFMaintenanceProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DFMaintenanceProvider Instance
        {
            get
            {
                DFMaintenanceProvider tempInstance;
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
        static DFMaintenanceProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DFMaintenanceProviderConfiguration config = DFMaintenanceProviderConfiguration.GetConfig();
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
        static DFMaintenanceProvider LoadProvider()
        {
            DFMaintenanceProviderConfiguration config = DFMaintenanceProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DFMaintenanceProvider LoadProvider(DFMaintenanceProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DFMaintenanceProvider _instanceLoader;
            cacheKey = "FS.Farm.DFMaintenanceProvider::" + providerName;
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
                _instanceLoader = (DFMaintenanceProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DFMaintenanceProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DFMaintenanceProvider.Type);
                    _instanceLoader = (DFMaintenanceProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DFMaintenanceProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DFMaintenanceProvider.Attributes.Add("connectionString", cString);
                    if (DFMaintenanceProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DFMaintenanceProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DFMaintenanceProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DFMaintenanceProvider.Name, DFMaintenanceProvider.Attributes);
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
        #region DFMaintenances
        public abstract IDataReader GetDFMaintenanceList(SessionContext context);
        public abstract Task<IDataReader> GetDFMaintenanceListAsync(SessionContext context);
        public abstract Guid GetDFMaintenanceCode(SessionContext context, int dFMaintenanceID);
        public abstract Task<Guid> GetDFMaintenanceCodeAsync(SessionContext context, int dFMaintenanceID);
        public abstract int GetDFMaintenanceID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDFMaintenanceIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDFMaintenance(SessionContext context, int dFMaintenanceID);
        public abstract Task<IDataReader> GetDFMaintenanceAsync(SessionContext context, int dFMaintenanceID);
        public abstract IDataReader GetDirtyDFMaintenance(SessionContext context, int dFMaintenanceID);
        public abstract Task<IDataReader> GetDirtyDFMaintenanceAsync(SessionContext context, int dFMaintenanceID);
        public abstract IDataReader GetDFMaintenance(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDFMaintenanceAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDFMaintenance(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDFMaintenanceAsync(SessionContext context, System.Guid code);
        public abstract void DFMaintenanceDelete(SessionContext context, int dFMaintenanceID);
        public abstract Task DFMaintenanceDeleteAsync(SessionContext context, int dFMaintenanceID);
        public abstract void DFMaintenanceCleanupTesting(SessionContext context);
        public abstract void DFMaintenanceCleanupChildObjectTesting(SessionContext context);
        public abstract int DFMaintenanceGetCount(SessionContext context);
        public abstract Task<int> DFMaintenanceGetCountAsync(SessionContext context);
        public abstract int DFMaintenanceGetMaxID(SessionContext context);
        public abstract Task<int> DFMaintenanceGetMaxIDAsync(SessionContext context);
        public abstract int DFMaintenanceInsert(
            SessionContext context,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
            System.Guid code);
        public abstract Task<int> DFMaintenanceInsertAsync(
            SessionContext context,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
            System.Guid code);
        public abstract void DFMaintenanceUpdate(
            SessionContext context,
            int dFMaintenanceID,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DFMaintenanceUpdateAsync(
            SessionContext context,
            int dFMaintenanceID,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDFMaintenances(
            SessionContext context,
            bool searchByDFMaintenanceID, int dFMaintenanceID,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsScheduledDFProcessRequestCompleted, Boolean isScheduledDFProcessRequestCompleted,
            bool searchByIsScheduledDFProcessRequestStarted, Boolean isScheduledDFProcessRequestStarted,
            bool searchByLastScheduledDFProcessRequestUTCDateTime, DateTime lastScheduledDFProcessRequestUTCDateTime,
            bool searchByNextScheduledDFProcessRequestUTCDateTime, DateTime nextScheduledDFProcessRequestUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByPausedByUsername, String pausedByUsername,
            bool searchByPausedUTCDateTime, DateTime pausedUTCDateTime,
            bool searchByScheduledDFProcessRequestProcessorIdentifier, String scheduledDFProcessRequestProcessorIdentifier,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDFMaintenancesAsync(
            SessionContext context,
            bool searchByDFMaintenanceID, int dFMaintenanceID,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsScheduledDFProcessRequestCompleted, Boolean isScheduledDFProcessRequestCompleted,
            bool searchByIsScheduledDFProcessRequestStarted, Boolean isScheduledDFProcessRequestStarted,
            bool searchByLastScheduledDFProcessRequestUTCDateTime, DateTime lastScheduledDFProcessRequestUTCDateTime,
            bool searchByNextScheduledDFProcessRequestUTCDateTime, DateTime nextScheduledDFProcessRequestUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByPausedByUsername, String pausedByUsername,
            bool searchByPausedUTCDateTime, DateTime pausedUTCDateTime,
            bool searchByScheduledDFProcessRequestProcessorIdentifier, String scheduledDFProcessRequestProcessorIdentifier,
            bool searchByCode, System.Guid code
            );
        public abstract int DFMaintenanceBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract Task<int> DFMaintenanceBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract int DFMaintenanceBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract Task<int> DFMaintenanceBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract int DFMaintenanceBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract Task<int> DFMaintenanceBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDFMaintenanceList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDFMaintenanceList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
