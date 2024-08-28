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
        public async override Task<IDataReader> GeneratePacUserDynaFlowTypeListReportAsync(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDynaFlowTypeListReportAsync";
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

                var pacUserDynaFlowTypeList = new EF.Reports.PacUserDynaFlowTypeList(dbContext);

                List<EF.Reports.PacUserDynaFlowTypeList.PacUserDynaFlowTypeListDTO> pacUserDynaFlowTypeListDTOs =
                    await pacUserDynaFlowTypeList.GetAsync(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacUserDynaFlowTypeListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTypeListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }

        public override IDataReader GeneratePacUserDynaFlowTypeListReport(
            SessionContext context,

            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDynaFlowTypeListReportAsync";
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

                var pacUserDynaFlowTypeList = new EF.Reports.PacUserDynaFlowTypeList(dbContext);
                List<EF.Reports.PacUserDynaFlowTypeList.PacUserDynaFlowTypeListDTO> pacUserDynaFlowTypeListDTOs =
                    pacUserDynaFlowTypeList.Get(

                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(pacUserDynaFlowTypeListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetPacUserDynaFlowTypeListReportItemCountAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacUserDynaFlowTypeListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacUserDynaFlowTypeList = new EF.Reports.PacUserDynaFlowTypeList(dbContext);
                iOut = await pacUserDynaFlowTypeList.GetCountAsync(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetPacUserDynaFlowTypeListReportItemCount(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacUserDynaFlowTypeListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;

            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacUserDynaFlowTypeList = new EF.Reports.PacUserDynaFlowTypeList(dbContext);

                iOut = pacUserDynaFlowTypeList.GetCount(

                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDynaFlowTypeListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacUserDynaFlowTypeListReportSummaryAsync(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDynaFlowTypeListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetPacUserDynaFlowTypeListReportSummary(
            SessionContext context,

            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDynaFlowTypeListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }

        private IDataReader BuildDataReader(List<EF.Reports.PacUserDynaFlowTypeList.PacUserDynaFlowTypeListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacUserDynaFlowTypeList.PacUserDynaFlowTypeListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacUserDynaFlowTypeList.PacUserDynaFlowTypeListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
