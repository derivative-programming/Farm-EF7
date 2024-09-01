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
    partial class EF7ErrorLogProvider : FS.Farm.Providers.ErrorLogProvider
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
        #region ErrorLog Methods
        public override int ErrorLogGetCount(
            SessionContext context )
        {
            string procedureName = "ErrorLogGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                iOut = errorLogManager.GetTotalCount();
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
        public override async Task<int> ErrorLogGetCountAsync(
            SessionContext context )
        {
            string procedureName = "ErrorLogGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                iOut = await errorLogManager.GetTotalCountAsync();

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
        public override int ErrorLogGetMaxID(
            SessionContext context)
        {
            string procedureName = "ErrorLogGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                iOut = errorLogManager.GetMaxId().Value;
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
        public override async Task<int> ErrorLogGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "ErrorLogGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                var maxId = await errorLogManager.GetMaxIdAsync();

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
        public override int ErrorLogInsert(
            SessionContext context,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            System.Guid code)
        {
            string procedureName = "ErrorLogInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Guid browserCode,
            //Guid contextCode,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //createdUTCDateTime
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Boolean isClientSideError,
            //Boolean isResolved,
            //Int32 pacID,
            //String url,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices UrlEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                url = UrlEncryptionServices.Encrypt(url);
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                errorLog.Code = code;
                errorLog.LastChangeCode = Guid.NewGuid();
                errorLog.BrowserCode = browserCode;
                errorLog.ContextCode = contextCode;
                errorLog.CreatedUTCDateTime = createdUTCDateTime;
                errorLog.Description = description;
                errorLog.IsClientSideError = isClientSideError;
                errorLog.IsResolved = isResolved;
                errorLog.PacID = pacID;
                errorLog.Url = url;
                errorLog = errorLogManager.Add(errorLog);

                iOut = errorLog.ErrorLogID;
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
        public override async Task<int> ErrorLogInsertAsync(
            SessionContext context,
            Guid browserCode,
            Guid contextCode,
            DateTime createdUTCDateTime,
            String description,
            Boolean isClientSideError,
            Boolean isResolved,
            Int32 pacID,
            String url,
            System.Guid code)
        {
            string procedureName = "ErrorLogInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Guid browserCode,
            //Guid contextCode,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Boolean isClientSideError,
            //Boolean isResolved,
            //Int32 pacID,
            //String url,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices UrlEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                url = UrlEncryptionServices.Encrypt(url);
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                errorLog.Code = code;
                errorLog.LastChangeCode = Guid.NewGuid();
                errorLog.BrowserCode = browserCode;
                errorLog.ContextCode = contextCode;
                errorLog.CreatedUTCDateTime = createdUTCDateTime;
                errorLog.Description = description;
                errorLog.IsClientSideError = isClientSideError;
                errorLog.IsResolved = isResolved;
                errorLog.PacID = pacID;
                errorLog.Url = url;
                errorLog = await errorLogManager.AddAsync(errorLog);

                iOut = errorLog.ErrorLogID;
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
        public override void ErrorLogUpdate(
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
             System.Guid code)
        {
            string procedureName = "ErrorLogUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Guid browserCode,
            //Guid contextCode,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Boolean isClientSideError,
            //Boolean isResolved,
            //Int32 pacID,
            //String url,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices UrlEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                url = UrlEncryptionServices.Encrypt(url);
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                errorLog.ErrorLogID = errorLogID;
                errorLog.Code = code;
                errorLog.BrowserCode = browserCode;
                errorLog.ContextCode = contextCode;
                errorLog.CreatedUTCDateTime = createdUTCDateTime;
                errorLog.Description = description;
                errorLog.IsClientSideError = isClientSideError;
                errorLog.IsResolved = isResolved;
                errorLog.PacID = pacID;
                errorLog.Url = url;
                errorLog.LastChangeCode = lastChangeCode;

                bool success = errorLogManager.Update(errorLog);
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
        public override async Task ErrorLogUpdateAsync(
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
            System.Guid code)
        {
            string procedureName = "ErrorLogUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Guid browserCode,
            //Guid contextCode,
            if (System.Convert.ToDateTime(createdUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 createdUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Boolean isClientSideError,
            //Boolean isResolved,
            //Int32 pacID,
            //String url,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices UrlEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                url = UrlEncryptionServices.Encrypt(url);
            }
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                errorLog.ErrorLogID = errorLogID;
                errorLog.Code = code;
                errorLog.BrowserCode = browserCode;
                errorLog.ContextCode = contextCode;
                errorLog.CreatedUTCDateTime = createdUTCDateTime;
                errorLog.Description = description;
                errorLog.IsClientSideError = isClientSideError;
                errorLog.IsResolved = isResolved;
                errorLog.PacID = pacID;
                errorLog.Url = url;
                errorLog.LastChangeCode = lastChangeCode;

                bool success = await errorLogManager.UpdateAsync(errorLog);
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
        public override IDataReader SearchErrorLogs(
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
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchErrorLogs";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_Search: \r\n";
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
        public override async Task<IDataReader> SearchErrorLogsAsync(
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
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchErrorLogsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_Search: \r\n";
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
        public override IDataReader GetErrorLogList(
            SessionContext context)
        {
            string procedureName = "GetErrorLogList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                rdr = BuildDataReader(errorLogManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetList: \r\n";
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
        public override async Task<IDataReader> GetErrorLogListAsync(
            SessionContext context)
        {
            string procedureName = "GetErrorLogListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                rdr = BuildDataReader(await errorLogManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetList: \r\n";
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
        public override Guid GetErrorLogCode(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetErrorLogCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::errorLogID::" + errorLogID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "ErrorLog::" + errorLogID.ToString() + "::code";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                var errorLog = errorLogManager.GetById(errorLogID);

                result = errorLog.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetCode: \r\n";
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
        public override async Task<Guid> GetErrorLogCodeAsync(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetErrorLogCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::errorLogID::" + errorLogID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "ErrorLog::" + errorLogID.ToString() + "::code";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                var errorLog = await errorLogManager.GetByIdAsync(errorLogID);

                result = errorLog.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetCode: \r\n";
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
        public override IDataReader GetErrorLog(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetErrorLog";
            Log(procedureName + "::Start");
            Log(procedureName + "::errorLogID::" + errorLogID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = errorLogManager.GetById(errorLogID);

                if(errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_Get: \r\n";
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
        public override async Task<IDataReader> GetErrorLogAsync(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetErrorLogAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::errorLogID::" + errorLogID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = await errorLogManager.GetByIdAsync(errorLogID);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_Get: \r\n";
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
        public override IDataReader GetDirtyErrorLog(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetDirtyErrorLog";
            Log(procedureName + "::Start");
            Log(procedureName + "::errorLogID::" + errorLogID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = errorLogManager.DirtyGetById(errorLogID);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyErrorLogAsync(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "GetDirtyErrorLogAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::errorLogID::" + errorLogID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = await errorLogManager.DirtyGetByIdAsync(errorLogID);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_DirtyGet: \r\n";
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
        public override IDataReader GetErrorLog(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetErrorLog";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = errorLogManager.GetByCode(code);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetErrorLogAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetErrorLogAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = await errorLogManager.GetByCodeAsync(code);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetByCode: \r\n";
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
        public override IDataReader GetDirtyErrorLog(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyErrorLog";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = errorLogManager.DirtyGetByCode(code);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyErrorLogAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyErrorLogAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                var errorLog = await errorLogManager.DirtyGetByCodeAsync(code);

                if (errorLog != null)
                    errorLogs.Add(errorLog);

                rdr = BuildDataReader(errorLogs);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_DirtyGetByCode: \r\n";
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
        public override int GetErrorLogID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetErrorLogID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                var errorLog = errorLogManager.GetByCode(code);

                result = errorLog.ErrorLogID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetID: \r\n";
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
        public override async Task<int> GetErrorLogIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetErrorLogIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                var errorLog = await errorLogManager.GetByCodeAsync(code);

                result = errorLog.ErrorLogID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_GetID: \r\n";
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
        public override int ErrorLogBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkInsertList";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].ErrorLogID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.Code = item.Code;
                    errorLog.LastChangeCode = Guid.NewGuid();
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    bool isEncrypted = false;
                    //Guid browserCode,
                    //Guid contextCode,
                    if (System.Convert.ToDateTime(errorLog.CreatedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        errorLog.CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Description = encryptionServices.Encrypt(errorLog.Description);
                    }
                    //Boolean isClientSideError,
                    //Boolean isResolved,
                    //Int32 pacID,
                    //String url,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Url = encryptionServices.Encrypt(errorLog.Url);
                    }
                    errorLogs.Add(errorLog);
                }

                errorLogManager.BulkInsert(errorLogs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> ErrorLogBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkInsertListAsync";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].ErrorLogID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.Code = item.Code;
                    errorLog.LastChangeCode = Guid.NewGuid();
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    bool isEncrypted = false;
                    //Guid browserCode,
                    //Guid contextCode,
                    if (System.Convert.ToDateTime(errorLog.CreatedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        errorLog.CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Description = encryptionServices.Encrypt(errorLog.Description);
                    }
                    //Boolean isClientSideError,
                    //Boolean isResolved,
                    //Int32 pacID,
                    //String url,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Url = encryptionServices.Encrypt(errorLog.Url);
                    }
                    errorLogs.Add(errorLog);
                }

                await errorLogManager.BulkInsertAsync(errorLogs);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int ErrorLogBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkUpdateList";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].ErrorLogID == 0)
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.ErrorLogID = item.ErrorLogID;
                    errorLog.Code = item.Code;
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    errorLog.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Guid browserCode,
                    //Guid contextCode,
                    if (System.Convert.ToDateTime(errorLog.CreatedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        errorLog.CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Description = encryptionServices.Encrypt(errorLog.Description);
                    }
                    //Boolean isClientSideError,
                    //Boolean isResolved,
                    //Int32 pacID,
                    //String url,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Url = encryptionServices.Encrypt(errorLog.Url);
                    }

                    errorLogs.Add(errorLog);
                }

                errorLogManager.BulkUpdate(errorLogs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> ErrorLogBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkUpdateListAsync";
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

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].ErrorLogID == 0)
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.ErrorLogID = item.ErrorLogID;
                    errorLog.Code = item.Code;
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    errorLog.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Guid browserCode,
                    //Guid contextCode,
                    if (System.Convert.ToDateTime(errorLog.CreatedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        errorLog.CreatedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Description = encryptionServices.Encrypt(errorLog.Description);
                    }
                    //Boolean isClientSideError,
                    //Boolean isResolved,
                    //Int32 pacID,
                    //String url,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        errorLog.Url = encryptionServices.Encrypt(errorLog.Url);
                    }
                    errorLogs.Add(errorLog);
                }

                errorLogManager.BulkUpdate(errorLogs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int ErrorLogBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].ErrorLogID == 0)
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.ErrorLogID = item.ErrorLogID;
                    errorLog.Code = item.Code;
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    errorLog.LastChangeCode = item.LastChangeCode;
                    errorLogs.Add(errorLog);
                }

                errorLogManager.BulkDelete(errorLogs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> ErrorLogBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.ErrorLog> dataList)
        {
            string procedureName = "ErrorLogBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                List<EF.Models.ErrorLog> errorLogs = new List<EF.Models.ErrorLog>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].ErrorLogID == 0)
                        continue;

                    actionCount++;

                    Objects.ErrorLog item = dataList[i];

                    EF.Models.ErrorLog errorLog = new EF.Models.ErrorLog();
                    errorLog.ErrorLogID = item.ErrorLogID;
                    errorLog.Code = item.Code;
                    errorLog.BrowserCode = item.BrowserCode;
                    errorLog.ContextCode = item.ContextCode;
                    errorLog.CreatedUTCDateTime = item.CreatedUTCDateTime;
                    errorLog.Description = item.Description;
                    errorLog.IsClientSideError = item.IsClientSideError;
                    errorLog.IsResolved = item.IsResolved;
                    errorLog.PacID = item.PacID;
                    errorLog.Url = item.Url;
                    errorLog.LastChangeCode = item.LastChangeCode;
                    errorLogs.Add(errorLog);
                }

                await errorLogManager.BulkDeleteAsync(errorLogs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_ErrorLog_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void ErrorLogDelete(
            SessionContext context,
            int errorLogID)
        {
            string procedureName = "ErrorLogDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::errorLogID::" + errorLogID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                errorLogManager.Delete(errorLogID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_ErrorLog_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task ErrorLogDeleteAsync(
           SessionContext context,
           int errorLogID)
        {
            string procedureName = "ErrorLogDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::errorLogID::" + errorLogID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                await errorLogManager.DeleteAsync(errorLogID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_ErrorLog_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void ErrorLogCleanupTesting(
            SessionContext context )
        {
            string procedureName = "ErrorLogCleanupTesting";
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
        public override void ErrorLogCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "ErrorLogCleanupChildObjectTesting";
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
        public override IDataReader GetErrorLogList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetErrorLogList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                rdr = BuildDataReader(errorLogManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_ErrorLog_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetErrorLogList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetErrorLogList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var errorLogManager = new EF.Managers.ErrorLogManager(dbContext);

                rdr = BuildDataReader(await errorLogManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_ErrorLog_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.ErrorLog> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.ErrorLog).GetProperties())
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
                foreach (var prop in typeof(EF.Models.ErrorLog).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
