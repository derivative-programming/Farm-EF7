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
        public abstract IDataReader GeneratePacConfigDynaFlowTaskRetryRunListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacConfigDynaFlowTaskRetryRunListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacConfigDynaFlowTaskRetryRunListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacConfigDynaFlowTaskRetryRunListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacConfigDynaFlowTaskRetryRunListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacConfigDynaFlowTaskRetryRunListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
    }
}
