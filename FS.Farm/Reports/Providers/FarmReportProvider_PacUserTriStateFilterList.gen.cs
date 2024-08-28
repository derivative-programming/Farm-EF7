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
        public abstract IDataReader GeneratePacUserTriStateFilterListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacUserTriStateFilterListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacUserTriStateFilterListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacUserTriStateFilterListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacUserTriStateFilterListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacUserTriStateFilterListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode);
    }
}
