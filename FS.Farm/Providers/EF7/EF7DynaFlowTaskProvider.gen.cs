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
    partial class EF7DynaFlowTaskProvider : FS.Farm.Providers.DynaFlowTaskProvider
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
        #region DynaFlowTask Methods
        public override int DynaFlowTaskGetCount(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                iOut = dynaFlowTaskManager.GetTotalCount();
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
        public override async Task<int> DynaFlowTaskGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                iOut = await dynaFlowTaskManager.GetTotalCountAsync();

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
        public override int DynaFlowTaskGetMaxID(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                iOut = dynaFlowTaskManager.GetMaxId().Value;
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
        public override async Task<int> DynaFlowTaskGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                var maxId = await dynaFlowTaskManager.GetMaxIdAsync();

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
        public override int DynaFlowTaskInsert(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
                        System.Guid code)
        {
            string procedureName = "DynaFlowTaskInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //completedUTCDateTime
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowTaskID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowID,
            //Guid dynaFlowSubjectCode,
            //Int32 dynaFlowTaskTypeID,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isParallelRunAllowed,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Int32 maxRetryCount,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //minStartUTCDateTime
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //String param2,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param2EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param2 = Param2EncryptionServices.Encrypt(param2);
            }
            //String processorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                processorIdentifier = ProcessorIdentifierEncryptionServices.Encrypt(processorIdentifier);
            }
            if (System.Convert.ToDateTime(requestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //requestedUTCDateTime
            {
                 requestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String resultValue,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ResultValueEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                resultValue = ResultValueEncryptionServices.Encrypt(resultValue);
            }
            //Int32 retryCount,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //startedUTCDateTime
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                dynaFlowTask.Code = code;
                dynaFlowTask.LastChangeCode = Guid.NewGuid();
                dynaFlowTask.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlowTask.DependencyDynaFlowTaskID = dependencyDynaFlowTaskID;
                dynaFlowTask.Description = description;
                dynaFlowTask.DynaFlowID = dynaFlowID;
                dynaFlowTask.DynaFlowSubjectCode = dynaFlowSubjectCode;
                dynaFlowTask.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTask.IsCanceled = isCanceled;
                dynaFlowTask.IsCancelRequested = isCancelRequested;
                dynaFlowTask.IsCompleted = isCompleted;
                dynaFlowTask.IsParallelRunAllowed = isParallelRunAllowed;
                dynaFlowTask.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlowTask.IsStarted = isStarted;
                dynaFlowTask.IsSuccessful = isSuccessful;
                dynaFlowTask.MaxRetryCount = maxRetryCount;
                dynaFlowTask.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlowTask.Param1 = param1;
                dynaFlowTask.Param2 = param2;
                dynaFlowTask.ProcessorIdentifier = processorIdentifier;
                dynaFlowTask.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlowTask.ResultValue = resultValue;
                dynaFlowTask.RetryCount = retryCount;
                dynaFlowTask.StartedUTCDateTime = startedUTCDateTime;

                dynaFlowTask = dynaFlowTaskManager.Add(dynaFlowTask);

                iOut = dynaFlowTask.DynaFlowTaskID;
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
        public override async Task<int> DynaFlowTaskInsertAsync(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
                        System.Guid code)
        {
            string procedureName = "DynaFlowTaskInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowTaskID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowID,
            //Guid dynaFlowSubjectCode,
            //Int32 dynaFlowTaskTypeID,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isParallelRunAllowed,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Int32 maxRetryCount,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //String param2,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param2EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param2 = Param2EncryptionServices.Encrypt(param2);
            }
            //String processorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                processorIdentifier = ProcessorIdentifierEncryptionServices.Encrypt(processorIdentifier);
            }
            if (System.Convert.ToDateTime(requestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 requestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String resultValue,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ResultValueEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                resultValue = ResultValueEncryptionServices.Encrypt(resultValue);
            }
            //Int32 retryCount,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                dynaFlowTask.Code = code;
                dynaFlowTask.LastChangeCode = Guid.NewGuid();
                dynaFlowTask.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlowTask.DependencyDynaFlowTaskID = dependencyDynaFlowTaskID;
                dynaFlowTask.Description = description;
                dynaFlowTask.DynaFlowID = dynaFlowID;
                dynaFlowTask.DynaFlowSubjectCode = dynaFlowSubjectCode;
                dynaFlowTask.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTask.IsCanceled = isCanceled;
                dynaFlowTask.IsCancelRequested = isCancelRequested;
                dynaFlowTask.IsCompleted = isCompleted;
                dynaFlowTask.IsParallelRunAllowed = isParallelRunAllowed;
                dynaFlowTask.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlowTask.IsStarted = isStarted;
                dynaFlowTask.IsSuccessful = isSuccessful;
                dynaFlowTask.MaxRetryCount = maxRetryCount;
                dynaFlowTask.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlowTask.Param1 = param1;
                dynaFlowTask.Param2 = param2;
                dynaFlowTask.ProcessorIdentifier = processorIdentifier;
                dynaFlowTask.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlowTask.ResultValue = resultValue;
                dynaFlowTask.RetryCount = retryCount;
                dynaFlowTask.StartedUTCDateTime = startedUTCDateTime;

                dynaFlowTask = await dynaFlowTaskManager.AddAsync(dynaFlowTask);

                iOut = dynaFlowTask.DynaFlowTaskID;
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
        public override void DynaFlowTaskUpdate(
            SessionContext context,
            int dynaFlowTaskID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
                         Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DynaFlowTaskUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowTaskID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowID,
            //Guid dynaFlowSubjectCode,
            //Int32 dynaFlowTaskTypeID,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isParallelRunAllowed,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Int32 maxRetryCount,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //String param2,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param2EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param2 = Param2EncryptionServices.Encrypt(param2);
            }
            //String processorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                processorIdentifier = ProcessorIdentifierEncryptionServices.Encrypt(processorIdentifier);
            }
            if (System.Convert.ToDateTime(requestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 requestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String resultValue,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ResultValueEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                resultValue = ResultValueEncryptionServices.Encrypt(resultValue);
            }
            //Int32 retryCount,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
                        EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                dynaFlowTask.DynaFlowTaskID = dynaFlowTaskID;
                dynaFlowTask.Code = code;
                dynaFlowTask.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlowTask.DependencyDynaFlowTaskID = dependencyDynaFlowTaskID;
                dynaFlowTask.Description = description;
                dynaFlowTask.DynaFlowID = dynaFlowID;
                dynaFlowTask.DynaFlowSubjectCode = dynaFlowSubjectCode;
                dynaFlowTask.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTask.IsCanceled = isCanceled;
                dynaFlowTask.IsCancelRequested = isCancelRequested;
                dynaFlowTask.IsCompleted = isCompleted;
                dynaFlowTask.IsParallelRunAllowed = isParallelRunAllowed;
                dynaFlowTask.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlowTask.IsStarted = isStarted;
                dynaFlowTask.IsSuccessful = isSuccessful;
                dynaFlowTask.MaxRetryCount = maxRetryCount;
                dynaFlowTask.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlowTask.Param1 = param1;
                dynaFlowTask.Param2 = param2;
                dynaFlowTask.ProcessorIdentifier = processorIdentifier;
                dynaFlowTask.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlowTask.ResultValue = resultValue;
                dynaFlowTask.RetryCount = retryCount;
                dynaFlowTask.StartedUTCDateTime = startedUTCDateTime;
                                dynaFlowTask.LastChangeCode = lastChangeCode;

                bool success = dynaFlowTaskManager.Update(dynaFlowTask);
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
        public override async Task DynaFlowTaskUpdateAsync(
            SessionContext context,
            int dynaFlowTaskID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowTaskID,
            String description,
            Int32 dynaFlowID,
            Guid dynaFlowSubjectCode,
            Int32 dynaFlowTaskTypeID,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isParallelRunAllowed,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Int32 maxRetryCount,
            DateTime minStartUTCDateTime,
            String param1,
            String param2,
            String processorIdentifier,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 retryCount,
            DateTime startedUTCDateTime,
                        Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DynaFlowTaskUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowTaskID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowID,
            //Guid dynaFlowSubjectCode,
            //Int32 dynaFlowTaskTypeID,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isParallelRunAllowed,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Int32 maxRetryCount,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //String param2,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param2EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param2 = Param2EncryptionServices.Encrypt(param2);
            }
            //String processorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                processorIdentifier = ProcessorIdentifierEncryptionServices.Encrypt(processorIdentifier);
            }
            if (System.Convert.ToDateTime(requestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 requestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //String resultValue,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices ResultValueEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                resultValue = ResultValueEncryptionServices.Encrypt(resultValue);
            }
            //Int32 retryCount,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
                        //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                dynaFlowTask.DynaFlowTaskID = dynaFlowTaskID;
                dynaFlowTask.Code = code;
                dynaFlowTask.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlowTask.DependencyDynaFlowTaskID = dependencyDynaFlowTaskID;
                dynaFlowTask.Description = description;
                dynaFlowTask.DynaFlowID = dynaFlowID;
                dynaFlowTask.DynaFlowSubjectCode = dynaFlowSubjectCode;
                dynaFlowTask.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTask.IsCanceled = isCanceled;
                dynaFlowTask.IsCancelRequested = isCancelRequested;
                dynaFlowTask.IsCompleted = isCompleted;
                dynaFlowTask.IsParallelRunAllowed = isParallelRunAllowed;
                dynaFlowTask.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlowTask.IsStarted = isStarted;
                dynaFlowTask.IsSuccessful = isSuccessful;
                dynaFlowTask.MaxRetryCount = maxRetryCount;
                dynaFlowTask.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlowTask.Param1 = param1;
                dynaFlowTask.Param2 = param2;
                dynaFlowTask.ProcessorIdentifier = processorIdentifier;
                dynaFlowTask.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlowTask.ResultValue = resultValue;
                dynaFlowTask.RetryCount = retryCount;
                dynaFlowTask.StartedUTCDateTime = startedUTCDateTime;
                                dynaFlowTask.LastChangeCode = lastChangeCode;

                bool success = await dynaFlowTaskManager.UpdateAsync(dynaFlowTask);
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
        public override IDataReader SearchDynaFlowTasks(
            SessionContext context,
            bool searchByDynaFlowTaskID, int dynaFlowTaskID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowTaskID, Int32 dependencyDynaFlowTaskID,
            bool searchByDescription, String description,
            bool searchByDynaFlowID, Int32 dynaFlowID,
            bool searchByDynaFlowSubjectCode, Guid dynaFlowSubjectCode,
            bool searchByDynaFlowTaskTypeID, Int32 dynaFlowTaskTypeID,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsParallelRunAllowed, Boolean isParallelRunAllowed,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByParam1, String param1,
            bool searchByParam2, String param2,
            bool searchByProcessorIdentifier, String processorIdentifier,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRetryCount, Int32 retryCount,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTasks";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_Search: \r\n";
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
        public override async Task<IDataReader> SearchDynaFlowTasksAsync(
                    SessionContext context,
                    bool searchByDynaFlowTaskID, int dynaFlowTaskID,
                    bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
                    bool searchByDependencyDynaFlowTaskID, Int32 dependencyDynaFlowTaskID,
                    bool searchByDescription, String description,
                    bool searchByDynaFlowID, Int32 dynaFlowID,
                    bool searchByDynaFlowSubjectCode, Guid dynaFlowSubjectCode,
                    bool searchByDynaFlowTaskTypeID, Int32 dynaFlowTaskTypeID,
                    bool searchByIsCanceled, Boolean isCanceled,
                    bool searchByIsCancelRequested, Boolean isCancelRequested,
                    bool searchByIsCompleted, Boolean isCompleted,
                    bool searchByIsParallelRunAllowed, Boolean isParallelRunAllowed,
                    bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
                    bool searchByIsStarted, Boolean isStarted,
                    bool searchByIsSuccessful, Boolean isSuccessful,
                    bool searchByMaxRetryCount, Int32 maxRetryCount,
                    bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
                    bool searchByParam1, String param1,
                    bool searchByParam2, String param2,
                    bool searchByProcessorIdentifier, String processorIdentifier,
                    bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
                    bool searchByResultValue, String resultValue,
                    bool searchByRetryCount, Int32 retryCount,
                    bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
                                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTasksAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_Search: \r\n";
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
        public override IDataReader GetDynaFlowTaskList(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTaskList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(dynaFlowTaskManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetList: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskListAsync(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTaskListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTaskManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetList: \r\n";
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
        public override Guid GetDynaFlowTaskCode(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDynaFlowTaskCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTask::" + dynaFlowTaskID.ToString() + "::code";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                var dynaFlowTask = dynaFlowTaskManager.GetById(dynaFlowTaskID);

                result = dynaFlowTask.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetCode: \r\n";
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
        public override async Task<Guid> GetDynaFlowTaskCodeAsync(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDynaFlowTaskCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTask::" + dynaFlowTaskID.ToString() + "::code";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                var dynaFlowTask = await dynaFlowTaskManager.GetByIdAsync(dynaFlowTaskID);

                result = dynaFlowTask.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetCode: \r\n";
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
        public override IDataReader GetDynaFlowTask(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDynaFlowTask";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = dynaFlowTaskManager.GetById(dynaFlowTaskID);

                if(dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_Get: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskAsync(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDynaFlowTaskAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = await dynaFlowTaskManager.GetByIdAsync(dynaFlowTaskID);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_Get: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTask(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDirtyDynaFlowTask";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = dynaFlowTaskManager.DirtyGetById(dynaFlowTaskID);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTaskAsync(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "GetDirtyDynaFlowTaskAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = await dynaFlowTaskManager.DirtyGetByIdAsync(dynaFlowTaskID);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_DirtyGet: \r\n";
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
        public override IDataReader GetDynaFlowTask(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTask";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = dynaFlowTaskManager.GetByCode(code);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = await dynaFlowTaskManager.GetByCodeAsync(code);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTask(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTask";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = dynaFlowTaskManager.DirtyGetByCode(code);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTaskAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTaskAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                var dynaFlowTask = await dynaFlowTaskManager.DirtyGetByCodeAsync(code);

                if (dynaFlowTask != null)
                    dynaFlowTasks.Add(dynaFlowTask);

                rdr = BuildDataReader(dynaFlowTasks);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_DirtyGetByCode: \r\n";
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
        public override int GetDynaFlowTaskID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                var dynaFlowTask = dynaFlowTaskManager.GetByCode(code);

                result = dynaFlowTask.DynaFlowTaskID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetID: \r\n";
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
        public override async Task<int> GetDynaFlowTaskIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                var dynaFlowTask = await dynaFlowTaskManager.GetByCodeAsync(code);

                result = dynaFlowTask.DynaFlowTaskID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_GetID: \r\n";
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
        public override int DynaFlowTaskBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkInsertList";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DynaFlowTaskID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.LastChangeCode = Guid.NewGuid();
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlowTask.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowTaskID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Description = encryptionServices.Encrypt(dynaFlowTask.Description);
                    }
                    //Int32 dynaFlowID,
                    //Guid dynaFlowSubjectCode,
                    //Int32 dynaFlowTaskTypeID,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isParallelRunAllowed,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Int32 maxRetryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param1 = encryptionServices.Encrypt(dynaFlowTask.Param1);
                    }
                    //String param2,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param2 = encryptionServices.Encrypt(dynaFlowTask.Param2);
                    }
                    //String processorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ProcessorIdentifier = encryptionServices.Encrypt(dynaFlowTask.ProcessorIdentifier);
                    }
                    if (System.Convert.ToDateTime(dynaFlowTask.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ResultValue = encryptionServices.Encrypt(dynaFlowTask.ResultValue);
                    }
                    //Int32 retryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                                        dynaFlowTasks.Add(dynaFlowTask);
                }

                dynaFlowTaskManager.BulkInsert(dynaFlowTasks);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkInsertListAsync";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.LastChangeCode = Guid.NewGuid();
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlowTask.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowTaskID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Description = encryptionServices.Encrypt(dynaFlowTask.Description);
                    }
                    //Int32 dynaFlowID,
                    //Guid dynaFlowSubjectCode,
                    //Int32 dynaFlowTaskTypeID,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isParallelRunAllowed,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Int32 maxRetryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param1 = encryptionServices.Encrypt(dynaFlowTask.Param1);
                    }
                    //String param2,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param2 = encryptionServices.Encrypt(dynaFlowTask.Param2);
                    }
                    //String processorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ProcessorIdentifier = encryptionServices.Encrypt(dynaFlowTask.ProcessorIdentifier);
                    }
                    if (System.Convert.ToDateTime(dynaFlowTask.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ResultValue = encryptionServices.Encrypt(dynaFlowTask.ResultValue);
                    }
                    //Int32 retryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                                        dynaFlowTasks.Add(dynaFlowTask);
                }

                await dynaFlowTaskManager.BulkInsertAsync(dynaFlowTasks);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTaskBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkUpdateList";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.DynaFlowTaskID = item.DynaFlowTaskID;
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;
                                        dynaFlowTask.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlowTask.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowTaskID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Description = encryptionServices.Encrypt(dynaFlowTask.Description);
                    }
                    //Int32 dynaFlowID,
                    //Guid dynaFlowSubjectCode,
                    //Int32 dynaFlowTaskTypeID,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isParallelRunAllowed,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Int32 maxRetryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param1 = encryptionServices.Encrypt(dynaFlowTask.Param1);
                    }
                    //String param2,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param2 = encryptionServices.Encrypt(dynaFlowTask.Param2);
                    }
                    //String processorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ProcessorIdentifier = encryptionServices.Encrypt(dynaFlowTask.ProcessorIdentifier);
                    }
                    if (System.Convert.ToDateTime(dynaFlowTask.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ResultValue = encryptionServices.Encrypt(dynaFlowTask.ResultValue);
                    }
                    //Int32 retryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }

                    dynaFlowTasks.Add(dynaFlowTask);
                }

                dynaFlowTaskManager.BulkUpdate(dynaFlowTasks);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkUpdateListAsync";
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

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.DynaFlowTaskID = item.DynaFlowTaskID;
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;
                                        dynaFlowTask.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlowTask.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowTaskID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Description = encryptionServices.Encrypt(dynaFlowTask.Description);
                    }
                    //Int32 dynaFlowID,
                    //Guid dynaFlowSubjectCode,
                    //Int32 dynaFlowTaskTypeID,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isParallelRunAllowed,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Int32 maxRetryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param1 = encryptionServices.Encrypt(dynaFlowTask.Param1);
                    }
                    //String param2,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.Param2 = encryptionServices.Encrypt(dynaFlowTask.Param2);
                    }
                    //String processorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ProcessorIdentifier = encryptionServices.Encrypt(dynaFlowTask.ProcessorIdentifier);
                    }
                    if (System.Convert.ToDateTime(dynaFlowTask.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTask.ResultValue = encryptionServices.Encrypt(dynaFlowTask.ResultValue);
                    }
                    //Int32 retryCount,
                    if (System.Convert.ToDateTime(dynaFlowTask.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTask.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                                        dynaFlowTasks.Add(dynaFlowTask);
                }

                dynaFlowTaskManager.BulkUpdate(dynaFlowTasks);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTaskBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.DynaFlowTaskID = item.DynaFlowTaskID;
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;
                                        dynaFlowTask.LastChangeCode = item.LastChangeCode;
                    dynaFlowTasks.Add(dynaFlowTask);
                }

                dynaFlowTaskManager.BulkDelete(dynaFlowTasks);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTask> dataList)
        {
            string procedureName = "DynaFlowTaskBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                List<EF.Models.DynaFlowTask> dynaFlowTasks = new List<EF.Models.DynaFlowTask>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTask item = dataList[i];

                    EF.Models.DynaFlowTask dynaFlowTask = new EF.Models.DynaFlowTask();
                    dynaFlowTask.DynaFlowTaskID = item.DynaFlowTaskID;
                    dynaFlowTask.Code = item.Code;
                    dynaFlowTask.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlowTask.DependencyDynaFlowTaskID = item.DependencyDynaFlowTaskID;
                    dynaFlowTask.Description = item.Description;
                    dynaFlowTask.DynaFlowID = item.DynaFlowID;
                    dynaFlowTask.DynaFlowSubjectCode = item.DynaFlowSubjectCode;
                    dynaFlowTask.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTask.IsCanceled = item.IsCanceled;
                    dynaFlowTask.IsCancelRequested = item.IsCancelRequested;
                    dynaFlowTask.IsCompleted = item.IsCompleted;
                    dynaFlowTask.IsParallelRunAllowed = item.IsParallelRunAllowed;
                    dynaFlowTask.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlowTask.IsStarted = item.IsStarted;
                    dynaFlowTask.IsSuccessful = item.IsSuccessful;
                    dynaFlowTask.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTask.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlowTask.Param1 = item.Param1;
                    dynaFlowTask.Param2 = item.Param2;
                    dynaFlowTask.ProcessorIdentifier = item.ProcessorIdentifier;
                    dynaFlowTask.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlowTask.ResultValue = item.ResultValue;
                    dynaFlowTask.RetryCount = item.RetryCount;
                    dynaFlowTask.StartedUTCDateTime = item.StartedUTCDateTime;
                                        dynaFlowTask.LastChangeCode = item.LastChangeCode;
                    dynaFlowTasks.Add(dynaFlowTask);
                }

                await dynaFlowTaskManager.BulkDeleteAsync(dynaFlowTasks);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTask_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DynaFlowTaskDelete(
            SessionContext context,
            int dynaFlowTaskID)
        {
            string procedureName = "DynaFlowTaskDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                dynaFlowTaskManager.Delete(dynaFlowTaskID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DynaFlowTask_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DynaFlowTaskDeleteAsync(
           SessionContext context,
           int dynaFlowTaskID)
        {
            string procedureName = "DynaFlowTaskDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskID::" + dynaFlowTaskID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                await dynaFlowTaskManager.DeleteAsync(dynaFlowTaskID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DynaFlowTask_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DynaFlowTaskCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskCleanupTesting";
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
        public override void DynaFlowTaskCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskCleanupChildObjectTesting";
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
        public override IDataReader GetDynaFlowTaskList_FetchByDynaFlowID(
            int dynaFlowID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskList_FetchByDynaFlowID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(dynaFlowTaskManager.GetByDynaFlowID(dynaFlowID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_FetchByDynaFlowID: \r\n";
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
        public override IDataReader GetDynaFlowTaskList_FetchByDynaFlowTaskTypeID(
            int dynaFlowTaskTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskList_FetchByDynaFlowTaskTypeID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(dynaFlowTaskManager.GetByDynaFlowTaskTypeID(dynaFlowTaskTypeID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_FetchByDynaFlowTaskTypeID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskList_FetchByDynaFlowIDAsync(
            int dynaFlowID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskList_FetchByDynaFlowIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTaskManager.GetByDynaFlowIDAsync(dynaFlowID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_FetchByDynaFlowID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskList_FetchByDynaFlowTaskTypeIDAsync(
            int dynaFlowTaskTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskList_FetchByDynaFlowTaskTypeIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskManager = new EF.Managers.DynaFlowTaskManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTaskManager.GetByDynaFlowTaskTypeIDAsync(dynaFlowTaskTypeID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTask_FetchByDynaFlowTaskTypeID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DynaFlowTask> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DynaFlowTask).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DynaFlowTask).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
