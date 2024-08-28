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
    internal abstract partial class DynaFlowProvider : System.Configuration.Provider.ProviderBase
    {
        static DynaFlowProvider _instance;
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
            DynaFlowProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DynaFlowProvider Instance
        {
            get
            {
                DynaFlowProvider tempInstance;
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
        static DynaFlowProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowProviderConfiguration config = DynaFlowProviderConfiguration.GetConfig();
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
        static DynaFlowProvider LoadProvider()
        {
            DynaFlowProviderConfiguration config = DynaFlowProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DynaFlowProvider LoadProvider(DynaFlowProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DynaFlowProvider _instanceLoader;
            cacheKey = "FS.Farm.DynaFlowProvider::" + providerName;
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
                _instanceLoader = (DynaFlowProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DynaFlowProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DynaFlowProvider.Type);
                    _instanceLoader = (DynaFlowProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DynaFlowProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DynaFlowProvider.Attributes.Add("connectionString", cString);
                    if (DynaFlowProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DynaFlowProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DynaFlowProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DynaFlowProvider.Name, DynaFlowProvider.Attributes);
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
        #region DynaFlows
        public abstract IDataReader GetDynaFlowList(SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowListAsync(SessionContext context);
        public abstract Guid GetDynaFlowCode(SessionContext context, int dynaFlowID);
        public abstract Task<Guid> GetDynaFlowCodeAsync(SessionContext context, int dynaFlowID);
        public abstract int GetDynaFlowID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDynaFlowIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDynaFlow(SessionContext context, int dynaFlowID);
        public abstract Task<IDataReader> GetDynaFlowAsync(SessionContext context, int dynaFlowID);
        public abstract IDataReader GetDirtyDynaFlow(SessionContext context, int dynaFlowID);
        public abstract Task<IDataReader> GetDirtyDynaFlowAsync(SessionContext context, int dynaFlowID);
        public abstract IDataReader GetDynaFlow(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDynaFlowAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDynaFlow(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDynaFlowAsync(SessionContext context, System.Guid code);
        public abstract void DynaFlowDelete(SessionContext context, int dynaFlowID);
        public abstract Task DynaFlowDeleteAsync(SessionContext context, int dynaFlowID);
        public abstract void DynaFlowCleanupTesting(SessionContext context);
        public abstract void DynaFlowCleanupChildObjectTesting(SessionContext context);
        public abstract int DynaFlowGetCount(SessionContext context);
        public abstract Task<int> DynaFlowGetCountAsync(SessionContext context);
        public abstract int DynaFlowGetMaxID(SessionContext context);
        public abstract Task<int> DynaFlowGetMaxIDAsync(SessionContext context);
        public abstract int DynaFlowInsert(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            System.Guid code);
        public abstract Task<int> DynaFlowInsertAsync(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            System.Guid code);
        public abstract void DynaFlowUpdate(
            SessionContext context,
            int dynaFlowID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DynaFlowUpdateAsync(
            SessionContext context,
            int dynaFlowID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDynaFlows(
            SessionContext context,
            bool searchByDynaFlowID, int dynaFlowID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowID, Int32 dependencyDynaFlowID,
            bool searchByDescription, String description,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByIsBuildTaskDebugRequired, Boolean isBuildTaskDebugRequired,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsResubmitted, Boolean isResubmitted,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByIsTaskCreationStarted, Boolean isTaskCreationStarted,
            bool searchByIsTasksCreated, Boolean isTasksCreated,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByParam1, String param1,
            bool searchByParentDynaFlowID, Int32 parentDynaFlowID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRootDynaFlowID, Int32 rootDynaFlowID,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
            bool searchBySubjectCode, Guid subjectCode,
            bool searchByTaskCreationProcessorIdentifier, String taskCreationProcessorIdentifier,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDynaFlowsAsync(
            SessionContext context,
            bool searchByDynaFlowID, int dynaFlowID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowID, Int32 dependencyDynaFlowID,
            bool searchByDescription, String description,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByIsBuildTaskDebugRequired, Boolean isBuildTaskDebugRequired,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsResubmitted, Boolean isResubmitted,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByIsTaskCreationStarted, Boolean isTaskCreationStarted,
            bool searchByIsTasksCreated, Boolean isTasksCreated,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByParam1, String param1,
            bool searchByParentDynaFlowID, Int32 parentDynaFlowID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRootDynaFlowID, Int32 rootDynaFlowID,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
            bool searchBySubjectCode, Guid subjectCode,
            bool searchByTaskCreationProcessorIdentifier, String taskCreationProcessorIdentifier,
            bool searchByCode, System.Guid code
            );
        public abstract int DynaFlowBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract Task<int> DynaFlowBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract int DynaFlowBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract Task<int> DynaFlowBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract int DynaFlowBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract Task<int> DynaFlowBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDynaFlowList_FetchByDynaFlowTypeID(
            int dynaFlowTypeID,
           SessionContext context);
        public abstract IDataReader GetDynaFlowList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowList_FetchByDynaFlowTypeIDAsync(
            int dynaFlowTypeID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
