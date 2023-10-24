using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class PacManager
	{
		private readonly FarmDbContext _dbContext;
		public PacManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Pac> AddAsync(Pac pac)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.PacSet.Add(pac);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(pac).State = EntityState.Detached;
					await transaction.CommitAsync();
					return pac;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.PacSet.Add(pac);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(pac).State = EntityState.Detached;
				return pac;
			}
		}
        public Pac Add(Pac pac)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.PacSet.Add(pac);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(pac).State = EntityState.Detached;
                    transaction.Commit();
                    return pac;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.PacSet.Add(pac);
                _dbContext.SaveChanges();
                _dbContext.Entry(pac).State = EntityState.Detached;
                return pac;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.PacSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.PacSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.PacSet.AsNoTracking().MaxAsync(x => (int?)x.PacID);
        }
        public int? GetMaxId()
        {
            return _dbContext.PacSet.AsNoTracking().Max(x => (int?)x.PacID);
        }
        public async Task<Pac> GetByIdAsync(int id)
		{
			var pacsWithCodes = await BuildQuery()
									.Where(x => x.PacObj.PacID == id)
									.ToListAsync();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            if (finalPacs.Count > 0)
			{
				return finalPacs[0];
            }
			return null;
        }
        public Pac GetById(int id)
        {
            var pacsWithCodes = BuildQuery()
                                    .Where(x => x.PacObj.PacID == id)
                                    .ToList();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            if (finalPacs.Count > 0)
            {
                return finalPacs[0];
            }
            return null;
        }
        public async Task<Pac> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.PacSet.AsNoTracking().FirstOrDefaultAsync(x => x.PacID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var pacsWithCodes = await BuildQuery()
                                        .Where(x => x.PacObj.PacID == id)
                                        .ToListAsync();
                List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalPacs.Count > 0)
                {
                    return finalPacs[0];
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
        public Pac DirtyGetById(int id) //to test
        {
            //return await _dbContext.PacSet.AsNoTracking().FirstOrDefaultAsync(x => x.PacID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var pacsWithCodes = BuildQuery()
                                        .Where(x => x.PacObj.PacID == id)
                                        .ToList();
                List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalPacs.Count > 0)
                {
                    return finalPacs[0];
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
        public async Task<Pac> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var pacsWithCodes = await BuildQuery()
                                    .Where(x => x.PacObj.Code == code)
                                    .ToListAsync();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            if (finalPacs.Count > 0)
            {
                return finalPacs[0];
            }
            return null;
        }
        public Pac GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var pacsWithCodes = BuildQuery()
                                    .Where(x => x.PacObj.Code == code)
                                    .ToList();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            if (finalPacs.Count > 0)
            {
                return finalPacs[0];
            }
            return null;
        }
        public async Task<Pac> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var pacsWithCodes = await BuildQuery()
                                        .Where(x => x.PacObj.Code == code)
                                        .ToListAsync();
                List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalPacs.Count > 0)
                {
                    return finalPacs[0];
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
        public Pac DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var pacsWithCodes = BuildQuery()
                                        .Where(x => x.PacObj.Code == code)
                                        .ToList();
                List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalPacs.Count > 0)
                {
                    return finalPacs[0];
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
        public async Task<IEnumerable<Pac>> GetAllAsync()
		{
            var pacsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            return finalPacs;
        }
        public IEnumerable<Pac> GetAll()
        {
            var pacsWithCodes = BuildQuery()
                                    .ToList();
            List<Pac> finalPacs = ProcessMappings(pacsWithCodes);
            return finalPacs;
        }
        public async Task<bool> UpdateAsync(Pac pacToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.PacSet.Attach(pacToUpdate);
					_dbContext.Entry(pacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					pacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(pacToUpdate).State = EntityState.Detached;
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
					_dbContext.PacSet.Attach(pacToUpdate);
					_dbContext.Entry(pacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					pacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(pacToUpdate).State = EntityState.Detached;
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}
        public bool Update(Pac pacToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.PacSet.Attach(pacToUpdate);
                    _dbContext.Entry(pacToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    pacToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(pacToUpdate).State = EntityState.Detached;
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
                    _dbContext.PacSet.Attach(pacToUpdate);
                    _dbContext.Entry(pacToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    pacToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(pacToUpdate).State = EntityState.Detached;
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
					var pac = await _dbContext.PacSet.FirstOrDefaultAsync(x => x.PacID == id);
					if (pac == null) return false;
					_dbContext.PacSet.Remove(pac);
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
					var pac = await _dbContext.PacSet.FirstOrDefaultAsync(x => x.PacID == id);
					if (pac == null) return false;
					_dbContext.PacSet.Remove(pac);
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
                    var pac = _dbContext.PacSet.FirstOrDefault(x => x.PacID == id);
                    if (pac == null) return false;
                    _dbContext.PacSet.Remove(pac);
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
                    var pac = _dbContext.PacSet.FirstOrDefault(x => x.PacID == id);
                    if (pac == null) return false;
                    _dbContext.PacSet.Remove(pac);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task BulkInsertAsync(IEnumerable<Pac> pacs)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var pac in pacs)
			{
				pac.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(pac);
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
					await _dbContext.BulkInsertAsync(pacs, bulkConfig);
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
				await _dbContext.BulkInsertAsync(pacs, bulkConfig);
			}
		}
        public void BulkInsert(IEnumerable<Pac> pacs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var pac in pacs)
            {
                pac.LastChangeCode = Guid.NewGuid();
                var entry = _dbContext.Entry(pac);
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
                    _dbContext.BulkInsert(pacs, bulkConfig);
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
                _dbContext.BulkInsert(pacs, bulkConfig);
            }
        }
        public async Task BulkUpdateAsync(IEnumerable<Pac> updatedPacs)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedPacs.Select(x => x.PacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.PacSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.PacID))
			//	.Select(p => new { p.PacID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.PacID, x => x.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedPac in updatedPacs)
			{
				//	if (!existingTokens.TryGetValue(updatedPac.PacID, out var token) || token != updatedPac.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedPac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.PacSet.Attach(updatedPac);
				//	_dbContext.Entry(updatedPac).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedPac);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.PacSet.Attach(updatedPac);
				//_dbContext.Entry(updatedPac).State = EntityState.Modified;
				//_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedPac.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(pacToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedPacs, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedPacs, bulkConfig);
			}
		}
        public void BulkUpdate(IEnumerable<Pac> updatedPacs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedPacs.Select(x => x.PacID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.PacSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.PacID))
            //	.Select(p => new { p.PacID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.PacID, x => x.LastChangeCode);
            //// Check concurrency conflicts
            foreach (var updatedPac in updatedPacs)
            {
                //	if (!existingTokens.TryGetValue(updatedPac.PacID, out var token) || token != updatedPac.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedPac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
                //	_dbContext.PacSet.Attach(updatedPac);
                //	_dbContext.Entry(updatedPac).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedPac);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                //_dbContext.PacSet.Attach(updatedPac);
                //_dbContext.Entry(updatedPac).State = EntityState.Modified;
                //_dbContext.Entry(pacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedPac.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(pacToUpdate).State = EntityState.Detached;
            }
            //TODO concurrency token check
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedPacs, bulkConfig);
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
                _dbContext.BulkUpdate(updatedPacs, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Pac> pacsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = pacsToDelete.Select(x => x.PacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.PacSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.PacID))
				.Select(p => new { p.PacID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.PacID, x => x.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedPac in pacsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedPac.PacID, out var token) || token != updatedPac.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedPac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(pacsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(pacsToDelete, bulkConfig);
			}
		}
        public void BulkDelete(IEnumerable<Pac> pacsToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Pac.PacID), nameof(Pac.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = pacsToDelete.Select(x => x.PacID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.PacSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.PacID))
                .Select(p => new { p.PacID, p.LastChangeCode })
                .ToDictionary(x => x.PacID, x => x.LastChangeCode);
            // Check concurrency conflicts
            foreach (var updatedPac in pacsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedPac.PacID, out var token) || token != updatedPac.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedPac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(pacsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(pacsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from pac in _dbContext.PacSet.AsNoTracking()
				   select new QueryDTO
                   {
					   PacObj = pac,
				   };
        }
		private List<Pac> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
            }
            List<Pac> results = data.Select(r => r.PacObj).ToList();
            return results;
        } 
        private class QueryDTO
        {
            public Pac PacObj { get; set; }
        }
    }
}