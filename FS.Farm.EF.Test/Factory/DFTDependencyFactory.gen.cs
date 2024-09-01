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
    public static class DFTDependencyFactory
    {
        private static int _counter = 0;

        public static Dictionary<string, string> GetCodeLineage(FarmDbContext context, Guid code)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();

            DFTDependencyManager dFTDependencyManager = new DFTDependencyManager(context);
            var dFTDependency = dFTDependencyManager.GetByCode(code);

            result = DynaFlowTaskFactory.GetCodeLineage(context, dFTDependency.DynaFlowTaskCodePeek); //DynaFlowTaskID
                                                                                //FlvrForeignKeyID

            result.Add("DFTDependencyCode", dFTDependency.Code.Value.ToString());

            return result;
        }

        public static async Task<DFTDependency> CreateAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowTask = await DynaFlowTaskFactory.CreateAndSaveAsync(context); //DynaFlowTaskID
            return new DFTDependency
            {
                DFTDependencyID = _counter,
                Code = Guid.NewGuid(),
                DependencyDFTaskID = 0,
                DynaFlowTaskID = dynaFlowTask.DynaFlowTaskID,
                IsPlaceholder = false,
            };
        }

        public static DFTDependency Create(FarmDbContext context)
        {
            _counter++;
            var dynaFlowTask = DynaFlowTaskFactory.CreateAndSave(context); //DynaFlowTaskID
            return new DFTDependency
            {
                DFTDependencyID = _counter,
                Code = Guid.NewGuid(),
                DependencyDFTaskID = 0,
                DynaFlowTaskID = dynaFlowTask.DynaFlowTaskID,
                IsPlaceholder = false,
            };
        }
        public static async Task<DFTDependency> CreateAndSaveAsync(FarmDbContext context)
        {
            _counter++;
            var dynaFlowTask = await DynaFlowTaskFactory.CreateAndSaveAsync(context); //DynaFlowTaskID
            DFTDependency result =  new DFTDependency
            {
                DFTDependencyID = _counter,
                Code = Guid.NewGuid(),
                DependencyDFTaskID = 0,
                DynaFlowTaskID = dynaFlowTask.DynaFlowTaskID,
                IsPlaceholder = false,
            };

            DFTDependencyManager dFTDependencyManager = new DFTDependencyManager(context);
            result = await dFTDependencyManager.AddAsync(result);
            return result;
        }

        public static DFTDependency CreateAndSave(FarmDbContext context)
        {
            _counter++;
            var dynaFlowTask =   DynaFlowTaskFactory.CreateAndSave(context); //DynaFlowTaskID
            DFTDependency result = new DFTDependency
            {
                DFTDependencyID = _counter,
                Code = Guid.NewGuid(),
                DependencyDFTaskID = 0,
                DynaFlowTaskID = dynaFlowTask.DynaFlowTaskID,
                IsPlaceholder = false,
            };

            DFTDependencyManager dFTDependencyManager = new DFTDependencyManager(context);
            result = dFTDependencyManager.Add(result);
            return result;
        }

    }
}
