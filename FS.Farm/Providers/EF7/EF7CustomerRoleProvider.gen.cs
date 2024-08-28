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
    partial class EF7CustomerRoleProvider : FS.Farm.Providers.CustomerRoleProvider
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
        #region CustomerRole Methods
        public override int CustomerRoleGetCount(
            SessionContext context )
        {
            string procedureName = "CustomerRoleGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                iOut = customerRoleManager.GetTotalCount();
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
        public override async Task<int> CustomerRoleGetCountAsync(
            SessionContext context )
        {
            string procedureName = "CustomerRoleGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                iOut = await customerRoleManager.GetTotalCountAsync();

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
        public override int CustomerRoleGetMaxID(
            SessionContext context)
        {
            string procedureName = "CustomerRoleGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                iOut = customerRoleManager.GetMaxId().Value;
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
        public override async Task<int> CustomerRoleGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "CustomerRoleGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                var maxId = await customerRoleManager.GetMaxIdAsync();

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
        public override int CustomerRoleInsert(
            SessionContext context,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
                        System.Guid code)
        {
            string procedureName = "CustomerRoleInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 customerID,
            //Boolean isPlaceholder,
            //Boolean placeholder,
            //Int32 roleID,
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                customerRole.Code = code;
                customerRole.LastChangeCode = Guid.NewGuid();
                customerRole.CustomerID = customerID;
                customerRole.IsPlaceholder = isPlaceholder;
                customerRole.Placeholder = placeholder;
                customerRole.RoleID = roleID;

                customerRole = customerRoleManager.Add(customerRole);

                iOut = customerRole.CustomerRoleID;
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
        public override async Task<int> CustomerRoleInsertAsync(
            SessionContext context,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
                        System.Guid code)
        {
            string procedureName = "CustomerRoleInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 customerID,
            //Boolean isPlaceholder,
            //Boolean placeholder,
            //Int32 roleID,
                        SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                customerRole.Code = code;
                customerRole.LastChangeCode = Guid.NewGuid();
                customerRole.CustomerID = customerID;
                customerRole.IsPlaceholder = isPlaceholder;
                customerRole.Placeholder = placeholder;
                customerRole.RoleID = roleID;

                customerRole = await customerRoleManager.AddAsync(customerRole);

                iOut = customerRole.CustomerRoleID;
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
        public override void CustomerRoleUpdate(
            SessionContext context,
            int customerRoleID,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
                         Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "CustomerRoleUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 customerID,
            //Boolean isPlaceholder,
            //Boolean placeholder,
            //Int32 roleID,
                        EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                customerRole.CustomerRoleID = customerRoleID;
                customerRole.Code = code;
                customerRole.CustomerID = customerID;
                customerRole.IsPlaceholder = isPlaceholder;
                customerRole.Placeholder = placeholder;
                customerRole.RoleID = roleID;
                                customerRole.LastChangeCode = lastChangeCode;

                bool success = customerRoleManager.Update(customerRole);
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
        public override async Task CustomerRoleUpdateAsync(
            SessionContext context,
            int customerRoleID,
            Int32 customerID,
            Boolean isPlaceholder,
            Boolean placeholder,
            Int32 roleID,
                        Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "CustomerRoleUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 customerID,
            //Boolean isPlaceholder,
            //Boolean placeholder,
            //Int32 roleID,
                        //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                customerRole.CustomerRoleID = customerRoleID;
                customerRole.Code = code;
                customerRole.CustomerID = customerID;
                customerRole.IsPlaceholder = isPlaceholder;
                customerRole.Placeholder = placeholder;
                customerRole.RoleID = roleID;
                                customerRole.LastChangeCode = lastChangeCode;

                bool success = await customerRoleManager.UpdateAsync(customerRole);
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
        public override IDataReader SearchCustomerRoles(
            SessionContext context,
            bool searchByCustomerRoleID, int customerRoleID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByPlaceholder, Boolean placeholder,
            bool searchByRoleID, Int32 roleID,
                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchCustomerRoles";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_Search: \r\n";
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
        public override async Task<IDataReader> SearchCustomerRolesAsync(
                    SessionContext context,
                    bool searchByCustomerRoleID, int customerRoleID,
                    bool searchByCustomerID, Int32 customerID,
                    bool searchByIsPlaceholder, Boolean isPlaceholder,
                    bool searchByPlaceholder, Boolean placeholder,
                    bool searchByRoleID, Int32 roleID,
                                        bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchCustomerRolesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_Search: \r\n";
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
        public override IDataReader GetCustomerRoleList(
            SessionContext context)
        {
            string procedureName = "GetCustomerRoleList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(customerRoleManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetList: \r\n";
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
        public override async Task<IDataReader> GetCustomerRoleListAsync(
            SessionContext context)
        {
            string procedureName = "GetCustomerRoleListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(await customerRoleManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetList: \r\n";
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
        public override Guid GetCustomerRoleCode(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetCustomerRoleCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerRoleID::" + customerRoleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "CustomerRole::" + customerRoleID.ToString() + "::code";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                var customerRole = customerRoleManager.GetById(customerRoleID);

                result = customerRole.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetCode: \r\n";
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
        public override async Task<Guid> GetCustomerRoleCodeAsync(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetCustomerRoleCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerRoleID::" + customerRoleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "CustomerRole::" + customerRoleID.ToString() + "::code";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                var customerRole = await customerRoleManager.GetByIdAsync(customerRoleID);

                result = customerRole.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetCode: \r\n";
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
        public override IDataReader GetCustomerRole(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetCustomerRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerRoleID::" + customerRoleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = customerRoleManager.GetById(customerRoleID);

                if(customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_Get: \r\n";
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
        public override async Task<IDataReader> GetCustomerRoleAsync(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetCustomerRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerRoleID::" + customerRoleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = await customerRoleManager.GetByIdAsync(customerRoleID);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_Get: \r\n";
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
        public override IDataReader GetDirtyCustomerRole(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetDirtyCustomerRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerRoleID::" + customerRoleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = customerRoleManager.DirtyGetById(customerRoleID);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyCustomerRoleAsync(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "GetDirtyCustomerRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerRoleID::" + customerRoleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = await customerRoleManager.DirtyGetByIdAsync(customerRoleID);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_DirtyGet: \r\n";
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
        public override IDataReader GetCustomerRole(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetCustomerRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = customerRoleManager.GetByCode(code);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetCustomerRoleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetCustomerRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = await customerRoleManager.GetByCodeAsync(code);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetByCode: \r\n";
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
        public override IDataReader GetDirtyCustomerRole(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyCustomerRole";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = customerRoleManager.DirtyGetByCode(code);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyCustomerRoleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyCustomerRoleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                var customerRole = await customerRoleManager.DirtyGetByCodeAsync(code);

                if (customerRole != null)
                    customerRoles.Add(customerRole);

                rdr = BuildDataReader(customerRoles);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_DirtyGetByCode: \r\n";
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
        public override int GetCustomerRoleID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetCustomerRoleID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                var customerRole = customerRoleManager.GetByCode(code);

                result = customerRole.CustomerRoleID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetID: \r\n";
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
        public override async Task<int> GetCustomerRoleIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetCustomerRoleIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                var customerRole = await customerRoleManager.GetByCodeAsync(code);

                result = customerRole.CustomerRoleID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_GetID: \r\n";
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
        public override int CustomerRoleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkInsertList";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].CustomerRoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.Code = item.Code;
                    customerRole.LastChangeCode = Guid.NewGuid();
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;

                    bool isEncrypted = false;
                    //Int32 customerID,
                    //Boolean isPlaceholder,
                    //Boolean placeholder,
                    //Int32 roleID,
                                        customerRoles.Add(customerRole);
                }

                customerRoleManager.BulkInsert(customerRoles);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> CustomerRoleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkInsertListAsync";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerRoleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.Code = item.Code;
                    customerRole.LastChangeCode = Guid.NewGuid();
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;

                    bool isEncrypted = false;
                    //Int32 customerID,
                    //Boolean isPlaceholder,
                    //Boolean placeholder,
                    //Int32 roleID,
                                        customerRoles.Add(customerRole);
                }

                await customerRoleManager.BulkInsertAsync(customerRoles);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int CustomerRoleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkUpdateList";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerRoleID == 0)
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.CustomerRoleID = item.CustomerRoleID;
                    customerRole.Code = item.Code;
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;
                                        customerRole.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 customerID,
                    //Boolean isPlaceholder,
                    //Boolean placeholder,
                    //Int32 roleID,

                    customerRoles.Add(customerRole);
                }

                customerRoleManager.BulkUpdate(customerRoles);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> CustomerRoleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkUpdateListAsync";
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

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerRoleID == 0)
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.CustomerRoleID = item.CustomerRoleID;
                    customerRole.Code = item.Code;
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;
                                        customerRole.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 customerID,
                    //Boolean isPlaceholder,
                    //Boolean placeholder,
                    //Int32 roleID,
                                        customerRoles.Add(customerRole);
                }

                customerRoleManager.BulkUpdate(customerRoles);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int CustomerRoleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerRoleID == 0)
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.CustomerRoleID = item.CustomerRoleID;
                    customerRole.Code = item.Code;
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;
                                        customerRole.LastChangeCode = item.LastChangeCode;
                    customerRoles.Add(customerRole);
                }

                customerRoleManager.BulkDelete(customerRoles);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> CustomerRoleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.CustomerRole> dataList)
        {
            string procedureName = "CustomerRoleBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                List<EF.Models.CustomerRole> customerRoles = new List<EF.Models.CustomerRole>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].CustomerRoleID == 0)
                        continue;

                    actionCount++;

                    Objects.CustomerRole item = dataList[i];

                    EF.Models.CustomerRole customerRole = new EF.Models.CustomerRole();
                    customerRole.CustomerRoleID = item.CustomerRoleID;
                    customerRole.Code = item.Code;
                    customerRole.CustomerID = item.CustomerID;
                    customerRole.IsPlaceholder = item.IsPlaceholder;
                    customerRole.Placeholder = item.Placeholder;
                    customerRole.RoleID = item.RoleID;
                                        customerRole.LastChangeCode = item.LastChangeCode;
                    customerRoles.Add(customerRole);
                }

                await customerRoleManager.BulkDeleteAsync(customerRoles);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_CustomerRole_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void CustomerRoleDelete(
            SessionContext context,
            int customerRoleID)
        {
            string procedureName = "CustomerRoleDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::customerRoleID::" + customerRoleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                customerRoleManager.Delete(customerRoleID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_CustomerRole_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task CustomerRoleDeleteAsync(
           SessionContext context,
           int customerRoleID)
        {
            string procedureName = "CustomerRoleDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::customerRoleID::" + customerRoleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                await customerRoleManager.DeleteAsync(customerRoleID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_CustomerRole_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void CustomerRoleCleanupTesting(
            SessionContext context )
        {
            string procedureName = "CustomerRoleCleanupTesting";
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
        public override void CustomerRoleCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "CustomerRoleCleanupChildObjectTesting";
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
        public override IDataReader GetCustomerRoleList_FetchByCustomerID(
            int customerID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerRoleList_FetchByCustomerID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(customerRoleManager.GetByCustomerID(customerID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_FetchByCustomerID: \r\n";
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
        public override IDataReader GetCustomerRoleList_FetchByRoleID(
            int roleID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerRoleList_FetchByRoleID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(customerRoleManager.GetByRoleID(roleID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_CustomerRole_FetchByRoleID: \r\n";
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
        public override async Task<IDataReader> GetCustomerRoleList_FetchByCustomerIDAsync(
            int customerID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerRoleList_FetchByCustomerIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(await customerRoleManager.GetByCustomerIDAsync(customerID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_FetchByCustomerID: \r\n";
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
        public override async Task<IDataReader> GetCustomerRoleList_FetchByRoleIDAsync(
            int roleID,
           SessionContext context
            )
        {
            string procedureName = "GetCustomerRoleList_FetchByRoleIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var customerRoleManager = new EF.Managers.CustomerRoleManager(dbContext);

                rdr = BuildDataReader(await customerRoleManager.GetByRoleIDAsync(roleID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_CustomerRole_FetchByRoleID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.CustomerRole> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.CustomerRole).GetProperties())
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
                foreach (var prop in typeof(EF.Models.CustomerRole).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
