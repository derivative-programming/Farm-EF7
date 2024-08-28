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
    partial class EF7PacProvider : FS.Farm.Providers.PacProvider
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
        #region Pac Methods
        public override int PacGetCount(
            SessionContext context )
        {
            string procedureName = "PacGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                iOut = pacManager.GetTotalCount();
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
        public override async Task<int> PacGetCountAsync(
            SessionContext context )
        {
            string procedureName = "PacGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                iOut = await pacManager.GetTotalCountAsync();

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
        public override int PacGetMaxID(
            SessionContext context)
        {
            string procedureName = "PacGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                iOut = pacManager.GetMaxId().Value;
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
        public override async Task<int> PacGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "PacGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                var maxId = await pacManager.GetMaxIdAsync();

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
        public override int PacInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
                        System.Guid code)
        {
            string procedureName = "PacInsert";
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
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                EF.Models.Pac pac = new EF.Models.Pac();
                pac.Code = code;
                pac.LastChangeCode = Guid.NewGuid();
                pac.Description = description;
                pac.DisplayOrder = displayOrder;
                pac.IsActive = isActive;
                pac.LookupEnumName = lookupEnumName;
                pac.Name = name;

                pac = pacManager.Add(pac);

                iOut = pac.PacID;
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
        public override async Task<int> PacInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
                        System.Guid code)
        {
            string procedureName = "PacInsertAsync";
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
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                EF.Models.Pac pac = new EF.Models.Pac();
                pac.Code = code;
                pac.LastChangeCode = Guid.NewGuid();
                pac.Description = description;
                pac.DisplayOrder = displayOrder;
                pac.IsActive = isActive;
                pac.LookupEnumName = lookupEnumName;
                pac.Name = name;

                pac = await pacManager.AddAsync(pac);

                iOut = pac.PacID;
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
        public override void PacUpdate(
            SessionContext context,
            int pacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
                         Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "PacUpdate";
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
                        EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                EF.Models.Pac pac = new EF.Models.Pac();
                pac.PacID = pacID;
                pac.Code = code;
                pac.Description = description;
                pac.DisplayOrder = displayOrder;
                pac.IsActive = isActive;
                pac.LookupEnumName = lookupEnumName;
                pac.Name = name;
                                pac.LastChangeCode = lastChangeCode;

                bool success = pacManager.Update(pac);
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
        public override async Task PacUpdateAsync(
            SessionContext context,
            int pacID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
                        Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "PacUpdateAsync";
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
                        //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                EF.Models.Pac pac = new EF.Models.Pac();
                pac.PacID = pacID;
                pac.Code = code;
                pac.Description = description;
                pac.DisplayOrder = displayOrder;
                pac.IsActive = isActive;
                pac.LookupEnumName = lookupEnumName;
                pac.Name = name;
                                pac.LastChangeCode = lastChangeCode;

                bool success = await pacManager.UpdateAsync(pac);
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
        public override IDataReader SearchPacs(
            SessionContext context,
            bool searchByPacID, int pacID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPacs";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_Search: \r\n";
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
        public override async Task<IDataReader> SearchPacsAsync(
                    SessionContext context,
                    bool searchByPacID, int pacID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchPacsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_Search: \r\n";
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
        public override IDataReader GetPacList(
            SessionContext context)
        {
            string procedureName = "GetPacList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                rdr = BuildDataReader(pacManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_GetList: \r\n";
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
        public override async Task<IDataReader> GetPacListAsync(
            SessionContext context)
        {
            string procedureName = "GetPacListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                rdr = BuildDataReader(await pacManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_GetList: \r\n";
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
        public override Guid GetPacCode(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetPacCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::pacID::" + pacID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Pac::" + pacID.ToString() + "::code";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                var pac = pacManager.GetById(pacID);

                result = pac.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_GetCode: \r\n";
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
        public override async Task<Guid> GetPacCodeAsync(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetPacCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::pacID::" + pacID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Pac::" + pacID.ToString() + "::code";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                var pac = await pacManager.GetByIdAsync(pacID);

                result = pac.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_GetCode: \r\n";
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
        public override IDataReader GetPac(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetPac";
            Log(procedureName + "::Start");
            Log(procedureName + "::pacID::" + pacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = pacManager.GetById(pacID);

                if(pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_Get: \r\n";
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
        public override async Task<IDataReader> GetPacAsync(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetPacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::pacID::" + pacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = await pacManager.GetByIdAsync(pacID);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_Get: \r\n";
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
        public override IDataReader GetDirtyPac(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetDirtyPac";
            Log(procedureName + "::Start");
            Log(procedureName + "::pacID::" + pacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = pacManager.DirtyGetById(pacID);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyPacAsync(
            SessionContext context,
            int pacID)
        {
            string procedureName = "GetDirtyPacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::pacID::" + pacID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = await pacManager.DirtyGetByIdAsync(pacID);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_DirtyGet: \r\n";
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
        public override IDataReader GetPac(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPac";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = pacManager.GetByCode(code);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetPacAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = await pacManager.GetByCodeAsync(code);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_GetByCode: \r\n";
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
        public override IDataReader GetDirtyPac(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyPac";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = pacManager.DirtyGetByCode(code);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyPacAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyPacAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                var pac = await pacManager.DirtyGetByCodeAsync(code);

                if (pac != null)
                    pacs.Add(pac);

                rdr = BuildDataReader(pacs);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_DirtyGetByCode: \r\n";
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
        public override int GetPacID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetPacID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                var pac = pacManager.GetByCode(code);

                result = pac.PacID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Pac_GetID: \r\n";
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
        public override async Task<int> GetPacIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                var pac = await pacManager.GetByCodeAsync(code);

                result = pac.PacID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Pac_GetID: \r\n";
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
        public override int PacBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkInsertList";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].PacID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.Code = item.Code;
                    pac.LastChangeCode = Guid.NewGuid();
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Description = encryptionServices.Encrypt(pac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.LookupEnumName = encryptionServices.Encrypt(pac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Name = encryptionServices.Encrypt(pac.Name);
                    }
                                        pacs.Add(pac);
                }

                pacManager.BulkInsert(pacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> PacBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkInsertListAsync";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PacID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.Code = item.Code;
                    pac.LastChangeCode = Guid.NewGuid();
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Description = encryptionServices.Encrypt(pac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.LookupEnumName = encryptionServices.Encrypt(pac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Name = encryptionServices.Encrypt(pac.Name);
                    }
                                        pacs.Add(pac);
                }

                await pacManager.BulkInsertAsync(pacs);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int PacBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkUpdateList";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PacID == 0)
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.PacID = item.PacID;
                    pac.Code = item.Code;
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;
                                        pac.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Description = encryptionServices.Encrypt(pac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.LookupEnumName = encryptionServices.Encrypt(pac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Name = encryptionServices.Encrypt(pac.Name);
                    }

                    pacs.Add(pac);
                }

                pacManager.BulkUpdate(pacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> PacBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkUpdateListAsync";
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

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PacID == 0)
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.PacID = item.PacID;
                    pac.Code = item.Code;
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;
                                        pac.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //String description,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Description = encryptionServices.Encrypt(pac.Description);
                    }
                    //Int32 displayOrder,
                    //Boolean isActive,
                    //String lookupEnumName,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.LookupEnumName = encryptionServices.Encrypt(pac.LookupEnumName);
                    }
                    //String name,
                    isEncrypted = false;
                    if (isEncrypted)
                    {
                        pac.Name = encryptionServices.Encrypt(pac.Name);
                    }
                                        pacs.Add(pac);
                }

                pacManager.BulkUpdate(pacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int PacBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PacID == 0)
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.PacID = item.PacID;
                    pac.Code = item.Code;
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;
                                        pac.LastChangeCode = item.LastChangeCode;
                    pacs.Add(pac);
                }

                pacManager.BulkDelete(pacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> PacBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Pac> dataList)
        {
            string procedureName = "PacBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                List<EF.Models.Pac> pacs = new List<EF.Models.Pac>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].PacID == 0)
                        continue;

                    actionCount++;

                    Objects.Pac item = dataList[i];

                    EF.Models.Pac pac = new EF.Models.Pac();
                    pac.PacID = item.PacID;
                    pac.Code = item.Code;
                    pac.Description = item.Description;
                    pac.DisplayOrder = item.DisplayOrder;
                    pac.IsActive = item.IsActive;
                    pac.LookupEnumName = item.LookupEnumName;
                    pac.Name = item.Name;
                                        pac.LastChangeCode = item.LastChangeCode;
                    pacs.Add(pac);
                }

                await pacManager.BulkDeleteAsync(pacs);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Pac_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void PacDelete(
            SessionContext context,
            int pacID)
        {
            string procedureName = "PacDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::pacID::" + pacID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                pacManager.Delete(pacID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Pac_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task PacDeleteAsync(
           SessionContext context,
           int pacID)
        {
            string procedureName = "PacDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::pacID::" + pacID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var pacManager = new EF.Managers.PacManager(dbContext);

                await pacManager.DeleteAsync(pacID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Pac_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void PacCleanupTesting(
            SessionContext context )
        {
            string procedureName = "PacCleanupTesting";
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
        public override void PacCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "PacCleanupChildObjectTesting";
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
        private IDataReader BuildDataReader(List<EF.Models.Pac> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Pac).GetProperties())
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
                foreach (var prop in typeof(EF.Models.Pac).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
