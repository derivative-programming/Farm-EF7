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
        public async override Task<IDataReader> GenerateLandPlantListReportAsync(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GenerateLandPlantListReportAsync";
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

                var landPlantList = new EF.Reports.LandPlantList(dbContext);

                List<EF.Reports.LandPlantList.LandPlantListDTO> landPlantListDTOs =
                    await landPlantList.GetAsync(
                            flavorCode,
                            someIntVal,
                            someBigIntVal,
                            someFloatVal,
                            someBitVal,
                            isEditAllowed,
                            isDeleteAllowed,
                            someDecimalVal,
                            someMinUTCDateTimeVal,
                            someMinDateVal,
                            someMoneyVal,
                            someNVarCharVal,
                            someVarCharVal,
                            someTextVal,
                            somePhoneNumber,
                            someEmailAddress,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(landPlantListDTOs); 
            }
            catch (Exception x)
            { 
                HandleError(x, "FS_Report_Farm_LandPlantListGetCount");
            }
            finally
            {
                if(dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
        }


        public override IDataReader GenerateLandPlantListReport(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            int pageNumber,
            int itemCountPerPage,
            string orderByColumnName,
            bool orderByDescending,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GenerateLandPlantListReportAsync";
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

                var landPlantList = new EF.Reports.LandPlantList(dbContext);
                List<EF.Reports.LandPlantList.LandPlantListDTO> landPlantListDTOs =
                    landPlantList.Get(
                            flavorCode,
                            someIntVal,
                            someBigIntVal,
                            someFloatVal,
                            someBitVal,
                            isEditAllowed,
                            isDeleteAllowed,
                            someDecimalVal,
                            someMinUTCDateTimeVal,
                            someMinDateVal,
                            someMoneyVal,
                            someNVarCharVal,
                            someVarCharVal,
                            someTextVal,
                            somePhoneNumber,
                            someEmailAddress,
                            userID,
                            contextCode,
                            pageNumber,
                            itemCountPerPage,
                            orderByColumnName,
                            orderByDescending);

                rdr = BuildDataReader(landPlantListDTOs);
            }
            catch (Exception x)
            { 
                HandleError(x, "FS_Report_Farm_LandPlantListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }

        public async override Task<int> GetLandPlantListReportItemCountAsync(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage)
        {
            string procedureName = "GetLandPlantListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            IDataReader rdr = null;

            int iOut = 0;
             
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landPlantList = new EF.Reports.LandPlantList(dbContext);
                iOut = await landPlantList.GetCountAsync(
                            flavorCode,
                            someIntVal,
                            someBigIntVal,
                            someFloatVal,
                            someBitVal,
                            isEditAllowed,
                            isDeleteAllowed,
                            someDecimalVal,
                            someMinUTCDateTimeVal,
                            someMinDateVal,
                            someMoneyVal,
                            someNVarCharVal,
                            someVarCharVal,
                            someTextVal,
                            somePhoneNumber,
                            someEmailAddress,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            { 
                HandleError(x, "FS_Report_Farm_LandPlantListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
        }

        public override int GetLandPlantListReportItemCount(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            System.Guid userID,
            System.Guid contextCode,
            int itemCountPerPage,
            ref int pageCount)
        {
            string procedureName = "GetLandPlantListReportItemCountAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;

            int iOut = 0;
             
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landPlantList = new EF.Reports.LandPlantList(dbContext);

                iOut = landPlantList.GetCount(
                            flavorCode,
                            someIntVal,
                            someBigIntVal,
                            someFloatVal,
                            someBitVal,
                            isEditAllowed,
                            isDeleteAllowed,
                            someDecimalVal,
                            someMinUTCDateTimeVal,
                            someMinDateVal,
                            someMoneyVal,
                            someNVarCharVal,
                            someVarCharVal,
                            someTextVal,
                            somePhoneNumber,
                            someEmailAddress,
                            userID,
                            contextCode);
            }
            catch (Exception x)
            { 
                HandleError(x, "FS_Report_Farm_LandPlantListGetCount");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async override Task<IDataReader> GetLandPlantListReportSummaryAsync(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetLandPlantListReportSummaryAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        public override IDataReader GetLandPlantListReportSummary(
            SessionContext context,
            Guid flavorCode,
            Int32 someIntVal,
            Int64 someBigIntVal,
            Double someFloatVal,
            Boolean someBitVal,
            Boolean isEditAllowed,
            Boolean isDeleteAllowed,
            Decimal someDecimalVal,
            DateTime someMinUTCDateTimeVal,
            DateTime someMinDateVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String someVarCharVal,
            String someTextVal,
            String somePhoneNumber,
            String someEmailAddress,
            System.Guid userID,
            System.Guid contextCode)
        {
            string procedureName = "GetLandPlantListReportSummaryAsync";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            throw new System.Exception("Not Implemented");
            Log(procedureName + "::End");
            return rdr;
        } 

        private IDataReader BuildDataReader(List<EF.Reports.LandPlantList.LandPlantListDTO> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties 
            foreach (var prop in typeof(EF.Reports.LandPlantList.LandPlantListDTO).GetProperties())
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
                foreach (var prop in typeof(EF.Reports.LandPlantList.LandPlantListDTO).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
