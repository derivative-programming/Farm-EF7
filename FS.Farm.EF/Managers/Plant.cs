/*
GetCount
GetMaxID
Insert(props)
Update(props)
Search (props)
GetList
GetCode(ID)
Get(ID)
DirtyGet(ID)
GetByCode(code)
DirtyGetByCode(code)
Get(code)
BulkInsert(list<obj))
BulkUpdate(list<obj))
BulkDelete(list<obj))
Delete(id)
FetchByLand(landid)
FetchByFetchByFlvrForeignKeyID(FlvrForeignKeyID)




*/


using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using FS.Common.Objects;
using System.Threading.Tasks;
using Farm.DbContexts.
namespace Farm.Managers
{
    public class PlantManager 
    { 
        
        private readonly PlantContext _dbContext;
        private SessionContext _sessionContext;

        public PlantManager(SessionContext sessionContext, PlantContext dbContext)
        {
            _sessionContext = sessionContext;
            _dbContext = dbContext;
        }
        
        #region Plant Methods
        public int PlantGetCount(
             )
        {
            string procedureName = "PlantGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetCount", paramArray);
                }
                else
                {
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetCount", paramArray);
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
                HandleError(paramArray, x, "FS_Farm_Plant_GetCount");
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async Task<int> PlantGetCountAsync(
             )
        {
            string procedureName = "PlantGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetCount", paramArray);
                }
                else
                {
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetCount", paramArray);
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
                await HandleErrorAsync(context, paramArray, x, "FS_Farm_Plant_GetCount");
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public int PlantGetMaxID(
            )
        {
            string procedureName = "PlantGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetMaxID", paramArray);
                }
                else
                {
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetMaxID", paramArray);
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
                HandleError(paramArray, x, "FS_Farm_Plant_GetMaxID");
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async Task<int> PlantGetMaxIDAsync(
            )
        {
            string procedureName = "PlantGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetMaxID", paramArray);
                }
                else
                {
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetMaxID", paramArray);
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
                await HandleErrorAsync(context, paramArray, x, "FS_Farm_Plant_GetMaxID");
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public int PlantInsert(
            ,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            System.Guid code)
        {
            string procedureName = "PlantInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
						 new SqlParameter("@i_uidInsertUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
                         new SqlParameter("@i_uidLastUpdateUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
    					
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Insert", paramArray);
                }
                else
                {
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Insert", paramArray);
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
                HandleError(paramArray, x, "FS_Farm_Plant_Insert");
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public async Task<int> PlantInsertAsync(
            ,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            System.Guid code)
        {
            string procedureName = "PlantInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
						 new SqlParameter("@i_uidInsertUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
                         new SqlParameter("@i_uidLastUpdateUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
    					
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Insert", paramArray);
                }
                else
                {
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Insert", paramArray);
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
                await HandleErrorAsync(context, paramArray, x, "FS_Farm_Plant_Insert");
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public void PlantUpdate(
            ,
            int plantID,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "PlantUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
			             new SqlParameter("@i_uidLastUpdateUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
    					 new SqlParameter("@i_uidLastChangeCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, lastChangeCode),
    					
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
 					};
            SqlConnection connection = null;
            try
            {
                //Execute the command using the connection string from the db base class
                //and get the number of rows affected by the operation
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
                    if (Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Update", paramArray) < 1)
                    {
                        throw new System.Exception("Your changes will overwrite changes made by another user.");
                    }
                }
                else
                {
                    if (Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Update", paramArray) < 1)
                    {
                        throw new System.Exception("Your changes will overwrite changes made by another user.");
                    }
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Farm_Plant_Update");
            }
            Log(procedureName + "::End");
        }
        public async Task PlantUpdateAsync(
            ,
            int plantID,
            Int32 flvrForeignKeyID,
            Boolean isDeleteAllowed,
            Boolean isEditAllowed,
            Int32 landID,
            String otherFlavor,
            Int64 someBigIntVal,
            Boolean someBitVal,
            DateTime someDateVal,
            Decimal someDecimalVal,
            String someEmailAddress,
            Double someFloatVal,
            Int32 someIntVal,
            Decimal someMoneyVal,
            String someNVarCharVal,
            String somePhoneNumber,
            String someTextVal,
            Guid someUniqueidentifierVal,
            DateTime someUTCDateTimeVal,
            String someVarCharVal,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "PlantUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
			             new SqlParameter("@i_uidLastUpdateUserID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, context.UserID),
    					 new SqlParameter("@i_uidLastChangeCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, lastChangeCode),
    					
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
 					};
            SqlConnection connection = null;
            try
            {
                //Execute the command using the connection string from the db base class
                //and get the number of rows affected by the operation
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
                    if (await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQueryAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Update", paramArray) < 1)
                    {
                        throw new System.Exception("Your changes will overwrite changes made by another user.");
                    }
                }
                else
                {
                    if (await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQueryAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Update", paramArray) < 1)
                    {
                        throw new System.Exception("Your changes will overwrite changes made by another user.");
                    }
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await HandleErrorAsync(context, paramArray, x, "FS_Farm_Plant_Update");
            }
            await LogAsync(context, procedureName + "::End");
        }
        public  SearchPlants(
            ,
            bool searchByPlantID, int plantID,
            bool searchByFlvrForeignKeyID, Int32 flvrForeignKeyID,
            bool searchByIsDeleteAllowed, Boolean isDeleteAllowed,
            bool searchByIsEditAllowed, Boolean isEditAllowed,
            bool searchByLandID, Int32 landID,
            bool searchByOtherFlavor, String otherFlavor,
            bool searchBySomeBigIntVal, Int64 someBigIntVal,
            bool searchBySomeBitVal, Boolean someBitVal,
            bool searchBySomeDateVal, DateTime someDateVal,
            bool searchBySomeDecimalVal, Decimal someDecimalVal,
            bool searchBySomeEmailAddress, String someEmailAddress,
            bool searchBySomeFloatVal, Double someFloatVal,
            bool searchBySomeIntVal, Int32 someIntVal,
            bool searchBySomeMoneyVal, Decimal someMoneyVal,
            bool searchBySomeNVarCharVal, String someNVarCharVal,
            bool searchBySomePhoneNumber, String somePhoneNumber,
            bool searchBySomeTextVal, String someTextVal,
            bool searchBySomeUniqueidentifierVal, Guid someUniqueidentifierVal,
            bool searchBySomeUTCDateTimeVal, DateTime someUTCDateTimeVal,
            bool searchBySomeVarCharVal, String someVarCharVal,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPlants";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                        new SqlParameter("@i_bSearchByPlantID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByPlantID),
                        new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
                        new SqlParameter("@i_bSearchByFlvrForeignKeyID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByFlvrForeignKeyID),
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                        new SqlParameter("@i_bSearchByIsDeleteAllowed", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByIsDeleteAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                        new SqlParameter("@i_bSearchByIsEditAllowed", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByIsEditAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                        new SqlParameter("@i_bSearchByLandID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByLandID),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                        new SqlParameter("@i_bSearchByOtherFlavor", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByOtherFlavor),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                        new SqlParameter("@i_bSearchBySomeBigIntVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeBigIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                        new SqlParameter("@i_bSearchBySomeBitVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeBitVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                        new SqlParameter("@i_bSearchBySomeDateVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeDateVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                        new SqlParameter("@i_bSearchBySomeDecimalVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeDecimalVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                        new SqlParameter("@i_bSearchBySomeEmailAddress", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeEmailAddress),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_bSearchBySomeFloatVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeFloatVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                        new SqlParameter("@i_bSearchBySomeIntVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeIntVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                        new SqlParameter("@i_bSearchBySomeMoneyVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeMoneyVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                        new SqlParameter("@i_bSearchBySomeNVarCharVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeNVarCharVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                        new SqlParameter("@i_bSearchBySomePhoneNumber", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomePhoneNumber),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                        new SqlParameter("@i_bSearchBySomeTextVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeTextVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                        new SqlParameter("@i_bSearchBySomeUniqueidentifierVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeUniqueidentifierVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                        new SqlParameter("@i_bSearchBySomeUTCDateTimeVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                        new SqlParameter("@i_bSearchBySomeVarCharVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                        new SqlParameter("@i_bSearchByCode", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByCode),
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code)
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Search", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Search", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_Search: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> SearchPlantsAsync(
                    SessionContext context,
                    bool searchByPlantID, int plantID,
                    bool searchByFlvrForeignKeyID, Int32 flvrForeignKeyID,
                    bool searchByIsDeleteAllowed, Boolean isDeleteAllowed,
                    bool searchByIsEditAllowed, Boolean isEditAllowed,
                    bool searchByLandID, Int32 landID,
                    bool searchByOtherFlavor, String otherFlavor,
                    bool searchBySomeBigIntVal, Int64 someBigIntVal,
                    bool searchBySomeBitVal, Boolean someBitVal,
                    bool searchBySomeDateVal, DateTime someDateVal,
                    bool searchBySomeDecimalVal, Decimal someDecimalVal,
                    bool searchBySomeEmailAddress, String someEmailAddress,
                    bool searchBySomeFloatVal, Double someFloatVal,
                    bool searchBySomeIntVal, Int32 someIntVal,
                    bool searchBySomeMoneyVal, Decimal someMoneyVal,
                    bool searchBySomeNVarCharVal, String someNVarCharVal,
                    bool searchBySomePhoneNumber, String somePhoneNumber,
                    bool searchBySomeTextVal, String someTextVal,
                    bool searchBySomeUniqueidentifierVal, Guid someUniqueidentifierVal,
                    bool searchBySomeUTCDateTimeVal, DateTime someUTCDateTimeVal,
                    bool searchBySomeVarCharVal, String someVarCharVal,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPlantsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                        new SqlParameter("@i_bSearchByPlantID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByPlantID),
                        new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
                        new SqlParameter("@i_bSearchByFlvrForeignKeyID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByFlvrForeignKeyID),
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
                        new SqlParameter("@i_bSearchByIsDeleteAllowed", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByIsDeleteAllowed),
                         new SqlParameter("@i_bIsDeleteAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isDeleteAllowed),
                        new SqlParameter("@i_bSearchByIsEditAllowed", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByIsEditAllowed),
                         new SqlParameter("@i_bIsEditAllowed", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, isEditAllowed),
                        new SqlParameter("@i_bSearchByLandID", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByLandID),
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
                        new SqlParameter("@i_bSearchByOtherFlavor", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByOtherFlavor),
                         new SqlParameter("@i_strOtherFlavor", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, otherFlavor),
                        new SqlParameter("@i_bSearchBySomeBigIntVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeBigIntVal),
                         new SqlParameter("@i_lSomeBigIntVal", SqlDbType.BigInt, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBigIntVal),
                        new SqlParameter("@i_bSearchBySomeBitVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeBitVal),
                         new SqlParameter("@i_bSomeBitVal", SqlDbType.Bit, 1 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someBitVal),
                        new SqlParameter("@i_bSearchBySomeDateVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeDateVal),
                         new SqlParameter("@i_dtSomeDateVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDateVal),
                        new SqlParameter("@i_bSearchBySomeDecimalVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeDecimalVal),
                         new SqlParameter("@i_dSomeDecimalVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someDecimalVal),
                        new SqlParameter("@i_bSearchBySomeEmailAddress", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeEmailAddress),
                         new SqlParameter("@i_strSomeEmailAddress", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someEmailAddress),
                        new SqlParameter("@i_bSearchBySomeFloatVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeFloatVal),
                         new SqlParameter("@i_fltSomeFloatVal", SqlDbType.Float, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someFloatVal),
                        new SqlParameter("@i_bSearchBySomeIntVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeIntVal),
                         new SqlParameter("@i_iSomeIntVal", SqlDbType.Int, 4 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someIntVal),
                        new SqlParameter("@i_bSearchBySomeMoneyVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeMoneyVal),
                         new SqlParameter("@i_dSomeMoneyVal", SqlDbType.Decimal, 9 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someMoneyVal),
                        new SqlParameter("@i_bSearchBySomeNVarCharVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeNVarCharVal),
                         new SqlParameter("@i_strSomeNVarCharVal", SqlDbType.NVarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someNVarCharVal),
                        new SqlParameter("@i_bSearchBySomePhoneNumber", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomePhoneNumber),
                         new SqlParameter("@i_strSomePhoneNumber", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, somePhoneNumber),
                        new SqlParameter("@i_bSearchBySomeTextVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeTextVal),
                         new SqlParameter("@i_strSomeTextVal", SqlDbType.Text, 2147483647 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someTextVal),
                        new SqlParameter("@i_bSearchBySomeUniqueidentifierVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeUniqueidentifierVal),
                         new SqlParameter("@i_uidSomeUniqueidentifierVal", SqlDbType.UniqueIdentifier, 16 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUniqueidentifierVal),
                        new SqlParameter("@i_bSearchBySomeUTCDateTimeVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeUTCDateTimeVal),
                         new SqlParameter("@i_dtSomeUTCDateTimeVal", SqlDbType.DateTime2, 8 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someUTCDateTimeVal),
                        new SqlParameter("@i_bSearchBySomeVarCharVal", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchBySomeVarCharVal),
                         new SqlParameter("@i_strSomeVarCharVal", SqlDbType.VarChar, 50 , ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, someVarCharVal),
                        new SqlParameter("@i_bSearchByCode", SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, searchByCode),
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code)
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Search", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Search", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_Search: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public  GetPlantList(
            SessionContext context)
        {
            string procedureName = "GetPlantList";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetList", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetList", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetList: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetPlantListAsync(
            SessionContext context)
        {
            string procedureName = "GetPlantListAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetList", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetList", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetList: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public Guid GetPlantCode(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlantCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Plant::" + plantID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    Log(procedureName + "::Get From Cache");
                    Log(procedureName + "::End");
                    return result;
                }
            }
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetCode", paramArray);
                }
                if (rdr.Read())
                {
                    result = Guid.Parse(rdr[0].ToString());
                    FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
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
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return result;
        }
        public async Task<Guid> GetPlantCodeAsync(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlantCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Plant::" + plantID.ToString() + "::code";
            if (FS.Common.Caches.StringCache.Exists(cacheKey))
            {
                string codeStr = FS.Common.Caches.StringCache.GetData(cacheKey);
                if (Guid.TryParse(codeStr, out result))
                {
                    await LogAsync(context, procedureName + "::Get From Cache");
                    await LogAsync(context, procedureName + "::End");
                    return result;
                }
            }
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetCode", paramArray);
                }
                bool readResult = await rdr.ReadAsync();
                if (readResult)
                {
                    result = Guid.Parse(rdr[0].ToString());
                    FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
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
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public  GetPlant(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Get", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Get", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_Get: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetPlantAsync(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Get", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Get", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_Get: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public  GetDirtyPlant(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetDirtyPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                        new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGet", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGet", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGet: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetDirtyPlantAsync(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetDirtyPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                        new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGet", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGet", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGet: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public  GetPlant(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetByCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetByCode", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetByCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetPlantAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetByCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetByCode", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetByCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public  GetDirtyPlant(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGetByCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGetByCode", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGetByCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetDirtyPlantAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
                        new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGetByCode", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_DirtyGetByCode", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGetByCode: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public int GetPlantID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPlantID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetID", paramArray);
                }
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr[0].ToString());
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
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return result;
        }
        public async Task<int> GetPlantIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPlantIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_uidCode", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, code),
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
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_GetID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_GetID", paramArray);
                }
                bool readResult = await rdr.ReadAsync();
                if (readResult)
                {
                    result = Convert.ToInt32(rdr[0].ToString());
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
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public int PlantBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
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
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                context.UserID,
                context.UserID,
                dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkInsert_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                cmd.ExecuteNonQuery();
                if (!context.UseTransactions)
                {
                    connection.Close();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public async Task<int> PlantBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
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
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                context.UserID,
                context.UserID,
                dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkInsert_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                await cmd.ExecuteNonQueryAsync();
                if (!context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public int PlantBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
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
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID == 0)
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                        context.UserID,
                        context.UserID,
                        dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkUpdate_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                cmd.ExecuteNonQuery();
                if (!context.UseTransactions)
                {
                    connection.Close();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public async Task<int> PlantBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
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
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            //context.RollBackTransactions();
                            //if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID == 0)
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                        context.UserID,
                        context.UserID,
                        dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkUpdate_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                await cmd.ExecuteNonQueryAsync();
                if (!context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public int PlantBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
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
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID == 0)
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                        context.UserID,
                        context.UserID,
                        dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkDelete_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                cmd.ExecuteNonQuery();
                if (!context.UseTransactions)
                {
                    connection.Close();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public async Task<int> PlantBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
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
                    connection = context.GetSqlTransaction(_connectionString).Connection;
                }
                else
                {
                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                }
                string ipAddress = FS.Common.Configuration.Machine.GetIP();
                DataTable table = new DataTable();
                // Add columns and rows. The following is a simple example.
                table.Columns.Add("PlantID", typeof(int));
                table.Columns.Add("FlvrForeignKeyID", typeof(Int32));
                table.Columns.Add("IsDeleteAllowed", typeof(Boolean));
                table.Columns.Add("IsEditAllowed", typeof(Boolean));
                table.Columns.Add("LandID", typeof(Int32));
                table.Columns.Add("OtherFlavor", typeof(String));
                table.Columns.Add("SomeBigIntVal", typeof(Int64));
                table.Columns.Add("SomeBitVal", typeof(Boolean));
                table.Columns.Add("SomeDateVal", typeof(DateTime));
                table.Columns.Add("SomeDecimalVal", typeof(Decimal));
                table.Columns.Add("SomeEmailAddress", typeof(String));
                table.Columns.Add("SomeFloatVal", typeof(Double));
                table.Columns.Add("SomeIntVal", typeof(Int32));
                table.Columns.Add("SomeMoneyVal", typeof(Decimal));
                table.Columns.Add("SomeNVarCharVal", typeof(String));
                table.Columns.Add("SomePhoneNumber", typeof(String));
                table.Columns.Add("SomeTextVal", typeof(String));
                table.Columns.Add("SomeUniqueidentifierVal", typeof(Guid));
                table.Columns.Add("SomeUTCDateTimeVal", typeof(DateTime));
                table.Columns.Add("SomeVarCharVal", typeof(String));
                table.Columns.Add("InsertUserID", typeof(Guid));
                table.Columns.Add("LastUpdateUserID", typeof(Guid));
                table.Columns.Add("LastChangeCode", typeof(Guid));
                table.Columns.Add("Code", typeof(Guid));
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID == 0)
                        continue;
                    table.Rows.Add(
                        dataList[i].PlantID,
                        dataList[i].FlvrForeignKeyID,
                        dataList[i].IsDeleteAllowed,
                        dataList[i].IsEditAllowed,
                        dataList[i].LandID,
                        dataList[i].OtherFlavor,
                        dataList[i].SomeBigIntVal,
                        dataList[i].SomeBitVal,
                        dataList[i].SomeDateVal,
                        dataList[i].SomeDecimalVal,
                        dataList[i].SomeEmailAddress,
                        dataList[i].SomeFloatVal,
                        dataList[i].SomeIntVal,
                        dataList[i].SomeMoneyVal,
                        dataList[i].SomeNVarCharVal,
                        dataList[i].SomePhoneNumber,
                        dataList[i].SomeTextVal,
                        dataList[i].SomeUniqueidentifierVal,
                        dataList[i].SomeUTCDateTimeVal,
                        dataList[i].SomeVarCharVal,
                        context.UserID,
                        context.UserID,
                        dataList[i].LastChangeCode,
                        dataList[i].Code
                        );
                }
                SqlCommand cmd = new SqlCommand(
                        "FS_Farm_Plant_BulkDelete_v19",
                        connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (context.UseTransactions)
                {
                    cmd.Transaction = context.GetSqlTransaction(_connectionString);
                }
                cmd.Parameters.Add(
                    new SqlParameter()
                    {
                        ParameterName = "@i_tblData",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "UDT_FS_Farm_Plant_v19",
                        Value = table,
                    });
                await cmd.ExecuteNonQueryAsync();
                if (!context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public void PlantDelete(
            SessionContext context,
            int plantID)
        {
            string procedureName = "PlantDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Delete", paramArray);
                }
                else
                {
                    //Execute the command using the connection string from the db base class
                    //and get the number of rows affected by the operation
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Delete", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Farm_Plant_Delete");
            }
            Log(procedureName + "::End");
        }
        public async Task PlantDeleteAsync(
           SessionContext context,
           int plantID)
        {
            string procedureName = "PlantDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
						new SqlParameter("@i_iPlantID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, plantID),
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
                    await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQueryAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_Delete", paramArray);
                }
                else
                {
                    //Execute the command using the connection string from the db base class
                    //and get the number of rows affected by the operation
                    await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQueryAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_Delete", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    await connection.CloseAsync();
                }
                await HandleErrorAsync(context, paramArray, x, "FS_Farm_Plant_Delete");
            }
            await LogAsync(context, procedureName + "::End");
        }
        public void PlantCleanupTesting(
            SessionContext context )
        {
            string procedureName = "PlantCleanupTesting";
            Log(procedureName + "::Start");
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
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
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_TestObjectCleanup", paramArray);
                }
                else
                {
                    //Execute the command using the connection string from the db base class
                    //and get the number of rows affected by the operation
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "FS_Farm_TestObjectCleanup", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Farm_TestObjectCleanup");
            }
            Log(procedureName + "::End");
        }
        public void PlantCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "PlantCleanupChildObjectTesting";
            Log(procedureName + "::Start");
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
                    {
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
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_TestChildObjectCleanup", paramArray);
                }
                else
                {
                    //Execute the command using the connection string from the db base class
                    //and get the number of rows affected by the operation
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, "FS_Farm_TestChildObjectCleanup", paramArray);
                }
            }
            catch (Exception x)
            {
                if (connection != null && !context.UseTransactions)
                {
                    connection.Close();
                }
                HandleError(paramArray, x, "FS_Farm_TestChildObjectCleanup");
            }
            Log(procedureName + "::End");
        }
        public bool SupportsTransactions()
        {
            return true;
        }
        #endregion
        #region Error Handling
        void HandleError(SqlParameter[] paramArray, Exception x, string sprocName)
        {
            Log(x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            foreach (SqlParameter p in paramArray)
            {
                sException += p.ParameterName + "=" + p.Value + "\r\n";
            }
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsync(SessionContext context, SqlParameter[] paramArray, Exception x, string sprocName)
        {
            await LogAsync(context, x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            foreach (SqlParameter p in paramArray)
            {
                sException += p.ParameterName + "=" + p.Value + "\r\n";
            }
            throw new Exception(sException, x);
        }
        #endregion

        public IDataReader GetPlantList_FetchByLandID(
            int landID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByLandID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_iLandID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
					};
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        SqlConnection connection = null;
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            connection.Open();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            context.RollBackTransactions();
                            if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_FetchByLandID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_FetchByLandID", paramArray);
                }
            }
            catch (Exception x)
            {
                context.RollBackTransactions();
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_FetchByLandID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<IDataReader> GetPlantList_FetchByLandIDAsync(
            int landID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByLandIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                        new SqlParameter("@i_iLandID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, landID),
					};
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        SqlConnection connection = null;
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            await connection.OpenAsync();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            await context.RollBackTransactionsAsync();
                            if (connection != null && connection.State == ConnectionState.Open)
                            {
                                await connection.CloseAsync();
                            }
                            throw;
                        }
                    }
                    rdr =await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_FetchByLandID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_FetchByLandID", paramArray);
                }
            }
            catch (Exception x)
            {
                await context.RollBackTransactionsAsync();
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_FetchByLandID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public IDataReader GetPlantList_FetchByFlvrForeignKeyID(
            int flvrForeignKeyID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByFlvrForeignKeyID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                         new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
					};
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        SqlConnection connection = null;
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            connection.Open();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            context.RollBackTransactions();
                            if (connection != null && connection.State == ConnectionState.Open) connection.Close();
                            throw;
                        }
                    }
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_FetchByFlvrForeignKeyID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_FetchByFlvrForeignKeyID", paramArray);
                }
            }
            catch (Exception x)
            {
                context.RollBackTransactions();
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_FetchByFlvrForeignKeyID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public async Task<> GetPlantList_FetchByFlvrForeignKeyIDAsync(
            int flvrForeignKeyID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByFlvrForeignKeyIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            SqlParameter[] paramArray = new SqlParameter[]
					{
                        new SqlParameter("@i_iFlvrForeignKeyID", SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, null, DataRowVersion.Current, flvrForeignKeyID),
					};
            try
            {
                if (context.UseTransactions)
                {
                    if (!context.SqlConnectionExists(_connectionString))
                    {
                        if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
                        SqlConnection connection = null;
                        try
                        {
                            connection = new SqlConnection(_connectionString);
                            await connection.OpenAsync();
                            context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                        }
                        catch
                        {
                            // If we fail to return the SqlDatReader, we need to close the connection ourselves
                            await context.RollBackTransactionsAsync();
                            if (connection != null && connection.State == ConnectionState.Open)
                            {
                                await connection.CloseAsync();
                            }
                            throw;
                        }
                    }
                    rdr =await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(context.GetSqlTransaction(_connectionString), CommandType.StoredProcedure, "FS_Farm_Plant_FetchByFlvrForeignKeyID", paramArray);
                }
                else
                {
                    //Fill the dataset using the connection string from the db base class
                    rdr = await Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReaderAsync(_connectionString, CommandType.StoredProcedure, "FS_Farm_Plant_FetchByFlvrForeignKeyID", paramArray);
                }
            }
            catch (Exception x)
            {
                await context.RollBackTransactionsAsync();
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_FetchByFlvrForeignKeyID: \r\n";
                foreach (SqlParameter p in paramArray)
                {
                    sException += p.ParameterName + "=" + p.Value + "\r\n";
                }
                throw new Exception(sException, x);
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
    }
}
