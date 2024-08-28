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
        public async override Task<IDataReader> GeneratePacConfigDynaFlowTaskRunToDoListReportAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskRunToDoListReportAsync";
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

                var pacConfigDynaFlowTaskRunToDoList = new EF.Reports.PacConfigDynaFlowTaskRunToDoList(dbContext);

                List<EF.Reports.PacConfigDynaFlowTaskRunToDoList.PacConfigDynaFlowTaskRunToDoListDTO> pacConfigDynaFlowTaskRunToDoListDTOs =
                    await pacConfigDynaFlowTaskRunToDoList.GetAsync(
                            isRunTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskRunToDoListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRunToDoListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacConfigDynaFlowTaskRunToDoListReport(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacConfigDynaFlowTaskRunToDoListReportAsync";
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

                var pacConfigDynaFlowTaskRunToDoList = new EF.Reports.PacConfigDynaFlowTaskRunToDoList(dbContext);
                List<EF.Reports.PacConfigDynaFlowTaskRunToDoList.PacConfigDynaFlowTaskRunToDoListDTO> pacConfigDynaFlowTaskRunToDoListDTOs =
                    pacConfigDynaFlowTaskRunToDoList.Get(
                            isRunTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacConfigDynaFlowTaskRunToDoListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRunToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacConfigDynaFlowTaskRunToDoListReportItemCountAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRunToDoListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacConfigDynaFlowTaskRunToDoList = new EF.Reports.PacConfigDynaFlowTaskRunToDoList(dbContext);
                iOut = await pacConfigDynaFlowTaskRunToDoList.GetCountAsync(
                            isRunTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRunToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacConfigDynaFlowTaskRunToDoListReportItemCount(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRunToDoListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacConfigDynaFlowTaskRunToDoList = new EF.Reports.PacConfigDynaFlowTaskRunToDoList(dbContext);

                iOut = pacConfigDynaFlowTaskRunToDoList.GetCount(
                            isRunTaskDebugRequiredTriStateFilterCode,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacConfigDynaFlowTaskRunToDoListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacConfigDynaFlowTaskRunToDoListReportSummaryAsync(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRunToDoListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacConfigDynaFlowTaskRunToDoListReportSummary(
            SessionContext context,
            Guid isRunTaskDebugRequiredTriStateFilterCode,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacConfigDynaFlowTaskRunToDoListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacConfigDynaFlowTaskRunToDoList.PacConfigDynaFlowTaskRunToDoListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskRunToDoList.PacConfigDynaFlowTaskRunToDoListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacConfigDynaFlowTaskRunToDoList.PacConfigDynaFlowTaskRunToDoListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
