using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class TriStateFilterManager
	{
		private readonly FarmDbContext _dbContext;
		public TriStateFilterManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<TriStateFilter> AddAsync(TriStateFilter triStateFilter)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TriStateFilterSet.Add(triStateFilter);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(triStateFilter).State = EntityState.Detached;
					await transaction.CommitAsync();
					return triStateFilter;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.TriStateFilterSet.Add(triStateFilter);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(triStateFilter).State = EntityState.Detached;
				return triStateFilter;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.TriStateFilterSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.TriStateFilterSet.AsNoTracking().MaxAsync(p => (int?)p.TriStateFilterID);
		}
		public async Task<TriStateFilter> GetByIdAsync(int id)
		{
			var triStateFiltersWithCodes = await BuildQuery()
									.Where(p => p.TriStateFilterObj.TriStateFilterID == id)
									.ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            if (finalTriStateFilters.Count > 0)
			{
				return finalTriStateFilters[0];
            }
			return null;
        }
		public async Task<TriStateFilter> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.TriStateFilterSet.AsNoTracking().FirstOrDefaultAsync(p => p.TriStateFilterID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var triStateFiltersWithCodes = await BuildQuery()
                                        .Where(p => p.TriStateFilterObj.TriStateFilterID == id)
                                        .ToListAsync();
                List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTriStateFilters.Count > 0)
                {
                    return finalTriStateFilters[0];
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
		public async Task<TriStateFilter> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var triStateFiltersWithCodes = await BuildQuery()
                                    .Where(p => p.TriStateFilterObj.Code == code)
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            if (finalTriStateFilters.Count > 0)
            {
                return finalTriStateFilters[0];
            }
            return null;
        }
		public async Task<TriStateFilter> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var triStateFiltersWithCodes = await BuildQuery()
                                        .Where(p => p.TriStateFilterObj.Code == code)
                                        .ToListAsync();
                List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTriStateFilters.Count > 0)
                {
                    return finalTriStateFilters[0];
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
		public async Task<IEnumerable<TriStateFilter>> GetAllAsync()
		{
            var triStateFiltersWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            return finalTriStateFilters;
        }
		public async Task<bool> UpdateAsync(TriStateFilter triStateFilterToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TriStateFilterSet.Attach(triStateFilterToUpdate);
					_dbContext.Entry(triStateFilterToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(triStateFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					triStateFilterToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(triStateFilterToUpdate).State = EntityState.Detached;
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
					_dbContext.TriStateFilterSet.Attach(triStateFilterToUpdate);
					_dbContext.Entry(triStateFilterToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(triStateFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					triStateFilterToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(triStateFilterToUpdate).State = EntityState.Detached;
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
					var triStateFilter = await _dbContext.TriStateFilterSet.FirstOrDefaultAsync(p => p.TriStateFilterID == id);
					if (triStateFilter == null) return false;
					_dbContext.TriStateFilterSet.Remove(triStateFilter);
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
					var triStateFilter = await _dbContext.TriStateFilterSet.FirstOrDefaultAsync(p => p.TriStateFilterID == id);
					if (triStateFilter == null) return false;
					_dbContext.TriStateFilterSet.Remove(triStateFilter);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<TriStateFilter> triStateFilters)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(TriStateFilter.TriStateFilterID), nameof(TriStateFilter.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var triStateFilter in triStateFilters)
			{
				triStateFilter.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(triStateFilter);
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
					await _dbContext.BulkInsertAsync(triStateFilters, bulkConfig);
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
				await _dbContext.BulkInsertAsync(triStateFilters, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<TriStateFilter> updatedTriStateFilters)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(TriStateFilter.TriStateFilterID), nameof(TriStateFilter.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedTriStateFilters.Select(p => p.TriStateFilterID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.TriStateFilterSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.TriStateFilterID))
			//	.Select(p => new { p.TriStateFilterID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.TriStateFilterID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedTriStateFilter in updatedTriStateFilters)
			{
				//	if (!existingTokens.TryGetValue(updatedTriStateFilter.TriStateFilterID, out var token) || token != updatedTriStateFilter.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedTriStateFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.TriStateFilterSet.Attach(updatedTriStateFilter);
				//	_dbContext.Entry(updatedTriStateFilter).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedTriStateFilter);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.TriStateFilterSet.Attach(updatedTriStateFilter);
				//_dbContext.Entry(updatedTriStateFilter).State = EntityState.Modified;
				//_dbContext.Entry(triStateFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedTriStateFilter.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(triStateFilterToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedTriStateFilters, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedTriStateFilters, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<TriStateFilter> triStateFiltersToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(TriStateFilter.TriStateFilterID), nameof(TriStateFilter.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = triStateFiltersToDelete.Select(p => p.TriStateFilterID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.TriStateFilterSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.TriStateFilterID))
				.Select(p => new { p.TriStateFilterID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.TriStateFilterID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedTriStateFilter in triStateFiltersToDelete)
			{
				if (!existingTokens.TryGetValue(updatedTriStateFilter.TriStateFilterID, out var token) || token != updatedTriStateFilter.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedTriStateFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(triStateFiltersToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(triStateFiltersToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from triStateFilter in _dbContext.TriStateFilterSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == triStateFilter.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   TriStateFilterObj = triStateFilter,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<TriStateFilter> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.TriStateFilterObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<TriStateFilter> results = data.Select(r => r.TriStateFilterObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<TriStateFilter>> GetByPacAsync(int id)
        {
            var triStateFiltersWithCodes = await BuildQuery()
                                    .Where(p => p.TriStateFilterObj.PacID == id)
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            return finalTriStateFilters;
        }
		//ENDSET
        private class QueryDTO
        {
            public TriStateFilter TriStateFilterObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
