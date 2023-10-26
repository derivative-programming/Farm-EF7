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
    partial class EF7OrganizationProvider : FS.Farm.Providers.OrganizationProvider
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
        #region Organization Methods
        public override int OrganizationGetCount(
            SessionContext context )
        {
            string procedureName = "OrganizationGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                iOut = organizationManager.GetTotalCount();
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
        public override async Task<int> OrganizationGetCountAsync(
            SessionContext context )
        {
            string procedureName = "OrganizationGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                iOut = await organizationManager.GetTotalCountAsync();
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
        public override int OrganizationGetMaxID(
            SessionContext context)
        {
            string procedureName = "OrganizationGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                iOut = organizationManager.GetMaxId().Value;
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
        public override async Task<int> OrganizationGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "OrganizationGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                var maxId = await organizationManager.GetMaxIdAsync();
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
        public override int OrganizationInsert(
            SessionContext context,
            String name,
            Int32 tacID,
            System.Guid code)
        {
            string procedureName = "OrganizationInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String name,
            //Int32 tacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                EF.Models.Organization organization = new EF.Models.Organization();
                organization.Code = code;
                organization.Name = name;
                organization.TacID = tacID;
                organization = organizationManager.Add(organization);
                iOut = organization.OrganizationID;
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
        public override async Task<int> OrganizationInsertAsync(
            SessionContext context,
            String name,
            Int32 tacID,
            System.Guid code)
        {
            string procedureName = "OrganizationInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String name,
            //Int32 tacID,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                EF.Models.Organization organization = new EF.Models.Organization();
                organization.Code = code;
                organization.Name = name;
                organization.TacID = tacID;
                organization = await organizationManager.AddAsync(organization);
                iOut = organization.OrganizationID;
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
        public override void OrganizationUpdate(
            SessionContext context,
            int organizationID,
            String name,
            Int32 tacID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "OrganizationUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String name,
            //Int32 tacID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                EF.Models.Organization organization = new EF.Models.Organization();
                organization.Code = code;
                organization.Name = name;
                organization.TacID = tacID;
                bool success = organizationManager.Update(organization);
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
        public override async Task OrganizationUpdateAsync(
            SessionContext context,
            int organizationID,
            String name,
            Int32 tacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "OrganizationUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String name,
            //Int32 tacID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                EF.Models.Organization organization = new EF.Models.Organization();
                organization.Code = code;
                organization.Name = name;
                organization.TacID = tacID;
                bool success = await organizationManager.UpdateAsync(organization);
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
        public override IDataReader SearchOrganizations(
            SessionContext context,
            bool searchByOrganizationID, int organizationID,
            bool searchByName, String name,
            bool searchByTacID, Int32 tacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrganizations";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_Search: \r\n";
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
        public override async Task<IDataReader> SearchOrganizationsAsync(
                    SessionContext context,
                    bool searchByOrganizationID, int organizationID,
                    bool searchByName, String name,
                    bool searchByTacID, Int32 tacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchOrganizationsAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_Search: \r\n";
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
        public override IDataReader GetOrganizationList(
            SessionContext context)
        {
            string procedureName = "GetOrganizationList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                rdr = BuildDataReader(organizationManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_GetList: \r\n";
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
        public override async Task<IDataReader> GetOrganizationListAsync(
            SessionContext context)
        {
            string procedureName = "GetOrganizationListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                rdr = BuildDataReader(await organizationManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_GetList: \r\n";
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
        public override Guid GetOrganizationCode(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetOrganizationCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::organizationID::" + organizationID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Organization::" + organizationID.ToString() + "::code";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                var organization = organizationManager.GetById(organizationID);
                result = organization.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_GetCode: \r\n";
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
        public override async Task<Guid> GetOrganizationCodeAsync(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetOrganizationCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::organizationID::" + organizationID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "Organization::" + organizationID.ToString() + "::code";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                var organization = await organizationManager.GetByIdAsync(organizationID);
                result = organization.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_GetCode: \r\n";
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
        public override IDataReader GetOrganization(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetOrganization";
            Log(procedureName + "::Start");
            Log(procedureName + "::organizationID::" + organizationID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = organizationManager.GetById(organizationID);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_Get: \r\n";
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
        public override async Task<IDataReader> GetOrganizationAsync(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetOrganizationAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::organizationID::" + organizationID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = await organizationManager.GetByIdAsync(organizationID);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_Get: \r\n";
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
        public override IDataReader GetDirtyOrganization(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetDirtyOrganization";
            Log(procedureName + "::Start");
            Log(procedureName + "::organizationID::" + organizationID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = organizationManager.DirtyGetById(organizationID);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyOrganizationAsync(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "GetDirtyOrganizationAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::organizationID::" + organizationID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = await organizationManager.DirtyGetByIdAsync(organizationID);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_DirtyGet: \r\n";
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
        public override IDataReader GetOrganization(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrganization";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = organizationManager.GetByCode(code);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetOrganizationAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrganizationAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = await organizationManager.GetByCodeAsync(code);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_GetByCode: \r\n";
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
        public override IDataReader GetDirtyOrganization(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyOrganization";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = organizationManager.DirtyGetByCode(code);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyOrganizationAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyOrganizationAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                var organization = await organizationManager.DirtyGetByCodeAsync(code);
                organizations.Add(organization);
                rdr = BuildDataReader(organizations);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_DirtyGetByCode: \r\n";
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
        public override int GetOrganizationID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetOrganizationID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                var organization = organizationManager.GetByCode(code);
                result = organization.OrganizationID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_GetID: \r\n";
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
        public override async Task<int> GetOrganizationIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetOrganizationIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                var organization = await organizationManager.GetByCodeAsync(code);
                result = organization.OrganizationID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_GetID: \r\n";
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
        public override int OrganizationBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkInsertList";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organizations.Add(organization);
                }
                organizationManager.BulkInsert(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrganizationBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkInsertListAsync";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organizations.Add(organization);
                }
                await organizationManager.BulkInsertAsync(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int OrganizationBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkUpdateList";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.OrganizationID = item.OrganizationID;
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organization.LastChangeCode = item.LastChangeCode;
                    organizations.Add(organization);
                }
                organizationManager.BulkUpdate(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrganizationBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkUpdateListAsync";
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
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.OrganizationID = item.OrganizationID;
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organization.LastChangeCode = item.LastChangeCode;
                    organizations.Add(organization);
                }
                organizationManager.BulkUpdate(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int OrganizationBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.OrganizationID = item.OrganizationID;
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organization.LastChangeCode = item.LastChangeCode;
                    organizations.Add(organization);
                }
                organizationManager.BulkDelete(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> OrganizationBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.Organization> dataList)
        {
            string procedureName = "OrganizationBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                List<EF.Models.Organization> organizations = new List<EF.Models.Organization>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].OrganizationID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.Organization item = dataList[i];
                    EF.Models.Organization organization = new EF.Models.Organization();
                    organization.OrganizationID = item.OrganizationID;
                    organization.Code = item.Code;
                    organization.Name = item.Name;
                    organization.TacID = item.TacID;
                    organization.LastChangeCode = item.LastChangeCode;
                    organizations.Add(organization);
                }
                await organizationManager.BulkDeleteAsync(organizations);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_Organization_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void OrganizationDelete(
            SessionContext context,
            int organizationID)
        {
            string procedureName = "OrganizationDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::organizationID::" + organizationID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                organizationManager.Delete(organizationID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_Organization_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task OrganizationDeleteAsync(
           SessionContext context,
           int organizationID)
        {
            string procedureName = "OrganizationDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::organizationID::" + organizationID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                await organizationManager.DeleteAsync(organizationID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_Organization_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void OrganizationCleanupTesting(
            SessionContext context )
        {
            string procedureName = "OrganizationCleanupTesting";
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
        public override void OrganizationCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "OrganizationCleanupChildObjectTesting";
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
        public override IDataReader GetOrganizationList_FetchByTacID(
            int tacID,
           SessionContext context
            )
        {
            string procedureName = "GetOrganizationList_FetchByTacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                rdr = BuildDataReader(organizationManager.GetByTacID(tacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_Organization_FetchByTacID: \r\n";
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
        public override async Task<IDataReader> GetOrganizationList_FetchByTacIDAsync(
            int tacID,
           SessionContext context
            )
        {
            string procedureName = "GetOrganizationList_FetchByTacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var organizationManager = new EF.Managers.OrganizationManager(dbContext);
                rdr = BuildDataReader(await organizationManager.GetByTacIDAsync(tacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_Organization_FetchByTacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.Organization> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.Organization).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.Organization).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
