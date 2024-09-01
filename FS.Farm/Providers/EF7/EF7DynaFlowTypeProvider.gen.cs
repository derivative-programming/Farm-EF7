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
    partial class EF7DynaFlowTypeProvider : FS.Farm.Providers.DynaFlowTypeProvider
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
        #region DynaFlowType Methods
        public override int DynaFlowTypeGetCount(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                iOut = dynaFlowTypeManager.GetTotalCount();
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
        public override async Task<int> DynaFlowTypeGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                iOut = await dynaFlowTypeManager.GetTotalCountAsync();

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
        public override int DynaFlowTypeGetMaxID(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                iOut = dynaFlowTypeManager.GetMaxId().Value;
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
        public override async Task<int> DynaFlowTypeGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                var maxId = await dynaFlowTypeManager.GetMaxIdAsync();

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
        public override int DynaFlowTypeInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeInsert";
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
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            //Int32 priorityLevel,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                dynaFlowType.Code = code;
                dynaFlowType.LastChangeCode = Guid.NewGuid();
                dynaFlowType.Description = description;
                dynaFlowType.DisplayOrder = displayOrder;
                dynaFlowType.IsActive = isActive;
                dynaFlowType.LookupEnumName = lookupEnumName;
                dynaFlowType.Name = name;
                dynaFlowType.PacID = pacID;
                dynaFlowType.PriorityLevel = priorityLevel;
                dynaFlowType = dynaFlowTypeManager.Add(dynaFlowType);

                iOut = dynaFlowType.DynaFlowTypeID;
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
        public override async Task<int> DynaFlowTypeInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeInsertAsync";
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
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            //Int32 priorityLevel,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                dynaFlowType.Code = code;
                dynaFlowType.LastChangeCode = Guid.NewGuid();
                dynaFlowType.Description = description;
                dynaFlowType.DisplayOrder = displayOrder;
                dynaFlowType.IsActive = isActive;
                dynaFlowType.LookupEnumName = lookupEnumName;
                dynaFlowType.Name = name;
                dynaFlowType.PacID = pacID;
                dynaFlowType.PriorityLevel = priorityLevel;
                dynaFlowType = await dynaFlowTypeManager.AddAsync(dynaFlowType);

                iOut = dynaFlowType.DynaFlowTypeID;
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
        public override void DynaFlowTypeUpdate(
            SessionContext context,
            int dynaFlowTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DynaFlowTypeUpdate";
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
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            //Int32 priorityLevel,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                dynaFlowType.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowType.Code = code;
                dynaFlowType.Description = description;
                dynaFlowType.DisplayOrder = displayOrder;
                dynaFlowType.IsActive = isActive;
                dynaFlowType.LookupEnumName = lookupEnumName;
                dynaFlowType.Name = name;
                dynaFlowType.PacID = pacID;
                dynaFlowType.PriorityLevel = priorityLevel;
                dynaFlowType.LastChangeCode = lastChangeCode;

                bool success = dynaFlowTypeManager.Update(dynaFlowType);
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
        public override async Task DynaFlowTypeUpdateAsync(
            SessionContext context,
            int dynaFlowTypeID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 priorityLevel,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeUpdateAsync";
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
            //String name,
            isEncrypted = false;
            if (isEncrypted)
            {
                FS.Common.Encryption.EncryptionServices NameEncryptionServices = new FS.Common.Encryption.EncryptionServices();
                name = NameEncryptionServices.Encrypt(name);
            }
            //Int32 pacID,
            //Int32 priorityLevel,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                dynaFlowType.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowType.Code = code;
                dynaFlowType.Description = description;
                dynaFlowType.DisplayOrder = displayOrder;
                dynaFlowType.IsActive = isActive;
                dynaFlowType.LookupEnumName = lookupEnumName;
                dynaFlowType.Name = name;
                dynaFlowType.PacID = pacID;
                dynaFlowType.PriorityLevel = priorityLevel;
                dynaFlowType.LastChangeCode = lastChangeCode;

                bool success = await dynaFlowTypeManager.UpdateAsync(dynaFlowType);
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
        public override IDataReader SearchDynaFlowTypes(
            SessionContext context,
            bool searchByDynaFlowTypeID, int dynaFlowTypeID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByPriorityLevel, Int32 priorityLevel,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTypes";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_Search: \r\n";
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
        public override async Task<IDataReader> SearchDynaFlowTypesAsync(
                    SessionContext context,
                    bool searchByDynaFlowTypeID, int dynaFlowTypeID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByPriorityLevel, Int32 priorityLevel,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTypesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_Search: \r\n";
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
        public override IDataReader GetDynaFlowTypeList(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTypeList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                rdr = BuildDataReader(dynaFlowTypeManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetList: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeListAsync(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTypeListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTypeManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetList: \r\n";
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
        public override Guid GetDynaFlowTypeCode(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDynaFlowTypeCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowType::" + dynaFlowTypeID.ToString() + "::code";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                var dynaFlowType = dynaFlowTypeManager.GetById(dynaFlowTypeID);

                result = dynaFlowType.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetCode: \r\n";
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
        public override async Task<Guid> GetDynaFlowTypeCodeAsync(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDynaFlowTypeCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowType::" + dynaFlowTypeID.ToString() + "::code";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                var dynaFlowType = await dynaFlowTypeManager.GetByIdAsync(dynaFlowTypeID);

                result = dynaFlowType.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetCode: \r\n";
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
        public override IDataReader GetDynaFlowType(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDynaFlowType";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = dynaFlowTypeManager.GetById(dynaFlowTypeID);

                if(dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_Get: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeAsync(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDynaFlowTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = await dynaFlowTypeManager.GetByIdAsync(dynaFlowTypeID);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_Get: \r\n";
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
        public override IDataReader GetDirtyDynaFlowType(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDirtyDynaFlowType";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = dynaFlowTypeManager.DirtyGetById(dynaFlowTypeID);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTypeAsync(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "GetDirtyDynaFlowTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = await dynaFlowTypeManager.DirtyGetByIdAsync(dynaFlowTypeID);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_DirtyGet: \r\n";
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
        public override IDataReader GetDynaFlowType(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowType";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = dynaFlowTypeManager.GetByCode(code);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = await dynaFlowTypeManager.GetByCodeAsync(code);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDynaFlowType(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowType";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = dynaFlowTypeManager.DirtyGetByCode(code);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTypeAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTypeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                var dynaFlowType = await dynaFlowTypeManager.DirtyGetByCodeAsync(code);

                if (dynaFlowType != null)
                    dynaFlowTypes.Add(dynaFlowType);

                rdr = BuildDataReader(dynaFlowTypes);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_DirtyGetByCode: \r\n";
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
        public override int GetDynaFlowTypeID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                var dynaFlowType = dynaFlowTypeManager.GetByCode(code);

                result = dynaFlowType.DynaFlowTypeID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetID: \r\n";
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
        public override async Task<int> GetDynaFlowTypeIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                var dynaFlowType = await dynaFlowTypeManager.GetByCodeAsync(code);

                result = dynaFlowType.DynaFlowTypeID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_GetID: \r\n";
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
        public override int DynaFlowTypeBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkInsertList";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DynaFlowTypeID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.LastChangeCode = Guid.NewGuid();
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Description = encryptionServices.Encrypt(dynaFlowType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.LookupEnumName = encryptionServices.Encrypt(dynaFlowType.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Name = encryptionServices.Encrypt(dynaFlowType.Name);
                    }
                    //Int32 pacID,
                    //Int32 priorityLevel,
                    dynaFlowTypes.Add(dynaFlowType);
                }

                dynaFlowTypeManager.BulkInsert(dynaFlowTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkInsertListAsync";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.LastChangeCode = Guid.NewGuid();
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Description = encryptionServices.Encrypt(dynaFlowType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.LookupEnumName = encryptionServices.Encrypt(dynaFlowType.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Name = encryptionServices.Encrypt(dynaFlowType.Name);
                    }
                    //Int32 pacID,
                    //Int32 priorityLevel,
                    dynaFlowTypes.Add(dynaFlowType);
                }

                await dynaFlowTypeManager.BulkInsertAsync(dynaFlowTypes);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTypeBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkUpdateList";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    dynaFlowType.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Description = encryptionServices.Encrypt(dynaFlowType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.LookupEnumName = encryptionServices.Encrypt(dynaFlowType.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Name = encryptionServices.Encrypt(dynaFlowType.Name);
                    }
                    //Int32 pacID,
                    //Int32 priorityLevel,

                    dynaFlowTypes.Add(dynaFlowType);
                }

                dynaFlowTypeManager.BulkUpdate(dynaFlowTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkUpdateListAsync";
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

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    dynaFlowType.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Description = encryptionServices.Encrypt(dynaFlowType.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.LookupEnumName = encryptionServices.Encrypt(dynaFlowType.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        dynaFlowType.Name = encryptionServices.Encrypt(dynaFlowType.Name);
                    }
                    //Int32 pacID,
                    //Int32 priorityLevel,
                    dynaFlowTypes.Add(dynaFlowType);
                }

                dynaFlowTypeManager.BulkUpdate(dynaFlowTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTypeBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    dynaFlowType.LastChangeCode = item.LastChangeCode;
                    dynaFlowTypes.Add(dynaFlowType);
                }

                dynaFlowTypeManager.BulkDelete(dynaFlowTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowType> dataList)
        {
            string procedureName = "DynaFlowTypeBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                List<EF.Models.DynaFlowType> dynaFlowTypes = new List<EF.Models.DynaFlowType>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowType item = dataList[i];

                    EF.Models.DynaFlowType dynaFlowType = new EF.Models.DynaFlowType();
                    dynaFlowType.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowType.Code = item.Code;
                    dynaFlowType.Description = item.Description;
                    dynaFlowType.DisplayOrder = item.DisplayOrder;
                    dynaFlowType.IsActive = item.IsActive;
                    dynaFlowType.LookupEnumName = item.LookupEnumName;
                    dynaFlowType.Name = item.Name;
                    dynaFlowType.PacID = item.PacID;
                    dynaFlowType.PriorityLevel = item.PriorityLevel;
                    dynaFlowType.LastChangeCode = item.LastChangeCode;
                    dynaFlowTypes.Add(dynaFlowType);
                }

                await dynaFlowTypeManager.BulkDeleteAsync(dynaFlowTypes);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowType_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DynaFlowTypeDelete(
            SessionContext context,
            int dynaFlowTypeID)
        {
            string procedureName = "DynaFlowTypeDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                dynaFlowTypeManager.Delete(dynaFlowTypeID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DynaFlowType_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DynaFlowTypeDeleteAsync(
           SessionContext context,
           int dynaFlowTypeID)
        {
            string procedureName = "DynaFlowTypeDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeID::" + dynaFlowTypeID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                await dynaFlowTypeManager.DeleteAsync(dynaFlowTypeID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DynaFlowType_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DynaFlowTypeCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeCleanupTesting";
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
        public override void DynaFlowTypeCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeCleanupChildObjectTesting";
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
        public override IDataReader GetDynaFlowTypeList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                rdr = BuildDataReader(dynaFlowTypeManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowType_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeManager = new EF.Managers.DynaFlowTypeManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTypeManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowType_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DynaFlowType> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DynaFlowType).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DynaFlowType).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
