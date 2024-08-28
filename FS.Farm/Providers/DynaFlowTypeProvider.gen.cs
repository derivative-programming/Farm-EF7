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
    internal abstract partial class DynaFlowTypeProvider : System.Configuration.Provider.ProviderBase
    {
        static DynaFlowTypeProvider _instance;
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
            DynaFlowTypeProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTypeProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DynaFlowTypeProvider Instance
        {
            get
            {
                DynaFlowTypeProvider tempInstance;
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
        static DynaFlowTypeProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTypeProviderConfiguration config = DynaFlowTypeProviderConfiguration.GetConfig();
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
        static DynaFlowTypeProvider LoadProvider()
        {
            DynaFlowTypeProviderConfiguration config = DynaFlowTypeProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DynaFlowTypeProvider LoadProvider(DynaFlowTypeProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DynaFlowTypeProvider _instanceLoader;
            cacheKey = "FS.Farm.DynaFlowTypeProvider::" + providerName;
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
                _instanceLoader = (DynaFlowTypeProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DynaFlowTypeProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DynaFlowTypeProvider.Type);
                    _instanceLoader = (DynaFlowTypeProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DynaFlowTypeProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DynaFlowTypeProvider.Attributes.Add("connectionString", cString);
                    if (DynaFlowTypeProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DynaFlowTypeProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DynaFlowTypeProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DynaFlowTypeProvider.Name, DynaFlowTypeProvider.Attributes);
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
        #region DynaFlowTypes
        public abstract IDataReader GetDynaFlowTypeList(SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTypeListAsync(SessionContext context);
        public abstract Guid GetDynaFlowTypeCode(SessionContext context, int dynaFlowTypeID);
        public abstract Task<Guid> GetDynaFlowTypeCodeAsync(SessionContext context, int dynaFlowTypeID);
        public abstract int GetDynaFlowTypeID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDynaFlowTypeIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDynaFlowType(SessionContext context, int dynaFlowTypeID);
        public abstract Task<IDataReader> GetDynaFlowTypeAsync(SessionContext context, int dynaFlowTypeID);
        public abstract IDataReader GetDirtyDynaFlowType(SessionContext context, int dynaFlowTypeID);
        public abstract Task<IDataReader> GetDirtyDynaFlowTypeAsync(SessionContext context, int dynaFlowTypeID);
        public abstract IDataReader GetDynaFlowType(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDynaFlowTypeAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDynaFlowType(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDynaFlowTypeAsync(SessionContext context, System.Guid code);
        public abstract void DynaFlowTypeDelete(SessionContext context, int dynaFlowTypeID);
        public abstract Task DynaFlowTypeDeleteAsync(SessionContext context, int dynaFlowTypeID);
        public abstract void DynaFlowTypeCleanupTesting(SessionContext context);
        public abstract void DynaFlowTypeCleanupChildObjectTesting(SessionContext context);
        public abstract int DynaFlowTypeGetCount(SessionContext context);
        public abstract Task<int> DynaFlowTypeGetCountAsync(SessionContext context);
        public abstract int DynaFlowTypeGetMaxID(SessionContext context);
        public abstract Task<int> DynaFlowTypeGetMaxIDAsync(SessionContext context);
        public abstract int DynaFlowTypeInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            System.Guid code);
        public abstract Task<int> DynaFlowTypeInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            System.Guid code);
        public abstract void DynaFlowTypeUpdate(
            SessionContext context,
            int dynaFlowTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DynaFlowTypeUpdateAsync(
            SessionContext context,
            int dynaFlowTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDynaFlowTypes(
            SessionContext context,
            bool searchByDynaFlowTypeID, int dynaFlowTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDynaFlowTypesAsync(
            SessionContext context,
            bool searchByDynaFlowTypeID, int dynaFlowTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByCode, System.Guid code
            );
        public abstract int DynaFlowTypeBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract Task<int> DynaFlowTypeBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract int DynaFlowTypeBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract Task<int> DynaFlowTypeBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract int DynaFlowTypeBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract Task<int> DynaFlowTypeBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDynaFlowTypeList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTypeList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
