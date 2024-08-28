using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.EF;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Models;

namespace FS.Farm.EF.Test.Factory
{
    public static class DynaFlowTaskFactory
    {
        private static int _counter = 0;

        public static async Task<DynaFlowTask> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlow = await DynaFlowFactory.CreateAndSaveAsync(context); //DynaFlowID
            var dynaFlowTaskType = await DynaFlowTaskTypeFactory.CreateAndSaveAsync(context);//DynaFlowTaskTypeID

            return new DynaFlowTask
            {
                DynaFlowTaskID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowTaskID = 0,
                Description = String.Empty,
                DynaFlowID = dynaFlow.DynaFlowID,
                DynaFlowSubjectCode = Guid.NewGuid(),
                DynaFlowTaskTypeID = dynaFlowTaskType.DynaFlowTaskTypeID,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsParallelRunAllowed = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                MaxRetryCount = 0,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Param1 = String.Empty,
                Param2 = String.Empty,
                ProcessorIdentifier = String.Empty,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RetryCount = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
        }

        public static DynaFlowTask Create(FarmDbContext context)
        {
            _counter++;
            var dynaFlow = DynaFlowFactory.CreateAndSave(context); //DynaFlowID
            var dynaFlowTaskType = DynaFlowTaskTypeFactory.CreateAndSave(context);//DynaFlowTaskTypeID

            return new DynaFlowTask
            {
                DynaFlowTaskID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowTaskID = 0,
                Description = String.Empty,
                DynaFlowID = dynaFlow.DynaFlowID,
                DynaFlowSubjectCode = Guid.NewGuid(),
                DynaFlowTaskTypeID = dynaFlowTaskType.DynaFlowTaskTypeID,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsParallelRunAllowed = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                MaxRetryCount = 0,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Param1 = String.Empty,
                Param2 = String.Empty,
                ProcessorIdentifier = String.Empty,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RetryCount = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
        }
        public static async Task<DynaFlowTask> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlow = await DynaFlowFactory.CreateAndSaveAsync(context); //DynaFlowID
            var dynaFlowTaskType = await DynaFlowTaskTypeFactory.CreateAndSaveAsync(context);//DynaFlowTaskTypeID

            DynaFlowTask result =  new DynaFlowTask
            {
                DynaFlowTaskID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowTaskID = 0,
                Description = String.Empty,
                DynaFlowID = dynaFlow.DynaFlowID,
                DynaFlowSubjectCode = Guid.NewGuid(),
                DynaFlowTaskTypeID = dynaFlowTaskType.DynaFlowTaskTypeID,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsParallelRunAllowed = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                MaxRetryCount = 0,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Param1 = String.Empty,
                Param2 = String.Empty,
                ProcessorIdentifier = String.Empty,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RetryCount = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };

            DynaFlowTaskManager dynaFlowTaskManager = new DynaFlowTaskManager(context);
            result = await dynaFlowTaskManager.AddAsync(result);
            return result;
        }

        public static DynaFlowTask CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var dynaFlow =   DynaFlowFactory.CreateAndSave(context); //DynaFlowID
            var dynaFlowTaskType =   DynaFlowTaskTypeFactory.CreateAndSave(context);//DynaFlowTaskTypeID

            DynaFlowTask result = new DynaFlowTask
            {
                DynaFlowTaskID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowTaskID = 0,
                Description = String.Empty,
                DynaFlowID = dynaFlow.DynaFlowID,
                DynaFlowSubjectCode = Guid.NewGuid(),
                DynaFlowTaskTypeID = dynaFlowTaskType.DynaFlowTaskTypeID,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsParallelRunAllowed = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                MaxRetryCount = 0,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                Param1 = String.Empty,
                Param2 = String.Empty,
                ProcessorIdentifier = String.Empty,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RetryCount = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };

            DynaFlowTaskManager dynaFlowTaskManager = new DynaFlowTaskManager(context);
            result = dynaFlowTaskManager.Add(result);
            return result;
        }

    }
}
