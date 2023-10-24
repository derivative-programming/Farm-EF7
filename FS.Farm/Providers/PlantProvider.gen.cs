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
    internal abstract partial class PlantProvider : System.Configuration.Provider.ProviderBase
    {
        static PlantProvider _instance;
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
            PlantProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider();
                _instance = tempInstance;
            }
        }
        public static void ForceDataProviderTypeInstance( FS.Base.Providers.DataProviderType dataProviderType)
        {
            PlantProvider tempInstance;
            lock (padLock)
            {
                tempInstance = LoadProvider(dataProviderType);
                _instance = tempInstance;
            }
        }
        public static PlantProvider Instance
        {
            get
            {
                PlantProvider tempInstance;
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
        static PlantProvider LoadProvider(FS.Base.Providers.DataProviderType dataProviderType)
        {
            PlantProviderConfiguration config = PlantProviderConfiguration.GetConfig();
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
        static PlantProvider LoadProvider()
        {
            PlantProviderConfiguration config = PlantProviderConfiguration.GetConfig();
            return LoadProvider(config,config.DefaultProvider);
        }
        static PlantProvider LoadProvider(PlantProviderConfiguration config, string providerName)
        {
            // Get the names of the providers
            // Use the cache because the reflection used later is expensive
            //Cache cache = System.Web.HttpRuntime.Cache;
            string cacheKey = null;
            PlantProvider _instanceLoader;
            cacheKey = "FS.Farm.PlantProvider::" + providerName;
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
                _instanceLoader = (PlantProvider)oProvider;
            }
            else
            {
                try
                {
                    // Read the configuration specific information for this provider
                    FS.Common.Providers.Provider PlantProvider = (FS.Common.Providers.Provider)config.Providers[providerName];
                    // The assembly should be in \bin or GAC
                    Type type = Type.GetType(PlantProvider.Type);
                    _instanceLoader = (PlantProvider)Activator.CreateInstance(type);
                    // Initialize the provider with the attributes.
                    string cStringName = PlantProvider.Attributes["connectionStringName"];
                    string cString = FS.Common.Configuration.ConnectionString.ReadConnectionString(cStringName);
                    //PlantProvider.Attributes.Add("connectionString", cString);
                    if (PlantProvider.Attributes.AllKeys.Contains("connectionString"))
                    {
                        PlantProvider.Attributes.Set("connectionString", cString);
                    }
                    else
                    {
                        PlantProvider.Attributes.Add("connectionString", cString);
                    }
                    _instanceLoader.Initialize(PlantProvider.Name, PlantProvider.Attributes);
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
        #region Plants
        public abstract IDataReader GetPlantList(SessionContext context);
        public abstract Task<IDataReader> GetPlantListAsync(SessionContext context);
        public abstract Guid GetPlantCode(SessionContext context, int plantID);
        public abstract Task<Guid> GetPlantCodeAsync(SessionContext context, int plantID);
        public abstract int GetPlantID(SessionContext context, System.Guid code);
        public abstract Task<int> GetPlantIDAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetPlant(SessionContext context, int plantID);
        public abstract Task<IDataReader> GetPlantAsync(SessionContext context, int plantID);
        public abstract IDataReader GetDirtyPlant(SessionContext context, int plantID);
        public abstract Task<IDataReader> GetDirtyPlantAsync(SessionContext context, int plantID);
        public abstract IDataReader GetPlant(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetPlantAsync(SessionContext context, System.Guid code);
        public abstract IDataReader GetDirtyPlant(SessionContext context, System.Guid code);
        public abstract Task<IDataReader> GetDirtyPlantAsync(SessionContext context, System.Guid code);
        public abstract void PlantDelete(SessionContext context, int plantID);
        public abstract Task PlantDeleteAsync(SessionContext context, int plantID);
        public abstract void PlantCleanupTesting(SessionContext context);
        public abstract void PlantCleanupChildObjectTesting(SessionContext context);
        public abstract int PlantGetCount(SessionContext context);
        public abstract Task<int> PlantGetCountAsync(SessionContext context);
        public abstract int PlantGetMaxID(SessionContext context);
        public abstract Task<int> PlantGetMaxIDAsync(SessionContext context);
        public abstract int PlantInsert(
            SessionContext context,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            System.Guid code);
        public abstract Task<int> PlantInsertAsync(
            SessionContext context,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            System.Guid code);
        public abstract void PlantUpdate(
            SessionContext context,
            int plantID,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
             Guid lastChangeCode,
            System.Guid code);
        public abstract Task PlantUpdateAsync(
            SessionContext context,
            int plantID,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
             Guid lastChangeCode,
            System.Guid code);
        public abstract IDataReader SearchPlants(
            SessionContext context,
            bool searchByPlantID, int plantID,
            bool searchByFlvrForeignKeyID, Int32 flvrForeignKeyID,
            bool searchByIsDeleteAllowed, Boolean isDeleteAllowed,
            bool searchByIsEditAllowed, Boolean isEditAllowed,
            bool searchByLandID, Int32 landID,
            bool searchByOtherFlavor, String otherFlavor,
            bool searchBySomeBigIntVal, Int64 someBigIntVal,
            bool searchBySomeBitVal, Boolean someBitVal,
            bool searchBySomeDateVal, DateTime someDateVal,
            bool searchBySomeDecimalVal, Decimal someDecimalVal,
            bool searchBySomeEmailAddress, String someEmailAddress,
            bool searchBySomeFloatVal, Double someFloatVal,
            bool searchBySomeIntVal, Int32 someIntVal,
            bool searchBySomeMoneyVal, Decimal someMoneyVal,
            bool searchBySomeNVarCharVal, String someNVarCharVal,
            bool searchBySomePhoneNumber, String somePhoneNumber,
            bool searchBySomeTextVal, String someTextVal,
            bool searchBySomeUniqueidentifierVal, Guid someUniqueidentifierVal,
            bool searchBySomeUTCDateTimeVal, DateTime someUTCDateTimeVal,
            bool searchBySomeVarCharVal, String someVarCharVal,
            bool searchByCode, System.Guid code
            );
        public abstract Task<IDataReader> SearchPlantsAsync(
            SessionContext context,
            bool searchByPlantID, int plantID,
            bool searchByFlvrForeignKeyID, Int32 flvrForeignKeyID,
            bool searchByIsDeleteAllowed, Boolean isDeleteAllowed,
            bool searchByIsEditAllowed, Boolean isEditAllowed,
            bool searchByLandID, Int32 landID,
            bool searchByOtherFlavor, String otherFlavor,
            bool searchBySomeBigIntVal, Int64 someBigIntVal,
            bool searchBySomeBitVal, Boolean someBitVal,
            bool searchBySomeDateVal, DateTime someDateVal,
            bool searchBySomeDecimalVal, Decimal someDecimalVal,
            bool searchBySomeEmailAddress, String someEmailAddress,
            bool searchBySomeFloatVal, Double someFloatVal,
            bool searchBySomeIntVal, Int32 someIntVal,
            bool searchBySomeMoneyVal, Decimal someMoneyVal,
            bool searchBySomeNVarCharVal, String someNVarCharVal,
            bool searchBySomePhoneNumber, String somePhoneNumber,
            bool searchBySomeTextVal, String someTextVal,
            bool searchBySomeUniqueidentifierVal, Guid someUniqueidentifierVal,
            bool searchBySomeUTCDateTimeVal, DateTime someUTCDateTimeVal,
            bool searchBySomeVarCharVal, String someVarCharVal,
            bool searchByCode, System.Guid code
            );
        public abstract int PlantBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract Task<int> PlantBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract int PlantBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract Task<int> PlantBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract int PlantBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract Task<int> PlantBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList);
        public abstract bool SupportsTransactions();
        #endregion
        public abstract IDataReader GetPlantList_FetchByLandID(
            int landID,
           SessionContext context);
        public abstract Task<IDataReader> GetPlantList_FetchByLandIDAsync(
            int landID,
           SessionContext context);
        public abstract IDataReader GetPlantList_FetchByFlvrForeignKeyID(
            int flvrForeignKeyID,
           SessionContext context);
        public abstract Task<IDataReader> GetPlantList_FetchByFlvrForeignKeyIDAsync(
            int flvrForeignKeyID,
           SessionContext context);
    }
}
