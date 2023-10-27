using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
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
        public async override Task<IDataReader> GeneratePacUserDateGreaterThanFilterListReportAsync(
            SessionContext context,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDateGreaterThanFilterListReportAsync";
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
                var pacUserDateGreaterThanFilterList = new EF.Reports.PacUserDateGreaterThanFilterList(dbContext);
                List<EF.Reports.PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListDTO> pacUserDateGreaterThanFilterListDTOs =
                    await pacUserDateGreaterThanFilterList.GetAsync(
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);
                rdr = BuildDataReader(pacUserDateGreaterThanFilterListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDateGreaterThanFilterListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }
        public override IDataReader GeneratePacUserDateGreaterThanFilterListReport(
            SessionContext context,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserDateGreaterThanFilterListReportAsync";
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
                var pacUserDateGreaterThanFilterList = new EF.Reports.PacUserDateGreaterThanFilterList(dbContext);
                List<EF.Reports.PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListDTO> pacUserDateGreaterThanFilterListDTOs =
                    pacUserDateGreaterThanFilterList.Get(
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);
                rdr = BuildDataReader(pacUserDateGreaterThanFilterListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDateGreaterThanFilterListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async override Task<int> GetPacUserDateGreaterThanFilterListReportItemCountAsync(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacUserDateGreaterThanFilterListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var pacUserDateGreaterThanFilterList = new EF.Reports.PacUserDateGreaterThanFilterList(dbContext);
                iOut = await pacUserDateGreaterThanFilterList.GetCountAsync(
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDateGreaterThanFilterListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }
        public override int GetPacUserDateGreaterThanFilterListReportItemCount(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacUserDateGreaterThanFilterListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);
                var pacUserDateGreaterThanFilterList = new EF.Reports.PacUserDateGreaterThanFilterList(dbContext);
                iOut = pacUserDateGreaterThanFilterList.GetCount(
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserDateGreaterThanFilterListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacUserDateGreaterThanFilterListReportSummaryAsync(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDateGreaterThanFilterListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetPacUserDateGreaterThanFilterListReportSummary(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserDateGreaterThanFilterListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }
        private IDataReader BuildDataReader(List<EF.Reports.PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListDTO> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
