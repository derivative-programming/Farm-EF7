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
    public static class DynaFlowFactory
    {
        private static int _counter = 0;

        public static async Task<DynaFlow> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = await DynaFlowTypeFactory.CreateAndSaveAsync(context);//DynaFlowTypeID
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            return new DynaFlow
            {
                DynaFlowID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowID = 0,
                Description = String.Empty,
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                IsBuildTaskDebugRequired = false,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsPaused = false,
                IsResubmitted = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                IsTaskCreationStarted = false,
                IsTasksCreated = false,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                Param1 = String.Empty,
                ParentDynaFlowID = 0,
                PriorityLevel = 0,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RootDynaFlowID = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SubjectCode = Guid.NewGuid(),
                TaskCreationProcessorIdentifier = String.Empty,
            };
        }

        public static DynaFlow Create(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = DynaFlowTypeFactory.CreateAndSave(context);//DynaFlowTypeID
            var pac = PacFactory.CreateAndSave(context); //PacID
            return new DynaFlow
            {
                DynaFlowID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowID = 0,
                Description = String.Empty,
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                IsBuildTaskDebugRequired = false,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsPaused = false,
                IsResubmitted = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                IsTaskCreationStarted = false,
                IsTasksCreated = false,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                Param1 = String.Empty,
                ParentDynaFlowID = 0,
                PriorityLevel = 0,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RootDynaFlowID = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SubjectCode = Guid.NewGuid(),
                TaskCreationProcessorIdentifier = String.Empty,
            };
        }
        public static async Task<DynaFlow> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = await DynaFlowTypeFactory.CreateAndSaveAsync(context);//DynaFlowTypeID
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            DynaFlow result =  new DynaFlow
            {
                DynaFlowID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowID = 0,
                Description = String.Empty,
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                IsBuildTaskDebugRequired = false,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsPaused = false,
                IsResubmitted = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                IsTaskCreationStarted = false,
                IsTasksCreated = false,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                Param1 = String.Empty,
                ParentDynaFlowID = 0,
                PriorityLevel = 0,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RootDynaFlowID = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SubjectCode = Guid.NewGuid(),
                TaskCreationProcessorIdentifier = String.Empty,
            };

            DynaFlowManager dynaFlowManager = new DynaFlowManager(context);
            result = await dynaFlowManager.AddAsync(result);
            return result;
        }

        public static DynaFlow CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType =   DynaFlowTypeFactory.CreateAndSave(context);//DynaFlowTypeID
            var pac =   PacFactory.CreateAndSave(context); //PacID
            DynaFlow result = new DynaFlow
            {
                DynaFlowID = _counter,
                Code = Guid.NewGuid(),
                CompletedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                DependencyDynaFlowID = 0,
                Description = String.Empty,
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                IsBuildTaskDebugRequired = false,
                IsCanceled = false,
                IsCancelRequested = false,
                IsCompleted = false,
                IsPaused = false,
                IsResubmitted = false,
                IsRunTaskDebugRequired = false,
                IsStarted = false,
                IsSuccessful = false,
                IsTaskCreationStarted = false,
                IsTasksCreated = false,
                MinStartUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                Param1 = String.Empty,
                ParentDynaFlowID = 0,
                PriorityLevel = 0,
                RequestedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ResultValue = String.Empty,
                RootDynaFlowID = 0,
                StartedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                SubjectCode = Guid.NewGuid(),
                TaskCreationProcessorIdentifier = String.Empty,
            };

            DynaFlowManager dynaFlowManager = new DynaFlowManager(context);
            result = dynaFlowManager.Add(result);
            return result;
        }

    }
}
