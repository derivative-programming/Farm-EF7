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
        public async override Task<IDataReader> GeneratePacConfigDynaFlowDFTBuildToDoListReportAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowDFTBuildToDoListReportAsync";
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

                var pacConfigDynaFlowDFTBuildToDoList = new EF.Reports.PacConfigDynaFlowDFTBuildToDoList(dbContext);

                List<EF.Reports.PacConfigDynaFlowDFTBuildToDoList.PacConfigDynaFlowDFTBuildToDoListDTO> pacConfigDynaFlowDFTBuildToDoListDTOs =
                    await pacConfigDynaFlowDFTBuildToDoList.GetAsync(
                            isBuildTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowDFTBuildToDoListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowDFTBuildToDoListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacConfigDynaFlowDFTBuildToDoListReport(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowDFTBuildToDoListReportAsync";
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

                var pacConfigDynaFlowDFTBuildToDoList = new EF.Reports.PacConfigDynaFlowDFTBuildToDoList(dbContext);
                List<EF.Reports.PacConfigDynaFlowDFTBuildToDoList.PacConfigDynaFlowDFTBuildToDoListDTO> pacConfigDynaFlowDFTBuildToDoListDTOs =
                    pacConfigDynaFlowDFTBuildToDoList.Get(
                            isBuildTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowDFTBuildToDoListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowDFTBuildToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacConfigDynaFlowDFTBuildToDoListReportItemCountAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacConfigDynaFlowDFTBuildToDoListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacConfigDynaFlowDFTBuildToDoList = new EF.Reports.PacConfigDynaFlowDFTBuildToDoList(dbContext);
                iOut = await pacConfigDynaFlowDFTBuildToDoList.GetCountAsync(
                            isBuildTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowDFTBuildToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacConfigDynaFlowDFTBuildToDoListReportItemCount(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacConfigDynaFlowDFTBuildToDoListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacConfigDynaFlowDFTBuildToDoList = new EF.Reports.PacConfigDynaFlowDFTBuildToDoList(dbContext);

                iOut = pacConfigDynaFlowDFTBuildToDoList.GetCount(
                            isBuildTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowDFTBuildToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacConfigDynaFlowDFTBuildToDoListReportSummaryAsync(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowDFTBuildToDoListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacConfigDynaFlowDFTBuildToDoListReportSummary(
            SessionContext context,
            Guid isBuildTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowDFTBuildToDoListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacConfigDynaFlowDFTBuildToDoList.PacConfigDynaFlowDFTBuildToDoListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowDFTBuildToDoList.PacConfigDynaFlowDFTBuildToDoListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowDFTBuildToDoList.PacConfigDynaFlowDFTBuildToDoListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
