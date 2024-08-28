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
        public async override Task<IDataReader> GeneratePacConfigDynaFlowTaskRetryRunListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskRetryRunListReportAsync";
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

                var pacConfigDynaFlowTaskRetryRunList = new EF.Reports.PacConfigDynaFlowTaskRetryRunList(dbContext);

                List<EF.Reports.PacConfigDynaFlowTaskRetryRunList.PacConfigDynaFlowTaskRetryRunListDTO> pacConfigDynaFlowTaskRetryRunListDTOs =
                    await pacConfigDynaFlowTaskRetryRunList.GetAsync(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskRetryRunListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRetryRunListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacConfigDynaFlowTaskRetryRunListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskRetryRunListReportAsync";
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

                var pacConfigDynaFlowTaskRetryRunList = new EF.Reports.PacConfigDynaFlowTaskRetryRunList(dbContext);
                List<EF.Reports.PacConfigDynaFlowTaskRetryRunList.PacConfigDynaFlowTaskRetryRunListDTO> pacConfigDynaFlowTaskRetryRunListDTOs =
                    pacConfigDynaFlowTaskRetryRunList.Get(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskRetryRunListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRetryRunListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacConfigDynaFlowTaskRetryRunListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRetryRunListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacConfigDynaFlowTaskRetryRunList = new EF.Reports.PacConfigDynaFlowTaskRetryRunList(dbContext);
                iOut = await pacConfigDynaFlowTaskRetryRunList.GetCountAsync(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRetryRunListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacConfigDynaFlowTaskRetryRunListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRetryRunListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacConfigDynaFlowTaskRetryRunList = new EF.Reports.PacConfigDynaFlowTaskRetryRunList(dbContext);

                iOut = pacConfigDynaFlowTaskRetryRunList.GetCount(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRetryRunListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacConfigDynaFlowTaskRetryRunListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRetryRunListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacConfigDynaFlowTaskRetryRunListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRetryRunListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacConfigDynaFlowTaskRetryRunList.PacConfigDynaFlowTaskRetryRunListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskRetryRunList.PacConfigDynaFlowTaskRetryRunListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskRetryRunList.PacConfigDynaFlowTaskRetryRunListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
