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
    internal abstract partial class CustomerRoleProvider : System.Configuration.Provider.ProviderBase
    {
        static CustomerRoleProvider _instance;
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
            CustomerRoleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            CustomerRoleProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static CustomerRoleProvider Instance
        {
            get
            {
                CustomerRoleProvider tempInstance;
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
        static CustomerRoleProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            CustomerRoleProviderConfiguration config = CustomerRoleProviderConfiguration.GetConfig();
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
        static CustomerRoleProvider LoadProvider()
        {
            CustomerRoleProviderConfiguration config = CustomerRoleProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static CustomerRoleProvider LoadProvider(CustomerRoleProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            CustomerRoleProvider _instanceLoader;
            cacheKey = "FS.Farm.CustomerRoleProvider::" + providerName;
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
                _instanceLoader = (CustomerRoleProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider CustomerRoleProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(CustomerRoleProvider.Type);
                    _instanceLoader = (CustomerRoleProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = CustomerRoleProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //CustomerRoleProvider.Attributes.Add("connectionString", cString);
                    if (CustomerRoleProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        CustomerRoleProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        CustomerRoleProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(CustomerRoleProvider.Name, CustomerRoleProvider.Attributes);
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
        #region CustomerRoles
        public abstract IDataReader GetCustomerRoleList(SessionContext context);
        public abstract Task<IDataReader> GetCustomerRoleListAsync(SessionContext context);
        public abstract Guid GetCustomerRoleCode(SessionContext context, int customerRoleID);
        public abstract Task<Guid> GetCustomerRoleCodeAsync(SessionContext context, int customerRoleID);
        public abstract int GetCustomerRoleID(SessionContext context, System.Guid code);
        public abstract Task<int> GetCustomerRoleIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetCustomerRole(SessionContext context, int customerRoleID);
        public abstract Task<IDataReader> GetCustomerRoleAsync(SessionContext context, int customerRoleID);
        public abstract IDataReader GetDirtyCustomerRole(SessionContext context, int customerRoleID);
        public abstract Task<IDataReader> GetDirtyCustomerRoleAsync(SessionContext context, int customerRoleID);
        public abstract IDataReader GetCustomerRole(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetCustomerRoleAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyCustomerRole(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyCustomerRoleAsync(SessionContext context, System.Guid code);
        public abstract void CustomerRoleDelete(SessionContext context, int customerRoleID);
        public abstract Task CustomerRoleDeleteAsync(SessionContext context, int customerRoleID);
        public abstract void CustomerRoleCleanupTesting(SessionContext context);
        public abstract void CustomerRoleCleanupChildObjectTesting(SessionContext context);
        public abstract int CustomerRoleGetCount(SessionContext context);
        public abstract Task<int> CustomerRoleGetCountAsync(SessionContext context);
        public abstract int CustomerRoleGetMaxID(SessionContext context);
        public abstract Task<int> CustomerRoleGetMaxIDAsync(SessionContext context);
        public abstract int CustomerRoleInsert(
            SessionContext context,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
            System.Guid code);
        public abstract Task<int> CustomerRoleInsertAsync(
            SessionContext context,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
            System.Guid code);
        public abstract void CustomerRoleUpdate(
            SessionContext context,
            int customerRoleID,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task CustomerRoleUpdateAsync(
            SessionContext context,
            int customerRoleID,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchCustomerRoles(
            SessionContext context,
            bool searchByCustomerRoleID, int customerRoleID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByPlaceholder, Boolean placeholder,
            bool searchByRoleID, Int32 roleID,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchCustomerRolesAsync(
            SessionContext context,
            bool searchByCustomerRoleID, int customerRoleID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByPlaceholder, Boolean placeholder,
            bool searchByRoleID, Int32 roleID,
            bool searchByCode, System.Guid code
            );
        public abstract int CustomerRoleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract Task<int> CustomerRoleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract int CustomerRoleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract Task<int> CustomerRoleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract int CustomerRoleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract Task<int> CustomerRoleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetCustomerRoleList_FetchByCustomerID(
            int customerID,
           SessionContext context);
        public abstract IDataReader GetCustomerRoleList_FetchByRoleID(
            int roleID,
           SessionContext context);
        public abstract Task<IDataReader> GetCustomerRoleList_FetchByCustomerIDAsync(
            int customerID,
           SessionContext context);
        public abstract Task<IDataReader> GetCustomerRoleList_FetchByRoleIDAsync(
            int roleID,
           SessionContext context);
    }
}
