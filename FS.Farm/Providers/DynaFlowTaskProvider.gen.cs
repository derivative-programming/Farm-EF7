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
    internal abstract partial class DynaFlowTaskProvider : System.Configuration.Provider.ProviderBase
    {
        static DynaFlowTaskProvider _instance;
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
            DynaFlowTaskProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTaskProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DynaFlowTaskProvider Instance
        {
            get
            {
                DynaFlowTaskProvider tempInstance;
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
        static DynaFlowTaskProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTaskProviderConfiguration config = DynaFlowTaskProviderConfiguration.GetConfig();
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
        static DynaFlowTaskProvider LoadProvider()
        {
            DynaFlowTaskProviderConfiguration config = DynaFlowTaskProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DynaFlowTaskProvider LoadProvider(DynaFlowTaskProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DynaFlowTaskProvider _instanceLoader;
            cacheKey = "FS.Farm.DynaFlowTaskProvider::" + providerName;
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
                _instanceLoader = (DynaFlowTaskProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DynaFlowTaskProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DynaFlowTaskProvider.Type);
                    _instanceLoader = (DynaFlowTaskProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DynaFlowTaskProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DynaFlowTaskProvider.Attributes.Add("connectionString", cString);
                    if (DynaFlowTaskProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DynaFlowTaskProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DynaFlowTaskProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DynaFlowTaskProvider.Name, DynaFlowTaskProvider.Attributes);
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
        #region DynaFlowTasks
        public abstract IDataReader GetDynaFlowTaskList(SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTaskListAsync(SessionContext context);
        public abstract Guid GetDynaFlowTaskCode(SessionContext context, int dynaFlowTaskID);
        public abstract Task<Guid> GetDynaFlowTaskCodeAsync(SessionContext context, int dynaFlowTaskID);
        public abstract int GetDynaFlowTaskID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDynaFlowTaskIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDynaFlowTask(SessionContext context, int dynaFlowTaskID);
        public abstract Task<IDataReader> GetDynaFlowTaskAsync(SessionContext context, int dynaFlowTaskID);
        public abstract IDataReader GetDirtyDynaFlowTask(SessionContext context, int dynaFlowTaskID);
        public abstract Task<IDataReader> GetDirtyDynaFlowTaskAsync(SessionContext context, int dynaFlowTaskID);
        public abstract IDataReader GetDynaFlowTask(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDynaFlowTaskAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDynaFlowTask(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDynaFlowTaskAsync(SessionContext context, System.Guid code);
        public abstract void DynaFlowTaskDelete(SessionContext context, int dynaFlowTaskID);
        public abstract Task DynaFlowTaskDeleteAsync(SessionContext context, int dynaFlowTaskID);
        public abstract void DynaFlowTaskCleanupTesting(SessionContext context);
        public abstract void DynaFlowTaskCleanupChildObjectTesting(SessionContext context);
        public abstract int DynaFlowTaskGetCount(SessionContext context);
        public abstract Task<int> DynaFlowTaskGetCountAsync(SessionContext context);
        public abstract int DynaFlowTaskGetMaxID(SessionContext context);
        public abstract Task<int> DynaFlowTaskGetMaxIDAsync(SessionContext context);
        public abstract int DynaFlowTaskInsert(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
            System.Guid code);
        public abstract Task<int> DynaFlowTaskInsertAsync(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
            System.Guid code);
        public abstract void DynaFlowTaskUpdate(
            SessionContext context,
            int dynaFlowTaskID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DynaFlowTaskUpdateAsync(
            SessionContext context,
            int dynaFlowTaskID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDynaFlowTasks(
            SessionContext context,
            bool searchByDynaFlowTaskID, int dynaFlowTaskID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowTaskID, Int32 dependencyDynaFlowTaskID,
            bool searchByDescription, String description,
            bool searchByDynaFlowID, Int32 dynaFlowID,
            bool searchByDynaFlowSubjectCode, Guid dynaFlowSubjectCode,
            bool searchByDynaFlowTaskTypeID, Int32 dynaFlowTaskTypeID,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsParallelRunAllowed, Boolean isParallelRunAllowed,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByParam1, String param1,
            bool searchByParam2, String param2,
            bool searchByProcessorIdentifier, String processorIdentifier,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRetryCount, Int32 retryCount,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDynaFlowTasksAsync(
            SessionContext context,
            bool searchByDynaFlowTaskID, int dynaFlowTaskID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowTaskID, Int32 dependencyDynaFlowTaskID,
            bool searchByDescription, String description,
            bool searchByDynaFlowID, Int32 dynaFlowID,
            bool searchByDynaFlowSubjectCode, Guid dynaFlowSubjectCode,
            bool searchByDynaFlowTaskTypeID, Int32 dynaFlowTaskTypeID,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsParallelRunAllowed, Boolean isParallelRunAllowed,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByParam1, String param1,
            bool searchByParam2, String param2,
            bool searchByProcessorIdentifier, String processorIdentifier,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRetryCount, Int32 retryCount,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
            bool searchByCode, System.Guid code
            );
        public abstract int DynaFlowTaskBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract Task<int> DynaFlowTaskBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract int DynaFlowTaskBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract Task<int> DynaFlowTaskBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract int DynaFlowTaskBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract Task<int> DynaFlowTaskBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDynaFlowTaskList_FetchByDynaFlowID(
            int dynaFlowID,
           SessionContext context);
        public abstract IDataReader GetDynaFlowTaskList_FetchByDynaFlowTaskTypeID(
            int dynaFlowTaskTypeID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTaskList_FetchByDynaFlowIDAsync(
            int dynaFlowID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTaskList_FetchByDynaFlowTaskTypeIDAsync(
            int dynaFlowTaskTypeID,
           SessionContext context);

    }
}
