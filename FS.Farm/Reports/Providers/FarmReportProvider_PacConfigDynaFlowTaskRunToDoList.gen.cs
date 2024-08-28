using System;
using System.Reflection;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;
using FS.Common.Objects;
namespace FS.Farm.Reports.Providers
{
    internal abstract partial class FarmReportProvider : System.Configuration.Provider.ProviderBase
    {
        public abstract IDataReader GeneratePacConfigDynaFlowTaskRunToDoListReport(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacConfigDynaFlowTaskRunToDoListReportAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacConfigDynaFlowTaskRunToDoListReportItemCount(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacConfigDynaFlowTaskRunToDoListReportItemCountAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacConfigDynaFlowTaskRunToDoListReportSummary(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacConfigDynaFlowTaskRunToDoListReportSummaryAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
    }
}
