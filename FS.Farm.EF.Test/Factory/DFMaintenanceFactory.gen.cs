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
    public static class DFMaintenanceFactory
    {
        private static int _counter = 0;

        public static async Task<DFMaintenance> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            return new DFMaintenance
            {
                DFMaintenanceID = _counter,
                Code = Guid.NewGuid(),
                IsPaused = false,
                IsScheduledDFProcessRequestCompleted = false,
                IsScheduledDFProcessRequestStarted = false,
                LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                PausedByUsername = String.Empty,
                PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ScheduledDFProcessRequestProcessorIdentifier = String.Empty,
                            };
        }

        public static DFMaintenance Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID

            return new DFMaintenance
            {
                DFMaintenanceID = _counter,
                Code = Guid.NewGuid(),
                IsPaused = false,
                IsScheduledDFProcessRequestCompleted = false,
                IsScheduledDFProcessRequestStarted = false,
                LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                PausedByUsername = String.Empty,
                PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ScheduledDFProcessRequestProcessorIdentifier = String.Empty,
                            };
        }
        public static async Task<DFMaintenance> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            DFMaintenance result =  new DFMaintenance
            {
                DFMaintenanceID = _counter,
                Code = Guid.NewGuid(),
                IsPaused = false,
                IsScheduledDFProcessRequestCompleted = false,
                IsScheduledDFProcessRequestStarted = false,
                LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                PausedByUsername = String.Empty,
                PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ScheduledDFProcessRequestProcessorIdentifier = String.Empty,
                            };

            DFMaintenanceManager dFMaintenanceManager = new DFMaintenanceManager(context);
            result = await dFMaintenanceManager.AddAsync(result);
            return result;
        }

        public static DFMaintenance CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID

            DFMaintenance result = new DFMaintenance
            {
                DFMaintenanceID = _counter,
                Code = Guid.NewGuid(),
                IsPaused = false,
                IsScheduledDFProcessRequestCompleted = false,
                IsScheduledDFProcessRequestStarted = false,
                LastScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextScheduledDFProcessRequestUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                PausedByUsername = String.Empty,
                PausedUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                ScheduledDFProcessRequestProcessorIdentifier = String.Empty,
                            };

            DFMaintenanceManager dFMaintenanceManager = new DFMaintenanceManager(context);
            result = dFMaintenanceManager.Add(result);
            return result;
        }

    }
}
