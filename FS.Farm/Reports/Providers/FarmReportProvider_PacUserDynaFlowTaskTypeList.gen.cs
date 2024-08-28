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
        public abstract IDataReader GeneratePacUserDynaFlowTaskTypeListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacUserDynaFlowTaskTypeListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacUserDynaFlowTaskTypeListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacUserDynaFlowTaskTypeListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacUserDynaFlowTaskTypeListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacUserDynaFlowTaskTypeListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
    }
}
