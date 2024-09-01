using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
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
    partial class EF7DynaFlowTaskTypeProvider : FS.Farm.Providers.DynaFlowTaskTypeProvider
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
        #region DynaFlowTaskType Methods
        public override int DynaFlowTaskTypeGetCount(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskTypeGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                iOut = dynaFlowTaskTypeManager.GetTotalCount();
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
        public override async Task<int> DynaFlowTaskTypeGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskTypeGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                iOut = await dynaFlowTaskTypeManager.GetTotalCountAsync();

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
        public override int DynaFlowTaskTypeGetMaxID(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskTypeGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                iOut = dynaFlowTaskTypeManager.GetMaxId().Value;
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
        public override async Task<int> DynaFlowTaskTypeGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskTypeGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                var maxId = await dynaFlowTaskTypeManager.GetMaxIdAsync();

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
        public override int DynaFlowTaskTypeInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DynaFlowTaskTypeInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices LookupEnumNameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                lookupEnumName = LookupEnumNameEncryptionServices.Encrypt(lookupEnumName);
            }
            //Int32 maxRetryCount,
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                dynaFlowTaskType.Code = code;
                dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                dynaFlowTaskType.Description = description;
                dynaFlowTaskType.DisplayOrder = displayOrder;
                dynaFlowTaskType.IsActive = isActive;
                dynaFlowTaskType.LookupEnumName = lookupEnumName;
                dynaFlowTaskType.MaxRetryCount = maxRetryCount;
                dynaFlowTaskType.Name = name;
                dynaFlowTaskType.PacID = pacID;
                dynaFlowTaskType = dynaFlowTaskTypeManager.Add(dynaFlowTaskType);

                iOut = dynaFlowTaskType.DynaFlowTaskTypeID;
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
        public override async Task<int> DynaFlowTaskTypeInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DynaFlowTaskTypeInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices LookupEnumNameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                lookupEnumName = LookupEnumNameEncryptionServices.Encrypt(lookupEnumName);
            }
            //Int32 maxRetryCount,
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                dynaFlowTaskType.Code = code;
                dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                dynaFlowTaskType.Description = description;
                dynaFlowTaskType.DisplayOrder = displayOrder;
                dynaFlowTaskType.IsActive = isActive;
                dynaFlowTaskType.LookupEnumName = lookupEnumName;
                dynaFlowTaskType.MaxRetryCount = maxRetryCount;
                dynaFlowTaskType.Name = name;
                dynaFlowTaskType.PacID = pacID;
                dynaFlowTaskType = await dynaFlowTaskTypeManager.AddAsync(dynaFlowTaskType);

                iOut = dynaFlowTaskType.DynaFlowTaskTypeID;
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
        public override void DynaFlowTaskTypeUpdate(
            SessionContext context,
            int dynaFlowTaskTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DynaFlowTaskTypeUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices LookupEnumNameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                lookupEnumName = LookupEnumNameEncryptionServices.Encrypt(lookupEnumName);
            }
            //Int32 maxRetryCount,
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                dynaFlowTaskType.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTaskType.Code = code;
                dynaFlowTaskType.Description = description;
                dynaFlowTaskType.DisplayOrder = displayOrder;
                dynaFlowTaskType.IsActive = isActive;
                dynaFlowTaskType.LookupEnumName = lookupEnumName;
                dynaFlowTaskType.MaxRetryCount = maxRetryCount;
                dynaFlowTaskType.Name = name;
                dynaFlowTaskType.PacID = pacID;
                dynaFlowTaskType.LastChangeCode = lastChangeCode;

                bool success = dynaFlowTaskTypeManager.Update(dynaFlowTaskType);
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
        public override async Task DynaFlowTaskTypeUpdateAsync(
            SessionContext context,
            int dynaFlowTaskTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            Int32 maxRetryCount,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DynaFlowTaskTypeUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //String description,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices DescriptionEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                description = DescriptionEncryptionServices.Encrypt(description);
            }
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices LookupEnumNameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                lookupEnumName = LookupEnumNameEncryptionServices.Encrypt(lookupEnumName);
            }
            //Int32 maxRetryCount,
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                dynaFlowTaskType.DynaFlowTaskTypeID = dynaFlowTaskTypeID;
                dynaFlowTaskType.Code = code;
                dynaFlowTaskType.Description = description;
                dynaFlowTaskType.DisplayOrder = displayOrder;
                dynaFlowTaskType.IsActive = isActive;
                dynaFlowTaskType.LookupEnumName = lookupEnumName;
                dynaFlowTaskType.MaxRetryCount = maxRetryCount;
                dynaFlowTaskType.Name = name;
                dynaFlowTaskType.PacID = pacID;
                dynaFlowTaskType.LastChangeCode = lastChangeCode;

                bool success = await dynaFlowTaskTypeManager.UpdateAsync(dynaFlowTaskType);
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
        public override IDataReader SearchDynaFlowTaskTypes(
            SessionContext context,
            bool searchByDynaFlowTaskTypeID, int dynaFlowTaskTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByMaxRetryCount, Int32 maxRetryCount,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTaskTypes";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_Search: \r\n";
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
        public override async Task<IDataReader> SearchDynaFlowTaskTypesAsync(
                    SessionContext context,
                    bool searchByDynaFlowTaskTypeID, int dynaFlowTaskTypeID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByMaxRetryCount, Int32 maxRetryCount,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTaskTypesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_Search: \r\n";
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
        public override IDataReader GetDynaFlowTaskTypeList(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTaskTypeList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                rdr = BuildDataReader(dynaFlowTaskTypeManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetList: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskTypeListAsync(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTaskTypeListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTaskTypeManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetList: \r\n";
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
        public override Guid GetDynaFlowTaskTypeCode(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDynaFlowTaskTypeCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTaskType::" + dynaFlowTaskTypeID.ToString() + "::code";
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

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                var dynaFlowTaskType = dynaFlowTaskTypeManager.GetById(dynaFlowTaskTypeID);

                result = dynaFlowTaskType.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetCode: \r\n";
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
        public override async Task<Guid> GetDynaFlowTaskTypeCodeAsync(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDynaFlowTaskTypeCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTaskType::" + dynaFlowTaskTypeID.ToString() + "::code";
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

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.GetByIdAsync(dynaFlowTaskTypeID);

                result = dynaFlowTaskType.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetCode: \r\n";
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
        public override IDataReader GetDynaFlowTaskType(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDynaFlowTaskType";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = dynaFlowTaskTypeManager.GetById(dynaFlowTaskTypeID);

                if(dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_Get: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskTypeAsync(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDynaFlowTaskTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.GetByIdAsync(dynaFlowTaskTypeID);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_Get: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTaskType(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDirtyDynaFlowTaskType";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = dynaFlowTaskTypeManager.DirtyGetById(dynaFlowTaskTypeID);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTaskTypeAsync(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "GetDirtyDynaFlowTaskTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.DirtyGetByIdAsync(dynaFlowTaskTypeID);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_DirtyGet: \r\n";
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
        public override IDataReader GetDynaFlowTaskType(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskType";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = dynaFlowTaskTypeManager.GetByCode(code);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskTypeAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.GetByCodeAsync(code);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTaskType(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTaskType";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = dynaFlowTaskTypeManager.DirtyGetByCode(code);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTaskTypeAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTaskTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.DirtyGetByCodeAsync(code);

                if (dynaFlowTaskType != null)
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);

                rdr = BuildDataReader(dynaFlowTaskTypes);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_DirtyGetByCode: \r\n";
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
        public override int GetDynaFlowTaskTypeID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskTypeID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                var dynaFlowTaskType = dynaFlowTaskTypeManager.GetByCode(code);

                result = dynaFlowTaskType.DynaFlowTaskTypeID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetID: \r\n";
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
        public override async Task<int> GetDynaFlowTaskTypeIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTaskTypeIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                var dynaFlowTaskType = await dynaFlowTaskTypeManager.GetByCodeAsync(code);

                result = dynaFlowTaskType.DynaFlowTaskTypeID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_GetID: \r\n";
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
        public override int DynaFlowTaskTypeBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkInsertList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Description = encryptionServices.Encrypt(dynaFlowTaskType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.LookupEnumName = encryptionServices.Encrypt(dynaFlowTaskType.LookupEnumName);
                    }
                    //Int32 maxRetryCount,
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Name = encryptionServices.Encrypt(dynaFlowTaskType.Name);
                    }
                    //Int32 pacID,
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                dynaFlowTaskTypeManager.BulkInsert(dynaFlowTaskTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskTypeBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkInsertListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Description = encryptionServices.Encrypt(dynaFlowTaskType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.LookupEnumName = encryptionServices.Encrypt(dynaFlowTaskType.LookupEnumName);
                    }
                    //Int32 maxRetryCount,
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Name = encryptionServices.Encrypt(dynaFlowTaskType.Name);
                    }
                    //Int32 pacID,
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                await dynaFlowTaskTypeManager.BulkInsertAsync(dynaFlowTaskTypes);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTaskTypeBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkUpdateList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    dynaFlowTaskType.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Description = encryptionServices.Encrypt(dynaFlowTaskType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.LookupEnumName = encryptionServices.Encrypt(dynaFlowTaskType.LookupEnumName);
                    }
                    //Int32 maxRetryCount,
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Name = encryptionServices.Encrypt(dynaFlowTaskType.Name);
                    }
                    //Int32 pacID,

                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                dynaFlowTaskTypeManager.BulkUpdate(dynaFlowTaskTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskTypeBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkUpdateListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (dataList.Count == 0)
                return bulkCount;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            FS.Common.Encryption.EncryptionServices encryptionServices = new FS.Common.Encryption.EncryptionServices();
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    dynaFlowTaskType.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Description = encryptionServices.Encrypt(dynaFlowTaskType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.LookupEnumName = encryptionServices.Encrypt(dynaFlowTaskType.LookupEnumName);
                    }
                    //Int32 maxRetryCount,
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowTaskType.Name = encryptionServices.Encrypt(dynaFlowTaskType.Name);
                    }
                    //Int32 pacID,
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                dynaFlowTaskTypeManager.BulkUpdate(dynaFlowTaskTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTaskTypeBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    dynaFlowTaskType.LastChangeCode = item.LastChangeCode;
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                dynaFlowTaskTypeManager.BulkDelete(dynaFlowTaskTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTaskTypeBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTaskType> dataList)
        {
            string procedureName = "DynaFlowTaskTypeBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                List<EF.Models.DynaFlowTaskType> dynaFlowTaskTypes = new List<EF.Models.DynaFlowTaskType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTaskTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTaskType item = dataList[i];

                    EF.Models.DynaFlowTaskType dynaFlowTaskType = new EF.Models.DynaFlowTaskType();
                    dynaFlowTaskType.DynaFlowTaskTypeID = item.DynaFlowTaskTypeID;
                    dynaFlowTaskType.Code = item.Code;
                    dynaFlowTaskType.Description = item.Description;
                    dynaFlowTaskType.DisplayOrder = item.DisplayOrder;
                    dynaFlowTaskType.IsActive = item.IsActive;
                    dynaFlowTaskType.LookupEnumName = item.LookupEnumName;
                    dynaFlowTaskType.MaxRetryCount = item.MaxRetryCount;
                    dynaFlowTaskType.Name = item.Name;
                    dynaFlowTaskType.PacID = item.PacID;
                    dynaFlowTaskType.LastChangeCode = item.LastChangeCode;
                    dynaFlowTaskTypes.Add(dynaFlowTaskType);
                }

                await dynaFlowTaskTypeManager.BulkDeleteAsync(dynaFlowTaskTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTaskType_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DynaFlowTaskTypeDelete(
            SessionContext context,
            int dynaFlowTaskTypeID)
        {
            string procedureName = "DynaFlowTaskTypeDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                dynaFlowTaskTypeManager.Delete(dynaFlowTaskTypeID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DynaFlowTaskType_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DynaFlowTaskTypeDeleteAsync(
           SessionContext context,
           int dynaFlowTaskTypeID)
        {
            string procedureName = "DynaFlowTaskTypeDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTaskTypeID::" + dynaFlowTaskTypeID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                await dynaFlowTaskTypeManager.DeleteAsync(dynaFlowTaskTypeID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DynaFlowTaskType_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DynaFlowTaskTypeCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DynaFlowTaskTypeCleanupTesting";
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
        public override void DynaFlowTaskTypeCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DynaFlowTaskTypeCleanupChildObjectTesting";
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
        public override IDataReader GetDynaFlowTaskTypeList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskTypeList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                rdr = BuildDataReader(dynaFlowTaskTypeManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTaskTypeList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTaskTypeList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTaskTypeManager = new EF.Managers.DynaFlowTaskTypeManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTaskTypeManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTaskType_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DynaFlowTaskType> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DynaFlowTaskType).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DynaFlowTaskType).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
