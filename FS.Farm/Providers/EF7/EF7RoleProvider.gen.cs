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
    partial class EF7RoleProvider : FS.Farm.Providers.RoleProvider
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
        #region Role Methods
        public override int RoleGetCount(
            SessionContext context )
        {
            string procedureName = "RoleGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                iOut = roleManager.GetTotalCount();
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
        public override async Task<int> RoleGetCountAsync(
            SessionContext context )
        {
            string procedureName = "RoleGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                iOut = await roleManager.GetTotalCountAsync();
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
        public override int RoleGetMaxID(
            SessionContext context)
        {
            string procedureName = "RoleGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                iOut = roleManager.GetMaxId().Value;
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
        public override async Task<int> RoleGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "RoleGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                var maxId = await roleManager.GetMaxIdAsync();
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
        public override int RoleInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "RoleInsert";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                EF.Models.Role role = new EF.Models.Role();
                role.Code = code;
                role.Description = description;
                role.DisplayOrder = displayOrder;
                role.IsActive = isActive;
                role.LookupEnumName = lookupEnumName;
                role.Name = name;
                role.PacID = pacID;
                role = roleManager.Add(role);
                iOut = role.RoleID;
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
        public override async Task<int> RoleInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "RoleInsertAsync";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                EF.Models.Role role = new EF.Models.Role();
                role.Code = code;
                role.Description = description;
                role.DisplayOrder = displayOrder;
                role.IsActive = isActive;
                role.LookupEnumName = lookupEnumName;
                role.Name = name;
                role.PacID = pacID;
                role = await roleManager.AddAsync(role);
                iOut = role.RoleID;
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
        public override void RoleUpdate(
            SessionContext context,
            int roleID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "RoleUpdate";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                EF.Models.Role role = new EF.Models.Role();
                role.Code = code;
                role.Description = description;
                role.DisplayOrder = displayOrder;
                role.IsActive = isActive;
                role.LookupEnumName = lookupEnumName;
                role.Name = name;
                role.PacID = pacID;
                bool success = roleManager.Update(role);
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
        public override async Task RoleUpdateAsync(
            SessionContext context,
            int roleID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "RoleUpdateAsync";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                EF.Models.Role role = new EF.Models.Role();
                role.Code = code;
                role.Description = description;
                role.DisplayOrder = displayOrder;
                role.IsActive = isActive;
                role.LookupEnumName = lookupEnumName;
                role.Name = name;
                role.PacID = pacID;
                bool success = await roleManager.UpdateAsync(role);
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
        public override IDataReader SearchRoles(
            SessionContext context,
            bool searchByRoleID, int roleID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchRoles";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_Search: \r\n";
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
        public override async Task<IDataReader> SearchRolesAsync(
                    SessionContext context,
                    bool searchByRoleID, int roleID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchRolesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_Search: \r\n";
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
        public override IDataReader GetRoleList(
            SessionContext context)
        {
            string procedureName = "GetRoleList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                rdr = BuildDataReader(roleManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_GetList: \r\n";
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
        public override async Task<IDataReader> GetRoleListAsync(
            SessionContext context)
        {
            string procedureName = "GetRoleListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                rdr = BuildDataReader(await roleManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_GetList: \r\n";
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
        public override Guid GetRoleCode(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetRoleCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::roleID::" + roleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Role::" + roleID.ToString() + "::code";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                var role = roleManager.GetById(roleID);
                result = role.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_GetCode: \r\n";
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
        public override async Task<Guid> GetRoleCodeAsync(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetRoleCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::roleID::" + roleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Role::" + roleID.ToString() + "::code";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                var role = await roleManager.GetByIdAsync(roleID);
                result = role.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_GetCode: \r\n";
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
        public override IDataReader GetRole(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::roleID::" + roleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = roleManager.GetById(roleID);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_Get: \r\n";
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
        public override async Task<IDataReader> GetRoleAsync(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::roleID::" + roleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = await roleManager.GetByIdAsync(roleID);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_Get: \r\n";
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
        public override IDataReader GetDirtyRole(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetDirtyRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::roleID::" + roleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = roleManager.DirtyGetById(roleID);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyRoleAsync(
            SessionContext context,
            int roleID)
        {
            string procedureName = "GetDirtyRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::roleID::" + roleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = await roleManager.DirtyGetByIdAsync(roleID);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_DirtyGet: \r\n";
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
        public override IDataReader GetRole(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = roleManager.GetByCode(code);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetRoleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = await roleManager.GetByCodeAsync(code);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_GetByCode: \r\n";
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
        public override IDataReader GetDirtyRole(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = roleManager.DirtyGetByCode(code);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyRoleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                var role = await roleManager.DirtyGetByCodeAsync(code);
                roles.Add(role);
                rdr = BuildDataReader(roles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_DirtyGetByCode: \r\n";
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
        public override int GetRoleID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetRoleID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                var role = roleManager.GetByCode(code);
                result = role.RoleID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_GetID: \r\n";
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
        public override async Task<int> GetRoleIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetRoleIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                var role = await roleManager.GetByCodeAsync(code);
                result = role.RoleID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_GetID: \r\n";
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
        public override int RoleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkInsertList";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    roles.Add(role);
                }
                roleManager.BulkInsert(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> RoleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkInsertListAsync";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    roles.Add(role);
                }
                await roleManager.BulkInsertAsync(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int RoleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkUpdateList";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.RoleID = item.RoleID;
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    role.LastChangeCode = item.LastChangeCode;
                    roles.Add(role);
                }
                roleManager.BulkUpdate(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> RoleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkUpdateListAsync";
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
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.RoleID = item.RoleID;
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    role.LastChangeCode = item.LastChangeCode;
                    roles.Add(role);
                }
                roleManager.BulkUpdate(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int RoleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.RoleID = item.RoleID;
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    role.LastChangeCode = item.LastChangeCode;
                    roles.Add(role);
                }
                roleManager.BulkDelete(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> RoleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Role> dataList)
        {
            string procedureName = "RoleBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                List<EF.Models.Role> roles = new List<EF.Models.Role>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].RoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Role item = dataList[i];
                    EF.Models.Role role = new EF.Models.Role();
                    role.RoleID = item.RoleID;
                    role.Code = item.Code;
                    role.Description = item.Description;
                    role.DisplayOrder = item.DisplayOrder;
                    role.IsActive = item.IsActive;
                    role.LookupEnumName = item.LookupEnumName;
                    role.Name = item.Name;
                    role.PacID = item.PacID;
                    role.LastChangeCode = item.LastChangeCode;
                    roles.Add(role);
                }
                await roleManager.BulkDeleteAsync(roles);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Role_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void RoleDelete(
            SessionContext context,
            int roleID)
        {
            string procedureName = "RoleDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::roleID::" + roleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                roleManager.Delete(roleID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Role_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task RoleDeleteAsync(
           SessionContext context,
           int roleID)
        {
            string procedureName = "RoleDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::roleID::" + roleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                await roleManager.DeleteAsync(roleID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Role_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void RoleCleanupTesting(
            SessionContext context )
        {
            string procedureName = "RoleCleanupTesting";
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
        public override void RoleCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "RoleCleanupChildObjectTesting";
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
        public override IDataReader GetRoleList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetRoleList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                rdr = BuildDataReader(roleManager.GetByPac(pacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Role_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetRoleList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetRoleList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var roleManager = new EF.Managers.RoleManager(dbContext);
                rdr = BuildDataReader(await roleManager.GetByPacAsync(pacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Role_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.Role> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Role).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.Role).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
