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
    internal abstract partial class RoleProvider : System.Configuration.Provider.ProviderBase
    {
        static RoleProvider _instance;
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
            RoleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            RoleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static RoleProvider Instance
        {
            get
            {
                RoleProvider tempInstance;
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
        static RoleProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            RoleProviderConfiguration config = RoleProviderConfiguration.GetConfig();
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
        static RoleProvider LoadProvider()
        {
            RoleProviderConfiguration config = RoleProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static RoleProvider LoadProvider(RoleProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            RoleProvider _instanceLoader;
            cacheKey = "FS.Farm.RoleProvider::" + providerName;
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
                _instanceLoader = (RoleProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider RoleProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(RoleProvider.Type);
                    _instanceLoader = (RoleProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = RoleProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //RoleProvider.Attributes.Add("connectionString", cString);
                    if (RoleProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        RoleProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        RoleProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(RoleProvider.Name, RoleProvider.Attributes);
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
        #region Roles
        public abstract IDataReader GetRoleList(SessionContext context);
        public abstract Task<IDataReader> GetRoleListAsync(SessionContext context);
        public abstract Guid GetRoleCode(SessionContext context, int roleID);
        public abstract Task<Guid> GetRoleCodeAsync(SessionContext context, int roleID);
        public abstract int GetRoleID(SessionContext context, System.Guid code);
        public abstract Task<int> GetRoleIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetRole(SessionContext context, int roleID);
        public abstract Task<IDataReader> GetRoleAsync(SessionContext context, int roleID);
        public abstract IDataReader GetDirtyRole(SessionContext context, int roleID);
        public abstract Task<IDataReader> GetDirtyRoleAsync(SessionContext context, int roleID);
        public abstract IDataReader GetRole(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetRoleAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyRole(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyRoleAsync(SessionContext context, System.Guid code);
        public abstract void RoleDelete(SessionContext context, int roleID);
        public abstract Task RoleDeleteAsync(SessionContext context, int roleID);
        public abstract void RoleCleanupTesting(SessionContext context);
        public abstract void RoleCleanupChildObjectTesting(SessionContext context);
        public abstract int RoleGetCount(SessionContext context);
        public abstract Task<int> RoleGetCountAsync(SessionContext context);
        public abstract int RoleGetMaxID(SessionContext context);
        public abstract Task<int> RoleGetMaxIDAsync(SessionContext context);
        public abstract int RoleInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract Task<int> RoleInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code);
        public abstract void RoleUpdate(
            SessionContext context,
            int roleID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task RoleUpdateAsync(
            SessionContext context,
            int roleID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchRoles(
            SessionContext context,
            bool searchByRoleID, int roleID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchRolesAsync(
            SessionContext context,
            bool searchByRoleID, int roleID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code
            );
        public abstract int RoleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract Task<int> RoleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract int RoleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract Task<int> RoleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract int RoleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract Task<int> RoleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetRoleList_FetchByPacID(
            int pacID,
           SessionContext context);
        public abstract Task<IDataReader> GetRoleList_FetchByPacIDAsync(
            int pacID,
           SessionContext context);

    }
}
