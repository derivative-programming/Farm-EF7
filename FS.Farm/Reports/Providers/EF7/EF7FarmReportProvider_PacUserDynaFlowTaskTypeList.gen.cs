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
        public async override Task<IDataReader> GeneratePacUserDynaFlowTaskTypeListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDynaFlowTaskTypeListReportAsync";
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

                var pacUserDynaFlowTaskTypeList = new EF.Reports.PacUserDynaFlowTaskTypeList(dbContext);

                List<EF.Reports.PacUserDynaFlowTaskTypeList.PacUserDynaFlowTaskTypeListDTO> pacUserDynaFlowTaskTypeListDTOs =
                    await pacUserDynaFlowTaskTypeList.GetAsync(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacUserDynaFlowTaskTypeListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTaskTypeListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacUserDynaFlowTaskTypeListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDynaFlowTaskTypeListReportAsync";
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

                var pacUserDynaFlowTaskTypeList = new EF.Reports.PacUserDynaFlowTaskTypeList(dbContext);
                List<EF.Reports.PacUserDynaFlowTaskTypeList.PacUserDynaFlowTaskTypeListDTO> pacUserDynaFlowTaskTypeListDTOs =
                    pacUserDynaFlowTaskTypeList.Get(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacUserDynaFlowTaskTypeListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTaskTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacUserDynaFlowTaskTypeListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacUserDynaFlowTaskTypeListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacUserDynaFlowTaskTypeList = new EF.Reports.PacUserDynaFlowTaskTypeList(dbContext);
                iOut = await pacUserDynaFlowTaskTypeList.GetCountAsync(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTaskTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacUserDynaFlowTaskTypeListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacUserDynaFlowTaskTypeListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacUserDynaFlowTaskTypeList = new EF.Reports.PacUserDynaFlowTaskTypeList(dbContext);

                iOut = pacUserDynaFlowTaskTypeList.GetCount(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTaskTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacUserDynaFlowTaskTypeListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDynaFlowTaskTypeListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacUserDynaFlowTaskTypeListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDynaFlowTaskTypeListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacUserDynaFlowTaskTypeList.PacUserDynaFlowTaskTypeListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacUserDynaFlowTaskTypeList.PacUserDynaFlowTaskTypeListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacUserDynaFlowTaskTypeList.PacUserDynaFlowTaskTypeListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
