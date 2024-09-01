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
    partial class EF7LandProvider : FS.Farm.Providers.LandProvider
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
        #region Land Methods
        public override int LandGetCount(
            SessionContext context )
        {
            string procedureName = "LandGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                iOut = landManager.GetTotalCount();
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
        public override async Task<int> LandGetCountAsync(
            SessionContext context )
        {
            string procedureName = "LandGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                iOut = await landManager.GetTotalCountAsync();

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
        public override int LandGetMaxID(
            SessionContext context)
        {
            string procedureName = "LandGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                iOut = landManager.GetMaxId().Value;
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
        public override async Task<int> LandGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "LandGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                var maxId = await landManager.GetMaxIdAsync();

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
        public override int LandInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "LandInsert";
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
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                EF.Models.Land land = new EF.Models.Land();
                land.Code = code;
                land.LastChangeCode = Guid.NewGuid();
                land.Description = description;
                land.DisplayOrder = displayOrder;
                land.IsActive = isActive;
                land.LookupEnumName = lookupEnumName;
                land.Name = name;
                land.PacID = pacID;
                land = landManager.Add(land);

                iOut = land.LandID;
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
        public override async Task<int> LandInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "LandInsertAsync";
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
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                EF.Models.Land land = new EF.Models.Land();
                land.Code = code;
                land.LastChangeCode = Guid.NewGuid();
                land.Description = description;
                land.DisplayOrder = displayOrder;
                land.IsActive = isActive;
                land.LookupEnumName = lookupEnumName;
                land.Name = name;
                land.PacID = pacID;
                land = await landManager.AddAsync(land);

                iOut = land.LandID;
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
        public override void LandUpdate(
            SessionContext context,
            int landID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "LandUpdate";
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
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                EF.Models.Land land = new EF.Models.Land();
                land.LandID = landID;
                land.Code = code;
                land.Description = description;
                land.DisplayOrder = displayOrder;
                land.IsActive = isActive;
                land.LookupEnumName = lookupEnumName;
                land.Name = name;
                land.PacID = pacID;
                land.LastChangeCode = lastChangeCode;

                bool success = landManager.Update(land);
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
        public override async Task LandUpdateAsync(
            SessionContext context,
            int landID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "LandUpdateAsync";
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
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                EF.Models.Land land = new EF.Models.Land();
                land.LandID = landID;
                land.Code = code;
                land.Description = description;
                land.DisplayOrder = displayOrder;
                land.IsActive = isActive;
                land.LookupEnumName = lookupEnumName;
                land.Name = name;
                land.PacID = pacID;
                land.LastChangeCode = lastChangeCode;

                bool success = await landManager.UpdateAsync(land);
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
        public override IDataReader SearchLands(
            SessionContext context,
            bool searchByLandID, int landID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchLands";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_Search: \r\n";
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
        public override async Task<IDataReader> SearchLandsAsync(
                    SessionContext context,
                    bool searchByLandID, int landID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchLandsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_Search: \r\n";
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
        public override IDataReader GetLandList(
            SessionContext context)
        {
            string procedureName = "GetLandList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                rdr = BuildDataReader(landManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_GetList: \r\n";
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
        public override async Task<IDataReader> GetLandListAsync(
            SessionContext context)
        {
            string procedureName = "GetLandListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                rdr = BuildDataReader(await landManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_GetList: \r\n";
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
        public override Guid GetLandCode(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetLandCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::landID::" + landID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Land::" + landID.ToString() + "::code";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                var land = landManager.GetById(landID);

                result = land.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_GetCode: \r\n";
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
        public override async Task<Guid> GetLandCodeAsync(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetLandCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::landID::" + landID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Land::" + landID.ToString() + "::code";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                var land = await landManager.GetByIdAsync(landID);

                result = land.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_GetCode: \r\n";
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
        public override IDataReader GetLand(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetLand";
            Log(procedureName + "::Start");
            Log(procedureName + "::landID::" + landID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = landManager.GetById(landID);

                if(land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_Get: \r\n";
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
        public override async Task<IDataReader> GetLandAsync(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetLandAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::landID::" + landID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = await landManager.GetByIdAsync(landID);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_Get: \r\n";
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
        public override IDataReader GetDirtyLand(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetDirtyLand";
            Log(procedureName + "::Start");
            Log(procedureName + "::landID::" + landID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = landManager.DirtyGetById(landID);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyLandAsync(
            SessionContext context,
            int landID)
        {
            string procedureName = "GetDirtyLandAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::landID::" + landID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = await landManager.DirtyGetByIdAsync(landID);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_DirtyGet: \r\n";
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
        public override IDataReader GetLand(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetLand";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = landManager.GetByCode(code);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetLandAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetLandAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = await landManager.GetByCodeAsync(code);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_GetByCode: \r\n";
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
        public override IDataReader GetDirtyLand(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyLand";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = landManager.DirtyGetByCode(code);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyLandAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyLandAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                var land = await landManager.DirtyGetByCodeAsync(code);

                if (land != null)
                    lands.Add(land);

                rdr = BuildDataReader(lands);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_DirtyGetByCode: \r\n";
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
        public override int GetLandID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetLandID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                var land = landManager.GetByCode(code);

                result = land.LandID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_GetID: \r\n";
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
        public override async Task<int> GetLandIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetLandIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                var land = await landManager.GetByCodeAsync(code);

                result = land.LandID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_GetID: \r\n";
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
        public override int LandBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkInsertList";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].LandID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.Code = item.Code;
                    land.LastChangeCode = Guid.NewGuid();
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Description = encryptionServices.Encrypt(land.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.LookupEnumName = encryptionServices.Encrypt(land.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Name = encryptionServices.Encrypt(land.Name);
                    }
                    //Int32 pacID,
                    lands.Add(land);
                }

                landManager.BulkInsert(lands);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> LandBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkInsertListAsync";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].LandID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.Code = item.Code;
                    land.LastChangeCode = Guid.NewGuid();
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Description = encryptionServices.Encrypt(land.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.LookupEnumName = encryptionServices.Encrypt(land.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Name = encryptionServices.Encrypt(land.Name);
                    }
                    //Int32 pacID,
                    lands.Add(land);
                }

                await landManager.BulkInsertAsync(lands);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int LandBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkUpdateList";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].LandID == 0)
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.LandID = item.LandID;
                    land.Code = item.Code;
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    land.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Description = encryptionServices.Encrypt(land.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.LookupEnumName = encryptionServices.Encrypt(land.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Name = encryptionServices.Encrypt(land.Name);
                    }
                    //Int32 pacID,

                    lands.Add(land);
                }

                landManager.BulkUpdate(lands);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> LandBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkUpdateListAsync";
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

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].LandID == 0)
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.LandID = item.LandID;
                    land.Code = item.Code;
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    land.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Description = encryptionServices.Encrypt(land.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.LookupEnumName = encryptionServices.Encrypt(land.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        land.Name = encryptionServices.Encrypt(land.Name);
                    }
                    //Int32 pacID,
                    lands.Add(land);
                }

                landManager.BulkUpdate(lands);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int LandBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].LandID == 0)
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.LandID = item.LandID;
                    land.Code = item.Code;
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    land.LastChangeCode = item.LastChangeCode;
                    lands.Add(land);
                }

                landManager.BulkDelete(lands);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> LandBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Land> dataList)
        {
            string procedureName = "LandBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                List<EF.Models.Land> lands = new List<EF.Models.Land>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].LandID == 0)
                        continue;

                    actionCount++;

                    Objects.Land item = dataList[i];

                    EF.Models.Land land = new EF.Models.Land();
                    land.LandID = item.LandID;
                    land.Code = item.Code;
                    land.Description = item.Description;
                    land.DisplayOrder = item.DisplayOrder;
                    land.IsActive = item.IsActive;
                    land.LookupEnumName = item.LookupEnumName;
                    land.Name = item.Name;
                    land.PacID = item.PacID;
                    land.LastChangeCode = item.LastChangeCode;
                    lands.Add(land);
                }

                await landManager.BulkDeleteAsync(lands);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Land_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void LandDelete(
            SessionContext context,
            int landID)
        {
            string procedureName = "LandDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::landID::" + landID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                landManager.Delete(landID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Land_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task LandDeleteAsync(
           SessionContext context,
           int landID)
        {
            string procedureName = "LandDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::landID::" + landID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                await landManager.DeleteAsync(landID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Land_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void LandCleanupTesting(
            SessionContext context )
        {
            string procedureName = "LandCleanupTesting";
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
        public override void LandCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "LandCleanupChildObjectTesting";
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
        public override IDataReader GetLandList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetLandList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                rdr = BuildDataReader(landManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Land_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetLandList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetLandList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var landManager = new EF.Managers.LandManager(dbContext);

                rdr = BuildDataReader(await landManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Land_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.Land> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Land).GetProperties())
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
                foreach (var prop in typeof(EF.Models.Land).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
