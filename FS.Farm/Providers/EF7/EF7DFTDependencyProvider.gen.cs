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
    partial class EF7DFTDependencyProvider : FS.Farm.Providers.DFTDependencyProvider
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
        #region DFTDependency Methods
        public override int DFTDependencyGetCount(
            SessionContext context )
        {
            string procedureName = "DFTDependencyGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                iOut = dFTDependencyManager.GetTotalCount();
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
        public override async Task<int> DFTDependencyGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DFTDependencyGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                iOut = await dFTDependencyManager.GetTotalCountAsync();

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
        public override int DFTDependencyGetMaxID(
            SessionContext context)
        {
            string procedureName = "DFTDependencyGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                iOut = dFTDependencyManager.GetMaxId().Value;
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
        public override async Task<int> DFTDependencyGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DFTDependencyGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                var maxId = await dFTDependencyManager.GetMaxIdAsync();

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
        public override int DFTDependencyInsert(
            SessionContext context,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            System.Guid code)
        {
            string procedureName = "DFTDependencyInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dependencyDFTaskID,
            //Int32 dynaFlowTaskID,
            //Boolean isPlaceholder,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                dFTDependency.Code = code;
                dFTDependency.LastChangeCode = Guid.NewGuid();
                dFTDependency.DependencyDFTaskID = dependencyDFTaskID;
                dFTDependency.DynaFlowTaskID = dynaFlowTaskID;
                dFTDependency.IsPlaceholder = isPlaceholder;
                dFTDependency = dFTDependencyManager.Add(dFTDependency);

                iOut = dFTDependency.DFTDependencyID;
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
        public override async Task<int> DFTDependencyInsertAsync(
            SessionContext context,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            System.Guid code)
        {
            string procedureName = "DFTDependencyInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dependencyDFTaskID,
            //Int32 dynaFlowTaskID,
            //Boolean isPlaceholder,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                dFTDependency.Code = code;
                dFTDependency.LastChangeCode = Guid.NewGuid();
                dFTDependency.DependencyDFTaskID = dependencyDFTaskID;
                dFTDependency.DynaFlowTaskID = dynaFlowTaskID;
                dFTDependency.IsPlaceholder = isPlaceholder;
                dFTDependency = await dFTDependencyManager.AddAsync(dFTDependency);

                iOut = dFTDependency.DFTDependencyID;
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
        public override void DFTDependencyUpdate(
            SessionContext context,
            int dFTDependencyID,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DFTDependencyUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dependencyDFTaskID,
            //Int32 dynaFlowTaskID,
            //Boolean isPlaceholder,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                dFTDependency.DFTDependencyID = dFTDependencyID;
                dFTDependency.Code = code;
                dFTDependency.DependencyDFTaskID = dependencyDFTaskID;
                dFTDependency.DynaFlowTaskID = dynaFlowTaskID;
                dFTDependency.IsPlaceholder = isPlaceholder;
                dFTDependency.LastChangeCode = lastChangeCode;

                bool success = dFTDependencyManager.Update(dFTDependency);
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
        public override async Task DFTDependencyUpdateAsync(
            SessionContext context,
            int dFTDependencyID,
            Int32 dependencyDFTaskID,
            Int32 dynaFlowTaskID,
            Boolean isPlaceholder,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DFTDependencyUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dependencyDFTaskID,
            //Int32 dynaFlowTaskID,
            //Boolean isPlaceholder,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                dFTDependency.DFTDependencyID = dFTDependencyID;
                dFTDependency.Code = code;
                dFTDependency.DependencyDFTaskID = dependencyDFTaskID;
                dFTDependency.DynaFlowTaskID = dynaFlowTaskID;
                dFTDependency.IsPlaceholder = isPlaceholder;
                dFTDependency.LastChangeCode = lastChangeCode;

                bool success = await dFTDependencyManager.UpdateAsync(dFTDependency);
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
        public override IDataReader SearchDFTDependencys(
            SessionContext context,
            bool searchByDFTDependencyID, int dFTDependencyID,
            bool searchByDependencyDFTaskID, Int32 dependencyDFTaskID,
            bool searchByDynaFlowTaskID, Int32 dynaFlowTaskID,
            bool searchByIsPlaceholder, Boolean isPlaceholder,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDFTDependencys";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_Search: \r\n";
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
        public override async Task<IDataReader> SearchDFTDependencysAsync(
                    SessionContext context,
                    bool searchByDFTDependencyID, int dFTDependencyID,
                    bool searchByDependencyDFTaskID, Int32 dependencyDFTaskID,
                    bool searchByDynaFlowTaskID, Int32 dynaFlowTaskID,
                    bool searchByIsPlaceholder, Boolean isPlaceholder,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDFTDependencysAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_Search: \r\n";
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
        public override IDataReader GetDFTDependencyList(
            SessionContext context)
        {
            string procedureName = "GetDFTDependencyList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                rdr = BuildDataReader(dFTDependencyManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetList: \r\n";
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
        public override async Task<IDataReader> GetDFTDependencyListAsync(
            SessionContext context)
        {
            string procedureName = "GetDFTDependencyListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                rdr = BuildDataReader(await dFTDependencyManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetList: \r\n";
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
        public override Guid GetDFTDependencyCode(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDFTDependencyCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DFTDependency::" + dFTDependencyID.ToString() + "::code";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                var dFTDependency = dFTDependencyManager.GetById(dFTDependencyID);

                result = dFTDependency.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetCode: \r\n";
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
        public override async Task<Guid> GetDFTDependencyCodeAsync(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDFTDependencyCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DFTDependency::" + dFTDependencyID.ToString() + "::code";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                var dFTDependency = await dFTDependencyManager.GetByIdAsync(dFTDependencyID);

                result = dFTDependency.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetCode: \r\n";
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
        public override IDataReader GetDFTDependency(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDFTDependency";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = dFTDependencyManager.GetById(dFTDependencyID);

                if(dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_Get: \r\n";
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
        public override async Task<IDataReader> GetDFTDependencyAsync(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDFTDependencyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = await dFTDependencyManager.GetByIdAsync(dFTDependencyID);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_Get: \r\n";
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
        public override IDataReader GetDirtyDFTDependency(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDirtyDFTDependency";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = dFTDependencyManager.DirtyGetById(dFTDependencyID);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDFTDependencyAsync(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "GetDirtyDFTDependencyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = await dFTDependencyManager.DirtyGetByIdAsync(dFTDependencyID);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_DirtyGet: \r\n";
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
        public override IDataReader GetDFTDependency(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDFTDependency";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = dFTDependencyManager.GetByCode(code);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDFTDependencyAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDFTDependencyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = await dFTDependencyManager.GetByCodeAsync(code);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDFTDependency(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDFTDependency";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = dFTDependencyManager.DirtyGetByCode(code);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDFTDependencyAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDFTDependencyAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                var dFTDependency = await dFTDependencyManager.DirtyGetByCodeAsync(code);

                if (dFTDependency != null)
                    dFTDependencys.Add(dFTDependency);

                rdr = BuildDataReader(dFTDependencys);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_DirtyGetByCode: \r\n";
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
        public override int GetDFTDependencyID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDFTDependencyID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                var dFTDependency = dFTDependencyManager.GetByCode(code);

                result = dFTDependency.DFTDependencyID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetID: \r\n";
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
        public override async Task<int> GetDFTDependencyIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDFTDependencyIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                var dFTDependency = await dFTDependencyManager.GetByCodeAsync(code);

                result = dFTDependency.DFTDependencyID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_GetID: \r\n";
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
        public override int DFTDependencyBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkInsertList";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DFTDependencyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.Code = item.Code;
                    dFTDependency.LastChangeCode = Guid.NewGuid();
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    bool isEncrypted = false;
                    //Int32 dependencyDFTaskID,
                    //Int32 dynaFlowTaskID,
                    //Boolean isPlaceholder,
                    dFTDependencys.Add(dFTDependency);
                }

                dFTDependencyManager.BulkInsert(dFTDependencys);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DFTDependencyBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkInsertListAsync";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFTDependencyID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.Code = item.Code;
                    dFTDependency.LastChangeCode = Guid.NewGuid();
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    bool isEncrypted = false;
                    //Int32 dependencyDFTaskID,
                    //Int32 dynaFlowTaskID,
                    //Boolean isPlaceholder,
                    dFTDependencys.Add(dFTDependency);
                }

                await dFTDependencyManager.BulkInsertAsync(dFTDependencys);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DFTDependencyBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkUpdateList";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFTDependencyID == 0)
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.DFTDependencyID = item.DFTDependencyID;
                    dFTDependency.Code = item.Code;
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    dFTDependency.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 dependencyDFTaskID,
                    //Int32 dynaFlowTaskID,
                    //Boolean isPlaceholder,

                    dFTDependencys.Add(dFTDependency);
                }

                dFTDependencyManager.BulkUpdate(dFTDependencys);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DFTDependencyBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkUpdateListAsync";
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

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFTDependencyID == 0)
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.DFTDependencyID = item.DFTDependencyID;
                    dFTDependency.Code = item.Code;
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    dFTDependency.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 dependencyDFTaskID,
                    //Int32 dynaFlowTaskID,
                    //Boolean isPlaceholder,
                    dFTDependencys.Add(dFTDependency);
                }

                dFTDependencyManager.BulkUpdate(dFTDependencys);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DFTDependencyBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFTDependencyID == 0)
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.DFTDependencyID = item.DFTDependencyID;
                    dFTDependency.Code = item.Code;
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    dFTDependency.LastChangeCode = item.LastChangeCode;
                    dFTDependencys.Add(dFTDependency);
                }

                dFTDependencyManager.BulkDelete(dFTDependencys);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DFTDependencyBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DFTDependency> dataList)
        {
            string procedureName = "DFTDependencyBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                List<EF.Models.DFTDependency> dFTDependencys = new List<EF.Models.DFTDependency>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DFTDependencyID == 0)
                        continue;

                    actionCount++;

                    Objects.DFTDependency item = dataList[i];

                    EF.Models.DFTDependency dFTDependency = new EF.Models.DFTDependency();
                    dFTDependency.DFTDependencyID = item.DFTDependencyID;
                    dFTDependency.Code = item.Code;
                    dFTDependency.DependencyDFTaskID = item.DependencyDFTaskID;
                    dFTDependency.DynaFlowTaskID = item.DynaFlowTaskID;
                    dFTDependency.IsPlaceholder = item.IsPlaceholder;
                    dFTDependency.LastChangeCode = item.LastChangeCode;
                    dFTDependencys.Add(dFTDependency);
                }

                await dFTDependencyManager.BulkDeleteAsync(dFTDependencys);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DFTDependency_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DFTDependencyDelete(
            SessionContext context,
            int dFTDependencyID)
        {
            string procedureName = "DFTDependencyDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                dFTDependencyManager.Delete(dFTDependencyID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DFTDependency_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DFTDependencyDeleteAsync(
           SessionContext context,
           int dFTDependencyID)
        {
            string procedureName = "DFTDependencyDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dFTDependencyID::" + dFTDependencyID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                await dFTDependencyManager.DeleteAsync(dFTDependencyID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DFTDependency_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DFTDependencyCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DFTDependencyCleanupTesting";
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
        public override void DFTDependencyCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DFTDependencyCleanupChildObjectTesting";
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
        public override IDataReader GetDFTDependencyList_FetchByDynaFlowTaskID(
            int dynaFlowTaskID,
           SessionContext context
            )
        {
            string procedureName = "GetDFTDependencyList_FetchByDynaFlowTaskID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                rdr = BuildDataReader(dFTDependencyManager.GetByDynaFlowTaskID(dynaFlowTaskID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DFTDependency_FetchByDynaFlowTaskID: \r\n";
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
        public override async Task<IDataReader> GetDFTDependencyList_FetchByDynaFlowTaskIDAsync(
            int dynaFlowTaskID,
           SessionContext context
            )
        {
            string procedureName = "GetDFTDependencyList_FetchByDynaFlowTaskIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dFTDependencyManager = new EF.Managers.DFTDependencyManager(dbContext);

                rdr = BuildDataReader(await dFTDependencyManager.GetByDynaFlowTaskIDAsync(dynaFlowTaskID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DFTDependency_FetchByDynaFlowTaskID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DFTDependency> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DFTDependency).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DFTDependency).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
