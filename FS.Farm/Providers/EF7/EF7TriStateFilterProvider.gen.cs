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
    partial class EF7TriStateFilterProvider : FS.Farm.Providers.TriStateFilterProvider
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
        #region TriStateFilter Methods
        public override int TriStateFilterGetCount(
            SessionContext context )
        {
            string procedureName = "TriStateFilterGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                iOut = triStateFilterManager.GetTotalCount();
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
        public override async Task<int> TriStateFilterGetCountAsync(
            SessionContext context )
        {
            string procedureName = "TriStateFilterGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                iOut = await triStateFilterManager.GetTotalCountAsync();
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
        public override int TriStateFilterGetMaxID(
            SessionContext context)
        {
            string procedureName = "TriStateFilterGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                iOut = triStateFilterManager.GetMaxId().Value;
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
        public override async Task<int> TriStateFilterGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "TriStateFilterGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                var maxId = await triStateFilterManager.GetMaxIdAsync();
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
        public override int TriStateFilterInsert(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            System.Guid code)
        {
            string procedureName = "TriStateFilterInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            //Int32 stateIntValue,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                triStateFilter.Code = code;
                triStateFilter.LastChangeCode = Guid.NewGuid();
                triStateFilter.Description = description;
                triStateFilter.DisplayOrder = displayOrder;
                triStateFilter.IsActive = isActive;
                triStateFilter.LookupEnumName = lookupEnumName;
                triStateFilter.Name = name;
                triStateFilter.PacID = pacID;
                triStateFilter.StateIntValue = stateIntValue;
                triStateFilter = triStateFilterManager.Add(triStateFilter);
                iOut = triStateFilter.TriStateFilterID;
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
        public override async Task<int> TriStateFilterInsertAsync(
            SessionContext context,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            System.Guid code)
        {
            string procedureName = "TriStateFilterInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            //Int32 stateIntValue,
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                triStateFilter.Code = code;
                triStateFilter.LastChangeCode = Guid.NewGuid();
                triStateFilter.Description = description;
                triStateFilter.DisplayOrder = displayOrder;
                triStateFilter.IsActive = isActive;
                triStateFilter.LookupEnumName = lookupEnumName;
                triStateFilter.Name = name;
                triStateFilter.PacID = pacID;
                triStateFilter.StateIntValue = stateIntValue;
                triStateFilter = await triStateFilterManager.AddAsync(triStateFilter);
                iOut = triStateFilter.TriStateFilterID;
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
        public override void TriStateFilterUpdate(
            SessionContext context,
            int triStateFilterID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "TriStateFilterUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            //Int32 stateIntValue,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                triStateFilter.Code = code;
                triStateFilter.Description = description;
                triStateFilter.DisplayOrder = displayOrder;
                triStateFilter.IsActive = isActive;
                triStateFilter.LookupEnumName = lookupEnumName;
                triStateFilter.Name = name;
                triStateFilter.PacID = pacID;
                triStateFilter.StateIntValue = stateIntValue;
                bool success = triStateFilterManager.Update(triStateFilter);
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
        public override async Task TriStateFilterUpdateAsync(
            SessionContext context,
            int triStateFilterID,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Int32 stateIntValue,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "TriStateFilterUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //String description,
            //Int32 displayOrder,
            //Boolean isActive,
            //String lookupEnumName,
            //String name,
            //Int32 pacID,
            //Int32 stateIntValue,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                triStateFilter.Code = code;
                triStateFilter.Description = description;
                triStateFilter.DisplayOrder = displayOrder;
                triStateFilter.IsActive = isActive;
                triStateFilter.LookupEnumName = lookupEnumName;
                triStateFilter.Name = name;
                triStateFilter.PacID = pacID;
                triStateFilter.StateIntValue = stateIntValue;
                bool success = await triStateFilterManager.UpdateAsync(triStateFilter);
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
        public override IDataReader SearchTriStateFilters(
            SessionContext context,
            bool searchByTriStateFilterID, int triStateFilterID,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByStateIntValue, Int32 stateIntValue,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchTriStateFilters";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_Search: \r\n";
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
        public override async Task<IDataReader> SearchTriStateFiltersAsync(
                    SessionContext context,
                    bool searchByTriStateFilterID, int triStateFilterID,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByStateIntValue, Int32 stateIntValue,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchTriStateFiltersAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_Search: \r\n";
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
        public override IDataReader GetTriStateFilterList(
            SessionContext context)
        {
            string procedureName = "GetTriStateFilterList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                rdr = BuildDataReader(triStateFilterManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetList: \r\n";
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
        public override async Task<IDataReader> GetTriStateFilterListAsync(
            SessionContext context)
        {
            string procedureName = "GetTriStateFilterListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                rdr = BuildDataReader(await triStateFilterManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetList: \r\n";
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
        public override Guid GetTriStateFilterCode(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetTriStateFilterCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "TriStateFilter::" + triStateFilterID.ToString() + "::code";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                var triStateFilter = triStateFilterManager.GetById(triStateFilterID);
                result = triStateFilter.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetCode: \r\n";
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
        public override async Task<Guid> GetTriStateFilterCodeAsync(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetTriStateFilterCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "TriStateFilter::" + triStateFilterID.ToString() + "::code";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                var triStateFilter = await triStateFilterManager.GetByIdAsync(triStateFilterID);
                result = triStateFilter.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetCode: \r\n";
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
        public override IDataReader GetTriStateFilter(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetTriStateFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = triStateFilterManager.GetById(triStateFilterID);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_Get: \r\n";
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
        public override async Task<IDataReader> GetTriStateFilterAsync(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetTriStateFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = await triStateFilterManager.GetByIdAsync(triStateFilterID);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_Get: \r\n";
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
        public override IDataReader GetDirtyTriStateFilter(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetDirtyTriStateFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = triStateFilterManager.DirtyGetById(triStateFilterID);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyTriStateFilterAsync(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "GetDirtyTriStateFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = await triStateFilterManager.DirtyGetByIdAsync(triStateFilterID);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_DirtyGet: \r\n";
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
        public override IDataReader GetTriStateFilter(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetTriStateFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = triStateFilterManager.GetByCode(code);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetTriStateFilterAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetTriStateFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = await triStateFilterManager.GetByCodeAsync(code);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetByCode: \r\n";
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
        public override IDataReader GetDirtyTriStateFilter(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyTriStateFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = triStateFilterManager.DirtyGetByCode(code);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyTriStateFilterAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyTriStateFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                var triStateFilter = await triStateFilterManager.DirtyGetByCodeAsync(code);
                triStateFilters.Add(triStateFilter);
                rdr = BuildDataReader(triStateFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_DirtyGetByCode: \r\n";
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
        public override int GetTriStateFilterID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetTriStateFilterID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                var triStateFilter = triStateFilterManager.GetByCode(code);
                result = triStateFilter.TriStateFilterID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetID: \r\n";
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
        public override async Task<int> GetTriStateFilterIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetTriStateFilterIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                var triStateFilter = await triStateFilterManager.GetByCodeAsync(code);
                result = triStateFilter.TriStateFilterID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_GetID: \r\n";
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
        public override int TriStateFilterBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkInsertList";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.Code = item.Code;
                    triStateFilter.LastChangeCode = Guid.NewGuid();
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilters.Add(triStateFilter);
                }
                triStateFilterManager.BulkInsert(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TriStateFilterBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkInsertListAsync";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.Code = item.Code;
                    triStateFilter.LastChangeCode = Guid.NewGuid();
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilters.Add(triStateFilter);
                }
                await triStateFilterManager.BulkInsertAsync(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int TriStateFilterBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkUpdateList";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.TriStateFilterID = item.TriStateFilterID;
                    triStateFilter.Code = item.Code;
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilter.LastChangeCode = item.LastChangeCode;
                    triStateFilters.Add(triStateFilter);
                }
                triStateFilterManager.BulkUpdate(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TriStateFilterBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkUpdateListAsync";
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
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.TriStateFilterID = item.TriStateFilterID;
                    triStateFilter.Code = item.Code;
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilter.LastChangeCode = item.LastChangeCode;
                    triStateFilters.Add(triStateFilter);
                }
                triStateFilterManager.BulkUpdate(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int TriStateFilterBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.TriStateFilterID = item.TriStateFilterID;
                    triStateFilter.Code = item.Code;
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilter.LastChangeCode = item.LastChangeCode;
                    triStateFilters.Add(triStateFilter);
                }
                triStateFilterManager.BulkDelete(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> TriStateFilterBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.TriStateFilter> dataList)
        {
            string procedureName = "TriStateFilterBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                List<EF.Models.TriStateFilter> triStateFilters = new List<EF.Models.TriStateFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].TriStateFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.TriStateFilter item = dataList[i];
                    EF.Models.TriStateFilter triStateFilter = new EF.Models.TriStateFilter();
                    triStateFilter.TriStateFilterID = item.TriStateFilterID;
                    triStateFilter.Code = item.Code;
                    triStateFilter.Description = item.Description;
                    triStateFilter.DisplayOrder = item.DisplayOrder;
                    triStateFilter.IsActive = item.IsActive;
                    triStateFilter.LookupEnumName = item.LookupEnumName;
                    triStateFilter.Name = item.Name;
                    triStateFilter.PacID = item.PacID;
                    triStateFilter.StateIntValue = item.StateIntValue;
                    triStateFilter.LastChangeCode = item.LastChangeCode;
                    triStateFilters.Add(triStateFilter);
                }
                await triStateFilterManager.BulkDeleteAsync(triStateFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_TriStateFilter_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void TriStateFilterDelete(
            SessionContext context,
            int triStateFilterID)
        {
            string procedureName = "TriStateFilterDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                triStateFilterManager.Delete(triStateFilterID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_TriStateFilter_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task TriStateFilterDeleteAsync(
           SessionContext context,
           int triStateFilterID)
        {
            string procedureName = "TriStateFilterDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::triStateFilterID::" + triStateFilterID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                await triStateFilterManager.DeleteAsync(triStateFilterID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_TriStateFilter_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void TriStateFilterCleanupTesting(
            SessionContext context )
        {
            string procedureName = "TriStateFilterCleanupTesting";
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
        public override void TriStateFilterCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "TriStateFilterCleanupChildObjectTesting";
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
        public override IDataReader GetTriStateFilterList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetTriStateFilterList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                rdr = BuildDataReader(triStateFilterManager.GetByPacID(pacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_TriStateFilter_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetTriStateFilterList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetTriStateFilterList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var triStateFilterManager = new EF.Managers.TriStateFilterManager(dbContext);
                rdr = BuildDataReader(await triStateFilterManager.GetByPacIDAsync(pacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_TriStateFilter_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.TriStateFilter> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.TriStateFilter).GetProperties())
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
                foreach (var prop in typeof(EF.Models.TriStateFilter).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
