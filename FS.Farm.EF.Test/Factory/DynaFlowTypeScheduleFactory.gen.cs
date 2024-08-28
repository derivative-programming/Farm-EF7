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
    public static class DynaFlowTypeScheduleFactory
    {
        private static int _counter = 0;

        public static async Task<DynaFlowTypeSchedule> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = await DynaFlowTypeFactory.CreateAndSaveAsync(context);//DynaFlowTypeID
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            return new DynaFlowTypeSchedule
            {
                DynaFlowTypeScheduleID = _counter,
                Code = Guid.NewGuid(),
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                FrequencyInHours = 0,
                IsActive = false,
                LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                            };
        }

        public static DynaFlowTypeSchedule Create(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = DynaFlowTypeFactory.CreateAndSave(context);//DynaFlowTypeID
            var pac = PacFactory.CreateAndSave(context); //PacID

            return new DynaFlowTypeSchedule
            {
                DynaFlowTypeScheduleID = _counter,
                Code = Guid.NewGuid(),
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                FrequencyInHours = 0,
                IsActive = false,
                LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                            };
        }
        public static async Task<DynaFlowTypeSchedule> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType = await DynaFlowTypeFactory.CreateAndSaveAsync(context);//DynaFlowTypeID
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID

            DynaFlowTypeSchedule result =  new DynaFlowTypeSchedule
            {
                DynaFlowTypeScheduleID = _counter,
                Code = Guid.NewGuid(),
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                FrequencyInHours = 0,
                IsActive = false,
                LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                            };

            DynaFlowTypeScheduleManager dynaFlowTypeScheduleManager = new DynaFlowTypeScheduleManager(context);
            result = await dynaFlowTypeScheduleManager.AddAsync(result);
            return result;
        }

        public static DynaFlowTypeSchedule CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var dynaFlowType =   DynaFlowTypeFactory.CreateAndSave(context);//DynaFlowTypeID
            var pac =   PacFactory.CreateAndSave(context); //PacID

            DynaFlowTypeSchedule result = new DynaFlowTypeSchedule
            {
                DynaFlowTypeScheduleID = _counter,
                Code = Guid.NewGuid(),
                DynaFlowTypeID = dynaFlowType.DynaFlowTypeID,
                FrequencyInHours = 0,
                IsActive = false,
                LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                PacID = pac.PacID,
                            };

            DynaFlowTypeScheduleManager dynaFlowTypeScheduleManager = new DynaFlowTypeScheduleManager(context);
            result = dynaFlowTypeScheduleManager.Add(result);
            return result;
        }

    }
}
