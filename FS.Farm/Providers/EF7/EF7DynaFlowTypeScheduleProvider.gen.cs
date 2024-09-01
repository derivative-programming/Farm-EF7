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
    partial class EF7DynaFlowTypeScheduleProvider : FS.Farm.Providers.DynaFlowTypeScheduleProvider
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
        #region DynaFlowTypeSchedule Methods
        public override int DynaFlowTypeScheduleGetCount(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeScheduleGetCount";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                iOut = dynaFlowTypeScheduleManager.GetTotalCount();
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
        public override async Task<int> DynaFlowTypeScheduleGetCountAsync(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeScheduleGetCountAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                iOut = await dynaFlowTypeScheduleManager.GetTotalCountAsync();

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
        public override int DynaFlowTypeScheduleGetMaxID(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeScheduleGetMaxID";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                iOut = dynaFlowTypeScheduleManager.GetMaxId().Value;
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
        public override async Task<int> DynaFlowTypeScheduleGetMaxIDAsync(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeScheduleGetMaxIDAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            //Define the parameters
            int iOut = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                var maxId = await dynaFlowTypeScheduleManager.GetMaxIdAsync();

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
        public override int DynaFlowTypeScheduleInsert(
            SessionContext context,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeScheduleInsert";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dynaFlowTypeID,
            //Int32 frequencyInHours,
            //Boolean isActive,
            if (System.Convert.ToDateTime(lastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //lastUTCDateTime
            {
                 lastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) //nextUTCDateTime
            {
                 nextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                dynaFlowTypeSchedule.Code = code;
                dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                dynaFlowTypeSchedule.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowTypeSchedule.FrequencyInHours = frequencyInHours;
                dynaFlowTypeSchedule.IsActive = isActive;
                dynaFlowTypeSchedule.LastUTCDateTime = lastUTCDateTime;
                dynaFlowTypeSchedule.NextUTCDateTime = nextUTCDateTime;
                dynaFlowTypeSchedule.PacID = pacID;
                dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.Add(dynaFlowTypeSchedule);

                iOut = dynaFlowTypeSchedule.DynaFlowTypeScheduleID;
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
        public override async Task<int> DynaFlowTypeScheduleInsertAsync(
            SessionContext context,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeScheduleInsertAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dynaFlowTypeID,
            //Int32 frequencyInHours,
            //Boolean isActive,
            if (System.Convert.ToDateTime(lastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                dynaFlowTypeSchedule.Code = code;
                dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                dynaFlowTypeSchedule.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowTypeSchedule.FrequencyInHours = frequencyInHours;
                dynaFlowTypeSchedule.IsActive = isActive;
                dynaFlowTypeSchedule.LastUTCDateTime = lastUTCDateTime;
                dynaFlowTypeSchedule.NextUTCDateTime = nextUTCDateTime;
                dynaFlowTypeSchedule.PacID = pacID;
                dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.AddAsync(dynaFlowTypeSchedule);

                iOut = dynaFlowTypeSchedule.DynaFlowTypeScheduleID;
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
        public override void DynaFlowTypeScheduleUpdate(
            SessionContext context,
            int dynaFlowTypeScheduleID,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
              Guid lastChangeCode,
             System.Guid code)
        {
            string procedureName = "DynaFlowTypeScheduleUpdate";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dynaFlowTypeID,
            //Int32 frequencyInHours,
            //Boolean isActive,
            if (System.Convert.ToDateTime(lastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                dynaFlowTypeSchedule.DynaFlowTypeScheduleID = dynaFlowTypeScheduleID;
                dynaFlowTypeSchedule.Code = code;
                dynaFlowTypeSchedule.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowTypeSchedule.FrequencyInHours = frequencyInHours;
                dynaFlowTypeSchedule.IsActive = isActive;
                dynaFlowTypeSchedule.LastUTCDateTime = lastUTCDateTime;
                dynaFlowTypeSchedule.NextUTCDateTime = nextUTCDateTime;
                dynaFlowTypeSchedule.PacID = pacID;
                dynaFlowTypeSchedule.LastChangeCode = lastChangeCode;

                bool success = dynaFlowTypeScheduleManager.Update(dynaFlowTypeSchedule);
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
        public override async Task DynaFlowTypeScheduleUpdateAsync(
            SessionContext context,
            int dynaFlowTypeScheduleID,
            Int32 dynaFlowTypeID,
            Int32 frequencyInHours,
            Boolean isActive,
            DateTime lastUTCDateTime,
            DateTime nextUTCDateTime,
            Int32 pacID,
            Guid lastChangeCode,
            System.Guid code)
        {
            string procedureName = "DynaFlowTypeScheduleUpdateAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());

            bool isEncrypted = false;
            //Int32 dynaFlowTypeID,
            //Int32 frequencyInHours,
            //Boolean isActive,
            if (System.Convert.ToDateTime(lastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 lastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (System.Convert.ToDateTime(nextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                 nextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            //Int32 pacID,
            //Define the parameters
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                dynaFlowTypeSchedule.DynaFlowTypeScheduleID = dynaFlowTypeScheduleID;
                dynaFlowTypeSchedule.Code = code;
                dynaFlowTypeSchedule.DynaFlowTypeID = dynaFlowTypeID;
                dynaFlowTypeSchedule.FrequencyInHours = frequencyInHours;
                dynaFlowTypeSchedule.IsActive = isActive;
                dynaFlowTypeSchedule.LastUTCDateTime = lastUTCDateTime;
                dynaFlowTypeSchedule.NextUTCDateTime = nextUTCDateTime;
                dynaFlowTypeSchedule.PacID = pacID;
                dynaFlowTypeSchedule.LastChangeCode = lastChangeCode;

                bool success = await dynaFlowTypeScheduleManager.UpdateAsync(dynaFlowTypeSchedule);
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
        public override IDataReader SearchDynaFlowTypeSchedules(
            SessionContext context,
            bool searchByDynaFlowTypeScheduleID, int dynaFlowTypeScheduleID,
            bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
            bool searchByFrequencyInHours, Int32 frequencyInHours,
            bool searchByIsActive, Boolean isActive,
            bool searchByLastUTCDateTime, DateTime lastUTCDateTime,
            bool searchByNextUTCDateTime, DateTime nextUTCDateTime,
            bool searchByPacID, Int32 pacID,
            bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTypeSchedules";
            Log(procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                throw new System.Exception("Not implemented");
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_Search: \r\n";
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
        public override async Task<IDataReader> SearchDynaFlowTypeSchedulesAsync(
                    SessionContext context,
                    bool searchByDynaFlowTypeScheduleID, int dynaFlowTypeScheduleID,
                    bool searchByDynaFlowTypeID, Int32 dynaFlowTypeID,
                    bool searchByFrequencyInHours, Int32 frequencyInHours,
                    bool searchByIsActive, Boolean isActive,
                    bool searchByLastUTCDateTime, DateTime lastUTCDateTime,
                    bool searchByNextUTCDateTime, DateTime nextUTCDateTime,
                    bool searchByPacID, Int32 pacID,
                    bool searchByCode, System.Guid code)
        {
            string procedureName = "SearchDynaFlowTypeSchedulesAsync";
            await LogAsync(context, procedureName + "::Start");
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                throw new System.Exception("Not implemented");

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_Search: \r\n";
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
        public override IDataReader GetDynaFlowTypeScheduleList(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTypeScheduleList";
            Log(procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(dynaFlowTypeScheduleManager.GetAll());
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetList: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeScheduleListAsync(
            SessionContext context)
        {
            string procedureName = "GetDynaFlowTypeScheduleListAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;

            EF.FarmDbContext dbContext = null;

            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTypeScheduleManager.GetAllAsync());
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetList: \r\n";
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
        public override Guid GetDynaFlowTypeScheduleCode(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDynaFlowTypeScheduleCode";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTypeSchedule::" + dynaFlowTypeScheduleID.ToString() + "::code";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.GetById(dynaFlowTypeScheduleID);

                result = dynaFlowTypeSchedule.Code.Value;

                FS.Common.Caches.StringCache.SetData(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetCode: \r\n";
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
        public override async Task<Guid> GetDynaFlowTypeScheduleCodeAsync(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDynaFlowTypeScheduleCodeAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            Guid result = Guid.Empty;
            SqlDataReader rdr = null;
            string cacheKey = "DynaFlowTypeSchedule::" + dynaFlowTypeScheduleID.ToString() + "::code";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.GetByIdAsync(dynaFlowTypeScheduleID);

                result = dynaFlowTypeSchedule.Code.Value;

                await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, result.ToString(), DateTime.Now.AddHours(1));
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetCode: \r\n";
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
        public override IDataReader GetDynaFlowTypeSchedule(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDynaFlowTypeSchedule";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.GetById(dynaFlowTypeScheduleID);

                if(dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_Get: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeScheduleAsync(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDynaFlowTypeScheduleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.GetByIdAsync(dynaFlowTypeScheduleID);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_Get: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTypeSchedule(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDirtyDynaFlowTypeSchedule";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.DirtyGetById(dynaFlowTypeScheduleID);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_DirtyGet: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTypeScheduleAsync(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "GetDirtyDynaFlowTypeScheduleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.DirtyGetByIdAsync(dynaFlowTypeScheduleID);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_DirtyGet: \r\n";
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
        public override IDataReader GetDynaFlowTypeSchedule(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeSchedule";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.GetByCode(code);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetByCode: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeScheduleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeScheduleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.GetByCodeAsync(code);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetByCode: \r\n";
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
        public override IDataReader GetDirtyDynaFlowTypeSchedule(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTypeSchedule";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.DirtyGetByCode(code);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_DirtyGetByCode: \r\n";
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
        public override async Task<IDataReader> GetDirtyDynaFlowTypeScheduleAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDirtyDynaFlowTypeScheduleAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.DirtyGetByCodeAsync(code);

                if (dynaFlowTypeSchedule != null)
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);

                rdr = BuildDataReader(dynaFlowTypeSchedules);

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_DirtyGetByCode: \r\n";
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
        public override int GetDynaFlowTypeScheduleID(
            SessionContext context,
            System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeScheduleID";
            Log(procedureName + "::Start");
            Log(procedureName + "::code::" + code.ToString());
            int result = 0;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                var dynaFlowTypeSchedule = dynaFlowTypeScheduleManager.GetByCode(code);

                result = dynaFlowTypeSchedule.DynaFlowTypeScheduleID;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetID: \r\n";
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
        public override async Task<int> GetDynaFlowTypeScheduleIDAsync(
           SessionContext context,
           System.Guid code)
        {
            string procedureName = "GetDynaFlowTypeScheduleIDAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::code::" + code.ToString());
            int result = 0;
            SqlDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                var dynaFlowTypeSchedule = await dynaFlowTypeScheduleManager.GetByCodeAsync(code);

                result = dynaFlowTypeSchedule.DynaFlowTypeScheduleID;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_GetID: \r\n";
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
        public override int DynaFlowTypeScheduleBulkInsertList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkInsertList";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for(int i = 0;i < dataList.Count;i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    bool isEncrypted = false;
                    //Int32 dynaFlowTypeID,
                    //Int32 frequencyInHours,
                    //Boolean isActive,
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.LastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.NextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                dynaFlowTypeScheduleManager.BulkInsert(dynaFlowTypeSchedules);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeScheduleBulkInsertListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkInsertListAsync";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID > 0 ||
                        dataList[i].Code.ToString() == "00000000-0000-0000-0000-000000000000")
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    bool isEncrypted = false;
                    //Int32 dynaFlowTypeID,
                    //Int32 frequencyInHours,
                    //Boolean isActive,
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.LastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.NextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                await dynaFlowTypeScheduleManager.BulkInsertAsync(dynaFlowTypeSchedules);
                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkInsert_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTypeScheduleBulkUpdateList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkUpdateList";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.DynaFlowTypeScheduleID = item.DynaFlowTypeScheduleID;
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    dynaFlowTypeSchedule.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 dynaFlowTypeID,
                    //Int32 frequencyInHours,
                    //Boolean isActive,
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.LastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.NextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,

                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                dynaFlowTypeScheduleManager.BulkUpdate(dynaFlowTypeSchedules);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeScheduleBulkUpdateListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkUpdateListAsync";
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

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.DynaFlowTypeScheduleID = item.DynaFlowTypeScheduleID;
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    dynaFlowTypeSchedule.LastChangeCode = item.LastChangeCode;

                    bool isEncrypted = false;
                    //Int32 dynaFlowTypeID,
                    //Int32 frequencyInHours,
                    //Boolean isActive,
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.LastUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.LastUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    if (System.Convert.ToDateTime(dynaFlowTypeSchedule.NextUTCDateTime) < (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                    {
                        dynaFlowTypeSchedule.NextUTCDateTime = (System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    //Int32 pacID,
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                dynaFlowTypeScheduleManager.BulkUpdate(dynaFlowTypeSchedules);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkUpdate_v19: " + x.Message + " \r\n";
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
        public override int DynaFlowTypeScheduleBulkDeleteList(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkDeleteList";
            Log(procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.DynaFlowTypeScheduleID = item.DynaFlowTypeScheduleID;
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    dynaFlowTypeSchedule.LastChangeCode = item.LastChangeCode;
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                dynaFlowTypeScheduleManager.BulkDelete(dynaFlowTypeSchedules);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override async Task<int> DynaFlowTypeScheduleBulkDeleteListAsync(
            SessionContext context,
            List<FS.Farm.Objects.DynaFlowTypeSchedule> dataList)
        {
            string procedureName = "DynaFlowTypeScheduleBulkDeleteListAsync";
            await LogAsync(context, procedureName + "::Start");
            int bulkCount = 0;
            if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                List<EF.Models.DynaFlowTypeSchedule> dynaFlowTypeSchedules = new List<EF.Models.DynaFlowTypeSchedule>();

                int actionCount = 0;

                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].DynaFlowTypeScheduleID == 0)
                        continue;

                    actionCount++;

                    Objects.DynaFlowTypeSchedule item = dataList[i];

                    EF.Models.DynaFlowTypeSchedule dynaFlowTypeSchedule = new EF.Models.DynaFlowTypeSchedule();
                    dynaFlowTypeSchedule.DynaFlowTypeScheduleID = item.DynaFlowTypeScheduleID;
                    dynaFlowTypeSchedule.Code = item.Code;
                    dynaFlowTypeSchedule.DynaFlowTypeID = item.DynaFlowTypeID;
                    dynaFlowTypeSchedule.FrequencyInHours = item.FrequencyInHours;
                    dynaFlowTypeSchedule.IsActive = item.IsActive;
                    dynaFlowTypeSchedule.LastUTCDateTime = item.LastUTCDateTime;
                    dynaFlowTypeSchedule.NextUTCDateTime = item.NextUTCDateTime;
                    dynaFlowTypeSchedule.PacID = item.PacID;
                    dynaFlowTypeSchedule.LastChangeCode = item.LastChangeCode;
                    dynaFlowTypeSchedules.Add(dynaFlowTypeSchedule);
                }

                await dynaFlowTypeScheduleManager.BulkDeleteAsync(dynaFlowTypeSchedules);

                bulkCount = actionCount;
            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Bulk Inserting Into FS_Farm_DynaFlowTypeSchedule_BulkDelete_v19: " + x.Message + " \r\n";
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
        public override void DynaFlowTypeScheduleDelete(
            SessionContext context,
            int dynaFlowTypeScheduleID)
        {
            string procedureName = "DynaFlowTypeScheduleDelete";
            Log(procedureName + "::Start");
            Log(procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                dynaFlowTypeScheduleManager.Delete(dynaFlowTypeScheduleID);

            }
            catch (Exception x)
            {
                HandleError( x, "FS_Farm_DynaFlowTypeSchedule_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            Log(procedureName + "::End");
        }
        public override async Task DynaFlowTypeScheduleDeleteAsync(
           SessionContext context,
           int dynaFlowTypeScheduleID)
        {
            string procedureName = "DynaFlowTypeScheduleDeleteAsync";
            await LogAsync(context, procedureName + "::Start");
            await LogAsync(context, procedureName + "::dynaFlowTypeScheduleID::" + dynaFlowTypeScheduleID.ToString());
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                await dynaFlowTypeScheduleManager.DeleteAsync(dynaFlowTypeScheduleID);

            }
            catch (Exception x)
            {
                await HandleErrorAsync(context, x, "FS_Farm_DynaFlowTypeSchedule_Delete");
            }
            finally
            {
                if (dbContext != null)
                    dbContext.Dispose();
            }
            await LogAsync(context, procedureName + "::End");
        }
        public override void DynaFlowTypeScheduleCleanupTesting(
            SessionContext context )
        {
            string procedureName = "DynaFlowTypeScheduleCleanupTesting";
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
        public override void DynaFlowTypeScheduleCleanupChildObjectTesting(
            SessionContext context)
        {
            string procedureName = "DynaFlowTypeScheduleCleanupChildObjectTesting";
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
        public override IDataReader GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeID(
            int dynaFlowTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(dynaFlowTypeScheduleManager.GetByDynaFlowTypeID(dynaFlowTypeID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_FetchByDynaFlowTypeID: \r\n";
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
        public override IDataReader GetDynaFlowTypeScheduleList_FetchByPacID(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeScheduleList_FetchByPacID";
            Log(procedureName + "::Start");
            EF.FarmDbContext dbContext = null;
            IDataReader rdr = null;
            try
            {
                dbContext = BuildDbContext(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(dynaFlowTypeScheduleManager.GetByPacID(pacID));

            }
            catch (Exception x)
            {
                Log(x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_FetchByPacID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeIDAsync(
            int dynaFlowTypeID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeScheduleList_FetchByDynaFlowTypeIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTypeScheduleManager.GetByDynaFlowTypeIDAsync(dynaFlowTypeID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_FetchByDynaFlowTypeID: \r\n";
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
        public override async Task<IDataReader> GetDynaFlowTypeScheduleList_FetchByPacIDAsync(
            int pacID,
           SessionContext context
            )
        {
            string procedureName = "GetDynaFlowTypeScheduleList_FetchByPacIDAsync";
            await LogAsync(context, procedureName + "::Start");
            IDataReader rdr = null;
            EF.FarmDbContext dbContext = null;
            try
            {
                dbContext = await BuildDbContextAsync(context);

                var dynaFlowTypeScheduleManager = new EF.Managers.DynaFlowTypeScheduleManager(dbContext);

                rdr = BuildDataReader(await dynaFlowTypeScheduleManager.GetByPacIDAsync(pacID));

            }
            catch (Exception x)
            {
                await LogAsync(context, x);
                string sException = "Error Executing FS_Farm_DynaFlowTypeSchedule_FetchByPacID: \r\n";
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
        private IDataReader BuildDataReader(List<EF.Models.DynaFlowTypeSchedule> data)
        {
            var dataTable = new DataTable();

            // Using reflection to create columns based on obj properties
            foreach (var prop in typeof(EF.Models.DynaFlowTypeSchedule).GetProperties())
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
                foreach (var prop in typeof(EF.Models.DynaFlowTypeSchedule).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable.CreateDataReader();

        }

    }
}
