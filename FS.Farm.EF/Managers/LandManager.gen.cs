using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class LandManager
	{
		private readonly FarmDbContext _dbContext;
		public LandManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Land> AddAsync(Land land)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.LandSet.Add(land);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(land).State = EntityState.Detached;
					await transaction.CommitAsync();
					return land;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.LandSet.Add(land);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(land).State = EntityState.Detached;
				return land;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.LandSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.LandSet.AsNoTracking().MaxAsync(p => (int?)p.LandID);
		}
		public async Task<Land> GetByIdAsync(int id)
		{
			var landsWithCodes = await BuildQuery()
									.Where(p => p.LandObj.LandID == id)
									.ToListAsync();
            List<Land> finalLands = ProcessMappings(landsWithCodes);
            if (finalLands.Count > 0)
			{
				return finalLands[0];
            }
			return null;
        }
		public async Task<Land> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.LandSet.AsNoTracking().FirstOrDefaultAsync(p => p.LandID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var landsWithCodes = await BuildQuery()
                                        .Where(p => p.LandObj.LandID == id)
                                        .ToListAsync();
                List<Land> finalLands = ProcessMappings(landsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalLands.Count > 0)
                {
                    return finalLands[0];
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
		public async Task<Land> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var landsWithCodes = await BuildQuery()
                                    .Where(p => p.LandObj.Code == code)
                                    .ToListAsync();
            List<Land> finalLands = ProcessMappings(landsWithCodes);
            if (finalLands.Count > 0)
            {
                return finalLands[0];
            }
            return null;
        }
		public async Task<Land> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var landsWithCodes = await BuildQuery()
                                        .Where(p => p.LandObj.Code == code)
                                        .ToListAsync();
                List<Land> finalLands = ProcessMappings(landsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalLands.Count > 0)
                {
                    return finalLands[0];
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
		public async Task<IEnumerable<Land>> GetAllAsync()
		{
            var landsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Land> finalLands = ProcessMappings(landsWithCodes);
            return finalLands;
        }
		public async Task<bool> UpdateAsync(Land landToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.LandSet.Attach(landToUpdate);
					_dbContext.Entry(landToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					landToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(landToUpdate).State = EntityState.Detached;
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
					_dbContext.LandSet.Attach(landToUpdate);
					_dbContext.Entry(landToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					landToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(landToUpdate).State = EntityState.Detached;
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
					var land = await _dbContext.LandSet.FirstOrDefaultAsync(p => p.LandID == id);
					if (land == null) return false;
					_dbContext.LandSet.Remove(land);
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
					var land = await _dbContext.LandSet.FirstOrDefaultAsync(p => p.LandID == id);
					if (land == null) return false;
					_dbContext.LandSet.Remove(land);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<Land> lands)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var land in lands)
			{
				land.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(land);
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
					await _dbContext.BulkInsertAsync(lands, bulkConfig);
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
				await _dbContext.BulkInsertAsync(lands, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<Land> updatedLands)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedLands.Select(p => p.LandID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.LandSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.LandID))
			//	.Select(p => new { p.LandID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.LandID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedLand in updatedLands)
			{
				//	if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.LandSet.Attach(updatedLand);
				//	_dbContext.Entry(updatedLand).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedLand);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.LandSet.Attach(updatedLand);
				//_dbContext.Entry(updatedLand).State = EntityState.Modified;
				//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedLand.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(landToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedLands, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedLands, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<Land> landsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = landsToDelete.Select(p => p.LandID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.LandSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.LandID))
				.Select(p => new { p.LandID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.LandID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedLand in landsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(landsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(landsToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from land in _dbContext.LandSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == land.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   LandObj = land,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<Land> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.LandObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<Land> results = data.Select(r => r.LandObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Land>> GetByPacAsync(int id)
        {
            var landsWithCodes = await BuildQuery()
                                    .Where(p => p.LandObj.PacID == id)
                                    .ToListAsync();
            List<Land> finalLands = ProcessMappings(landsWithCodes);
            return finalLands;
        }
		//ENDSET
        private class QueryDTO
        {
            public Land LandObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
