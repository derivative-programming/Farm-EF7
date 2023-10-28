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
    partial class EF7OrgCustomerProvider : FS.Farm.Providers.OrgCustomerProvider
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
        #region OrgCustomer Methods
        public override int OrgCustomerGetCount(
            SessionContext context )
        {
            string procedureName = "OrgCustomerGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                iOut = orgCustomerManager.GetTotalCount();
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
        public override async Task<int> OrgCustomerGetCountAsync(
            SessionContext context )
        {
            string procedureName = "OrgCustomerGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                iOut = await orgCustomerManager.GetTotalCountAsync();
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
        public override int OrgCustomerGetMaxID(
            SessionContext context)
        {
            string procedureName = "OrgCustomerGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                iOut = orgCustomerManager.GetMaxId().Value;
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
        public override async Task<int> OrgCustomerGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "OrgCustomerGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                var maxId = await orgCustomerManager.GetMaxIdAsync();
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
        public override int OrgCustomerInsert(
            SessionContext context,
            Int32 customerID,
            String email,
            Int32 organizationID,
            System.Guid code)
        {
            string procedureName = "OrgCustomerInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            bool isEncrypted = false;
            //Int32 customerID,
            //String email,
            //Int32 organizationID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                orgCustomer.Code = code;
                orgCustomer.LastChangeCode = Guid.NewGuid();
                orgCustomer.CustomerID = customerID;
                orgCustomer.Email = email;
                orgCustomer.OrganizationID = organizationID;
                orgCustomer = orgCustomerManager.Add(orgCustomer);
                iOut = orgCustomer.OrgCustomerID;
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
        public override async Task<int> OrgCustomerInsertAsync(
            SessionContext context,
            Int32 customerID,
            String email,
            Int32 organizationID,
            System.Guid code)
        {
            string procedureName = "OrgCustomerInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            bool isEncrypted = false;
            //Int32 customerID,
            //String email,
            //Int32 organizationID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                orgCustomer.Code = code;
                orgCustomer.LastChangeCode = Guid.NewGuid();
                orgCustomer.CustomerID = customerID;
                orgCustomer.Email = email;
                orgCustomer.OrganizationID = organizationID;
                orgCustomer = await orgCustomerManager.AddAsync(orgCustomer);
                iOut = orgCustomer.OrgCustomerID;
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
        public override void OrgCustomerUpdate(
            SessionContext context,
            int orgCustomerID,
            Int32 customerID,
            String email,
            Int32 organizationID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "OrgCustomerUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            bool isEncrypted = false;
            //Int32 customerID,
            //String email,
            //Int32 organizationID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                orgCustomer.OrgCustomerID = orgCustomerID;
                orgCustomer.Code = code;
                orgCustomer.CustomerID = customerID;
                orgCustomer.Email = email;
                orgCustomer.OrganizationID = organizationID;
                orgCustomer.LastChangeCode = lastChangeCode;
                bool success = orgCustomerManager.Update(orgCustomer);
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
        public override async Task OrgCustomerUpdateAsync(
            SessionContext context,
            int orgCustomerID,
            Int32 customerID,
            String email,
            Int32 organizationID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "OrgCustomerUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            bool isEncrypted = false;
            //Int32 customerID,
            //String email,
            //Int32 organizationID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                orgCustomer.OrgCustomerID = orgCustomerID;
                orgCustomer.Code = code;
                orgCustomer.CustomerID = customerID;
                orgCustomer.Email = email;
                orgCustomer.OrganizationID = organizationID;
                orgCustomer.LastChangeCode = lastChangeCode;
                bool success = await orgCustomerManager.UpdateAsync(orgCustomer);
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
        public override IDataReader SearchOrgCustomers(
            SessionContext context,
            bool searchByOrgCustomerID, int orgCustomerID,
            bool searchByCustomerID, Int32 customerID,
            bool searchByEmail, String email,
            bool searchByOrganizationID, Int32 organizationID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrgCustomers";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_Search: \r\n";
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
        public override async Task<IDataReader> SearchOrgCustomersAsync(
                    SessionContext context,
                    bool searchByOrgCustomerID, int orgCustomerID,
                    bool searchByCustomerID, Int32 customerID,
                    bool searchByEmail, String email,
                    bool searchByOrganizationID, Int32 organizationID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrgCustomersAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_Search: \r\n";
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
        public override IDataReader GetOrgCustomerList(
            SessionContext context)
        {
            string procedureName = "GetOrgCustomerList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(orgCustomerManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetList: \r\n";
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
        public override async Task<IDataReader> GetOrgCustomerListAsync(
            SessionContext context)
        {
            string procedureName = "GetOrgCustomerListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(await orgCustomerManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetList: \r\n";
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
        public override Guid GetOrgCustomerCode(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetOrgCustomerCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "OrgCustomer::" + orgCustomerID.ToString() + "::code";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                var orgCustomer = orgCustomerManager.GetById(orgCustomerID);
                result = orgCustomer.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetCode: \r\n";
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
        public override async Task<Guid> GetOrgCustomerCodeAsync(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetOrgCustomerCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "OrgCustomer::" + orgCustomerID.ToString() + "::code";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                var orgCustomer = await orgCustomerManager.GetByIdAsync(orgCustomerID);
                result = orgCustomer.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetCode: \r\n";
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
        public override IDataReader GetOrgCustomer(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetOrgCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = orgCustomerManager.GetById(orgCustomerID);
                if(orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_Get: \r\n";
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
        public override async Task<IDataReader> GetOrgCustomerAsync(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetOrgCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = await orgCustomerManager.GetByIdAsync(orgCustomerID);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_Get: \r\n";
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
        public override IDataReader GetDirtyOrgCustomer(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetDirtyOrgCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = orgCustomerManager.DirtyGetById(orgCustomerID);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyOrgCustomerAsync(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "GetDirtyOrgCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = await orgCustomerManager.DirtyGetByIdAsync(orgCustomerID);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_DirtyGet: \r\n";
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
        public override IDataReader GetOrgCustomer(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrgCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = orgCustomerManager.GetByCode(code);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetOrgCustomerAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrgCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = await orgCustomerManager.GetByCodeAsync(code);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetByCode: \r\n";
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
        public override IDataReader GetDirtyOrgCustomer(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyOrgCustomer";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = orgCustomerManager.DirtyGetByCode(code);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyOrgCustomerAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyOrgCustomerAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                var orgCustomer = await orgCustomerManager.DirtyGetByCodeAsync(code);
                if (orgCustomer != null)
                    orgCustomers.Add(orgCustomer);
                rdr = BuildDataReader(orgCustomers);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_DirtyGetByCode: \r\n";
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
        public override int GetOrgCustomerID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrgCustomerID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                var orgCustomer = orgCustomerManager.GetByCode(code);
                result = orgCustomer.OrgCustomerID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetID: \r\n";
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
        public override async Task<int> GetOrgCustomerIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrgCustomerIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                var orgCustomer = await orgCustomerManager.GetByCodeAsync(code);
                result = orgCustomer.OrgCustomerID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_GetID: \r\n";
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
        public override int OrgCustomerBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkInsertList";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].OrgCustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.Code = item.Code;
                    orgCustomer.LastChangeCode = Guid.NewGuid();
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    bool isEncrypted = false;
                    //Int32 customerID,
                    //String email,
                    //Int32 organizationID,
                    orgCustomers.Add(orgCustomer);
                }
                orgCustomerManager.BulkInsert(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrgCustomerBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkInsertListAsync";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgCustomerID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.Code = item.Code;
                    orgCustomer.LastChangeCode = Guid.NewGuid();
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    bool isEncrypted = false;
                    //Int32 customerID,
                    //String email,
                    //Int32 organizationID,
                    orgCustomers.Add(orgCustomer);
                }
                await orgCustomerManager.BulkInsertAsync(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int OrgCustomerBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkUpdateList";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgCustomerID == 0)
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.OrgCustomerID = item.OrgCustomerID;
                    orgCustomer.Code = item.Code;
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    orgCustomer.LastChangeCode = item.LastChangeCode;
                    bool isEncrypted = false;
                    //Int32 customerID,
                    //String email,
                    //Int32 organizationID,
                    orgCustomers.Add(orgCustomer);
                }
                orgCustomerManager.BulkUpdate(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrgCustomerBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkUpdateListAsync";
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
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgCustomerID == 0)
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.OrgCustomerID = item.OrgCustomerID;
                    orgCustomer.Code = item.Code;
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    orgCustomer.LastChangeCode = item.LastChangeCode;
                    bool isEncrypted = false;
                    //Int32 customerID,
                    //String email,
                    //Int32 organizationID,
                    orgCustomers.Add(orgCustomer);
                }
                orgCustomerManager.BulkUpdate(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int OrgCustomerBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgCustomerID == 0)
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.OrgCustomerID = item.OrgCustomerID;
                    orgCustomer.Code = item.Code;
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    orgCustomer.LastChangeCode = item.LastChangeCode;
                    orgCustomers.Add(orgCustomer);
                }
                orgCustomerManager.BulkDelete(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrgCustomerBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.OrgCustomer> dataList)
        {
            string procedureName = "OrgCustomerBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                List<EF.Models.OrgCustomer> orgCustomers = new List<EF.Models.OrgCustomer>();
                int actionCount = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrgCustomerID == 0)
                        continue;
                    actionCount++;
                    Objects.OrgCustomer item = dataList[i];
                    EF.Models.OrgCustomer orgCustomer = new EF.Models.OrgCustomer();
                    orgCustomer.OrgCustomerID = item.OrgCustomerID;
                    orgCustomer.Code = item.Code;
                    orgCustomer.CustomerID = item.CustomerID;
                    orgCustomer.Email = item.Email;
                    orgCustomer.OrganizationID = item.OrganizationID;
                    orgCustomer.LastChangeCode = item.LastChangeCode;
                    orgCustomers.Add(orgCustomer);
                }
                await orgCustomerManager.BulkDeleteAsync(orgCustomers);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_OrgCustomer_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void OrgCustomerDelete(
            SessionContext context,
            int orgCustomerID)
        {
            string procedureName = "OrgCustomerDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                orgCustomerManager.Delete(orgCustomerID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_OrgCustomer_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task OrgCustomerDeleteAsync(
           SessionContext context,
           int orgCustomerID)
        {
            string procedureName = "OrgCustomerDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::orgCustomerID::" + orgCustomerID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                await orgCustomerManager.DeleteAsync(orgCustomerID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_OrgCustomer_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void OrgCustomerCleanupTesting(
            SessionContext context )
        {
            string procedureName = "OrgCustomerCleanupTesting";
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
        public override void OrgCustomerCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "OrgCustomerCleanupChildObjectTesting";
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
        public override IDataReader GetOrgCustomerList_FetchByCustomerID(
            int customerID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgCustomerList_FetchByCustomerID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(orgCustomerManager.GetByCustomerID(customerID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_FetchByCustomerID: \r\n";
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
        public override IDataReader GetOrgCustomerList_FetchByOrganizationID(
            int organizationID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgCustomerList_FetchByOrganizationID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(orgCustomerManager.GetByOrganizationID(organizationID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_OrgCustomer_FetchByOrganizationID: \r\n";
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
        public override async Task<IDataReader> GetOrgCustomerList_FetchByCustomerIDAsync(
            int customerID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgCustomerList_FetchByCustomerIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(await orgCustomerManager.GetByCustomerIDAsync(customerID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_FetchByCustomerID: \r\n";
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
        public override async Task<IDataReader> GetOrgCustomerList_FetchByOrganizationIDAsync(
            int organizationID,
           SessionContext context
            )
        {
            string procedureName = "GetOrgCustomerList_FetchByOrganizationIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var orgCustomerManager = new EF.Managers.OrgCustomerManager(dbContext);
                rdr = BuildDataReader(await orgCustomerManager.GetByOrganizationIDAsync(organizationID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_OrgCustomer_FetchByOrganizationID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.OrgCustomer> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.OrgCustomer).GetProperties())
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
                foreach (var prop in typeof(EF.Models.OrgCustomer).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
