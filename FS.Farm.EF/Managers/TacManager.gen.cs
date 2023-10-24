using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class TacManager
	{
		private readonly FarmDbContext _dbContext;
		public TacManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Tac> AddAsync(Tac tac)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TacSet.Add(tac);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tac).State = EntityState.Detached;
					await transaction.CommitAsync();
					return tac;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.TacSet.Add(tac);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(tac).State = EntityState.Detached;
				return tac;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.TacSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.TacSet.AsNoTracking().MaxAsync(p => (int?)p.TacID);
		}
		public async Task<Tac> GetByIdAsync(int id)
		{
			var tacsWithCodes = await BuildQuery()
									.Where(p => p.TacObj.TacID == id)
									.ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
			{
				return finalTacs[0];
            }
			return null;
        }
		public async Task<Tac> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.TacSet.AsNoTracking().FirstOrDefaultAsync(p => p.TacID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var tacsWithCodes = await BuildQuery()
                                        .Where(p => p.TacObj.TacID == id)
                                        .ToListAsync();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
		public async Task<Tac> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var tacsWithCodes = await BuildQuery()
                                    .Where(p => p.TacObj.Code == code)
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
            {
                return finalTacs[0];
            }
            return null;
        }
		public async Task<Tac> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var tacsWithCodes = await BuildQuery()
                                        .Where(p => p.TacObj.Code == code)
                                        .ToListAsync();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
		public async Task<IEnumerable<Tac>> GetAllAsync()
		{
            var tacsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
		public async Task<bool> UpdateAsync(Tac tacToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TacSet.Attach(tacToUpdate);
					_dbContext.Entry(tacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					tacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
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
					_dbContext.TacSet.Attach(tacToUpdate);
					_dbContext.Entry(tacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					tacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
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
					var tac = await _dbContext.TacSet.FirstOrDefaultAsync(p => p.TacID == id);
					if (tac == null) return false;
					_dbContext.TacSet.Remove(tac);
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
					var tac = await _dbContext.TacSet.FirstOrDefaultAsync(p => p.TacID == id);
					if (tac == null) return false;
					_dbContext.TacSet.Remove(tac);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<Tac> tacs)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var tac in tacs)
			{
				tac.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(tac);
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
					await _dbContext.BulkInsertAsync(tacs, bulkConfig);
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
				await _dbContext.BulkInsertAsync(tacs, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<Tac> updatedTacs)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedTacs.Select(p => p.TacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.TacSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.TacID))
			//	.Select(p => new { p.TacID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.TacID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedTac in updatedTacs)
			{
				//	if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.TacSet.Attach(updatedTac);
				//	_dbContext.Entry(updatedTac).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedTac);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.TacSet.Attach(updatedTac);
				//_dbContext.Entry(updatedTac).State = EntityState.Modified;
				//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedTac.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedTacs, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedTacs, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<Tac> tacsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = tacsToDelete.Select(p => p.TacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.TacSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.TacID))
				.Select(p => new { p.TacID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.TacID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedTac in tacsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(tacsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(tacsToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from tac in _dbContext.TacSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == tac.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   TacObj = tac,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<Tac> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.TacObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<Tac> results = data.Select(r => r.TacObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Tac>> GetByPacAsync(int id)
        {
            var tacsWithCodes = await BuildQuery()
                                    .Where(p => p.TacObj.PacID == id)
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
		//ENDSET
        private class QueryDTO
        {
            public Tac TacObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
