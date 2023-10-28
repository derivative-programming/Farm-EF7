using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
namespace FS.Farm.EF.Managers
{
	public partial class ErrorLogManager
	{
		private readonly FarmDbContext _dbContext;
		public ErrorLogManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<ErrorLog> AddAsync(ErrorLog errorLog)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.ErrorLogSet.Add(errorLog);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(errorLog).State = EntityState.Detached;
					await transaction.CommitAsync();
					return errorLog;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.ErrorLogSet.Add(errorLog);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(errorLog).State = EntityState.Detached;
				return errorLog;
			}
		}
        public ErrorLog Add(ErrorLog errorLog)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.ErrorLogSet.Add(errorLog);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(errorLog).State = EntityState.Detached;
                    transaction.Commit();
                    return errorLog;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.ErrorLogSet.Add(errorLog);
                _dbContext.SaveChanges();
                _dbContext.Entry(errorLog).State = EntityState.Detached;
                return errorLog;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.ErrorLogSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.ErrorLogSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.ErrorLogSet.AsNoTracking().MaxAsync(x => (int?)x.ErrorLogID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }
        public int? GetMaxId()
        {
            int? maxId = _dbContext.ErrorLogSet.AsNoTracking().Max(x => (int?)x.ErrorLogID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }
        public async Task<ErrorLog> GetByIdAsync(int id)
		{
			var errorLogsWithCodes = await BuildQuery()
									.Where(x => x.ErrorLogObj.ErrorLogID == id)
									.ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            if (finalErrorLogs.Count > 0)
			{
				return finalErrorLogs[0];
            }
			return null;
        }
        public ErrorLog GetById(int id)
        {
            var errorLogsWithCodes = BuildQuery()
                                    .Where(x => x.ErrorLogObj.ErrorLogID == id)
                                    .ToList();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            if (finalErrorLogs.Count > 0)
            {
                return finalErrorLogs[0];
            }
            return null;
        }
        public async Task<ErrorLog> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.ErrorLogSet.AsNoTracking().FirstOrDefaultAsync(x => x.ErrorLogID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var errorLogsWithCodes = await BuildQuery()
                                        .Where(x => x.ErrorLogObj.ErrorLogID == id)
                                        .ToListAsync();
                List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalErrorLogs.Count > 0)
                {
                    return finalErrorLogs[0];
                }
                return null;
            }
			catch
			{
				// Rollback the transaction in case of any exceptions
				await transaction.RollbackAsync();
				throw; // Re-throw the exception
			}
		}
        public ErrorLog DirtyGetById(int id) //to test
        {
            //return await _dbContext.ErrorLogSet.AsNoTracking().FirstOrDefaultAsync(x => x.ErrorLogID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var errorLogsWithCodes = BuildQuery()
                                        .Where(x => x.ErrorLogObj.ErrorLogID == id)
                                        .ToList();
                List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalErrorLogs.Count > 0)
                {
                    return finalErrorLogs[0];
                }
                return null;
            }
            catch
            {
                // Rollback the transaction in case of any exceptions
                transaction.Rollback();
                throw; // Re-throw the exception
            }
        }
        public async Task<ErrorLog> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var errorLogsWithCodes = await BuildQuery()
                                    .Where(x => x.ErrorLogObj.Code == code)
                                    .ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            if (finalErrorLogs.Count > 0)
            {
                return finalErrorLogs[0];
            }
            return null;
        }
        public ErrorLog GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var errorLogsWithCodes = BuildQuery()
                                    .Where(x => x.ErrorLogObj.Code == code)
                                    .ToList();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            if (finalErrorLogs.Count > 0)
            {
                return finalErrorLogs[0];
            }
            return null;
        }
        public async Task<ErrorLog> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var errorLogsWithCodes = await BuildQuery()
                                        .Where(x => x.ErrorLogObj.Code == code)
                                        .ToListAsync();
                List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalErrorLogs.Count > 0)
                {
                    return finalErrorLogs[0];
                }
                return null;
            }
			catch
			{
				// Rollback the transaction in case of any exceptions
				await transaction.RollbackAsync();
				throw; // Re-throw the exception
			}
		}
        public ErrorLog DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var errorLogsWithCodes = BuildQuery()
                                        .Where(x => x.ErrorLogObj.Code == code)
                                        .ToList();
                List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalErrorLogs.Count > 0)
                {
                    return finalErrorLogs[0];
                }
                return null;
            }
            catch
            {
                // Rollback the transaction in case of any exceptions
                transaction.Rollback();
                throw; // Re-throw the exception
            }
        }
        public async Task<List<ErrorLog>> GetAllAsync()
		{
            var errorLogsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            return finalErrorLogs;
        }
        public List<ErrorLog> GetAll()
        {
            var errorLogsWithCodes = BuildQuery()
                                    .ToList();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            return finalErrorLogs;
        }
        public async Task<bool> UpdateAsync(ErrorLog errorLogToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.ErrorLogSet.Attach(errorLogToUpdate);
					_dbContext.Entry(errorLogToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					errorLogToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
					await transaction.CommitAsync();
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					await transaction.RollbackAsync();
					return false;
				}
			}
			else
			{
				try
				{
					_dbContext.ErrorLogSet.Attach(errorLogToUpdate);
					_dbContext.Entry(errorLogToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					errorLogToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}
        public bool Update(ErrorLog errorLogToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.ErrorLogSet.Attach(errorLogToUpdate);
                    _dbContext.Entry(errorLogToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    errorLogToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            else
            {
                try
                {
                    _dbContext.ErrorLogSet.Attach(errorLogToUpdate);
                    _dbContext.Entry(errorLogToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    errorLogToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
            }
        }
        public async Task<bool> DeleteAsync(int id)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					var errorLog = await _dbContext.ErrorLogSet.FirstOrDefaultAsync(x => x.ErrorLogID == id);
					if (errorLog == null) return false;
					_dbContext.ErrorLogSet.Remove(errorLog);
					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();
					return true;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				try
				{
					var errorLog = await _dbContext.ErrorLogSet.FirstOrDefaultAsync(x => x.ErrorLogID == id);
					if (errorLog == null) return false;
					_dbContext.ErrorLogSet.Remove(errorLog);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
        public bool Delete(int id)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    var errorLog = _dbContext.ErrorLogSet.FirstOrDefault(x => x.ErrorLogID == id);
                    if (errorLog == null) return false;
                    _dbContext.ErrorLogSet.Remove(errorLog);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                try
                {
                    var errorLog = _dbContext.ErrorLogSet.FirstOrDefault(x => x.ErrorLogID == id);
                    if (errorLog == null) return false;
                    _dbContext.ErrorLogSet.Remove(errorLog);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task BulkInsertAsync(IEnumerable<ErrorLog> errorLogs)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var errorLog in errorLogs)
			{
				errorLog.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(errorLog);
				if (entry.State == EntityState.Added || entry.State == EntityState.Detached)
				{
					entry.Property("insert_utc_date_time").CurrentValue = DateTime.UtcNow;
					entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
				}
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkInsertAsync(errorLogs, bulkConfig);
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
			else
			{
				await _dbContext.BulkInsertAsync(errorLogs, bulkConfig);
			}
		}
        public void BulkInsert(IEnumerable<ErrorLog> errorLogs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var errorLog in errorLogs)
            {
                errorLog.LastChangeCode = Guid.NewGuid();
                var entry = _dbContext.Entry(errorLog);
                if (entry.State == EntityState.Added || entry.State == EntityState.Detached)
                {
                    entry.Property("insert_utc_date_time").CurrentValue = DateTime.UtcNow;
                    entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
                }
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkInsert(errorLogs, bulkConfig);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.BulkInsert(errorLogs, bulkConfig);
            }
        }
        public async Task BulkUpdateAsync(IEnumerable<ErrorLog> updatedErrorLogs)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedErrorLogs.Select(x => x.ErrorLogID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.ErrorLogSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.ErrorLogID))
			//	.Select(p => new { p.ErrorLogID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.ErrorLogID, x => x.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedErrorLog in updatedErrorLogs)
			{
				//	if (!existingTokens.TryGetValue(updatedErrorLog.ErrorLogID, out var token) || token != updatedErrorLog.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedErrorLog.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.ErrorLogSet.Attach(updatedErrorLog);
				//	_dbContext.Entry(updatedErrorLog).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedErrorLog);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.ErrorLogSet.Attach(updatedErrorLog);
				//_dbContext.Entry(updatedErrorLog).State = EntityState.Modified;
				//_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedErrorLog.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedErrorLogs, bulkConfig);
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
			else
			{
				// If there's an existing transaction, just perform the bulk update without managing the transaction here.
				await _dbContext.BulkUpdateAsync(updatedErrorLogs, bulkConfig);
			}
		}
        public void BulkUpdate(IEnumerable<ErrorLog> updatedErrorLogs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedErrorLogs.Select(x => x.ErrorLogID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.ErrorLogSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.ErrorLogID))
            //	.Select(p => new { p.ErrorLogID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.ErrorLogID, x => x.LastChangeCode);
            //// Check concurrency conflicts
            foreach (var updatedErrorLog in updatedErrorLogs)
            {
                //	if (!existingTokens.TryGetValue(updatedErrorLog.ErrorLogID, out var token) || token != updatedErrorLog.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedErrorLog.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
                //	_dbContext.ErrorLogSet.Attach(updatedErrorLog);
                //	_dbContext.Entry(updatedErrorLog).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedErrorLog);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                //_dbContext.ErrorLogSet.Attach(updatedErrorLog);
                //_dbContext.Entry(updatedErrorLog).State = EntityState.Modified;
                //_dbContext.Entry(errorLogToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedErrorLog.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(errorLogToUpdate).State = EntityState.Detached;
            }
            //TODO concurrency token check
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedErrorLogs, bulkConfig);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                // If there's an existing transaction, just perform the bulk update without managing the transaction here.
                _dbContext.BulkUpdate(updatedErrorLogs, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<ErrorLog> errorLogsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = errorLogsToDelete.Select(x => x.ErrorLogID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.ErrorLogSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.ErrorLogID))
				.Select(p => new { p.ErrorLogID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.ErrorLogID, x => x.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedErrorLog in errorLogsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedErrorLog.ErrorLogID, out var token) || token != updatedErrorLog.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedErrorLog.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(errorLogsToDelete, bulkConfig);
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
			else
			{
				// If there's an existing transaction, just perform the bulk update without managing the transaction here.
				await _dbContext.BulkDeleteAsync(errorLogsToDelete, bulkConfig);
			}
		}
        public void BulkDelete(IEnumerable<ErrorLog> errorLogsToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(ErrorLog.ErrorLogID), nameof(ErrorLog.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = errorLogsToDelete.Select(x => x.ErrorLogID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.ErrorLogSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.ErrorLogID))
                .Select(p => new { p.ErrorLogID, p.LastChangeCode })
                .ToDictionary(x => x.ErrorLogID, x => x.LastChangeCode);
            // Check concurrency conflicts
            foreach (var updatedErrorLog in errorLogsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedErrorLog.ErrorLogID, out var token) || token != updatedErrorLog.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedErrorLog.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(errorLogsToDelete, bulkConfig);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                // If there's an existing transaction, just perform the bulk update without managing the transaction here.
                _dbContext.BulkDelete(errorLogsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from errorLog in _dbContext.ErrorLogSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == errorLog.PacID).DefaultIfEmpty() //PacID
				   select new QueryDTO
                   {
					   ErrorLogObj = errorLog,
					   PacCode = pac.Code, //PacID
				   };
        }
        public int ClearTestObjects()
        {
            int delCount = 0;
            bool found = true;
            try
            {
                while (found)
                {
                    found = false;
                    var errorLog = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (errorLog != null)
                    {
                        found = true;
                        Delete(errorLog.ErrorLogID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var errorLog = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (errorLog != null)
                    {
                        found = true;
                        Delete(errorLog.ErrorLogID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        public int ClearTestChildObjects()
        {
            int delCount = 0;
            bool found = true;
            try
            {
                while (found)
                {
                    found = false;
                    var errorLog = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (errorLog != null)
                    {
                        found = true;
                        Delete(errorLog.ErrorLogID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<ErrorLog> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.ErrorLogObj.PacCodePeek = item.PacCode.Value; //PacID
            }
            List<ErrorLog> results = data.Select(r => r.ErrorLogObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<ErrorLog>> GetByPacIDAsync(int id)
        {
            var errorLogsWithCodes = await BuildQuery()
                                    .Where(x => x.ErrorLogObj.PacID == id)
                                    .ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            return finalErrorLogs;
        }
        //PacID
        public List<ErrorLog> GetByPacID(int id)
        {
            var errorLogsWithCodes = BuildQuery()
                                    .Where(x => x.ErrorLogObj.PacID == id)
                                    .ToList();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            return finalErrorLogs;
        }
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        private class QueryDTO
        {
            public ErrorLog ErrorLogObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
