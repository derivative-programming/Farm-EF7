using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using FS.Common.Objects;
using System.Threading.Tasks;
using FS.Farm.EF;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using FS.Farm.EF.Models;
using NetTopologySuite.Index.HPRtree;
using FS.Farm.Objects;

namespace FS.Farm.Providers.EF7
{
    partial class EF7DFMaintenanceProvider : FS.Farm.Providers.DFMaintenanceProvider
    {
        string _connectionString = "";
        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = configValue["connectionString"].ToString();
        }
        public override string Name
        {
            get
            {
                return null;
            }
        }
        #endregion
        #region DFMaintenance Methods
        public override int DFMaintenanceGetCount(
            SessionContext context )
        {
            string procedureName = "DFMaintenanceGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                iOut = dFMaintenanceManager.GetTotalCount();
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> DFMaintenanceGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DFMaintenanceGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                iOut = await dFMaintenanceManager.GetTotalCountAsync();

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context,  x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int DFMaintenanceGetMaxID(
            SessionContext context)
        {
            string procedureName = "DFMaintenanceGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                iOut = dFMaintenanceManager.GetMaxId().Value;
            }
            catch (Exception x)
            {
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> DFMaintenanceGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DFMaintenanceGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                var maxId = await dFMaintenanceManager.GetMaxIdAsync();

                iOut = maxId.Value;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int DFMaintenanceInsert(
            SessionContext context,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
                        System.Guid code)
        {
            string procedureName = "DFMaintenanceInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Boolean isPaused,
            //Boolean isScheduledDFProcessRequestCompleted,
            //Boolean isScheduledDFProcessRequestStarted,
            if (System.Convert.ToDateTime(lastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //lastScheduledDFProcessRequestUTCDateTime
            {
                 lastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //nextScheduledDFProcessRequestUTCDateTime
            {
                 nextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String pausedByUsername,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices PausedByUsernameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                pausedByUsername = PausedByUsernameEncryptionServices.Encrypt(pausedByUsername);
            }
            if (System.Convert.ToDateTime(pausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //pausedUTCDateTime
            {
                 pausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String scheduledDFProcessRequestProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ScheduledDFProcessRequestProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                scheduledDFProcessRequestProcessorIdentifier = ScheduledDFProcessRequestProcessorIdentifierEncryptionServices.Encrypt(scheduledDFProcessRequestProcessorIdentifier);
            }
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                dFMaintenance.Code = code;
                dFMaintenance.LastChangeCode = Guid.NewGuid();
                dFMaintenance.IsPaused = isPaused;
                dFMaintenance.IsScheduledDFProcessRequestCompleted = isScheduledDFProcessRequestCompleted;
                dFMaintenance.IsScheduledDFProcessRequestStarted = isScheduledDFProcessRequestStarted;
                dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = lastScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = nextScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.PacID = pacID;
                dFMaintenance.PausedByUsername = pausedByUsername;
                dFMaintenance.PausedUTCDateTime = pausedUTCDateTime;
                dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = scheduledDFProcessRequestProcessorIdentifier;

                dFMaintenance = dFMaintenanceManager.Add(dFMaintenance);

                iOut = dFMaintenance.DFMaintenanceID;
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> DFMaintenanceInsertAsync(
            SessionContext context,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
                        System.Guid code)
        {
            string procedureName = "DFMaintenanceInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Boolean isPaused,
            //Boolean isScheduledDFProcessRequestCompleted,
            //Boolean isScheduledDFProcessRequestStarted,
            if (System.Convert.ToDateTime(lastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String pausedByUsername,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices PausedByUsernameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                pausedByUsername = PausedByUsernameEncryptionServices.Encrypt(pausedByUsername);
            }
            if (System.Convert.ToDateTime(pausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 pausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String scheduledDFProcessRequestProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ScheduledDFProcessRequestProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                scheduledDFProcessRequestProcessorIdentifier = ScheduledDFProcessRequestProcessorIdentifierEncryptionServices.Encrypt(scheduledDFProcessRequestProcessorIdentifier);
            }
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                dFMaintenance.Code = code;
                dFMaintenance.LastChangeCode = Guid.NewGuid();
                dFMaintenance.IsPaused = isPaused;
                dFMaintenance.IsScheduledDFProcessRequestCompleted = isScheduledDFProcessRequestCompleted;
                dFMaintenance.IsScheduledDFProcessRequestStarted = isScheduledDFProcessRequestStarted;
                dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = lastScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = nextScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.PacID = pacID;
                dFMaintenance.PausedByUsername = pausedByUsername;
                dFMaintenance.PausedUTCDateTime = pausedUTCDateTime;
                dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = scheduledDFProcessRequestProcessorIdentifier;

                dFMaintenance = await dFMaintenanceManager.AddAsync(dFMaintenance);

                iOut = dFMaintenance.DFMaintenanceID;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override void DFMaintenanceUpdate(
            SessionContext context,
            int dFMaintenanceID,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
                         Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DFMaintenanceUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Boolean isPaused,
            //Boolean isScheduledDFProcessRequestCompleted,
            //Boolean isScheduledDFProcessRequestStarted,
            if (System.Convert.ToDateTime(lastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String pausedByUsername,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices PausedByUsernameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                pausedByUsername = PausedByUsernameEncryptionServices.Encrypt(pausedByUsername);
            }
            if (System.Convert.ToDateTime(pausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 pausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String scheduledDFProcessRequestProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ScheduledDFProcessRequestProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                scheduledDFProcessRequestProcessorIdentifier = ScheduledDFProcessRequestProcessorIdentifierEncryptionServices.Encrypt(scheduledDFProcessRequestProcessorIdentifier);
            }
                        EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                dFMaintenance.DFMaintenanceID = dFMaintenanceID;
                dFMaintenance.Code = code;
                dFMaintenance.IsPaused = isPaused;
                dFMaintenance.IsScheduledDFProcessRequestCompleted = isScheduledDFProcessRequestCompleted;
                dFMaintenance.IsScheduledDFProcessRequestStarted = isScheduledDFProcessRequestStarted;
                dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = lastScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = nextScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.PacID = pacID;
                dFMaintenance.PausedByUsername = pausedByUsername;
                dFMaintenance.PausedUTCDateTime = pausedUTCDateTime;
                dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = scheduledDFProcessRequestProcessorIdentifier;
                                dFMaintenance.LastChangeCode = lastChangeCode;

                bool success = dFMaintenanceManager.Update(dFMaintenance);
                if (!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }

            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DFMaintenanceUpdateAsync(
            SessionContext context,
            int dFMaintenanceID,
            Boolean isPaused,
            Boolean isScheduledDFProcessRequestCompleted,
            Boolean isScheduledDFProcessRequestStarted,
            DateTime lastScheduledDFProcessRequestUTCDateTime,
            DateTime nextScheduledDFProcessRequestUTCDateTime,
            Int32 pacID,
            String pausedByUsername,
            DateTime pausedUTCDateTime,
            String scheduledDFProcessRequestProcessorIdentifier,
                        Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DFMaintenanceUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Boolean isPaused,
            //Boolean isScheduledDFProcessRequestCompleted,
            //Boolean isScheduledDFProcessRequestStarted,
            if (System.Convert.ToDateTime(lastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String pausedByUsername,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices PausedByUsernameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                pausedByUsername = PausedByUsernameEncryptionServices.Encrypt(pausedByUsername);
            }
            if (System.Convert.ToDateTime(pausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 pausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String scheduledDFProcessRequestProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ScheduledDFProcessRequestProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                scheduledDFProcessRequestProcessorIdentifier = ScheduledDFProcessRequestProcessorIdentifierEncryptionServices.Encrypt(scheduledDFProcessRequestProcessorIdentifier);
            }
                        //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                dFMaintenance.DFMaintenanceID = dFMaintenanceID;
                dFMaintenance.Code = code;
                dFMaintenance.IsPaused = isPaused;
                dFMaintenance.IsScheduledDFProcessRequestCompleted = isScheduledDFProcessRequestCompleted;
                dFMaintenance.IsScheduledDFProcessRequestStarted = isScheduledDFProcessRequestStarted;
                dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = lastScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = nextScheduledDFProcessRequestUTCDateTime;
                dFMaintenance.PacID = pacID;
                dFMaintenance.PausedByUsername = pausedByUsername;
                dFMaintenance.PausedUTCDateTime = pausedUTCDateTime;
                dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = scheduledDFProcessRequestProcessorIdentifier;
                                dFMaintenance.LastChangeCode = lastChangeCode;

                bool success = await dFMaintenanceManager.UpdateAsync(dFMaintenance);
                if(!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override IDataReader SearchDFMaintenances(
            SessionContext context,
            bool searchByDFMaintenanceID, int dFMaintenanceID,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsScheduledDFProcessRequestCompleted, Boolean isScheduledDFProcessRequestCompleted,
            bool searchByIsScheduledDFProcessRequestStarted, Boolean isScheduledDFProcessRequestStarted,
            bool searchByLastScheduledDFProcessRequestUTCDateTime, DateTime lastScheduledDFProcessRequestUTCDateTime,
            bool searchByNextScheduledDFProcessRequestUTCDateTime, DateTime nextScheduledDFProcessRequestUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByPausedByUsername, String pausedByUsername,
            bool searchByPausedUTCDateTime, DateTime pausedUTCDateTime,
            bool searchByScheduledDFProcessRequestProcessorIdentifier, String scheduledDFProcessRequestProcessorIdentifier,
                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDFMaintenances";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> SearchDFMaintenancesAsync(
                    SessionContext context,
                    bool searchByDFMaintenanceID, int dFMaintenanceID,
                    bool searchByIsPaused, Boolean isPaused,
                    bool searchByIsScheduledDFProcessRequestCompleted, Boolean isScheduledDFProcessRequestCompleted,
                    bool searchByIsScheduledDFProcessRequestStarted, Boolean isScheduledDFProcessRequestStarted,
                    bool searchByLastScheduledDFProcessRequestUTCDateTime, DateTime lastScheduledDFProcessRequestUTCDateTime,
                    bool searchByNextScheduledDFProcessRequestUTCDateTime, DateTime nextScheduledDFProcessRequestUTCDateTime,
                    bool searchByPacID, Int32 pacID,
                    bool searchByPausedByUsername, String pausedByUsername,
                    bool searchByPausedUTCDateTime, DateTime pausedUTCDateTime,
                    bool searchByScheduledDFProcessRequestProcessorIdentifier, String scheduledDFProcessRequestProcessorIdentifier,
                                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDFMaintenancesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_Search: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDFMaintenanceList(
            SessionContext context)
        {
            string procedureName = "GetDFMaintenanceList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                rdr = BuildDataReader(dFMaintenanceManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDFMaintenanceListAsync(
            SessionContext context)
        {
            string procedureName = "GetDFMaintenanceListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                rdr = BuildDataReader(await dFMaintenanceManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetList: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override Guid GetDFMaintenanceCode(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDFMaintenanceCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DFMaintenance::" + dFMaintenanceID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    Log(procedureName + "::Get From Cache");
                    Log(procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                var dFMaintenance = dFMaintenanceManager.GetById(dFMaintenanceID);

                result = dFMaintenance.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<Guid> GetDFMaintenanceCodeAsync(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDFMaintenanceCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DFMaintenance::" + dFMaintenanceID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    await LogAsync(context, procedureName + "::Get From Cache");
                    await LogAsync(context, procedureName + "::End");
                    return result;
                }
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                var dFMaintenance = await dFMaintenanceManager.GetByIdAsync(dFMaintenanceID);

                result = dFMaintenance.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override IDataReader GetDFMaintenance(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDFMaintenance";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = dFMaintenanceManager.GetById(dFMaintenanceID);

                if(dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDFMaintenanceAsync(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDFMaintenanceAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = await dFMaintenanceManager.GetByIdAsync(dFMaintenanceID);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_Get: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyDFMaintenance(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDirtyDFMaintenance";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = dFMaintenanceManager.DirtyGetById(dFMaintenanceID);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyDFMaintenanceAsync(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "GetDirtyDFMaintenanceAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = await dFMaintenanceManager.DirtyGetByIdAsync(dFMaintenanceID);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_DirtyGet: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDFMaintenance(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDFMaintenance";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = dFMaintenanceManager.GetByCode(code);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDFMaintenanceAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDFMaintenanceAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = await dFMaintenanceManager.GetByCodeAsync(code);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyDFMaintenance(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDFMaintenance";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = dFMaintenanceManager.DirtyGetByCode(code);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyDFMaintenanceAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDFMaintenanceAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                var dFMaintenance = await dFMaintenanceManager.DirtyGetByCodeAsync(code);

                if (dFMaintenance != null)
                    dFMaintenances.Add(dFMaintenance);

                rdr = BuildDataReader(dFMaintenances);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_DirtyGetByCode: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override int GetDFMaintenanceID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDFMaintenanceID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                var dFMaintenance = dFMaintenanceManager.GetByCode(code);

                result = dFMaintenance.DFMaintenanceID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<int> GetDFMaintenanceIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDFMaintenanceIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                var dFMaintenance = await dFMaintenanceManager.GetByCodeAsync(code);

                result = dFMaintenance.DFMaintenanceID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_GetID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override int DFMaintenanceBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DFMaintenanceID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.LastChangeCode = Guid.NewGuid();
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;

                    bool isEncrypted = false;
                    //Boolean isPaused,
                    //Boolean isScheduledDFProcessRequestCompleted,
                    //Boolean isScheduledDFProcessRequestStarted,
                    if (System.Convert.ToDateTime(dFMaintenance.LastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.NextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String pausedByUsername,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.PausedByUsername = encryptionServices.Encrypt(dFMaintenance.PausedByUsername);
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.PausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String scheduledDFProcessRequestProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = encryptionServices.Encrypt(dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier);
                    }
                                        dFMaintenances.Add(dFMaintenance);
                }

                dFMaintenanceManager.BulkInsert(dFMaintenances);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> DFMaintenanceBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFMaintenanceID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.LastChangeCode = Guid.NewGuid();
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;

                    bool isEncrypted = false;
                    //Boolean isPaused,
                    //Boolean isScheduledDFProcessRequestCompleted,
                    //Boolean isScheduledDFProcessRequestStarted,
                    if (System.Convert.ToDateTime(dFMaintenance.LastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.NextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String pausedByUsername,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.PausedByUsername = encryptionServices.Encrypt(dFMaintenance.PausedByUsername);
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.PausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String scheduledDFProcessRequestProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = encryptionServices.Encrypt(dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier);
                    }
                                        dFMaintenances.Add(dFMaintenance);
                }

                await dFMaintenanceManager.BulkInsertAsync(dFMaintenances);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int DFMaintenanceBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFMaintenanceID == 0)
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.DFMaintenanceID = item.DFMaintenanceID;
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;
                                        dFMaintenance.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Boolean isPaused,
                    //Boolean isScheduledDFProcessRequestCompleted,
                    //Boolean isScheduledDFProcessRequestStarted,
                    if (System.Convert.ToDateTime(dFMaintenance.LastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.NextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String pausedByUsername,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.PausedByUsername = encryptionServices.Encrypt(dFMaintenance.PausedByUsername);
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.PausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String scheduledDFProcessRequestProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = encryptionServices.Encrypt(dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier);
                    }

                    dFMaintenances.Add(dFMaintenance);
                }

                dFMaintenanceManager.BulkUpdate(dFMaintenances);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> DFMaintenanceBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFMaintenanceID == 0)
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.DFMaintenanceID = item.DFMaintenanceID;
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;
                                        dFMaintenance.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Boolean isPaused,
                    //Boolean isScheduledDFProcessRequestCompleted,
                    //Boolean isScheduledDFProcessRequestStarted,
                    if (System.Convert.ToDateTime(dFMaintenance.LastScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.NextScheduledDFProcessRequestUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String pausedByUsername,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.PausedByUsername = encryptionServices.Encrypt(dFMaintenance.PausedByUsername);
                    }
                    if (System.Convert.ToDateTime(dFMaintenance.PausedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dFMaintenance.PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String scheduledDFProcessRequestProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = encryptionServices.Encrypt(dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier);
                    }
                                        dFMaintenances.Add(dFMaintenance);
                }

                dFMaintenanceManager.BulkUpdate(dFMaintenances);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int DFMaintenanceBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFMaintenanceID == 0)
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.DFMaintenanceID = item.DFMaintenanceID;
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;
                                        dFMaintenance.LastChangeCode = item.LastChangeCode;
                    dFMaintenances.Add(dFMaintenance);
                }

                dFMaintenanceManager.BulkDelete(dFMaintenances);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> DFMaintenanceBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFMaintenance> dataList)
        {
            string procedureName = "DFMaintenanceBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                List<EF.Models.DFMaintenance> dFMaintenances = new List<EF.Models.DFMaintenance>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFMaintenanceID == 0)
                        continue;

                    actionCount++;

                    Objects.DFMaintenance item = dataList[i];

                    EF.Models.DFMaintenance dFMaintenance = new EF.Models.DFMaintenance();
                    dFMaintenance.DFMaintenanceID = item.DFMaintenanceID;
                    dFMaintenance.Code = item.Code;
                    dFMaintenance.IsPaused = item.IsPaused;
                    dFMaintenance.IsScheduledDFProcessRequestCompleted = item.IsScheduledDFProcessRequestCompleted;
                    dFMaintenance.IsScheduledDFProcessRequestStarted = item.IsScheduledDFProcessRequestStarted;
                    dFMaintenance.LastScheduledDFProcessRequestUTCDateTime = item.LastScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.NextScheduledDFProcessRequestUTCDateTime = item.NextScheduledDFProcessRequestUTCDateTime;
                    dFMaintenance.PacID = item.PacID;
                    dFMaintenance.PausedByUsername = item.PausedByUsername;
                    dFMaintenance.PausedUTCDateTime = item.PausedUTCDateTime;
                    dFMaintenance.ScheduledDFProcessRequestProcessorIdentifier = item.ScheduledDFProcessRequestProcessorIdentifier;
                                        dFMaintenance.LastChangeCode = item.LastChangeCode;
                    dFMaintenances.Add(dFMaintenance);
                }

                await dFMaintenanceManager.BulkDeleteAsync(dFMaintenances);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFMaintenance_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override void DFMaintenanceDelete(
            SessionContext context,
            int dFMaintenanceID)
        {
            string procedureName = "DFMaintenanceDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                dFMaintenanceManager.Delete(dFMaintenanceID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DFMaintenance_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DFMaintenanceDeleteAsync(
           SessionContext context,
           int dFMaintenanceID)
        {
            string procedureName = "DFMaintenanceDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFMaintenanceID::" + dFMaintenanceID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                await dFMaintenanceManager.DeleteAsync(dFMaintenanceID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DFMaintenance_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DFMaintenanceCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DFMaintenanceCleanupTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                EF.CurrentRuntime.ClearTestObjects(dbContext);

            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override void DFMaintenanceCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DFMaintenanceCleanupChildObjectTesting";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                EF.CurrentRuntime.ClearTestChildObjects(dbContext);

            }
            catch (Exception x)
            {
                HandleError(  x, "FS_Farm_TestChildObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override bool SupportsTransactions()
        {
            return true;
        }
        #endregion
        #region Error Handling
        void HandleError( Exception x, string sprocName)
        {
            Log(x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsync(SessionContext context, Exception x, string sprocName)
        {
            await LogAsync(context, x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            throw new Exception(sException, x);
        }
        #endregion
        public override IDataReader GetDFMaintenanceList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDFMaintenanceList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                rdr = BuildDataReader(dFMaintenanceManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFMaintenance_FetchByPacID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDFMaintenanceList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDFMaintenanceList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFMaintenanceManager = new EF.Managers.DFMaintenanceManager(dbContext);

                rdr = BuildDataReader(await dFMaintenanceManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFMaintenance_FetchByPacID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        private async Task<EF.FarmDbContext> BuildDbContextAsync(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                await dbContext.Database.UseTransactionAsync(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }

        private EF.FarmDbContext BuildDbContext(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                dbContext.Database.UseTransaction(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }
        private IDataReader BuildDataReader(List<EF.Models.DFMaintenance> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DFMaintenance).GetProperties())
            {
                Type columnType = prop.PropertyType;

                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }

                dataTable.Columns.Add(prop.Name, columnType);
            }

            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.DFMaintenance).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
