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
    partial class EF7DateGreaterThanFilterProvider : FS.Farm.Providers.DateGreaterThanFilterProvider
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
        #region DateGreaterThanFilter Methods
        public override int DateGreaterThanFilterGetCount(
            SessionContext context )
        {
            string procedureName = "DateGreaterThanFilterGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                iOut = dateGreaterThanFilterManager.GetTotalCount();
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
        public override async Task<int> DateGreaterThanFilterGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DateGreaterThanFilterGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                iOut = await dateGreaterThanFilterManager.GetTotalCountAsync();
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
        public override int DateGreaterThanFilterGetMaxID(
            SessionContext context)
        {
            string procedureName = "DateGreaterThanFilterGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                iOut = dateGreaterThanFilterManager.GetMaxId().Value;
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
        public override async Task<int> DateGreaterThanFilterGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DateGreaterThanFilterGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                var maxId = await dateGreaterThanFilterManager.GetMaxIdAsync();
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
        public override int DateGreaterThanFilterInsert(
            SessionContext context,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DateGreaterThanFilterInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //Int32 dayCount,
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                dateGreaterThanFilter.Code = code;
                dateGreaterThanFilter.DayCount = dayCount;
                dateGreaterThanFilter.Description = description;
                dateGreaterThanFilter.DisplayOrder = displayOrder;
                dateGreaterThanFilter.IsActive = isActive;
                dateGreaterThanFilter.LookupEnumName = lookupEnumName;
                dateGreaterThanFilter.Name = name;
                dateGreaterThanFilter.PacID = pacID;
                dateGreaterThanFilter = dateGreaterThanFilterManager.Add(dateGreaterThanFilter);
                iOut = dateGreaterThanFilter.DateGreaterThanFilterID;
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
        public override async Task<int> DateGreaterThanFilterInsertAsync(
            SessionContext context,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DateGreaterThanFilterInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 dayCount,
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                dateGreaterThanFilter.Code = code;
                dateGreaterThanFilter.DayCount = dayCount;
                dateGreaterThanFilter.Description = description;
                dateGreaterThanFilter.DisplayOrder = displayOrder;
                dateGreaterThanFilter.IsActive = isActive;
                dateGreaterThanFilter.LookupEnumName = lookupEnumName;
                dateGreaterThanFilter.Name = name;
                dateGreaterThanFilter.PacID = pacID;
                dateGreaterThanFilter = await dateGreaterThanFilterManager.AddAsync(dateGreaterThanFilter);
                iOut = dateGreaterThanFilter.DateGreaterThanFilterID;
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
        public override void DateGreaterThanFilterUpdate(
            SessionContext context,
            int dateGreaterThanFilterID,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
             Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DateGreaterThanFilterUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            //Int32 dayCount,
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                dateGreaterThanFilter.Code = code;
                dateGreaterThanFilter.DayCount = dayCount;
                dateGreaterThanFilter.Description = description;
                dateGreaterThanFilter.DisplayOrder = displayOrder;
                dateGreaterThanFilter.IsActive = isActive;
                dateGreaterThanFilter.LookupEnumName = lookupEnumName;
                dateGreaterThanFilter.Name = name;
                dateGreaterThanFilter.PacID = pacID;
                bool success = dateGreaterThanFilterManager.Update(dateGreaterThanFilter);
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
        public override async Task DateGreaterThanFilterUpdateAsync(
            SessionContext context,
            int dateGreaterThanFilterID,
            Int32 dayCount,
            String description,
            Int32 displayOrder,
            Boolean isActive,
            String lookupEnumName,
            String name,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DateGreaterThanFilterUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            //Int32 dayCount,
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                dateGreaterThanFilter.Code = code;
                dateGreaterThanFilter.DayCount = dayCount;
                dateGreaterThanFilter.Description = description;
                dateGreaterThanFilter.DisplayOrder = displayOrder;
                dateGreaterThanFilter.IsActive = isActive;
                dateGreaterThanFilter.LookupEnumName = lookupEnumName;
                dateGreaterThanFilter.Name = name;
                dateGreaterThanFilter.PacID = pacID;
                bool success = await dateGreaterThanFilterManager.UpdateAsync(dateGreaterThanFilter);
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
        public override IDataReader SearchDateGreaterThanFilters(
            SessionContext context,
            bool searchByDateGreaterThanFilterID, int dateGreaterThanFilterID,
            bool searchByDayCount, Int32 dayCount,
            bool searchByDescription, String description,
            bool searchByDisplayOrder, Int32 displayOrder,
            bool searchByIsActive, Boolean isActive,
            bool searchByLookupEnumName, String lookupEnumName,
            bool searchByName, String name,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDateGreaterThanFilters";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_Search: \r\n";
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
        public override async Task<IDataReader> SearchDateGreaterThanFiltersAsync(
                    SessionContext context,
                    bool searchByDateGreaterThanFilterID, int dateGreaterThanFilterID,
                    bool searchByDayCount, Int32 dayCount,
                    bool searchByDescription, String description,
                    bool searchByDisplayOrder, Int32 displayOrder,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLookupEnumName, String lookupEnumName,
                    bool searchByName, String name,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDateGreaterThanFiltersAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_Search: \r\n";
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
        public override IDataReader GetDateGreaterThanFilterList(
            SessionContext context)
        {
            string procedureName = "GetDateGreaterThanFilterList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                rdr = BuildDataReader(dateGreaterThanFilterManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetList: \r\n";
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
        public override async Task<IDataReader> GetDateGreaterThanFilterListAsync(
            SessionContext context)
        {
            string procedureName = "GetDateGreaterThanFilterListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                rdr = BuildDataReader(await dateGreaterThanFilterManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetList: \r\n";
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
        public override Guid GetDateGreaterThanFilterCode(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDateGreaterThanFilterCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DateGreaterThanFilter::" + dateGreaterThanFilterID.ToString() + "::code";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                var dateGreaterThanFilter = dateGreaterThanFilterManager.GetById(dateGreaterThanFilterID);
                result = dateGreaterThanFilter.Code.Value;
                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetCode: \r\n";
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
        public override async Task<Guid> GetDateGreaterThanFilterCodeAsync(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDateGreaterThanFilterCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DateGreaterThanFilter::" + dateGreaterThanFilterID.ToString() + "::code";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.GetByIdAsync(dateGreaterThanFilterID);
                result = dateGreaterThanFilter.Code.Value;
                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetCode: \r\n";
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
        public override IDataReader GetDateGreaterThanFilter(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDateGreaterThanFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = dateGreaterThanFilterManager.GetById(dateGreaterThanFilterID);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_Get: \r\n";
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
        public override async Task<IDataReader> GetDateGreaterThanFilterAsync(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDateGreaterThanFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.GetByIdAsync(dateGreaterThanFilterID);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_Get: \r\n";
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
        public override IDataReader GetDirtyDateGreaterThanFilter(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDirtyDateGreaterThanFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = dateGreaterThanFilterManager.DirtyGetById(dateGreaterThanFilterID);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDateGreaterThanFilterAsync(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "GetDirtyDateGreaterThanFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.DirtyGetByIdAsync(dateGreaterThanFilterID);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_DirtyGet: \r\n";
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
        public override IDataReader GetDateGreaterThanFilter(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDateGreaterThanFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = dateGreaterThanFilterManager.GetByCode(code);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDateGreaterThanFilterAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDateGreaterThanFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.GetByCodeAsync(code);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDateGreaterThanFilter(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDateGreaterThanFilter";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = dateGreaterThanFilterManager.DirtyGetByCode(code);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDateGreaterThanFilterAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDateGreaterThanFilterAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.DirtyGetByCodeAsync(code);
                dateGreaterThanFilters.Add(dateGreaterThanFilter);
                rdr = BuildDataReader(dateGreaterThanFilters);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_DirtyGetByCode: \r\n";
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
        public override int GetDateGreaterThanFilterID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDateGreaterThanFilterID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                var dateGreaterThanFilter = dateGreaterThanFilterManager.GetByCode(code);
                result = dateGreaterThanFilter.DateGreaterThanFilterID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetID: \r\n";
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
        public override async Task<int> GetDateGreaterThanFilterIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDateGreaterThanFilterIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                var dateGreaterThanFilter = await dateGreaterThanFilterManager.GetByCodeAsync(code);
                result = dateGreaterThanFilter.DateGreaterThanFilterID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_GetID: \r\n";
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
        public override int DateGreaterThanFilterBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkInsertList";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                dateGreaterThanFilterManager.BulkInsert(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DateGreaterThanFilterBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkInsertListAsync";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                await dateGreaterThanFilterManager.BulkInsertAsync(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DateGreaterThanFilterBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkUpdateList";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.DateGreaterThanFilterID = item.DateGreaterThanFilterID;
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilter.LastChangeCode = item.LastChangeCode;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                dateGreaterThanFilterManager.BulkUpdate(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DateGreaterThanFilterBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkUpdateListAsync";
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
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.DateGreaterThanFilterID = item.DateGreaterThanFilterID;
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilter.LastChangeCode = item.LastChangeCode;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                dateGreaterThanFilterManager.BulkUpdate(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DateGreaterThanFilterBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.DateGreaterThanFilterID = item.DateGreaterThanFilterID;
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilter.LastChangeCode = item.LastChangeCode;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                dateGreaterThanFilterManager.BulkDelete(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DateGreaterThanFilterBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DateGreaterThanFilter> dataList)
        {
            string procedureName = "DateGreaterThanFilterBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                List<EF.Models.DateGreaterThanFilter> dateGreaterThanFilters = new List<EF.Models.DateGreaterThanFilter>();
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DateGreaterThanFilterID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;
                    Objects.DateGreaterThanFilter item = dataList[i];
                    EF.Models.DateGreaterThanFilter dateGreaterThanFilter = new EF.Models.DateGreaterThanFilter();
                    dateGreaterThanFilter.DateGreaterThanFilterID = item.DateGreaterThanFilterID;
                    dateGreaterThanFilter.Code = item.Code;
                    dateGreaterThanFilter.DayCount = item.DayCount;
                    dateGreaterThanFilter.Description = item.Description;
                    dateGreaterThanFilter.DisplayOrder = item.DisplayOrder;
                    dateGreaterThanFilter.IsActive = item.IsActive;
                    dateGreaterThanFilter.LookupEnumName = item.LookupEnumName;
                    dateGreaterThanFilter.Name = item.Name;
                    dateGreaterThanFilter.PacID = item.PacID;
                    dateGreaterThanFilter.LastChangeCode = item.LastChangeCode;
                    dateGreaterThanFilters.Add(dateGreaterThanFilter);
                }
                await dateGreaterThanFilterManager.BulkDeleteAsync(dateGreaterThanFilters);
                bulkCount = dataList.Count;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DateGreaterThanFilter_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DateGreaterThanFilterDelete(
            SessionContext context,
            int dateGreaterThanFilterID)
        {
            string procedureName = "DateGreaterThanFilterDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                dateGreaterThanFilterManager.Delete(dateGreaterThanFilterID);
            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DateGreaterThanFilter_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DateGreaterThanFilterDeleteAsync(
           SessionContext context,
           int dateGreaterThanFilterID)
        {
            string procedureName = "DateGreaterThanFilterDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dateGreaterThanFilterID::" + dateGreaterThanFilterID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                await dateGreaterThanFilterManager.DeleteAsync(dateGreaterThanFilterID);
            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DateGreaterThanFilter_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DateGreaterThanFilterCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DateGreaterThanFilterCleanupTesting";
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
        public override void DateGreaterThanFilterCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DateGreaterThanFilterCleanupChildObjectTesting";
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
        public override IDataReader GetDateGreaterThanFilterList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDateGreaterThanFilterList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                rdr = BuildDataReader(dateGreaterThanFilterManager.GetByPac(pacID));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetDateGreaterThanFilterList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDateGreaterThanFilterList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);
                var dateGreaterThanFilterManager = new EF.Managers.DateGreaterThanFilterManager(dbContext);
                rdr = BuildDataReader(await dateGreaterThanFilterManager.GetByPacAsync(pacID));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DateGreaterThanFilter_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DateGreaterThanFilter> data)
        {
            var dataTable = new DataTable();
            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DateGreaterThanFilter).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            // Populating the DataTable
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in typeof(EF.Models.DateGreaterThanFilter).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable.CreateDataReader();
        }
    }
}
