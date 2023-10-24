using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
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
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.FlavorSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.FlavorSet.AsNoTracking().MaxAsync(p => (int?)p.FlavorID);
		}
		public async Task<Flavor> GetByIdAsync(int id)
		{
			var flavorsWithCodes = await BuildQuery()
									.Where(p => p.FlavorObj.FlavorID == id)
									.ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            if (finalFlavors.Count > 0)
			{
				return finalFlavors[0];
            }
			return null;
        }
		public async Task<Flavor> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.FlavorSet.AsNoTracking().FirstOrDefaultAsync(p => p.FlavorID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var flavorsWithCodes = await BuildQuery()
                                        .Where(p => p.FlavorObj.FlavorID == id)
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
		public async Task<Flavor> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var flavorsWithCodes = await BuildQuery()
                                    .Where(p => p.FlavorObj.Code == code)
                                    .ToListAsync();
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
                                        .Where(p => p.FlavorObj.Code == code)
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
		public async Task<IEnumerable<Flavor>> GetAllAsync()
		{
            var flavorsWithCodes = await BuildQuery()
                                    .ToListAsync();
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
		public async Task<bool> DeleteAsync(int id)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					var flavor = await _dbContext.FlavorSet.FirstOrDefaultAsync(p => p.FlavorID == id);
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
					var flavor = await _dbContext.FlavorSet.FirstOrDefaultAsync(p => p.FlavorID == id);
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
			var idsToUpdate = updatedFlavors.Select(p => p.FlavorID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.FlavorSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.FlavorID))
			//	.Select(p => new { p.FlavorID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.FlavorID, p => p.LastChangeCode);
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
			var idsToUpdate = flavorsToDelete.Select(p => p.FlavorID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.FlavorSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.FlavorID))
				.Select(p => new { p.FlavorID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.FlavorID, p => p.LastChangeCode);
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
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from flavor in _dbContext.FlavorSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == flavor.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   FlavorObj = flavor,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<Flavor> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.FlavorObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<Flavor> results = data.Select(r => r.FlavorObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Flavor>> GetByPacAsync(int id)
        {
            var flavorsWithCodes = await BuildQuery()
                                    .Where(p => p.FlavorObj.PacID == id)
                                    .ToListAsync();
            List<Flavor> finalFlavors = ProcessMappings(flavorsWithCodes);
            return finalFlavors;
        }
		//ENDSET
        private class QueryDTO
        {
            public Flavor FlavorObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
