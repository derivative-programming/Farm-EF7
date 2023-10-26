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
        public TriStateFilter Add(TriStateFilter triStateFilter)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.TriStateFilterSet.Add(triStateFilter);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(triStateFilter).State = EntityState.Detached;
                    transaction.Commit();
                    return triStateFilter;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.TriStateFilterSet.Add(triStateFilter);
                _dbContext.SaveChanges();
                _dbContext.Entry(triStateFilter).State = EntityState.Detached;
                return triStateFilter;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.TriStateFilterSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.TriStateFilterSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.TriStateFilterSet.AsNoTracking().MaxAsync(x => (int?)x.TriStateFilterID);
        }
        public int? GetMaxId()
        {
            return _dbContext.TriStateFilterSet.AsNoTracking().Max(x => (int?)x.TriStateFilterID);
        }
        public async Task<TriStateFilter> GetByIdAsync(int id)
		{
			var triStateFiltersWithCodes = await BuildQuery()
									.Where(x => x.TriStateFilterObj.TriStateFilterID == id)
									.ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            if (finalTriStateFilters.Count > 0)
			{
				return finalTriStateFilters[0];
            }
			return null;
        }
        public TriStateFilter GetById(int id)
        {
            var triStateFiltersWithCodes = BuildQuery()
                                    .Where(x => x.TriStateFilterObj.TriStateFilterID == id)
                                    .ToList();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            if (finalTriStateFilters.Count > 0)
            {
                return finalTriStateFilters[0];
            }
            return null;
        }
        public async Task<TriStateFilter> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.TriStateFilterSet.AsNoTracking().FirstOrDefaultAsync(x => x.TriStateFilterID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var triStateFiltersWithCodes = await BuildQuery()
                                        .Where(x => x.TriStateFilterObj.TriStateFilterID == id)
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
        public TriStateFilter DirtyGetById(int id) //to test
        {
            //return await _dbContext.TriStateFilterSet.AsNoTracking().FirstOrDefaultAsync(x => x.TriStateFilterID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var triStateFiltersWithCodes = BuildQuery()
                                        .Where(x => x.TriStateFilterObj.TriStateFilterID == id)
                                        .ToList();
                List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalTriStateFilters.Count > 0)
                {
                    return finalTriStateFilters[0];
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
        public async Task<TriStateFilter> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var triStateFiltersWithCodes = await BuildQuery()
                                    .Where(x => x.TriStateFilterObj.Code == code)
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            if (finalTriStateFilters.Count > 0)
            {
                return finalTriStateFilters[0];
            }
            return null;
        }
        public TriStateFilter GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var triStateFiltersWithCodes = BuildQuery()
                                    .Where(x => x.TriStateFilterObj.Code == code)
                                    .ToList();
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
                                        .Where(x => x.TriStateFilterObj.Code == code)
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
        public TriStateFilter DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var triStateFiltersWithCodes = BuildQuery()
                                        .Where(x => x.TriStateFilterObj.Code == code)
                                        .ToList();
                List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalTriStateFilters.Count > 0)
                {
                    return finalTriStateFilters[0];
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
        public async Task<List<TriStateFilter>> GetAllAsync()
		{
            var triStateFiltersWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            return finalTriStateFilters;
        }
        public List<TriStateFilter> GetAll()
        {
            var triStateFiltersWithCodes = BuildQuery()
                                    .ToList();
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
        public bool Update(TriStateFilter triStateFilterToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.TriStateFilterSet.Attach(triStateFilterToUpdate);
                    _dbContext.Entry(triStateFilterToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(triStateFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    triStateFilterToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(triStateFilterToUpdate).State = EntityState.Detached;
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
                    _dbContext.TriStateFilterSet.Attach(triStateFilterToUpdate);
                    _dbContext.Entry(triStateFilterToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(triStateFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    triStateFilterToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
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
					var triStateFilter = await _dbContext.TriStateFilterSet.FirstOrDefaultAsync(x => x.TriStateFilterID == id);
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
					var triStateFilter = await _dbContext.TriStateFilterSet.FirstOrDefaultAsync(x => x.TriStateFilterID == id);
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
        public bool Delete(int id)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    var triStateFilter = _dbContext.TriStateFilterSet.FirstOrDefault(x => x.TriStateFilterID == id);
                    if (triStateFilter == null) return false;
                    _dbContext.TriStateFilterSet.Remove(triStateFilter);
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
                    var triStateFilter = _dbContext.TriStateFilterSet.FirstOrDefault(x => x.TriStateFilterID == id);
                    if (triStateFilter == null) return false;
                    _dbContext.TriStateFilterSet.Remove(triStateFilter);
                    _dbContext.SaveChanges();
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
        public void BulkInsert(IEnumerable<TriStateFilter> triStateFilters)
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
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkInsert(triStateFilters, bulkConfig);
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
                _dbContext.BulkInsert(triStateFilters, bulkConfig);
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
			var idsToUpdate = updatedTriStateFilters.Select(x => x.TriStateFilterID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.TriStateFilterSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.TriStateFilterID))
			//	.Select(p => new { p.TriStateFilterID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.TriStateFilterID, x => x.LastChangeCode);
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
        public void BulkUpdate(IEnumerable<TriStateFilter> updatedTriStateFilters)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(TriStateFilter.TriStateFilterID), nameof(TriStateFilter.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedTriStateFilters.Select(x => x.TriStateFilterID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.TriStateFilterSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.TriStateFilterID))
            //	.Select(p => new { p.TriStateFilterID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.TriStateFilterID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedTriStateFilters, bulkConfig);
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
                _dbContext.BulkUpdate(updatedTriStateFilters, bulkConfig);
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
			var idsToUpdate = triStateFiltersToDelete.Select(x => x.TriStateFilterID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.TriStateFilterSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.TriStateFilterID))
				.Select(p => new { p.TriStateFilterID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.TriStateFilterID, x => x.LastChangeCode);
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
        public void BulkDelete(IEnumerable<TriStateFilter> triStateFiltersToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(TriStateFilter.TriStateFilterID), nameof(TriStateFilter.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = triStateFiltersToDelete.Select(x => x.TriStateFilterID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.TriStateFilterSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.TriStateFilterID))
                .Select(p => new { p.TriStateFilterID, p.LastChangeCode })
                .ToDictionary(x => x.TriStateFilterID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(triStateFiltersToDelete, bulkConfig);
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
                _dbContext.BulkDelete(triStateFiltersToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from triStateFilter in _dbContext.TriStateFilterSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == triStateFilter.PacID).DefaultIfEmpty() //PacID
				   select new QueryDTO
                   {
					   TriStateFilterObj = triStateFilter,
					   PacCode = pac.Code, //PacID
				   };
        }
        public int ClearTestObjects()
        {
            int delCount = 0;
            bool found = false;
            try
            {
                while (found)
                {
                    found = false;
                    var triStateFilter = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (triStateFilter != null)
                    {
                        found = true;
                        Delete(triStateFilter.TriStateFilterID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var triStateFilter = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (triStateFilter != null)
                    {
                        found = true;
                        Delete(triStateFilter.TriStateFilterID);
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
            bool found = false;
            try
            {
                while (found)
                {
                    found = false;
                    var triStateFilter = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (triStateFilter != null)
                    {
                        found = true;
                        Delete(triStateFilter.TriStateFilterID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<TriStateFilter> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.TriStateFilterObj.PacCodePeek = item.PacCode.Value; //PacID
            }
            List<TriStateFilter> results = data.Select(r => r.TriStateFilterObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<TriStateFilter>> GetByPacAsync(int id)
        {
            var triStateFiltersWithCodes = await BuildQuery()
                                    .Where(x => x.TriStateFilterObj.PacID == id)
                                    .ToListAsync();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            return finalTriStateFilters;
        }
        //PacID
        public List<TriStateFilter> GetByPac(int id)
        {
            var triStateFiltersWithCodes = BuildQuery()
                                    .Where(x => x.TriStateFilterObj.PacID == id)
                                    .ToList();
            List<TriStateFilter> finalTriStateFilters = ProcessMappings(triStateFiltersWithCodes);
            return finalTriStateFilters;
        }
        private class QueryDTO
        {
            public TriStateFilter TriStateFilterObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
