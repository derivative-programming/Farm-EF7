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
    public static class DynaFlowTypeFactory
    {
        private static int _counter = 0;

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            DynaFlowTypeManager dynaFlowTypeManager = new DynaFlowTypeManager(context);
            var dynaFlowType = dynaFlowTypeManager.GetByCode(code);

            result = PacFactory.GetCodeLineage(context, dynaFlowType.PacCodePeek); //PacID
                                                                                //FlvrForeignKeyID

            result.Add("DynaFlowTypeCode", dynaFlowType.Code.Value.ToString());

            return result;
        }

        public static async Task<DynaFlowType> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            return new DynaFlowType
            {
                DynaFlowTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                PriorityLevel = 0,
            };
        }

        public static DynaFlowType Create(FarmDbContext context)
        {
            _counter++;
            var pac = PacFactory.CreateAndSave(context); //PacID
            return new DynaFlowType
            {
                DynaFlowTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                PriorityLevel = 0,
            };
        }
        public static async Task<DynaFlowType> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var pac = await PacFactory.CreateAndSaveAsync(context); //PacID
            DynaFlowType result =  new DynaFlowType
            {
                DynaFlowTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                PriorityLevel = 0,
            };

            DynaFlowTypeManager dynaFlowTypeManager = new DynaFlowTypeManager(context);
            result = await dynaFlowTypeManager.AddAsync(result);
            return result;
        }

        public static DynaFlowType CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var pac =   PacFactory.CreateAndSave(context); //PacID
            DynaFlowType result = new DynaFlowType
            {
                DynaFlowTypeID = _counter,
                Code = Guid.NewGuid(),
                Description = String.Empty,
                DisplayOrder = 0,
                IsActive = false,
                LookupEnumName = String.Empty,
                Name = String.Empty,
                PacID = pac.PacID,
                PriorityLevel = 0,
            };

            DynaFlowTypeManager dynaFlowTypeManager = new DynaFlowTypeManager(context);
            result = dynaFlowTypeManager.Add(result);
            return result;
        }

    }
}
