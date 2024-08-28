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
        public abstract IDataReader GeneratePacConfigDynaFlowDFTBuildToDoListReport(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacConfigDynaFlowDFTBuildToDoListReportAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacConfigDynaFlowDFTBuildToDoListReportItemCount(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacConfigDynaFlowDFTBuildToDoListReportItemCountAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacConfigDynaFlowDFTBuildToDoListReportSummary(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacConfigDynaFlowDFTBuildToDoListReportSummaryAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
    }
}
