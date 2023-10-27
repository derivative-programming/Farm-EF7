using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using FS.Common.Objects;
using System.Threading.Tasks;
using FS.Farm.EF;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using FS.Farm.EF.Models;
using NetTopologySuite.Index.HPRtree;
using FS.Farm.Objects;

namespace FS.Farm.Providers.EF7
{
    partial class EF7PlantProvider : FS.Farm.Providers.PlantProvider
    {
        string _connectionString = "";
        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = configValue["connectionString"].ToString();
        }
        public override string Name
        {
            get
            {
                return null;
            }
        }
        #endregion
        #region Plant Methods
        public override int PlantGetCount(
            SessionContext context )
        {
            string procedureName = "PlantGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                iOut = plantManager.GetTotalCount(); 
            }
            catch (Exception x)
            { 
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> PlantGetCountAsync(
            SessionContext context )
        {
            string procedureName = "PlantGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                iOut = await plantManager.GetTotalCountAsync();
                 
            }
            catch (Exception x)
            { 
                await HandleErrorAsync(context,  x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int PlantGetMaxID(
            SessionContext context)
        {
            string procedureName = "PlantGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                iOut = plantManager.GetMaxId().Value; 
            }
            catch (Exception x)
            { 
                HandleError( x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> PlantGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "PlantGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                var maxId = await plantManager.GetMaxIdAsync();

                iOut = maxId.Value;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override int PlantInsert(
            SessionContext context,
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
            //ENDSET
            System.Guid code)
        {
            string procedureName = "PlantInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            //Int32 flvrForeignKeyID,
            //Boolean isDeleteAllowed,
            //Boolean isEditAllowed,
            //Int32 landID,
            //String otherFlavor,
            //Int64 someBigIntVal,
            //Boolean someBitVal, 
            //Decimal someDecimalVal,
            //String someEmailAddress,
            //Double someFloatVal,
            //Int32 someIntVal,
            //Decimal someMoneyVal,
            //String someNVarCharVal,
            //String somePhoneNumber,
            //String someTextVal,
            //Guid someUniqueidentifierVal, 
            //String someVarCharVal,
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
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                EF.Models.Plant plant = new EF.Models.Plant();
                plant.Code = code;
                plant.LastChangeCode = Guid.NewGuid();
                plant.SomeTextVal = someTextVal;
                plant.FlvrForeignKeyID = flvrForeignKeyID;
                plant.IsDeleteAllowed = isDeleteAllowed;
                plant.IsEditAllowed = isEditAllowed;
                plant.LandID = landID;
                plant.OtherFlavor = otherFlavor;
                plant.SomeBigIntVal = someBigIntVal;
                plant.SomeBitVal = someBitVal;
                plant.SomeDateVal = someDateVal;
                plant.SomeDecimalVal = someDecimalVal;
                plant.SomeEmailAddress = someEmailAddress;
                plant.SomeFloatVal = (float)someFloatVal;
                plant.SomeIntVal = someIntVal;
                plant.SomeMoneyVal = someMoneyVal;
                plant.SomeNVarCharVal = someNVarCharVal;
                plant.SomePhoneNumber = somePhoneNumber;
                plant.SomeUniqueidentifierVal = someUniqueidentifierVal;
                plant.SomeUTCDateTimeVal = someUTCDateTimeVal;
                plant.SomeVarCharVal = someVarCharVal;
                //ENDSET

                plant = plantManager.Add(plant);

                iOut = plant.PlantID; 
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return iOut;
        }
        public override async Task<int> PlantInsertAsync(
            SessionContext context,
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
            //ENDSET
            System.Guid code)
        {
            string procedureName = "PlantInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 flvrForeignKeyID,
            //Boolean isDeleteAllowed,
            //Boolean isEditAllowed,
            //Int32 landID,
            //String otherFlavor,
            //Int64 someBigIntVal,
            //Boolean someBitVal, 
            //Decimal someDecimalVal,
            //String someEmailAddress,
            //Double someFloatVal,
            //Int32 someIntVal,
            //Decimal someMoneyVal,
            //String someNVarCharVal,
            //String somePhoneNumber,
            //String someTextVal,
            //Guid someUniqueidentifierVal, 
            //String someVarCharVal,
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
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                EF.Models.Plant plant = new EF.Models.Plant();
                plant.Code = code;
                plant.LastChangeCode = Guid.NewGuid();
                plant.SomeTextVal = someTextVal;
                plant.FlvrForeignKeyID = flvrForeignKeyID;
                plant.IsDeleteAllowed = isDeleteAllowed;
                plant.IsEditAllowed = isEditAllowed;
                plant.LandID = landID;
                plant.OtherFlavor = otherFlavor;
                plant.SomeBigIntVal = someBigIntVal;
                plant.SomeBitVal = someBitVal;
                plant.SomeDateVal = someDateVal;
                plant.SomeDecimalVal = someDecimalVal;
                plant.SomeEmailAddress = someEmailAddress;
                plant.SomeFloatVal = (float)someFloatVal;
                plant.SomeIntVal = someIntVal;
                plant.SomeMoneyVal = someMoneyVal;
                plant.SomeNVarCharVal = someNVarCharVal;
                plant.SomePhoneNumber = somePhoneNumber;
                plant.SomeUniqueidentifierVal = someUniqueidentifierVal;
                plant.SomeUTCDateTimeVal = someUTCDateTimeVal;
                plant.SomeVarCharVal = someVarCharVal;
                //ENDSET

                plant = await plantManager.AddAsync(plant);

                iOut = plant.PlantID;
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return iOut;
        }
        public override void PlantUpdate(
            SessionContext context,
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
            //ENDSET
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "PlantUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //Int32 flvrForeignKeyID,
            //Boolean isDeleteAllowed,
            //Boolean isEditAllowed,
            //Int32 landID,
            //String otherFlavor,
            //Int64 someBigIntVal,
            //Boolean someBitVal, 
            //Decimal someDecimalVal,
            //String someEmailAddress,
            //Double someFloatVal,
            //Int32 someIntVal,
            //Decimal someMoneyVal,
            //String someNVarCharVal,
            //String somePhoneNumber,
            //String someTextVal,
            //Guid someUniqueidentifierVal, 
            //String someVarCharVal,
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                EF.Models.Plant plant = new EF.Models.Plant();
                plant.Code = code;
                plant.SomeTextVal = someTextVal;
                plant.FlvrForeignKeyID = flvrForeignKeyID;
                plant.IsDeleteAllowed = isDeleteAllowed;
                plant.IsEditAllowed = isEditAllowed;
                plant.LandID = landID;
                plant.OtherFlavor = otherFlavor;
                plant.SomeBigIntVal = someBigIntVal;
                plant.SomeBitVal = someBitVal;
                plant.SomeDateVal = someDateVal;
                plant.SomeDecimalVal = someDecimalVal;
                plant.SomeEmailAddress = someEmailAddress;
                plant.SomeFloatVal = (float)someFloatVal;
                plant.SomeIntVal = someIntVal;
                plant.SomeMoneyVal = someMoneyVal;
                plant.SomeNVarCharVal = someNVarCharVal;
                plant.SomePhoneNumber = somePhoneNumber;
                plant.SomeUniqueidentifierVal = someUniqueidentifierVal;
                plant.SomeUTCDateTimeVal = someUTCDateTimeVal;
                plant.SomeVarCharVal = someVarCharVal;
                //ENDSET

                bool success = plantManager.Update(plant);
                if (!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }
                 
            }
            catch (Exception x)
            {
                HandleError(x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task PlantUpdateAsync(
            SessionContext context,
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
            //ENDSET
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "PlantUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 flvrForeignKeyID,
            //Boolean isDeleteAllowed,
            //Boolean isEditAllowed,
            //Int32 landID,
            //String otherFlavor,
            //Int64 someBigIntVal,
            //Boolean someBitVal, 
            //Decimal someDecimalVal,
            //String someEmailAddress,
            //Double someFloatVal,
            //Int32 someIntVal,
            //Decimal someMoneyVal,
            //String someNVarCharVal,
            //String somePhoneNumber,
            //String someTextVal,
            //Guid someUniqueidentifierVal, 
            //String someVarCharVal,
            if (System.Convert.ToDateTime(someDateVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someDateVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(someUTCDateTimeVal) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 someUTCDateTimeVal = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);


                EF.Models.Plant plant = new EF.Models.Plant();
                plant.Code = code;
                plant.SomeTextVal = someTextVal;
                plant.FlvrForeignKeyID = flvrForeignKeyID;
                plant.IsDeleteAllowed = isDeleteAllowed;
                plant.IsEditAllowed = isEditAllowed;
                plant.LandID = landID;
                plant.OtherFlavor = otherFlavor;
                plant.SomeBigIntVal = someBigIntVal;
                plant.SomeBitVal = someBitVal;
                plant.SomeDateVal = someDateVal;
                plant.SomeDecimalVal = someDecimalVal;
                plant.SomeEmailAddress = someEmailAddress;
                plant.SomeFloatVal = (float)someFloatVal;
                plant.SomeIntVal = someIntVal;
                plant.SomeMoneyVal = someMoneyVal;
                plant.SomeNVarCharVal = someNVarCharVal;
                plant.SomePhoneNumber = somePhoneNumber;
                plant.SomeUniqueidentifierVal = someUniqueidentifierVal;
                plant.SomeUTCDateTimeVal = someUTCDateTimeVal;
                plant.SomeVarCharVal = someVarCharVal;
                //ENDSET

                bool success = await plantManager.UpdateAsync(plant);
                if(!success)
                {
                    throw new System.Exception("Your changes will overwrite changes made by another user.");
                }

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, procedureName);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override IDataReader SearchPlants(
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
            //ENDSET
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPlants";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_Search: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> SearchPlantsAsync(
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
                    //ENDSET
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPlantsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_Search: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetPlantList(
            SessionContext context)
        {
            string procedureName = "GetPlantList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(plantManager.GetAll());
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetList: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetPlantListAsync(
            SessionContext context)
        {
            string procedureName = "GetPlantListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(await plantManager.GetAllAsync());
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetList: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override Guid GetPlantCode(
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
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                var plant = plantManager.GetById(plantID);

                result = plant.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1)); 
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<Guid> GetPlantCodeAsync(
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
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                var plant = await plantManager.GetByIdAsync(plantID);

                result = plant.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override IDataReader GetPlant(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = plantManager.GetById(plantID);

                plants.Add(plant);
                 
                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_Get: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetPlantAsync(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);


                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = await plantManager.GetByIdAsync(plantID);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_Get: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyPlant(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetDirtyPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);


                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = plantManager.DirtyGetById(plantID);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGet: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyPlantAsync(
            SessionContext context,
            int plantID)
        {
            string procedureName = "GetDirtyPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);
                 
                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = await plantManager.DirtyGetByIdAsync(plantID);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGet: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetPlant(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);
                 
                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = plantManager.GetByCode(code);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetByCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetPlantAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);


                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = await plantManager.GetByCodeAsync(code);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetByCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetDirtyPlant(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyPlant";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext); 

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = plantManager.DirtyGetByCode(code);

                plants.Add(plant);

                rdr = BuildDataReader(plants);
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGetByCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetDirtyPlantAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyPlantAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                var plant = await plantManager.DirtyGetByCodeAsync(code);

                plants.Add(plant);

                rdr = BuildDataReader(plants);

            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_DirtyGetByCode: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override int GetPlantID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPlantID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);  

                var plant = plantManager.GetByCode(code);

                result = plant.PlantID; 
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_GetID: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return result;
        }
        public override async Task<int> GetPlantIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPlantIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                var plant = await plantManager.GetByCodeAsync(code);

                result = plant.PlantID;
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_GetID: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return result;
        }
        public override int PlantBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i]; 
                     
                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.Code = item.Code;
                    plant.LastChangeCode = Guid.NewGuid();
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    //ENDSET
                    plants.Add(plant);
                }

                plantManager.BulkInsert(plants); 

                bulkCount = dataList.Count;
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> PlantBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i];

                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.Code = item.Code;
                    plant.LastChangeCode = Guid.NewGuid();
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    //ENDSET
                    plants.Add(plant);
                }

                await plantManager.BulkInsertAsync(plants);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkInsert_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int PlantBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i];

                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.PlantID = item.PlantID;
                    plant.Code = item.Code;
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    plant.LastChangeCode = item.LastChangeCode;
                    //ENDSET
                    plants.Add(plant);
                }

                plantManager.BulkUpdate(plants); 

                bulkCount = dataList.Count;
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> PlantBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i];

                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.PlantID = item.PlantID;
                    plant.Code = item.Code;
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    //ENDSET
                    plant.LastChangeCode = item.LastChangeCode;
                    plants.Add(plant);
                }

                plantManager.BulkUpdate(plants); 

                bulkCount = dataList.Count;
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkUpdate_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override int PlantBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i];

                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.PlantID = item.PlantID;
                    plant.Code = item.Code;
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    //ENDSET
                    plant.LastChangeCode = item.LastChangeCode;
                    plants.Add(plant);
                }

                plantManager.BulkDelete(plants);

                bulkCount = dataList.Count; 
            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return bulkCount;
        }
        public override async Task<int> PlantBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Plant> dataList)
        {
            string procedureName = "PlantBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                List<EF.Models.Plant> plants = new List<EF.Models.Plant>();

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PlantID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    Objects.Plant item = dataList[i];

                    EF.Models.Plant plant = new EF.Models.Plant();
                    plant.PlantID = item.PlantID;
                    plant.Code = item.Code;
                    plant.SomeTextVal = item.SomeTextVal;
                    plant.FlvrForeignKeyID = item.FlvrForeignKeyID;
                    plant.IsDeleteAllowed = item.IsDeleteAllowed;
                    plant.IsEditAllowed = item.IsEditAllowed;
                    plant.LandID = item.LandID;
                    plant.OtherFlavor = item.OtherFlavor;
                    plant.SomeBigIntVal = item.SomeBigIntVal;
                    plant.SomeBitVal = item.SomeBitVal;
                    plant.SomeDateVal = item.SomeDateVal;
                    plant.SomeDecimalVal = item.SomeDecimalVal;
                    plant.SomeEmailAddress = item.SomeEmailAddress;
                    plant.SomeFloatVal = (float)item.SomeFloatVal;
                    plant.SomeIntVal = item.SomeIntVal;
                    plant.SomeMoneyVal = item.SomeMoneyVal;
                    plant.SomeNVarCharVal = item.SomeNVarCharVal;
                    plant.SomePhoneNumber = item.SomePhoneNumber;
                    plant.SomeUniqueidentifierVal = item.SomeUniqueidentifierVal;
                    plant.SomeUTCDateTimeVal = item.SomeUTCDateTimeVal;
                    plant.SomeVarCharVal = item.SomeVarCharVal;
                    //ENDSET
                    plant.LastChangeCode = item.LastChangeCode;
                    plants.Add(plant);
                }

                await plantManager.BulkDeleteAsync(plants);

                bulkCount = dataList.Count;
            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Plant_BulkDelete_v19: " + x.Message + " \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return bulkCount;
        }
        public override void PlantDelete(
            SessionContext context,
            int plantID)
        {
            string procedureName = "PlantDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::plantID::" + plantID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                plantManager.Delete(plantID);

            }
            catch (Exception x)
            { 
                HandleError( x, "FS_Farm_Plant_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task PlantDeleteAsync(
           SessionContext context,
           int plantID)
        {
            string procedureName = "PlantDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::plantID::" + plantID.ToString()); 
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                await plantManager.DeleteAsync(plantID);

            }
            catch (Exception x)
            { 
                await HandleErrorAsync(context, x, "FS_Farm_Plant_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void PlantCleanupTesting(
            SessionContext context )
        {
            string procedureName = "PlantCleanupTesting";
            Log(procedureName + "::Start"); 
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                EF.CurrentRuntime.ClearTestObjects(dbContext);

            }
            catch (Exception x)
            { 
                HandleError(  x, "FS_Farm_TestObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override void PlantCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "PlantCleanupChildObjectTesting";
            Log(procedureName + "::Start"); 
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context); 

                EF.CurrentRuntime.ClearTestChildObjects(dbContext);

            }
            catch (Exception x)
            { 
                HandleError(  x, "FS_Farm_TestChildObjectCleanup");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override bool SupportsTransactions()
        {
            return true;
        }
        #endregion
        #region Error Handling
        void HandleError( Exception x, string sprocName)
        {
            Log(x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n"; 
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsync(SessionContext context, Exception x, string sprocName)
        {
            await LogAsync(context, x);
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n"; 
            throw new Exception(sException, x);
        }
        #endregion
        public override IDataReader GetPlantList_FetchByLandID(
            int landID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByLandID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null; 
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(plantManager.GetByLandID(landID));


            }
            catch (Exception x)
            { 
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_FetchByLandID: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override IDataReader GetPlantList_FetchByFlvrForeignKeyID(
            int flvrForeignKeyID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByFlvrForeignKeyID";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = BuildDbContext(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(plantManager.GetByFlvrForeignKeyID(flvrForeignKeyID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Plant_FetchByFlvrForeignKeyID: \r\n";
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetPlantList_FetchByLandIDAsync(
            int landID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByLandIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null; 
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(await plantManager.GetByLandIDAsync(landID));

            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_FetchByLandID: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }
        public override async Task<IDataReader> GetPlantList_FetchByFlvrForeignKeyIDAsync(
            int flvrForeignKeyID,
           SessionContext context
            )
        {
            string procedureName = "GetPlantList_FetchByFlvrForeignKeyIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null; 
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var plantManager = new EF.Managers.PlantManager(dbContext);

                rdr = BuildDataReader(await plantManager.GetByFlvrForeignKeyIDAsync(flvrForeignKeyID));

            }
            catch (Exception x)
            { 
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Plant_FetchByFlvrForeignKeyID: \r\n"; 
                throw new Exception(sException, x);
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
            return rdr;
        }

        private async Task<EF.FarmDbContext> BuildDbContextAsync(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                await dbContext.Database.UseTransactionAsync(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }

        private EF.FarmDbContext BuildDbContext(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                    connection.Open();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                dbContext.Database.UseTransaction(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }
        private IDataReader BuildDataReader(List<EF.Models.Plant> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties 
            foreach (var prop in typeof(EF.Models.Plant).GetProperties())
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
                foreach (var prop in typeof(EF.Models.Plant).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }
    }
}
