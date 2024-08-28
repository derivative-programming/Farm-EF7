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
        public abstract IDataReader GeneratePacConfigDynaFlowTaskSearchReport(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GeneratePacConfigDynaFlowTaskSearchReportAsync(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode);
        public abstract int GetPacConfigDynaFlowTaskSearchReportItemCount(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount);
        public abstract Task<int> GetPacConfigDynaFlowTaskSearchReportItemCountAsync(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
           int itemCountPerPage );
        public abstract IDataReader GetPacConfigDynaFlowTaskSearchReportSummary(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
        public abstract Task<IDataReader> GetPacConfigDynaFlowTaskSearchReportSummaryAsync(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode);
    }
}
