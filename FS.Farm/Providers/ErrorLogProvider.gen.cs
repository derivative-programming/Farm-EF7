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
    internal abstract partial class ErrorLogProvider : System.Configuration.Provider.ProviderBase
    {
        static ErrorLogProvider _instance;
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
            ErrorLogProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            ErrorLogProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static ErrorLogProvider Instance
        {
            get
            {
                ErrorLogProvider tempInstance;
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
        static ErrorLogProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            ErrorLogProviderConfiguration config = ErrorLogProviderConfiguration.GetConfig();
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
        static ErrorLogProvider LoadProvider()
        {
            ErrorLogProviderConfiguration config = ErrorLogProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static ErrorLogProvider LoadProvider(ErrorLogProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            ErrorLogProvider _instanceLoader;
            cacheKey = "FS.Farm.ErrorLogProvider::" + providerName;
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
                _instanceLoader = (ErrorLogProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider ErrorLogProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(ErrorLogProvider.Type);
                    _instanceLoader = (ErrorLogProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = ErrorLogProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //ErrorLogProvider.Attributes.Add("connectionString", cString);
                    if (ErrorLogProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        ErrorLogProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        ErrorLogProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(ErrorLogProvider.Name, ErrorLogProvider.Attributes);
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
        #region ErrorLogs
        public abstract IDataReader GetErrorLogList(SessionContext context);
        public abstract Task<IDataReader> GetErrorLogListAsync(SessionContext context);
        public abstract Guid GetErrorLogCode(SessionContext context, int errorLogID);
        public abstract Task<Guid> GetErrorLogCodeAsync(SessionContext context, int errorLogID);
        public abstract int GetErrorLogID(SessionContext context, System.Guid code);
        public abstract Task<int> GetErrorLogIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetErrorLog(SessionContext context, int errorLogID);
        public abstract Task<IDataReader> GetErrorLogAsync(SessionContext context, int errorLogID);
        public abstract IDataReader GetDirtyErrorLog(SessionContext context, int errorLogID);
        public abstract Task<IDataReader> GetDirtyErrorLogAsync(SessionContext context, int errorLogID);
        public abstract IDataReader GetErrorLog(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetErrorLogAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyErrorLog(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyErrorLogAsync(SessionContext context, System.Guid code);
        public abstract void ErrorLogDelete(SessionContext context, int errorLogID);
        public abstract Task ErrorLogDeleteAsync(SessionContext context, int errorLogID);
        public abstract void ErrorLogCleanupTesting(SessionContext context);
        public abstract void ErrorLogCleanupChildObjectTesting(SessionContext context);
        public abstract int ErrorLogGetCount(SessionContext context);
        public abstract Task<int> ErrorLogGetCountAsync(SessionContext context);
        public abstract int ErrorLogGetMaxID(SessionContext context);
        public abstract Task<int> ErrorLogGetMaxIDAsync(SessionContext context);
        public abstract int ErrorLogInsert(
            SessionContext context,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            System.Guid code);
        public abstract Task<int> ErrorLogInsertAsync(
            SessionContext context,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            System.Guid code);
        public abstract void ErrorLogUpdate(
            SessionContext context,
            int errorLogID,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task ErrorLogUpdateAsync(
            SessionContext context,
            int errorLogID,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchErrorLogs(
            SessionContext context,
            bool searchByErrorLogID, int errorLogID,
            bool searchByBrowserCode, Guid browserCode,
            bool searchByContextCode, Guid contextCode,
            bool searchByCreatedUTCDateTime, DateTime createdUTCDateTime,
            bool searchByDescription, String description,
            bool searchByIsClientSideError, Boolean isClientSideError,
            bool searchByIsResolved, Boolean isResolved,
            bool searchByPacID, Int32 pacID,
            bool searchByUrl, String url,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchErrorLogsAsync(
            SessionContext context,
            bool searchByErrorLogID, int errorLogID,
            bool searchByBrowserCode, Guid browserCode,
            bool searchByContextCode, Guid contextCode,
            bool searchByCreatedUTCDateTime, DateTime createdUTCDateTime,
            bool searchByDescription, String description,
            bool searchByIsClientSideError, Boolean isClientSideError,
            bool searchByIsResolved, Boolean isResolved,
            bool searchByPacID, Int32 pacID,
            bool searchByUrl, String url,
            bool searchByCode, System.Guid code
            );
        public abstract int ErrorLogBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract Task<int> ErrorLogBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract int ErrorLogBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract Task<int> ErrorLogBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract int ErrorLogBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract Task<int> ErrorLogBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetErrorLogList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetErrorLogList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);
    }
}
