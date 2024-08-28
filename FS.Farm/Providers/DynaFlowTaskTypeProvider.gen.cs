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
    internal abstract partial class DynaFlowTaskTypeProvider : System.Configuration.Provider.ProviderBase
    {
        static DynaFlowTaskTypeProvider _instance;
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
            DynaFlowTaskTypeProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTaskTypeProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DynaFlowTaskTypeProvider Instance
        {
            get
            {
                DynaFlowTaskTypeProvider tempInstance;
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
        static DynaFlowTaskTypeProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTaskTypeProviderConfiguration config = DynaFlowTaskTypeProviderConfiguration.GetConfig();
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
        static DynaFlowTaskTypeProvider LoadProvider()
        {
            DynaFlowTaskTypeProviderConfiguration config = DynaFlowTaskTypeProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DynaFlowTaskTypeProvider LoadProvider(DynaFlowTaskTypeProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DynaFlowTaskTypeProvider _instanceLoader;
            cacheKey = "FS.Farm.DynaFlowTaskTypeProvider::" + providerName;
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
                _instanceLoader = (DynaFlowTaskTypeProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DynaFlowTaskTypeProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DynaFlowTaskTypeProvider.Type);
                    _instanceLoader = (DynaFlowTaskTypeProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DynaFlowTaskTypeProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DynaFlowTaskTypeProvider.Attributes.Add("connectionString", cString);
                    if (DynaFlowTaskTypeProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DynaFlowTaskTypeProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DynaFlowTaskTypeProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DynaFlowTaskTypeProvider.Name, DynaFlowTaskTypeProvider.Attributes);
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
        #region DynaFlowTaskTypes
        public abstract IDataReader GetDynaFlowTaskTypeList(SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTaskTypeListAsync(SessionContext context);
        public abstract Guid GetDynaFlowTaskTypeCode(SessionContext context, int dynaFlowTaskTypeID);
        public abstract Task<Guid> GetDynaFlowTaskTypeCodeAsync(SessionContext context, int dynaFlowTaskTypeID);
        public abstract int GetDynaFlowTaskTypeID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDynaFlowTaskTypeIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDynaFlowTaskType(SessionContext context, int dynaFlowTaskTypeID);
        public abstract Task<IDataReader> GetDynaFlowTaskTypeAsync(SessionContext context, int dynaFlowTaskTypeID);
        public abstract IDataReader GetDirtyDynaFlowTaskType(SessionContext context, int dynaFlowTaskTypeID);
        public abstract Task<IDataReader> GetDirtyDynaFlowTaskTypeAsync(SessionContext context, int dynaFlowTaskTypeID);
        public abstract IDataReader GetDynaFlowTaskType(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDynaFlowTaskTypeAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDynaFlowTaskType(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDynaFlowTaskTypeAsync(SessionContext context, System.Guid code);
        public abstract void DynaFlowTaskTypeDelete(SessionContext context, int dynaFlowTaskTypeID);
        public abstract Task DynaFlowTaskTypeDeleteAsync(SessionContext context, int dynaFlowTaskTypeID);
        public abstract void DynaFlowTaskTypeCleanupTesting(SessionContext context);
        public abstract void DynaFlowTaskTypeCleanupChildObjectTesting(SessionContext context);
        public abstract int DynaFlowTaskTypeGetCount(SessionContext context);
        public abstract Task<int> DynaFlowTaskTypeGetCountAsync(SessionContext context);
        public abstract int DynaFlowTaskTypeGetMaxID(SessionContext context);
        public abstract Task<int> DynaFlowTaskTypeGetMaxIDAsync(SessionContext context);
        public abstract int DynaFlowTaskTypeInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> DynaFlowTaskTypeInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void DynaFlowTaskTypeUpdate(
            SessionContext context,
            int dynaFlowTaskTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DynaFlowTaskTypeUpdateAsync(
            SessionContext context,
            int dynaFlowTaskTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDynaFlowTaskTypes(
            SessionContext context,
            bool searchByDynaFlowTaskTypeID, int dynaFlowTaskTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDynaFlowTaskTypesAsync(
            SessionContext context,
            bool searchByDynaFlowTaskTypeID, int dynaFlowTaskTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int DynaFlowTaskTypeBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract Task<int> DynaFlowTaskTypeBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract int DynaFlowTaskTypeBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract Task<int> DynaFlowTaskTypeBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract int DynaFlowTaskTypeBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract Task<int> DynaFlowTaskTypeBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDynaFlowTaskTypeList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTaskTypeList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
