using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using FS.Common.Objects;
namespace FS.Farm.Reports.Providers.SqlServer
{
    partial class EF7FarmReportProvider : FS.Farm.Reports.Providers.FarmReportProvider
    {
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
            string procedureName = "GenerateLandPlantListReport";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_iPageNumber", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, pageNumber),
                        new SqlParameter("@i_iItemCountPerPage", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, itemCountPerPage),
                        new SqlParameter("@i_strOrderByColumnName", SqlDbType.VarChar, 50, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, orderByColumnName),
                        new SqlParameter("@i_bOrderByDescending", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, orderByDescending),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
					};
            SqlConnection connection = null;
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            connection.Open();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantList", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantList", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                string sException = "Error Executing FS_Report_Farm_LandPlantList: \r\n";
                Log(x);
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
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
                orderByDescending = System.Convert.ToBoolean("False");
            }
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_iPageNumber", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, pageNumber),
                        new SqlParameter("@i_iItemCountPerPage", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, itemCountPerPage),
                        new SqlParameter("@i_strOrderByColumnName", SqlDbType.VarChar, 50, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, orderByColumnName),
                        new SqlParameter("@i_bOrderByDescending", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, orderByDescending),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
					};
            SqlConnection connection = null;
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            await connection.OpenAsync();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantList", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantList", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                string sException = "Error Executing FS_Report_Farm_LandPlantList: \r\n";
                await LogAsync(context,x);
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context,procedureName + "::End");
            return rdr;
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
            string procedureName = "GetLandPlantListReportItemCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
    				};
            SqlConnection connection = null;
            try
            {
                //Fill the dataset using the connection string from the db base class
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            connection.Open();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetCount", paramArray);
                }
                else
                {
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetCount", paramArray);
                }
                if (rdr.Read())
                {
                    iOut = Convert.ToInt32(rdr[0].ToString());
                }
                rdr.Close();
                rdr.Dispose();
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Report_Farm_LandPlantListGetCount");
            }
            pageCount = 0;
            if (iOut > 0)
                pageCount = iOut / itemCountPerPage;
            Log(procedureName + "::End");
            return iOut;
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
            int itemCountPerPage )
        {
            string procedureName = "GetLandPlantListReportItemCountAsync";
            await LogAsync(context,procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
    				};
            SqlConnection connection = null;
            try
            {
                //Fill the dataset using the connection string from the db base class
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            await connection.OpenAsync();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetCount", paramArray);
                }
                else
                {
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetCount", paramArray);
                }
                bool readResult = await rdr.ReadAsync();
                if (readResult)
                {
                    iOut = Convert.ToInt32(rdr[0].ToString());
                }
                await rdr.CloseAsync();
                await rdr.DisposeAsync();
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                HandleError(paramArray, x, "FS_Report_Farm_LandPlantListGetCount");
            }
            await LogAsync(context,procedureName + "::End");
            return iOut;
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
            string procedureName = "GetLandPlantListReportSummary";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
                    };
            SqlConnection connection = null;
            try
            {
                //Fill the dataset using the connection string from the db base class
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            connection.Open();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetSummary", paramArray);
                }
                else
                {
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetSummary", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Report_Farm_LandPlantListGetSummary");
            }
            Log(procedureName + "::End");
            return rdr;
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
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                         new SqlParameter("@i_uidFlavorCode", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flavorCode),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_dtSomeMinUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeMinDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMinDateVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_uidUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, userID),
                        new SqlParameter("@i_uidContextCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, contextCode),
                    };
            SqlConnection connection = null;
            try
            {
                //Fill the dataset using the connection string from the db base class
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            await connection.OpenAsync();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            //// If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetSummary", paramArray);
                }
                else
                {
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Report_Farm_LandPlantListGetSummary", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                HandleError(paramArray, x, "FS_Report_Farm_LandPlantListGetSummary");
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
    }
}
