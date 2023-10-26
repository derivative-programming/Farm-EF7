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
        public async override Task<IDataReader> GeneratePacUserRoleListReportAsync(
            SessionContext context,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserRoleListReportAsync";
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
                var pacUserRoleList = new EF.Reports.PacUserRoleList(dbContext);
                List<EF.Reports.PacUserRoleList.PacUserRoleListDTO> pacUserRoleListDTOs =
                    await pacUserRoleList.GetAsync(
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);
                rdr = BuildDataReader(pacUserRoleListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserRoleListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }
        public override IDataReader GeneratePacUserRoleListReport(
            SessionContext context,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GeneratePacUserRoleListReportAsync";
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
                var pacUserRoleList = new EF.Reports.PacUserRoleList(dbContext);
                List<EF.Reports.PacUserRoleList.PacUserRoleListDTO> pacUserRoleListDTOs =
                    pacUserRoleList.Get(
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);
                rdr = BuildDataReader(pacUserRoleListDTOs);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserRoleListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async override Task<int> GetPacUserRoleListReportItemCountAsync(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetPacUserRoleListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var pacUserRoleList = new EF.Reports.PacUserRoleList(dbContext);
                iOut = await pacUserRoleList.GetCountAsync(
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserRoleListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }
        public override int GetPacUserRoleListReportItemCount(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetPacUserRoleListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);
                var pacUserRoleList = new EF.Reports.PacUserRoleList(dbContext);
                iOut = pacUserRoleList.GetCount(
                            userID,
                            contextCode);
            }
            catch (Exception x)
            {
                HandleError(x, "FS_Report_Farm_PacUserRoleListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetPacUserRoleListReportSummaryAsync(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserRoleListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetPacUserRoleListReportSummary(
            SessionContext context,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetPacUserRoleListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        }
        private IDataReader BuildDataReader(List<EF.Reports.PacUserRoleList.PacUserRoleListDTO> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Reports.PacUserRoleList.PacUserRoleListDTO).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Reports.PacUserRoleList.PacUserRoleListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
