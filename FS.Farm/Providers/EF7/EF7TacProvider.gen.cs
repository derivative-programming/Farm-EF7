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
    partial class EF7TacProvider : FS.Farm.Providers.TacProvider
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
        #region Tac Methods
        public override int TacGetCount(
            SessionContext context )
        {
            string procedureName = "TacGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                iOut = tacManager.GetTotalCount();
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
        public override async Task<int> TacGetCountAsync(
            SessionContext context )
        {
            string procedureName = "TacGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                iOut = await tacManager.GetTotalCountAsync();

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
        public override int TacGetMaxID(
            SessionContext context)
        {
            string procedureName = "TacGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                iOut = tacManager.GetMaxId().Value;
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
        public override async Task<int> TacGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "TacGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                var maxId = await tacManager.GetMaxIdAsync();

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
        public override int TacInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
                        System.Guid code)
        {
            string procedureName = "TacInsert";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                EF.Models.Tac tac = new EF.Models.Tac();
                tac.Code = code;
                tac.LastChangeCode = Guid.NewGuid();
                tac.Description = description;
                tac.DisplayOrder = displayOrder;
                tac.IsActive = isActive;
                tac.LookupEnumName = lookupEnumName;
                tac.Name = name;
                tac.PacID = pacID;

                tac = tacManager.Add(tac);

                iOut = tac.TacID;
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
        public override async Task<int> TacInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
                        System.Guid code)
        {
            string procedureName = "TacInsertAsync";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                EF.Models.Tac tac = new EF.Models.Tac();
                tac.Code = code;
                tac.LastChangeCode = Guid.NewGuid();
                tac.Description = description;
                tac.DisplayOrder = displayOrder;
                tac.IsActive = isActive;
                tac.LookupEnumName = lookupEnumName;
                tac.Name = name;
                tac.PacID = pacID;

                tac = await tacManager.AddAsync(tac);

                iOut = tac.TacID;
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
        public override void TacUpdate(
            SessionContext context,
            int tacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
                         Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "TacUpdate";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                EF.Models.Tac tac = new EF.Models.Tac();
                tac.TacID = tacID;
                tac.Code = code;
                tac.Description = description;
                tac.DisplayOrder = displayOrder;
                tac.IsActive = isActive;
                tac.LookupEnumName = lookupEnumName;
                tac.Name = name;
                tac.PacID = pacID;
                                tac.LastChangeCode = lastChangeCode;

                bool success = tacManager.Update(tac);
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
        public override async Task TacUpdateAsync(
            SessionContext context,
            int tacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
                        Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "TacUpdateAsync";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                EF.Models.Tac tac = new EF.Models.Tac();
                tac.TacID = tacID;
                tac.Code = code;
                tac.Description = description;
                tac.DisplayOrder = displayOrder;
                tac.IsActive = isActive;
                tac.LookupEnumName = lookupEnumName;
                tac.Name = name;
                tac.PacID = pacID;
                                tac.LastChangeCode = lastChangeCode;

                bool success = await tacManager.UpdateAsync(tac);
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
        public override IDataReader SearchTacs(
            SessionContext context,
            bool searchByTacID, int tacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchTacs";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_Search: \r\n";
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
        public override async Task<IDataReader> SearchTacsAsync(
                    SessionContext context,
                    bool searchByTacID, int tacID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchTacsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_Search: \r\n";
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
        public override IDataReader GetTacList(
            SessionContext context)
        {
            string procedureName = "GetTacList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                rdr = BuildDataReader(tacManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_GetList: \r\n";
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
        public override async Task<IDataReader> GetTacListAsync(
            SessionContext context)
        {
            string procedureName = "GetTacListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                rdr = BuildDataReader(await tacManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_GetList: \r\n";
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
        public override Guid GetTacCode(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetTacCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::tacID::" + tacID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Tac::" + tacID.ToString() + "::code";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                var tac = tacManager.GetById(tacID);

                result = tac.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_GetCode: \r\n";
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
        public override async Task<Guid> GetTacCodeAsync(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetTacCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::tacID::" + tacID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Tac::" + tacID.ToString() + "::code";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                var tac = await tacManager.GetByIdAsync(tacID);

                result = tac.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_GetCode: \r\n";
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
        public override IDataReader GetTac(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetTac";
            Log(procedureName + "::Start");
            Log(procedureName + "::tacID::" + tacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = tacManager.GetById(tacID);

                if(tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_Get: \r\n";
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
        public override async Task<IDataReader> GetTacAsync(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetTacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::tacID::" + tacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = await tacManager.GetByIdAsync(tacID);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_Get: \r\n";
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
        public override IDataReader GetDirtyTac(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetDirtyTac";
            Log(procedureName + "::Start");
            Log(procedureName + "::tacID::" + tacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = tacManager.DirtyGetById(tacID);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyTacAsync(
            SessionContext context,
            int tacID)
        {
            string procedureName = "GetDirtyTacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::tacID::" + tacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = await tacManager.DirtyGetByIdAsync(tacID);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_DirtyGet: \r\n";
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
        public override IDataReader GetTac(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetTac";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = tacManager.GetByCode(code);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetTacAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetTacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = await tacManager.GetByCodeAsync(code);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_GetByCode: \r\n";
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
        public override IDataReader GetDirtyTac(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyTac";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = tacManager.DirtyGetByCode(code);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyTacAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyTacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                var tac = await tacManager.DirtyGetByCodeAsync(code);

                if (tac != null)
                    tacs.Add(tac);

                rdr = BuildDataReader(tacs);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_DirtyGetByCode: \r\n";
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
        public override int GetTacID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetTacID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                var tac = tacManager.GetByCode(code);

                result = tac.TacID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_GetID: \r\n";
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
        public override async Task<int> GetTacIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetTacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                var tac = await tacManager.GetByCodeAsync(code);

                result = tac.TacID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_GetID: \r\n";
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
        public override int TacBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkInsertList";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].TacID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.Code = item.Code;
                    tac.LastChangeCode = Guid.NewGuid();
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Description = encryptionServices.Encrypt(tac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.LookupEnumName = encryptionServices.Encrypt(tac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Name = encryptionServices.Encrypt(tac.Name);
                    }
                    //Int32 pacID,
                                        tacs.Add(tac);
                }

                tacManager.BulkInsert(tacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TacBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkInsertListAsync";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TacID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.Code = item.Code;
                    tac.LastChangeCode = Guid.NewGuid();
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Description = encryptionServices.Encrypt(tac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.LookupEnumName = encryptionServices.Encrypt(tac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Name = encryptionServices.Encrypt(tac.Name);
                    }
                    //Int32 pacID,
                                        tacs.Add(tac);
                }

                await tacManager.BulkInsertAsync(tacs);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int TacBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkUpdateList";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TacID == 0)
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.TacID = item.TacID;
                    tac.Code = item.Code;
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;
                                        tac.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Description = encryptionServices.Encrypt(tac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.LookupEnumName = encryptionServices.Encrypt(tac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Name = encryptionServices.Encrypt(tac.Name);
                    }
                    //Int32 pacID,

                    tacs.Add(tac);
                }

                tacManager.BulkUpdate(tacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TacBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkUpdateListAsync";
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

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TacID == 0)
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.TacID = item.TacID;
                    tac.Code = item.Code;
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;
                                        tac.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Description = encryptionServices.Encrypt(tac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.LookupEnumName = encryptionServices.Encrypt(tac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        tac.Name = encryptionServices.Encrypt(tac.Name);
                    }
                    //Int32 pacID,
                                        tacs.Add(tac);
                }

                tacManager.BulkUpdate(tacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int TacBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TacID == 0)
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.TacID = item.TacID;
                    tac.Code = item.Code;
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;
                                        tac.LastChangeCode = item.LastChangeCode;
                    tacs.Add(tac);
                }

                tacManager.BulkDelete(tacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TacBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Tac> dataList)
        {
            string procedureName = "TacBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                List<EF.Models.Tac> tacs = new List<EF.Models.Tac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TacID == 0)
                        continue;

                    actionCount++;

                    Objects.Tac item = dataList[i];

                    EF.Models.Tac tac = new EF.Models.Tac();
                    tac.TacID = item.TacID;
                    tac.Code = item.Code;
                    tac.Description = item.Description;
                    tac.DisplayOrder = item.DisplayOrder;
                    tac.IsActive = item.IsActive;
                    tac.LookupEnumName = item.LookupEnumName;
                    tac.Name = item.Name;
                    tac.PacID = item.PacID;
                                        tac.LastChangeCode = item.LastChangeCode;
                    tacs.Add(tac);
                }

                await tacManager.BulkDeleteAsync(tacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Tac_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void TacDelete(
            SessionContext context,
            int tacID)
        {
            string procedureName = "TacDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::tacID::" + tacID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                tacManager.Delete(tacID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Tac_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task TacDeleteAsync(
           SessionContext context,
           int tacID)
        {
            string procedureName = "TacDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::tacID::" + tacID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                await tacManager.DeleteAsync(tacID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Tac_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void TacCleanupTesting(
            SessionContext context )
        {
            string procedureName = "TacCleanupTesting";
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
        public override void TacCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "TacCleanupChildObjectTesting";
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
        public override IDataReader GetTacList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetTacList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                rdr = BuildDataReader(tacManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Tac_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetTacList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetTacList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var tacManager = new EF.Managers.TacManager(dbContext);

                rdr = BuildDataReader(await tacManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Tac_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.Tac> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Tac).GetProperties())
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
                foreach (var prop in typeof(EF.Models.Tac).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
