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
    public static class DynaFlowTaskTypeFactory
    {
        private static int _counter = 0;

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            DynaFlowTaskTypeManager dynaFlowTaskTypeManager = new DynaFlowTaskTypeManager(context);
            var dynaFlowTaskType = dynaFlowTaskTypeManager.GetByCode(code);

            result = PacFactory.GetCodeLineage(context, dynaFlowTaskType.PacCodePeek); //PacID
                                                                                //FlvrForeignKeyID

            result.Add("DynaFlowTaskTypeCode", dynaFlowTaskType.Code.Value.ToString());

            return result;
        }

        public static async Task<DynaFlowTaskType> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            return new DynaFlowTaskType
            {
                DynaFlowTaskTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                MaxRetryCount = 0,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }

        public static DynaFlowTaskType Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID
            return new DynaFlowTaskType
            {
                DynaFlowTaskTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                MaxRetryCount = 0,
                Name = String.Empty,
                PacID = pac.PacID,
            };
        }
        public static async Task<DynaFlowTaskType> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            DynaFlowTaskType result =  new DynaFlowTaskType
            {
                DynaFlowTaskTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                MaxRetryCount = 0,
                Name = String.Empty,
                PacID = pac.PacID,
            };

            DynaFlowTaskTypeManager dynaFlowTaskTypeManager = new DynaFlowTaskTypeManager(context);
            result = await dynaFlowTaskTypeManager.AddAsync(result);
            return result;
        }

        public static DynaFlowTaskType CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID
            DynaFlowTaskType result = new DynaFlowTaskType
            {
                DynaFlowTaskTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                MaxRetryCount = 0,
                Name = String.Empty,
                PacID = pac.PacID,
            };

            DynaFlowTaskTypeManager dynaFlowTaskTypeManager = new DynaFlowTaskTypeManager(context);
            result = dynaFlowTaskTypeManager.Add(result);
            return result;
        }

    }
}
