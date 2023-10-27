using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
namespace FS.Farm.EF.Managers
{
	public class FlavorManager
	{
		private readonly FarmDbContext _dbContext;
		public FlavorManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Flavor> AddAsync(Flavor flavor)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.FlavorSet.Add(flavor);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(flavor).State = EntityState.Detached;
					await transaction.CommitAsync();
					return flavor;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.FlavorSet.Add(flavor);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(flavor).State = EntityState.Detached;
				return flavor;
			}
		}
        public Flavor Add(Flavor flavor)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.FlavorSet.Add(flavor);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(flavor).State = EntityState.Detached;
                    transaction.Commit();
                    return flavor;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.FlavorSet.Add(flavor);
                _dbContext.SaveChanges();
                _dbContext.Entry(flavor).State = EntityState.Detached;
                return flavor;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.FlavorSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.FlavorSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.FlavorSet.AsNoTracking().MaxAsync(x => (int?)x.FlavorID);
        }
        public int? GetMaxId()
        {
            return _dbContext.FlavorSet.AsNoTracking().Max(x => (int?)x.FlavorID);
        }
        public async Task<Flavor> GetByIdAsync(int id)
		{
			var flavorsWithCodes = await BuildQuery()
									.Where(x => x.FlavorObj.FlavorID == id)
									.ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            if (finalFlavors.Count > 0)
			{
				return finalFlavors[0];
            }
			return null;
        }
        public Flavor GetById(int id)
        {
            var flavorsWithCodes = BuildQuery()
                                    .Where(x => x.FlavorObj.FlavorID == id)
                                    .ToList();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            if (finalFlavors.Count > 0)
            {
                return finalFlavors[0];
            }
            return null;
        }
        public async Task<Flavor> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.FlavorSet.AsNoTracking().FirstOrDefaultAsync(x => x.FlavorID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var flavorsWithCodes = await BuildQuery()
                                        .Where(x => x.FlavorObj.FlavorID == id)
                                        .ToListAsync();
                List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalFlavors.Count > 0)
                {
                    return finalFlavors[0];
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
        public Flavor DirtyGetById(int id) //to test
        {
            //return await _dbContext.FlavorSet.AsNoTracking().FirstOrDefaultAsync(x => x.FlavorID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var flavorsWithCodes = BuildQuery()
                                        .Where(x => x.FlavorObj.FlavorID == id)
                                        .ToList();
                List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalFlavors.Count > 0)
                {
                    return finalFlavors[0];
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
        public async Task<Flavor> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var flavorsWithCodes = await BuildQuery()
                                    .Where(x => x.FlavorObj.Code == code)
                                    .ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            if (finalFlavors.Count > 0)
            {
                return finalFlavors[0];
            }
            return null;
        }
        public Flavor GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var flavorsWithCodes = BuildQuery()
                                    .Where(x => x.FlavorObj.Code == code)
                                    .ToList();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            if (finalFlavors.Count > 0)
            {
                return finalFlavors[0];
            }
            return null;
        }
        public async Task<Flavor> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var flavorsWithCodes = await BuildQuery()
                                        .Where(x => x.FlavorObj.Code == code)
                                        .ToListAsync();
                List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalFlavors.Count > 0)
                {
                    return finalFlavors[0];
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
        public Flavor DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var flavorsWithCodes = BuildQuery()
                                        .Where(x => x.FlavorObj.Code == code)
                                        .ToList();
                List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalFlavors.Count > 0)
                {
                    return finalFlavors[0];
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
        public async Task<List<Flavor>> GetAllAsync()
		{
            var flavorsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            return finalFlavors;
        }
        public List<Flavor> GetAll()
        {
            var flavorsWithCodes = BuildQuery()
                                    .ToList();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            return finalFlavors;
        }
        public async Task<bool> UpdateAsync(Flavor flavorToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.FlavorSet.Attach(flavorToUpdate);
					_dbContext.Entry(flavorToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					flavorToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
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
					_dbContext.FlavorSet.Attach(flavorToUpdate);
					_dbContext.Entry(flavorToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					flavorToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}
        public bool Update(Flavor flavorToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.FlavorSet.Attach(flavorToUpdate);
                    _dbContext.Entry(flavorToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    flavorToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
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
                    _dbContext.FlavorSet.Attach(flavorToUpdate);
                    _dbContext.Entry(flavorToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    flavorToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
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
					var flavor = await _dbContext.FlavorSet.FirstOrDefaultAsync(x => x.FlavorID == id);
					if (flavor == null) return false;
					_dbContext.FlavorSet.Remove(flavor);
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
					var flavor = await _dbContext.FlavorSet.FirstOrDefaultAsync(x => x.FlavorID == id);
					if (flavor == null) return false;
					_dbContext.FlavorSet.Remove(flavor);
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
                    var flavor = _dbContext.FlavorSet.FirstOrDefault(x => x.FlavorID == id);
                    if (flavor == null) return false;
                    _dbContext.FlavorSet.Remove(flavor);
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
                    var flavor = _dbContext.FlavorSet.FirstOrDefault(x => x.FlavorID == id);
                    if (flavor == null) return false;
                    _dbContext.FlavorSet.Remove(flavor);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task BulkInsertAsync(IEnumerable<Flavor> flavors)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var flavor in flavors)
			{
				flavor.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(flavor);
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
					await _dbContext.BulkInsertAsync(flavors, bulkConfig);
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
				await _dbContext.BulkInsertAsync(flavors, bulkConfig);
			}
		}
        public void BulkInsert(IEnumerable<Flavor> flavors)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var flavor in flavors)
            {
                flavor.LastChangeCode = Guid.NewGuid();
                var entry = _dbContext.Entry(flavor);
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
                    _dbContext.BulkInsert(flavors, bulkConfig);
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
                _dbContext.BulkInsert(flavors, bulkConfig);
            }
        }
        public async Task BulkUpdateAsync(IEnumerable<Flavor> updatedFlavors)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedFlavors.Select(x => x.FlavorID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.FlavorSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.FlavorID))
			//	.Select(p => new { p.FlavorID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.FlavorID, x => x.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedFlavor in updatedFlavors)
			{
				//	if (!existingTokens.TryGetValue(updatedFlavor.FlavorID, out var token) || token != updatedFlavor.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedFlavor.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.FlavorSet.Attach(updatedFlavor);
				//	_dbContext.Entry(updatedFlavor).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedFlavor);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.FlavorSet.Attach(updatedFlavor);
				//_dbContext.Entry(updatedFlavor).State = EntityState.Modified;
				//_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedFlavor.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedFlavors, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedFlavors, bulkConfig);
			}
		}
        public void BulkUpdate(IEnumerable<Flavor> updatedFlavors)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedFlavors.Select(x => x.FlavorID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.FlavorSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.FlavorID))
            //	.Select(p => new { p.FlavorID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.FlavorID, x => x.LastChangeCode);
            //// Check concurrency conflicts
            foreach (var updatedFlavor in updatedFlavors)
            {
                //	if (!existingTokens.TryGetValue(updatedFlavor.FlavorID, out var token) || token != updatedFlavor.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedFlavor.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
                //	_dbContext.FlavorSet.Attach(updatedFlavor);
                //	_dbContext.Entry(updatedFlavor).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedFlavor);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                //_dbContext.FlavorSet.Attach(updatedFlavor);
                //_dbContext.Entry(updatedFlavor).State = EntityState.Modified;
                //_dbContext.Entry(flavorToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedFlavor.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(flavorToUpdate).State = EntityState.Detached;
            }
            //TODO concurrency token check
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedFlavors, bulkConfig);
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
                _dbContext.BulkUpdate(updatedFlavors, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Flavor> flavorsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = flavorsToDelete.Select(x => x.FlavorID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.FlavorSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.FlavorID))
				.Select(p => new { p.FlavorID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.FlavorID, x => x.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedFlavor in flavorsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedFlavor.FlavorID, out var token) || token != updatedFlavor.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedFlavor.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(flavorsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(flavorsToDelete, bulkConfig);
			}
		}
        public void BulkDelete(IEnumerable<Flavor> flavorsToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Flavor.FlavorID), nameof(Flavor.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = flavorsToDelete.Select(x => x.FlavorID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.FlavorSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.FlavorID))
                .Select(p => new { p.FlavorID, p.LastChangeCode })
                .ToDictionary(x => x.FlavorID, x => x.LastChangeCode);
            // Check concurrency conflicts
            foreach (var updatedFlavor in flavorsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedFlavor.FlavorID, out var token) || token != updatedFlavor.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedFlavor.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(flavorsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(flavorsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from flavor in _dbContext.FlavorSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == flavor.PacID).DefaultIfEmpty() //PacID
				   select new QueryDTO
                   {
					   FlavorObj = flavor,
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
                    var flavor = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (flavor != null)
                    {
                        found = true;
                        Delete(flavor.FlavorID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var flavor = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (flavor != null)
                    {
                        found = true;
                        Delete(flavor.FlavorID);
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
                    var flavor = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (flavor != null)
                    {
                        found = true;
                        Delete(flavor.FlavorID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<Flavor> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.FlavorObj.PacCodePeek = item.PacCode.Value; //PacID
            }
            List<Flavor> results = data.Select(r => r.FlavorObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Flavor>> GetByPacIDAsync(int id)
        {
            var flavorsWithCodes = await BuildQuery()
                                    .Where(x => x.FlavorObj.PacID == id)
                                    .ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            return finalFlavors;
        }
        //PacID
        public List<Flavor> GetByPacID(int id)
        {
            var flavorsWithCodes = BuildQuery()
                                    .Where(x => x.FlavorObj.PacID == id)
                                    .ToList();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            return finalFlavors;
        }
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        private class QueryDTO
        {
            public Flavor FlavorObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
