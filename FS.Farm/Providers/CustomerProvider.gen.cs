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
    internal abstract partial class CustomerProvider : System.Configuration.Provider.ProviderBase
    {
        static CustomerProvider _instance;
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
            CustomerProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            CustomerProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static CustomerProvider Instance
        {
            get
            {
                CustomerProvider tempInstance;
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
        static CustomerProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            CustomerProviderConfiguration config = CustomerProviderConfiguration.GetConfig();
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
        static CustomerProvider LoadProvider()
        {
            CustomerProviderConfiguration config = CustomerProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static CustomerProvider LoadProvider(CustomerProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            CustomerProvider _instanceLoader;
            cacheKey = "FS.Farm.CustomerProvider::" + providerName;
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
                _instanceLoader = (CustomerProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider CustomerProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(CustomerProvider.Type);
                    _instanceLoader = (CustomerProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = CustomerProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //CustomerProvider.Attributes.Add("connectionString", cString);
                    if (CustomerProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        CustomerProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        CustomerProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(CustomerProvider.Name, CustomerProvider.Attributes);
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
        #region Customers
        public abstract IDataReader GetCustomerList(SessionContext context);
        public abstract Task<IDataReader> GetCustomerListAsync(SessionContext context);
        public abstract Guid GetCustomerCode(SessionContext context, int customerID);
        public abstract Task<Guid> GetCustomerCodeAsync(SessionContext context, int customerID);
        public abstract int GetCustomerID(SessionContext context, System.Guid code);
        public abstract Task<int> GetCustomerIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetCustomer(SessionContext context, int customerID);
        public abstract Task<IDataReader> GetCustomerAsync(SessionContext context, int customerID);
        public abstract IDataReader GetDirtyCustomer(SessionContext context, int customerID);
        public abstract Task<IDataReader> GetDirtyCustomerAsync(SessionContext context, int customerID);
        public abstract IDataReader GetCustomer(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetCustomerAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyCustomer(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyCustomerAsync(SessionContext context, System.Guid code);
        public abstract void CustomerDelete(SessionContext context, int customerID);
        public abstract Task CustomerDeleteAsync(SessionContext context, int customerID);
        public abstract void CustomerCleanupTesting(SessionContext context);
        public abstract void CustomerCleanupChildObjectTesting(SessionContext context);
        public abstract int CustomerGetCount(SessionContext context);
        public abstract Task<int> CustomerGetCountAsync(SessionContext context);
        public abstract int CustomerGetMaxID(SessionContext context);
        public abstract Task<int> CustomerGetMaxIDAsync(SessionContext context);
        public abstract int CustomerInsert(
            SessionContext context,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            System.Guid code);
        public abstract Task<int> CustomerInsertAsync(
            SessionContext context,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            System.Guid code);
        public abstract void CustomerUpdate(
            SessionContext context,
            int customerID,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            Guid lastChangeCode,
            System.Guid code);
        public abstract Task CustomerUpdateAsync(
            SessionContext context,
            int customerID,
            Int32 activeOrganizationID,
            String email,
            DateTime emailConfirmedUTCDateTime,
            String firstName,
            DateTime forgotPasswordKeyExpirationUTCDateTime,
            String forgotPasswordKeyValue,
            Guid fSUserCodeValue,
            Boolean isActive,
            Boolean isEmailAllowed,
            Boolean isEmailConfirmed,
            Boolean isEmailMarketingAllowed,
            Boolean isLocked,
            Boolean isMultipleOrganizationsAllowed,
            Boolean isVerboseLoggingForced,
            DateTime lastLoginUTCDateTime,
            String lastName,
            String password,
            String phone,
            String province,
            DateTime registrationUTCDateTime,
            Int32 tacID,
            Int32 uTCOffsetInMinutes,
            String zip,
            Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchCustomers(
            SessionContext context,
            bool searchByCustomerID, int customerID,
            bool searchByActiveOrganizationID, Int32 activeOrganizationID,
            bool searchByEmail, String email,
            bool searchByEmailConfirmedUTCDateTime, DateTime emailConfirmedUTCDateTime,
            bool searchByFirstName, String firstName,
            bool searchByForgotPasswordKeyExpirationUTCDateTime, DateTime forgotPasswordKeyExpirationUTCDateTime,
            bool searchByForgotPasswordKeyValue, String forgotPasswordKeyValue,
            bool searchByFSUserCodeValue, Guid fSUserCodeValue,
            bool searchByIsActive, Boolean isActive,
            bool searchByIsEmailAllowed, Boolean isEmailAllowed,
            bool searchByIsEmailConfirmed, Boolean isEmailConfirmed,
            bool searchByIsEmailMarketingAllowed, Boolean isEmailMarketingAllowed,
            bool searchByIsLocked, Boolean isLocked,
            bool searchByIsMultipleOrganizationsAllowed, Boolean isMultipleOrganizationsAllowed,
            bool searchByIsVerboseLoggingForced, Boolean isVerboseLoggingForced,
            bool searchByLastLoginUTCDateTime, DateTime lastLoginUTCDateTime,
            bool searchByLastName, String lastName,
            bool searchByPassword, String password,
            bool searchByPhone, String phone,
            bool searchByProvince, String province,
            bool searchByRegistrationUTCDateTime, DateTime registrationUTCDateTime,
            bool searchByTacID, Int32 tacID,
            bool searchByUTCOffsetInMinutes, Int32 uTCOffsetInMinutes,
            bool searchByZip, String zip,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchCustomersAsync(
            SessionContext context,
            bool searchByCustomerID, int customerID,
            bool searchByActiveOrganizationID, Int32 activeOrganizationID,
            bool searchByEmail, String email,
            bool searchByEmailConfirmedUTCDateTime, DateTime emailConfirmedUTCDateTime,
            bool searchByFirstName, String firstName,
            bool searchByForgotPasswordKeyExpirationUTCDateTime, DateTime forgotPasswordKeyExpirationUTCDateTime,
            bool searchByForgotPasswordKeyValue, String forgotPasswordKeyValue,
            bool searchByFSUserCodeValue, Guid fSUserCodeValue,
            bool searchByIsActive, Boolean isActive,
            bool searchByIsEmailAllowed, Boolean isEmailAllowed,
            bool searchByIsEmailConfirmed, Boolean isEmailConfirmed,
            bool searchByIsEmailMarketingAllowed, Boolean isEmailMarketingAllowed,
            bool searchByIsLocked, Boolean isLocked,
            bool searchByIsMultipleOrganizationsAllowed, Boolean isMultipleOrganizationsAllowed,
            bool searchByIsVerboseLoggingForced, Boolean isVerboseLoggingForced,
            bool searchByLastLoginUTCDateTime, DateTime lastLoginUTCDateTime,
            bool searchByLastName, String lastName,
            bool searchByPassword, String password,
            bool searchByPhone, String phone,
            bool searchByProvince, String province,
            bool searchByRegistrationUTCDateTime, DateTime registrationUTCDateTime,
            bool searchByTacID, Int32 tacID,
            bool searchByUTCOffsetInMinutes, Int32 uTCOffsetInMinutes,
            bool searchByZip, String zip,
            bool searchByCode, System.Guid code
            );
        public abstract int CustomerBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract Task<int> CustomerBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract int CustomerBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract Task<int> CustomerBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract int CustomerBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract Task<int> CustomerBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Customer> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetCustomerList_FetchByTacID(
            int tacID,
           SessionContext context);
        public abstract Task<IDataReader> GetCustomerList_FetchByTacIDAsync(
            int tacID,
           SessionContext context);
        public abstract IDataReader GetCustomerList_QueryByEmail(
            String email,
           SessionContext context);
        public abstract Task<IDataReader> GetCustomerList_QueryByEmailAsync(
            String email,
           SessionContext context);
        public abstract IDataReader GetCustomerList_QueryByForgotPasswordKeyValue(
            String forgotPasswordKeyValue,
           SessionContext context);
        public abstract Task<IDataReader> GetCustomerList_QueryByForgotPasswordKeyValueAsync(
            String forgotPasswordKeyValue,
           SessionContext context);
    }
}
