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
    partial class EF7DynaFlowProvider : FS.Farm.Providers.DynaFlowProvider
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
        #region DynaFlow Methods
        public override int DynaFlowGetCount(
            SessionContext context )
        {
            string procedureName = "DynaFlowGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                iOut = dynaFlowManager.GetTotalCount();
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
        public override async Task<int> DynaFlowGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DynaFlowGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                iOut = await dynaFlowManager.GetTotalCountAsync();

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
        public override int DynaFlowGetMaxID(
            SessionContext context)
        {
            string procedureName = "DynaFlowGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                iOut = dynaFlowManager.GetMaxId().Value;
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
        public override async Task<int> DynaFlowGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DynaFlowGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                var maxId = await dynaFlowManager.GetMaxIdAsync();

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
        public override int DynaFlowInsert(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            System.Guid code)
        {
            string procedureName = "DynaFlowInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //completedUTCDateTime
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowTypeID,
            //Boolean isBuildTaskDebugRequired,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isPaused,
            //Boolean isResubmitted,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Boolean isTaskCreationStarted,
            //Boolean isTasksCreated,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //minStartUTCDateTime
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //Int32 parentDynaFlowID,
            //Int32 priorityLevel,
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
            //Int32 rootDynaFlowID,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //startedUTCDateTime
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Guid subjectCode,
            //String taskCreationProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices TaskCreationProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                taskCreationProcessorIdentifier = TaskCreationProcessorIdentifierEncryptionServices.Encrypt(taskCreationProcessorIdentifier);
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                dynaFlow.Code = code;
                dynaFlow.LastChangeCode = Guid.NewGuid();
                dynaFlow.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlow.DependencyDynaFlowID = dependencyDynaFlowID;
                dynaFlow.Description = description;
                dynaFlow.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlow.IsBuildTaskDebugRequired = isBuildTaskDebugRequired;
                dynaFlow.IsCanceled = isCanceled;
                dynaFlow.IsCancelRequested = isCancelRequested;
                dynaFlow.IsCompleted = isCompleted;
                dynaFlow.IsPaused = isPaused;
                dynaFlow.IsResubmitted = isResubmitted;
                dynaFlow.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlow.IsStarted = isStarted;
                dynaFlow.IsSuccessful = isSuccessful;
                dynaFlow.IsTaskCreationStarted = isTaskCreationStarted;
                dynaFlow.IsTasksCreated = isTasksCreated;
                dynaFlow.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlow.PacID = pacID;
                dynaFlow.Param1 = param1;
                dynaFlow.ParentDynaFlowID = parentDynaFlowID;
                dynaFlow.PriorityLevel = priorityLevel;
                dynaFlow.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlow.ResultValue = resultValue;
                dynaFlow.RootDynaFlowID = rootDynaFlowID;
                dynaFlow.StartedUTCDateTime = startedUTCDateTime;
                dynaFlow.SubjectCode = subjectCode;
                dynaFlow.TaskCreationProcessorIdentifier = taskCreationProcessorIdentifier;
                dynaFlow = dynaFlowManager.Add(dynaFlow);

                iOut = dynaFlow.DynaFlowID;
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
        public override async Task<int> DynaFlowInsertAsync(
            SessionContext context,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            System.Guid code)
        {
            string procedureName = "DynaFlowInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowTypeID,
            //Boolean isBuildTaskDebugRequired,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isPaused,
            //Boolean isResubmitted,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Boolean isTaskCreationStarted,
            //Boolean isTasksCreated,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //Int32 parentDynaFlowID,
            //Int32 priorityLevel,
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
            //Int32 rootDynaFlowID,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Guid subjectCode,
            //String taskCreationProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices TaskCreationProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                taskCreationProcessorIdentifier = TaskCreationProcessorIdentifierEncryptionServices.Encrypt(taskCreationProcessorIdentifier);
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                dynaFlow.Code = code;
                dynaFlow.LastChangeCode = Guid.NewGuid();
                dynaFlow.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlow.DependencyDynaFlowID = dependencyDynaFlowID;
                dynaFlow.Description = description;
                dynaFlow.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlow.IsBuildTaskDebugRequired = isBuildTaskDebugRequired;
                dynaFlow.IsCanceled = isCanceled;
                dynaFlow.IsCancelRequested = isCancelRequested;
                dynaFlow.IsCompleted = isCompleted;
                dynaFlow.IsPaused = isPaused;
                dynaFlow.IsResubmitted = isResubmitted;
                dynaFlow.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlow.IsStarted = isStarted;
                dynaFlow.IsSuccessful = isSuccessful;
                dynaFlow.IsTaskCreationStarted = isTaskCreationStarted;
                dynaFlow.IsTasksCreated = isTasksCreated;
                dynaFlow.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlow.PacID = pacID;
                dynaFlow.Param1 = param1;
                dynaFlow.ParentDynaFlowID = parentDynaFlowID;
                dynaFlow.PriorityLevel = priorityLevel;
                dynaFlow.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlow.ResultValue = resultValue;
                dynaFlow.RootDynaFlowID = rootDynaFlowID;
                dynaFlow.StartedUTCDateTime = startedUTCDateTime;
                dynaFlow.SubjectCode = subjectCode;
                dynaFlow.TaskCreationProcessorIdentifier = taskCreationProcessorIdentifier;
                dynaFlow = await dynaFlowManager.AddAsync(dynaFlow);

                iOut = dynaFlow.DynaFlowID;
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
        public override void DynaFlowUpdate(
            SessionContext context,
            int dynaFlowID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DynaFlowUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowTypeID,
            //Boolean isBuildTaskDebugRequired,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isPaused,
            //Boolean isResubmitted,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Boolean isTaskCreationStarted,
            //Boolean isTasksCreated,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //Int32 parentDynaFlowID,
            //Int32 priorityLevel,
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
            //Int32 rootDynaFlowID,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Guid subjectCode,
            //String taskCreationProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices TaskCreationProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                taskCreationProcessorIdentifier = TaskCreationProcessorIdentifierEncryptionServices.Encrypt(taskCreationProcessorIdentifier);
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                dynaFlow.DynaFlowID = dynaFlowID;
                dynaFlow.Code = code;
                dynaFlow.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlow.DependencyDynaFlowID = dependencyDynaFlowID;
                dynaFlow.Description = description;
                dynaFlow.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlow.IsBuildTaskDebugRequired = isBuildTaskDebugRequired;
                dynaFlow.IsCanceled = isCanceled;
                dynaFlow.IsCancelRequested = isCancelRequested;
                dynaFlow.IsCompleted = isCompleted;
                dynaFlow.IsPaused = isPaused;
                dynaFlow.IsResubmitted = isResubmitted;
                dynaFlow.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlow.IsStarted = isStarted;
                dynaFlow.IsSuccessful = isSuccessful;
                dynaFlow.IsTaskCreationStarted = isTaskCreationStarted;
                dynaFlow.IsTasksCreated = isTasksCreated;
                dynaFlow.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlow.PacID = pacID;
                dynaFlow.Param1 = param1;
                dynaFlow.ParentDynaFlowID = parentDynaFlowID;
                dynaFlow.PriorityLevel = priorityLevel;
                dynaFlow.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlow.ResultValue = resultValue;
                dynaFlow.RootDynaFlowID = rootDynaFlowID;
                dynaFlow.StartedUTCDateTime = startedUTCDateTime;
                dynaFlow.SubjectCode = subjectCode;
                dynaFlow.TaskCreationProcessorIdentifier = taskCreationProcessorIdentifier;
                dynaFlow.LastChangeCode = lastChangeCode;

                bool success = dynaFlowManager.Update(dynaFlow);
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
        public override async Task DynaFlowUpdateAsync(
            SessionContext context,
            int dynaFlowID,
            DateTime completedUTCDateTime,
            Int32 dependencyDynaFlowID,
            String description,
            Int32 dynaFlowTypeID,
            Boolean isBuildTaskDebugRequired,
            Boolean isCanceled,
            Boolean isCancelRequested,
            Boolean isCompleted,
            Boolean isPaused,
            Boolean isResubmitted,
            Boolean isRunTaskDebugRequired,
            Boolean isStarted,
            Boolean isSuccessful,
            Boolean isTaskCreationStarted,
            Boolean isTasksCreated,
            DateTime minStartUTCDateTime,
            Int32 pacID,
            String param1,
            Int32 parentDynaFlowID,
            Int32 priorityLevel,
            DateTime requestedUTCDateTime,
            String resultValue,
            Int32 rootDynaFlowID,
            DateTime startedUTCDateTime,
            Guid subjectCode,
            String taskCreationProcessorIdentifier,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DynaFlowUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            if (System.Convert.ToDateTime(completedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 completedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 dependencyDynaFlowID,
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 dynaFlowTypeID,
            //Boolean isBuildTaskDebugRequired,
            //Boolean isCanceled,
            //Boolean isCancelRequested,
            //Boolean isCompleted,
            //Boolean isPaused,
            //Boolean isResubmitted,
            //Boolean isRunTaskDebugRequired,
            //Boolean isStarted,
            //Boolean isSuccessful,
            //Boolean isTaskCreationStarted,
            //Boolean isTasksCreated,
            if (System.Convert.ToDateTime(minStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 minStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //String param1,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices Param1EncryptionServices = new FS.Common.Encryption.EncryptionServices();
                param1 = Param1EncryptionServices.Encrypt(param1);
            }
            //Int32 parentDynaFlowID,
            //Int32 priorityLevel,
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
            //Int32 rootDynaFlowID,
            if (System.Convert.ToDateTime(startedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 startedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Guid subjectCode,
            //String taskCreationProcessorIdentifier,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices TaskCreationProcessorIdentifierEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                taskCreationProcessorIdentifier = TaskCreationProcessorIdentifierEncryptionServices.Encrypt(taskCreationProcessorIdentifier);
            }
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                dynaFlow.DynaFlowID = dynaFlowID;
                dynaFlow.Code = code;
                dynaFlow.CompletedUTCDateTime = completedUTCDateTime;
                dynaFlow.DependencyDynaFlowID = dependencyDynaFlowID;
                dynaFlow.Description = description;
                dynaFlow.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlow.IsBuildTaskDebugRequired = isBuildTaskDebugRequired;
                dynaFlow.IsCanceled = isCanceled;
                dynaFlow.IsCancelRequested = isCancelRequested;
                dynaFlow.IsCompleted = isCompleted;
                dynaFlow.IsPaused = isPaused;
                dynaFlow.IsResubmitted = isResubmitted;
                dynaFlow.IsRunTaskDebugRequired = isRunTaskDebugRequired;
                dynaFlow.IsStarted = isStarted;
                dynaFlow.IsSuccessful = isSuccessful;
                dynaFlow.IsTaskCreationStarted = isTaskCreationStarted;
                dynaFlow.IsTasksCreated = isTasksCreated;
                dynaFlow.MinStartUTCDateTime = minStartUTCDateTime;
                dynaFlow.PacID = pacID;
                dynaFlow.Param1 = param1;
                dynaFlow.ParentDynaFlowID = parentDynaFlowID;
                dynaFlow.PriorityLevel = priorityLevel;
                dynaFlow.RequestedUTCDateTime = requestedUTCDateTime;
                dynaFlow.ResultValue = resultValue;
                dynaFlow.RootDynaFlowID = rootDynaFlowID;
                dynaFlow.StartedUTCDateTime = startedUTCDateTime;
                dynaFlow.SubjectCode = subjectCode;
                dynaFlow.TaskCreationProcessorIdentifier = taskCreationProcessorIdentifier;
                dynaFlow.LastChangeCode = lastChangeCode;

                bool success = await dynaFlowManager.UpdateAsync(dynaFlow);
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
        public override IDataReader SearchDynaFlows(
            SessionContext context,
            bool searchByDynaFlowID, int dynaFlowID,
            bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
            bool searchByDependencyDynaFlowID, Int32 dependencyDynaFlowID,
            bool searchByDescription, String description,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByIsBuildTaskDebugRequired, Boolean isBuildTaskDebugRequired,
            bool searchByIsCanceled, Boolean isCanceled,
            bool searchByIsCancelRequested, Boolean isCancelRequested,
            bool searchByIsCompleted, Boolean isCompleted,
            bool searchByIsPaused, Boolean isPaused,
            bool searchByIsResubmitted, Boolean isResubmitted,
            bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
            bool searchByIsStarted, Boolean isStarted,
            bool searchByIsSuccessful, Boolean isSuccessful,
            bool searchByIsTaskCreationStarted, Boolean isTaskCreationStarted,
            bool searchByIsTasksCreated, Boolean isTasksCreated,
            bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByParam1, String param1,
            bool searchByParentDynaFlowID, Int32 parentDynaFlowID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
            bool searchByResultValue, String resultValue,
            bool searchByRootDynaFlowID, Int32 rootDynaFlowID,
            bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
            bool searchBySubjectCode, Guid subjectCode,
            bool searchByTaskCreationProcessorIdentifier, String taskCreationProcessorIdentifier,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlows";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_Search: \r\n";
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
        public override async Task<IDataReader> SearchDynaFlowsAsync(
                    SessionContext context,
                    bool searchByDynaFlowID, int dynaFlowID,
                    bool searchByCompletedUTCDateTime, DateTime completedUTCDateTime,
                    bool searchByDependencyDynaFlowID, Int32 dependencyDynaFlowID,
                    bool searchByDescription, String description,
                    bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
                    bool searchByIsBuildTaskDebugRequired, Boolean isBuildTaskDebugRequired,
                    bool searchByIsCanceled, Boolean isCanceled,
                    bool searchByIsCancelRequested, Boolean isCancelRequested,
                    bool searchByIsCompleted, Boolean isCompleted,
                    bool searchByIsPaused, Boolean isPaused,
                    bool searchByIsResubmitted, Boolean isResubmitted,
                    bool searchByIsRunTaskDebugRequired, Boolean isRunTaskDebugRequired,
                    bool searchByIsStarted, Boolean isStarted,
                    bool searchByIsSuccessful, Boolean isSuccessful,
                    bool searchByIsTaskCreationStarted, Boolean isTaskCreationStarted,
                    bool searchByIsTasksCreated, Boolean isTasksCreated,
                    bool searchByMinStartUTCDateTime, DateTime minStartUTCDateTime,
                    bool searchByPacID, Int32 pacID,
                    bool searchByParam1, String param1,
                    bool searchByParentDynaFlowID, Int32 parentDynaFlowID,
                    bool searchByPriorityLevel, Int32 priorityLevel,
                    bool searchByRequestedUTCDateTime, DateTime requestedUTCDateTime,
                    bool searchByResultValue, String resultValue,
                    bool searchByRootDynaFlowID, Int32 rootDynaFlowID,
                    bool searchByStartedUTCDateTime, DateTime startedUTCDateTime,
                    bool searchBySubjectCode, Guid subjectCode,
                    bool searchByTaskCreationProcessorIdentifier, String taskCreationProcessorIdentifier,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_Search: \r\n";
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
        public override IDataReader GetDynaFlowList(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(dynaFlowManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetList: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowListAsync(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(await dynaFlowManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetList: \r\n";
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
        public override Guid GetDynaFlowCode(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDynaFlowCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlow::" + dynaFlowID.ToString() + "::code";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                var dynaFlow = dynaFlowManager.GetById(dynaFlowID);

                result = dynaFlow.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetCode: \r\n";
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
        public override async Task<Guid> GetDynaFlowCodeAsync(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDynaFlowCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlow::" + dynaFlowID.ToString() + "::code";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                var dynaFlow = await dynaFlowManager.GetByIdAsync(dynaFlowID);

                result = dynaFlow.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetCode: \r\n";
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
        public override IDataReader GetDynaFlow(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDynaFlow";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = dynaFlowManager.GetById(dynaFlowID);

                if(dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_Get: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowAsync(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDynaFlowAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = await dynaFlowManager.GetByIdAsync(dynaFlowID);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_Get: \r\n";
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
        public override IDataReader GetDirtyDynaFlow(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDirtyDynaFlow";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = dynaFlowManager.DirtyGetById(dynaFlowID);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowAsync(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "GetDirtyDynaFlowAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = await dynaFlowManager.DirtyGetByIdAsync(dynaFlowID);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_DirtyGet: \r\n";
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
        public override IDataReader GetDynaFlow(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlow";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = dynaFlowManager.GetByCode(code);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = await dynaFlowManager.GetByCodeAsync(code);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDynaFlow(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlow";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = dynaFlowManager.DirtyGetByCode(code);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                var dynaFlow = await dynaFlowManager.DirtyGetByCodeAsync(code);

                if (dynaFlow != null)
                    dynaFlows.Add(dynaFlow);

                rdr = BuildDataReader(dynaFlows);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_DirtyGetByCode: \r\n";
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
        public override int GetDynaFlowID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                var dynaFlow = dynaFlowManager.GetByCode(code);

                result = dynaFlow.DynaFlowID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetID: \r\n";
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
        public override async Task<int> GetDynaFlowIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                var dynaFlow = await dynaFlowManager.GetByCodeAsync(code);

                result = dynaFlow.DynaFlowID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_GetID: \r\n";
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
        public override int DynaFlowBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkInsertList";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DynaFlowID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.Code = item.Code;
                    dynaFlow.LastChangeCode = Guid.NewGuid();
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlow.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Description = encryptionServices.Encrypt(dynaFlow.Description);
                    }
                    //Int32 dynaFlowTypeID,
                    //Boolean isBuildTaskDebugRequired,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isPaused,
                    //Boolean isResubmitted,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Boolean isTaskCreationStarted,
                    //Boolean isTasksCreated,
                    if (System.Convert.ToDateTime(dynaFlow.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Param1 = encryptionServices.Encrypt(dynaFlow.Param1);
                    }
                    //Int32 parentDynaFlowID,
                    //Int32 priorityLevel,
                    if (System.Convert.ToDateTime(dynaFlow.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.ResultValue = encryptionServices.Encrypt(dynaFlow.ResultValue);
                    }
                    //Int32 rootDynaFlowID,
                    if (System.Convert.ToDateTime(dynaFlow.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Guid subjectCode,
                    //String taskCreationProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.TaskCreationProcessorIdentifier = encryptionServices.Encrypt(dynaFlow.TaskCreationProcessorIdentifier);
                    }
                    dynaFlows.Add(dynaFlow);
                }

                dynaFlowManager.BulkInsert(dynaFlows);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkInsertListAsync";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.Code = item.Code;
                    dynaFlow.LastChangeCode = Guid.NewGuid();
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlow.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Description = encryptionServices.Encrypt(dynaFlow.Description);
                    }
                    //Int32 dynaFlowTypeID,
                    //Boolean isBuildTaskDebugRequired,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isPaused,
                    //Boolean isResubmitted,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Boolean isTaskCreationStarted,
                    //Boolean isTasksCreated,
                    if (System.Convert.ToDateTime(dynaFlow.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Param1 = encryptionServices.Encrypt(dynaFlow.Param1);
                    }
                    //Int32 parentDynaFlowID,
                    //Int32 priorityLevel,
                    if (System.Convert.ToDateTime(dynaFlow.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.ResultValue = encryptionServices.Encrypt(dynaFlow.ResultValue);
                    }
                    //Int32 rootDynaFlowID,
                    if (System.Convert.ToDateTime(dynaFlow.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Guid subjectCode,
                    //String taskCreationProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.TaskCreationProcessorIdentifier = encryptionServices.Encrypt(dynaFlow.TaskCreationProcessorIdentifier);
                    }
                    dynaFlows.Add(dynaFlow);
                }

                await dynaFlowManager.BulkInsertAsync(dynaFlows);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkUpdateList";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.DynaFlowID = item.DynaFlowID;
                    dynaFlow.Code = item.Code;
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    dynaFlow.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlow.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Description = encryptionServices.Encrypt(dynaFlow.Description);
                    }
                    //Int32 dynaFlowTypeID,
                    //Boolean isBuildTaskDebugRequired,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isPaused,
                    //Boolean isResubmitted,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Boolean isTaskCreationStarted,
                    //Boolean isTasksCreated,
                    if (System.Convert.ToDateTime(dynaFlow.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Param1 = encryptionServices.Encrypt(dynaFlow.Param1);
                    }
                    //Int32 parentDynaFlowID,
                    //Int32 priorityLevel,
                    if (System.Convert.ToDateTime(dynaFlow.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.ResultValue = encryptionServices.Encrypt(dynaFlow.ResultValue);
                    }
                    //Int32 rootDynaFlowID,
                    if (System.Convert.ToDateTime(dynaFlow.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Guid subjectCode,
                    //String taskCreationProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.TaskCreationProcessorIdentifier = encryptionServices.Encrypt(dynaFlow.TaskCreationProcessorIdentifier);
                    }

                    dynaFlows.Add(dynaFlow);
                }

                dynaFlowManager.BulkUpdate(dynaFlows);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkUpdateListAsync";
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

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.DynaFlowID = item.DynaFlowID;
                    dynaFlow.Code = item.Code;
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    dynaFlow.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    if (System.Convert.ToDateTime(dynaFlow.CompletedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 dependencyDynaFlowID,
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Description = encryptionServices.Encrypt(dynaFlow.Description);
                    }
                    //Int32 dynaFlowTypeID,
                    //Boolean isBuildTaskDebugRequired,
                    //Boolean isCanceled,
                    //Boolean isCancelRequested,
                    //Boolean isCompleted,
                    //Boolean isPaused,
                    //Boolean isResubmitted,
                    //Boolean isRunTaskDebugRequired,
                    //Boolean isStarted,
                    //Boolean isSuccessful,
                    //Boolean isTaskCreationStarted,
                    //Boolean isTasksCreated,
                    if (System.Convert.ToDateTime(dynaFlow.MinStartUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    //String param1,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.Param1 = encryptionServices.Encrypt(dynaFlow.Param1);
                    }
                    //Int32 parentDynaFlowID,
                    //Int32 priorityLevel,
                    if (System.Convert.ToDateTime(dynaFlow.RequestedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //String resultValue,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.ResultValue = encryptionServices.Encrypt(dynaFlow.ResultValue);
                    }
                    //Int32 rootDynaFlowID,
                    if (System.Convert.ToDateTime(dynaFlow.StartedUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlow.StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Guid subjectCode,
                    //String taskCreationProcessorIdentifier,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlow.TaskCreationProcessorIdentifier = encryptionServices.Encrypt(dynaFlow.TaskCreationProcessorIdentifier);
                    }
                    dynaFlows.Add(dynaFlow);
                }

                dynaFlowManager.BulkUpdate(dynaFlows);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.DynaFlowID = item.DynaFlowID;
                    dynaFlow.Code = item.Code;
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    dynaFlow.LastChangeCode = item.LastChangeCode;
                    dynaFlows.Add(dynaFlow);
                }

                dynaFlowManager.BulkDelete(dynaFlows);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlow> dataList)
        {
            string procedureName = "DynaFlowBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                List<EF.Models.DynaFlow> dynaFlows = new List<EF.Models.DynaFlow>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlow item = dataList[i];

                    EF.Models.DynaFlow dynaFlow = new EF.Models.DynaFlow();
                    dynaFlow.DynaFlowID = item.DynaFlowID;
                    dynaFlow.Code = item.Code;
                    dynaFlow.CompletedUTCDateTime = item.CompletedUTCDateTime;
                    dynaFlow.DependencyDynaFlowID = item.DependencyDynaFlowID;
                    dynaFlow.Description = item.Description;
                    dynaFlow.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlow.IsBuildTaskDebugRequired = item.IsBuildTaskDebugRequired;
                    dynaFlow.IsCanceled = item.IsCanceled;
                    dynaFlow.IsCancelRequested = item.IsCancelRequested;
                    dynaFlow.IsCompleted = item.IsCompleted;
                    dynaFlow.IsPaused = item.IsPaused;
                    dynaFlow.IsResubmitted = item.IsResubmitted;
                    dynaFlow.IsRunTaskDebugRequired = item.IsRunTaskDebugRequired;
                    dynaFlow.IsStarted = item.IsStarted;
                    dynaFlow.IsSuccessful = item.IsSuccessful;
                    dynaFlow.IsTaskCreationStarted = item.IsTaskCreationStarted;
                    dynaFlow.IsTasksCreated = item.IsTasksCreated;
                    dynaFlow.MinStartUTCDateTime = item.MinStartUTCDateTime;
                    dynaFlow.PacID = item.PacID;
                    dynaFlow.Param1 = item.Param1;
                    dynaFlow.ParentDynaFlowID = item.ParentDynaFlowID;
                    dynaFlow.PriorityLevel = item.PriorityLevel;
                    dynaFlow.RequestedUTCDateTime = item.RequestedUTCDateTime;
                    dynaFlow.ResultValue = item.ResultValue;
                    dynaFlow.RootDynaFlowID = item.RootDynaFlowID;
                    dynaFlow.StartedUTCDateTime = item.StartedUTCDateTime;
                    dynaFlow.SubjectCode = item.SubjectCode;
                    dynaFlow.TaskCreationProcessorIdentifier = item.TaskCreationProcessorIdentifier;
                    dynaFlow.LastChangeCode = item.LastChangeCode;
                    dynaFlows.Add(dynaFlow);
                }

                await dynaFlowManager.BulkDeleteAsync(dynaFlows);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlow_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DynaFlowDelete(
            SessionContext context,
            int dynaFlowID)
        {
            string procedureName = "DynaFlowDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                dynaFlowManager.Delete(dynaFlowID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DynaFlow_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DynaFlowDeleteAsync(
           SessionContext context,
           int dynaFlowID)
        {
            string procedureName = "DynaFlowDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowID::" + dynaFlowID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                await dynaFlowManager.DeleteAsync(dynaFlowID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DynaFlow_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DynaFlowCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DynaFlowCleanupTesting";
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
        public override void DynaFlowCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DynaFlowCleanupChildObjectTesting";
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
        public override IDataReader GetDynaFlowList_FetchByDynaFlowTypeID(
            int dynaFlowTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowList_FetchByDynaFlowTypeID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(dynaFlowManager.GetByDynaFlowTypeID(dynaFlowTypeID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_FetchByDynaFlowTypeID: \r\n";
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
        public override IDataReader GetDynaFlowList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(dynaFlowManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlow_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowList_FetchByDynaFlowTypeIDAsync(
            int dynaFlowTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowList_FetchByDynaFlowTypeIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(await dynaFlowManager.GetByDynaFlowTypeIDAsync(dynaFlowTypeID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_FetchByDynaFlowTypeID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowManager = new EF.Managers.DynaFlowManager(dbContext);

                rdr = BuildDataReader(await dynaFlowManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlow_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DynaFlow> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DynaFlow).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DynaFlow).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
