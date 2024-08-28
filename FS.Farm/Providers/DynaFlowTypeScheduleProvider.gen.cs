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
    internal abstract partial class DynaFlowTypeScheduleProvider : System.Configuration.Provider.ProviderBase
    {
        static DynaFlowTypeScheduleProvider _instance;
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
            DynaFlowTypeScheduleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTypeScheduleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static DynaFlowTypeScheduleProvider Instance
        {
            get
            {
                DynaFlowTypeScheduleProvider tempInstance;
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
        static DynaFlowTypeScheduleProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            DynaFlowTypeScheduleProviderConfiguration config = DynaFlowTypeScheduleProviderConfiguration.GetConfig();
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
        static DynaFlowTypeScheduleProvider LoadProvider()
        {
            DynaFlowTypeScheduleProviderConfiguration config = DynaFlowTypeScheduleProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static DynaFlowTypeScheduleProvider LoadProvider(DynaFlowTypeScheduleProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            DynaFlowTypeScheduleProvider _instanceLoader;
            cacheKey = "FS.Farm.DynaFlowTypeScheduleProvider::" + providerName;
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
                _instanceLoader = (DynaFlowTypeScheduleProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider DynaFlowTypeScheduleProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(DynaFlowTypeScheduleProvider.Type);
                    _instanceLoader = (DynaFlowTypeScheduleProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = DynaFlowTypeScheduleProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //DynaFlowTypeScheduleProvider.Attributes.Add("connectionString", cString);
                    if (DynaFlowTypeScheduleProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        DynaFlowTypeScheduleProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        DynaFlowTypeScheduleProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(DynaFlowTypeScheduleProvider.Name, DynaFlowTypeScheduleProvider.Attributes);
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
        #region DynaFlowTypeSchedules
        public abstract IDataReader GetDynaFlowTypeScheduleList(SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTypeScheduleListAsync(SessionContext context);
        public abstract Guid GetDynaFlowTypeScheduleCode(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract Task<Guid> GetDynaFlowTypeScheduleCodeAsync(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract int GetDynaFlowTypeScheduleID(SessionContext context, System.Guid code);
        public abstract Task<int> GetDynaFlowTypeScheduleIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDynaFlowTypeSchedule(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract Task<IDataReader> GetDynaFlowTypeScheduleAsync(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract IDataReader GetDirtyDynaFlowTypeSchedule(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract Task<IDataReader> GetDirtyDynaFlowTypeScheduleAsync(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract IDataReader GetDynaFlowTypeSchedule(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDynaFlowTypeScheduleAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyDynaFlowTypeSchedule(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyDynaFlowTypeScheduleAsync(SessionContext context, System.Guid code);
        public abstract void DynaFlowTypeScheduleDelete(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract Task DynaFlowTypeScheduleDeleteAsync(SessionContext context, int dynaFlowTypeScheduleID);
        public abstract void DynaFlowTypeScheduleCleanupTesting(SessionContext context);
        public abstract void DynaFlowTypeScheduleCleanupChildObjectTesting(SessionContext context);
        public abstract int DynaFlowTypeScheduleGetCount(SessionContext context);
        public abstract Task<int> DynaFlowTypeScheduleGetCountAsync(SessionContext context);
        public abstract int DynaFlowTypeScheduleGetMaxID(SessionContext context);
        public abstract Task<int> DynaFlowTypeScheduleGetMaxIDAsync(SessionContext context);
        public abstract int DynaFlowTypeScheduleInsert(
            SessionContext context,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> DynaFlowTypeScheduleInsertAsync(
            SessionContext context,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            System.Guid code);
        public abstract void DynaFlowTypeScheduleUpdate(
            SessionContext context,
            int dynaFlowTypeScheduleID,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task DynaFlowTypeScheduleUpdateAsync(
            SessionContext context,
            int dynaFlowTypeScheduleID,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchDynaFlowTypeSchedules(
            SessionContext context,
            bool searchByDynaFlowTypeScheduleID, int dynaFlowTypeScheduleID,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByFrequencyInHours, Int32 frequencyInHours,
            bool searchByIsActive, Boolean isActive,
            bool searchByLastUTCDateTime, DateTime lastUTCDateTime,
            bool searchByNextUTCDateTime, DateTime nextUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchDynaFlowTypeSchedulesAsync(
            SessionContext context,
            bool searchByDynaFlowTypeScheduleID, int dynaFlowTypeScheduleID,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByFrequencyInHours, Int32 frequencyInHours,
            bool searchByIsActive, Boolean isActive,
            bool searchByLastUTCDateTime, DateTime lastUTCDateTime,
            bool searchByNextUTCDateTime, DateTime nextUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int DynaFlowTypeScheduleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract Task<int> DynaFlowTypeScheduleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract int DynaFlowTypeScheduleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract Task<int> DynaFlowTypeScheduleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract int DynaFlowTypeScheduleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract Task<int> DynaFlowTypeScheduleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeID(
            int dynaFlowTypeID,
           SessionContext context);
        public abstract IDataReader GetDynaFlowTypeScheduleList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeIDAsync(
            int dynaFlowTypeID,
           SessionContext context);
        public abstract Task<IDataReader> GetDynaFlowTypeScheduleList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
