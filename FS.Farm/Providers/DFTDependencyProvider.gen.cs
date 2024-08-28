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
    internal abstract partial class DFTDependencyProvider : System.Configuration.Provider.ProviderBase
    {
        static DFTDependencyProvider _instance;
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
            DFTDependencyProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DFTDependencyProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DFTDependencyProvider Instance
        {
            get
            {
                DFTDependencyProvider tempInstance;
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
        static DFTDependencyProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DFTDependencyProviderConfiguration config = DFTDependencyProviderConfiguration.GetConfig();
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
        static DFTDependencyProvider LoadProvider()
        {
            DFTDependencyProviderConfiguration config = DFTDependencyProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DFTDependencyProvider LoadProvider(DFTDependencyProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DFTDependencyProvider _instanceLoader;
            cacheKey = "FS.Farm.DFTDependencyProvider::" + providerName;
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
                _instanceLoader = (DFTDependencyProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DFTDependencyProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DFTDependencyProvider.Type);
                    _instanceLoader = (DFTDependencyProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DFTDependencyProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DFTDependencyProvider.Attributes.Add("connectionString", cString);
                    if (DFTDependencyProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DFTDependencyProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DFTDependencyProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DFTDependencyProvider.Name, DFTDependencyProvider.Attributes);
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
        #region DFTDependencys
        public abstract IDataReader GetDFTDependencyList(SessionContext context);
        public abstract Task<IDataReader> GetDFTDependencyListAsync(SessionContext context);
        public abstract Guid GetDFTDependencyCode(SessionContext context, int dFTDependencyID);
        public abstract Task<Guid> GetDFTDependencyCodeAsync(SessionContext context, int dFTDependencyID);
        public abstract int GetDFTDependencyID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDFTDependencyIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDFTDependency(SessionContext context, int dFTDependencyID);
        public abstract Task<IDataReader> GetDFTDependencyAsync(SessionContext context, int dFTDependencyID);
        public abstract IDataReader GetDirtyDFTDependency(SessionContext context, int dFTDependencyID);
        public abstract Task<IDataReader> GetDirtyDFTDependencyAsync(SessionContext context, int dFTDependencyID);
        public abstract IDataReader GetDFTDependency(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDFTDependencyAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDFTDependency(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDFTDependencyAsync(SessionContext context, System.Guid code);
        public abstract void DFTDependencyDelete(SessionContext context, int dFTDependencyID);
        public abstract Task DFTDependencyDeleteAsync(SessionContext context, int dFTDependencyID);
        public abstract void DFTDependencyCleanupTesting(SessionContext context);
        public abstract void DFTDependencyCleanupChildObjectTesting(SessionContext context);
        public abstract int DFTDependencyGetCount(SessionContext context);
        public abstract Task<int> DFTDependencyGetCountAsync(SessionContext context);
        public abstract int DFTDependencyGetMaxID(SessionContext context);
        public abstract Task<int> DFTDependencyGetMaxIDAsync(SessionContext context);
        public abstract int DFTDependencyInsert(
            SessionContext context,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            System.Guid code);
        public abstract Task<int> DFTDependencyInsertAsync(
            SessionContext context,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            System.Guid code);
        public abstract void DFTDependencyUpdate(
            SessionContext context,
            int dFTDependencyID,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DFTDependencyUpdateAsync(
            SessionContext context,
            int dFTDependencyID,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDFTDependencys(
            SessionContext context,
            bool searchByDFTDependencyID, int dFTDependencyID,
            bool searchByDependencyDFTaskID, Int32 dependencyDFTaskID,
            bool searchByDynaFlowTaskID, Int32 dynaFlowTaskID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDFTDependencysAsync(
            SessionContext context,
            bool searchByDFTDependencyID, int dFTDependencyID,
            bool searchByDependencyDFTaskID, Int32 dependencyDFTaskID,
            bool searchByDynaFlowTaskID, Int32 dynaFlowTaskID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByCode, System.Guid code
            );
        public abstract int DFTDependencyBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract Task<int> DFTDependencyBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract int DFTDependencyBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract Task<int> DFTDependencyBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract int DFTDependencyBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract Task<int> DFTDependencyBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDFTDependencyList_FetchByDynaFlowTaskID(
            int dynaFlowTaskID,
           SessionContext context);
        public abstract Task<IDataReader> GetDFTDependencyList_FetchByDynaFlowTaskIDAsync(
            int dynaFlowTaskID,
           SessionContext context);

    }
}
