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
    partial class EF7FlavorProvider : FS.Farm.Providers.FlavorProvider
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
        #region Flavor Methods
        public override int FlavorGetCount(
            SessionContext context )
        {
            string procedureName = "FlavorGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                iOut = flavorManager.GetTotalCount();
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
        public override async Task<int> FlavorGetCountAsync(
            SessionContext context )
        {
            string procedureName = "FlavorGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                iOut = await flavorManager.GetTotalCountAsync();
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
        public override int FlavorGetMaxID(
            SessionContext context)
        {
            string procedureName = "FlavorGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                iOut = flavorManager.GetMaxId().Value;
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
        public override async Task<int> FlavorGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "FlavorGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                var maxId = await flavorManager.GetMaxIdAsync();
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
        public override int FlavorInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "FlavorInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                EF.Models.Flavor flavor = new EF.Models.Flavor();
                flavor.Code = code;
                flavor.LastChangeCode = Guid.NewGuid();
                flavor.Description = description;
                flavor.DisplayOrder = displayOrder;
                flavor.IsActive = isActive;
                flavor.LookupEnumName = lookupEnumName;
                flavor.Name = name;
                flavor.PacID = pacID;
                flavor = flavorManager.Add(flavor);
                iOut = flavor.FlavorID;
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
        public override async Task<int> FlavorInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "FlavorInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                EF.Models.Flavor flavor = new EF.Models.Flavor();
                flavor.Code = code;
                flavor.LastChangeCode = Guid.NewGuid();
                flavor.Description = description;
                flavor.DisplayOrder = displayOrder;
                flavor.IsActive = isActive;
                flavor.LookupEnumName = lookupEnumName;
                flavor.Name = name;
                flavor.PacID = pacID;
                flavor = await flavorManager.AddAsync(flavor);
                iOut = flavor.FlavorID;
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
        public override void FlavorUpdate(
            SessionContext context,
            int flavorID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "FlavorUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                EF.Models.Flavor flavor = new EF.Models.Flavor();
                flavor.Code = code;
                flavor.Description = description;
                flavor.DisplayOrder = displayOrder;
                flavor.IsActive = isActive;
                flavor.LookupEnumName = lookupEnumName;
                flavor.Name = name;
                flavor.PacID = pacID;
                bool success = flavorManager.Update(flavor);
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
        public override async Task FlavorUpdateAsync(
            SessionContext context,
            int flavorID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "FlavorUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                EF.Models.Flavor flavor = new EF.Models.Flavor();
                flavor.Code = code;
                flavor.Description = description;
                flavor.DisplayOrder = displayOrder;
                flavor.IsActive = isActive;
                flavor.LookupEnumName = lookupEnumName;
                flavor.Name = name;
                flavor.PacID = pacID;
                bool success = await flavorManager.UpdateAsync(flavor);
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
        public override IDataReader SearchFlavors(
            SessionContext context,
            bool searchByFlavorID, int flavorID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchFlavors";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_Search: \r\n";
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
        public override async Task<IDataReader> SearchFlavorsAsync(
                    SessionContext context,
                    bool searchByFlavorID, int flavorID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchFlavorsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_Search: \r\n";
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
        public override IDataReader GetFlavorList(
            SessionContext context)
        {
            string procedureName = "GetFlavorList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                rdr = BuildDataReader(flavorManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_GetList: \r\n";
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
        public override async Task<IDataReader> GetFlavorListAsync(
            SessionContext context)
        {
            string procedureName = "GetFlavorListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                rdr = BuildDataReader(await flavorManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_GetList: \r\n";
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
        public override Guid GetFlavorCode(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetFlavorCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::flavorID::" + flavorID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Flavor::" + flavorID.ToString() + "::code";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                var flavor = flavorManager.GetById(flavorID);
                result = flavor.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_GetCode: \r\n";
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
        public override async Task<Guid> GetFlavorCodeAsync(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetFlavorCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::flavorID::" + flavorID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Flavor::" + flavorID.ToString() + "::code";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                var flavor = await flavorManager.GetByIdAsync(flavorID);
                result = flavor.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_GetCode: \r\n";
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
        public override IDataReader GetFlavor(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetFlavor";
            Log(procedureName + "::Start");
            Log(procedureName + "::flavorID::" + flavorID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = flavorManager.GetById(flavorID);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_Get: \r\n";
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
        public override async Task<IDataReader> GetFlavorAsync(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetFlavorAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::flavorID::" + flavorID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = await flavorManager.GetByIdAsync(flavorID);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_Get: \r\n";
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
        public override IDataReader GetDirtyFlavor(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetDirtyFlavor";
            Log(procedureName + "::Start");
            Log(procedureName + "::flavorID::" + flavorID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = flavorManager.DirtyGetById(flavorID);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyFlavorAsync(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "GetDirtyFlavorAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::flavorID::" + flavorID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = await flavorManager.DirtyGetByIdAsync(flavorID);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_DirtyGet: \r\n";
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
        public override IDataReader GetFlavor(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetFlavor";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = flavorManager.GetByCode(code);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetFlavorAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetFlavorAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = await flavorManager.GetByCodeAsync(code);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_GetByCode: \r\n";
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
        public override IDataReader GetDirtyFlavor(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyFlavor";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = flavorManager.DirtyGetByCode(code);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyFlavorAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyFlavorAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                var flavor = await flavorManager.DirtyGetByCodeAsync(code);
                flavors.Add(flavor);
                rdr = BuildDataReader(flavors);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_DirtyGetByCode: \r\n";
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
        public override int GetFlavorID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetFlavorID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                var flavor = flavorManager.GetByCode(code);
                result = flavor.FlavorID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_GetID: \r\n";
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
        public override async Task<int> GetFlavorIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetFlavorIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                var flavor = await flavorManager.GetByCodeAsync(code);
                result = flavor.FlavorID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_GetID: \r\n";
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
        public override int FlavorBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkInsertList";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.Code = item.Code;
                    flavor.LastChangeCode = Guid.NewGuid();
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavors.Add(flavor);
                }
                flavorManager.BulkInsert(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> FlavorBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkInsertListAsync";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.Code = item.Code;
                    flavor.LastChangeCode = Guid.NewGuid();
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavors.Add(flavor);
                }
                await flavorManager.BulkInsertAsync(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int FlavorBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkUpdateList";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.FlavorID = item.FlavorID;
                    flavor.Code = item.Code;
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavor.LastChangeCode = item.LastChangeCode;
                    flavors.Add(flavor);
                }
                flavorManager.BulkUpdate(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> FlavorBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkUpdateListAsync";
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
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.FlavorID = item.FlavorID;
                    flavor.Code = item.Code;
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavor.LastChangeCode = item.LastChangeCode;
                    flavors.Add(flavor);
                }
                flavorManager.BulkUpdate(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int FlavorBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.FlavorID = item.FlavorID;
                    flavor.Code = item.Code;
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavor.LastChangeCode = item.LastChangeCode;
                    flavors.Add(flavor);
                }
                flavorManager.BulkDelete(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> FlavorBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Flavor> dataList)
        {
            string procedureName = "FlavorBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                List<EF.Models.Flavor> flavors = new List<EF.Models.Flavor>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].FlavorID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Flavor item = dataList[i];
                    EF.Models.Flavor flavor = new EF.Models.Flavor();
                    flavor.FlavorID = item.FlavorID;
                    flavor.Code = item.Code;
                    flavor.Description = item.Description;
                    flavor.DisplayOrder = item.DisplayOrder;
                    flavor.IsActive = item.IsActive;
                    flavor.LookupEnumName = item.LookupEnumName;
                    flavor.Name = item.Name;
                    flavor.PacID = item.PacID;
                    flavor.LastChangeCode = item.LastChangeCode;
                    flavors.Add(flavor);
                }
                await flavorManager.BulkDeleteAsync(flavors);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Flavor_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void FlavorDelete(
            SessionContext context,
            int flavorID)
        {
            string procedureName = "FlavorDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::flavorID::" + flavorID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                flavorManager.Delete(flavorID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Flavor_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task FlavorDeleteAsync(
           SessionContext context,
           int flavorID)
        {
            string procedureName = "FlavorDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::flavorID::" + flavorID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                await flavorManager.DeleteAsync(flavorID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Flavor_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void FlavorCleanupTesting(
            SessionContext context )
        {
            string procedureName = "FlavorCleanupTesting";
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
        public override void FlavorCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "FlavorCleanupChildObjectTesting";
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
        public override IDataReader GetFlavorList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetFlavorList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                rdr = BuildDataReader(flavorManager.GetByPacID(pacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Flavor_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetFlavorList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetFlavorList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var flavorManager = new EF.Managers.FlavorManager(dbContext);
                rdr = BuildDataReader(await flavorManager.GetByPacIDAsync(pacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Flavor_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.Flavor> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Flavor).GetProperties())
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
                foreach (var prop in typeof(EF.Models.Flavor).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
