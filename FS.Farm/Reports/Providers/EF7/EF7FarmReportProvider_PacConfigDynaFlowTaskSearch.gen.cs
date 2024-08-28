using System;
using System.Collections.Specialized;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using FS.Common.Objects;
using FS.Farm.EF;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Reports;
using System.Collections.Generic;

namespace FS.Farm.Reports.Providers.EF7
{
    partial class EF7FarmReportProvider : FS.Farm.Reports.Providers.FarmReportProvider
    {
        public async override Task<IDataReader> GeneratePacConfigDynaFlowTaskSearchReportAsync(
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
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskSearchReportAsync";
            await LogAsync(context,procedureName + "::Start");
            if (orderByColumnName == null)
                orderByColumnName = string.Empty;
            if (orderByColumnName == string.Empty)
            {
                orderByColumnName = "";
                orderByDescending = false;
            }
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;
            //Define the parameters
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacConfigDynaFlowTaskSearch = new EF.Reports.PacConfigDynaFlowTaskSearch(dbContext);

                List<EF.Reports.PacConfigDynaFlowTaskSearch.PacConfigDynaFlowTaskSearchDTO> pacConfigDynaFlowTaskSearchDTOs =
                    await pacConfigDynaFlowTaskSearch.GetAsync(
                            startedDateGreaterThanFilterCode,
                            processorIdentifier,
                            isStartedTriStateFilterCode,
                            isCompletedTriStateFilterCode,
                            isSuccessfulTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskSearchDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskSearchGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacConfigDynaFlowTaskSearchReport(
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
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskSearchReportAsync";
            Log(procedureName + "::Start");
            if (orderByColumnName == null)
                orderByColumnName = string.Empty;
            if (orderByColumnName == string.Empty)
            {
                orderByColumnName = "";
                orderByDescending = false;
            }
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;
            //Define the parameters
            try
            {
                dbContext = BuildDbContext(context);

                var pacConfigDynaFlowTaskSearch = new EF.Reports.PacConfigDynaFlowTaskSearch(dbContext);
                List<EF.Reports.PacConfigDynaFlowTaskSearch.PacConfigDynaFlowTaskSearchDTO> pacConfigDynaFlowTaskSearchDTOs =
                    pacConfigDynaFlowTaskSearch.Get(
                            startedDateGreaterThanFilterCode,
                            processorIdentifier,
                            isStartedTriStateFilterCode,
                            isCompletedTriStateFilterCode,
                            isSuccessfulTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskSearchDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskSearchGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacConfigDynaFlowTaskSearchReportItemCountAsync(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacConfigDynaFlowTaskSearchReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacConfigDynaFlowTaskSearch = new EF.Reports.PacConfigDynaFlowTaskSearch(dbContext);
                iOut = await pacConfigDynaFlowTaskSearch.GetCountAsync(
                            startedDateGreaterThanFilterCode,
                            processorIdentifier,
                            isStartedTriStateFilterCode,
                            isCompletedTriStateFilterCode,
                            isSuccessfulTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskSearchGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacConfigDynaFlowTaskSearchReportItemCount(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacConfigDynaFlowTaskSearchReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacConfigDynaFlowTaskSearch = new EF.Reports.PacConfigDynaFlowTaskSearch(dbContext);

                iOut = pacConfigDynaFlowTaskSearch.GetCount(
                            startedDateGreaterThanFilterCode,
                            processorIdentifier,
                            isStartedTriStateFilterCode,
                            isCompletedTriStateFilterCode,
                            isSuccessfulTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskSearchGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacConfigDynaFlowTaskSearchReportSummaryAsync(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskSearchReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacConfigDynaFlowTaskSearchReportSummary(
            SessionContext context,
            Guid startedDateGreaterThanFilterCode,
            String processorIdentifier,
            Guid isStartedTriStateFilterCode,
            Guid isCompletedTriStateFilterCode,
            Guid isSuccessfulTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskSearchReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacConfigDynaFlowTaskSearch.PacConfigDynaFlowTaskSearchDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskSearch.PacConfigDynaFlowTaskSearchDTO).GetProperties())
            {
                Type columnType = prop.PropertyType;

                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }

                dataTable.Columns.Add(prop.Name, columnType);
            }

            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskSearch.PacConfigDynaFlowTaskSearchDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
