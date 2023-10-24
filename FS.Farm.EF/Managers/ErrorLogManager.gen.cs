using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class ErrorLogManager
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
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.ErrorLogSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.ErrorLogSet.AsNoTracking().MaxAsync(p => (int?)p.ErrorLogID);
		}
		public async Task<ErrorLog> GetByIdAsync(int id)
		{
			var errorLogsWithCodes = await BuildQuery()
									.Where(p => p.ErrorLogObj.ErrorLogID == id)
									.ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            if (finalErrorLogs.Count > 0)
			{
				return finalErrorLogs[0];
            }
			return null;
        }
		public async Task<ErrorLog> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.ErrorLogSet.AsNoTracking().FirstOrDefaultAsync(p => p.ErrorLogID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var errorLogsWithCodes = await BuildQuery()
                                        .Where(p => p.ErrorLogObj.ErrorLogID == id)
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
		public async Task<ErrorLog> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var errorLogsWithCodes = await BuildQuery()
                                    .Where(p => p.ErrorLogObj.Code == code)
                                    .ToListAsync();
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
                                        .Where(p => p.ErrorLogObj.Code == code)
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
		public async Task<IEnumerable<ErrorLog>> GetAllAsync()
		{
            var errorLogsWithCodes = await BuildQuery()
                                    .ToListAsync();
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
		public async Task<bool> DeleteAsync(int id)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					var errorLog = await _dbContext.ErrorLogSet.FirstOrDefaultAsync(p => p.ErrorLogID == id);
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
					var errorLog = await _dbContext.ErrorLogSet.FirstOrDefaultAsync(p => p.ErrorLogID == id);
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
					entry.Property("InsertUtcDateTime").CurrentValue = DateTime.UtcNow;
					entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
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
			var idsToUpdate = updatedErrorLogs.Select(p => p.ErrorLogID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.ErrorLogSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.ErrorLogID))
			//	.Select(p => new { p.ErrorLogID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.ErrorLogID, p => p.LastChangeCode);
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
			var idsToUpdate = errorLogsToDelete.Select(p => p.ErrorLogID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.ErrorLogSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.ErrorLogID))
				.Select(p => new { p.ErrorLogID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.ErrorLogID, p => p.LastChangeCode);
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
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from errorLog in _dbContext.ErrorLogSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == errorLog.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   ErrorLogObj = errorLog,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<ErrorLog> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.ErrorLogObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<ErrorLog> results = data.Select(r => r.ErrorLogObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<ErrorLog>> GetByPacAsync(int id)
        {
            var errorLogsWithCodes = await BuildQuery()
                                    .Where(p => p.ErrorLogObj.PacID == id)
                                    .ToListAsync();
            List<ErrorLog> finalErrorLogs = ProcessMappings(errorLogsWithCodes);
            return finalErrorLogs;
        }
		//ENDSET
        private class QueryDTO
        {
            public ErrorLog ErrorLogObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
