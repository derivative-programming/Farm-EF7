using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class OrgCustomerManager
	{
		private readonly FarmDbContext _dbContext;
		public OrgCustomerManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<OrgCustomer> AddAsync(OrgCustomer orgCustomer)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrgCustomerSet.Add(orgCustomer);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgCustomer).State = EntityState.Detached;
					await transaction.CommitAsync();
					return orgCustomer;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.OrgCustomerSet.Add(orgCustomer);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(orgCustomer).State = EntityState.Detached;
				return orgCustomer;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.OrgCustomerSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.OrgCustomerSet.AsNoTracking().MaxAsync(p => (int?)p.OrgCustomerID);
		}
		public async Task<OrgCustomer> GetByIdAsync(int id)
		{
			var orgCustomersWithCodes = await BuildQuery()
									.Where(p => p.OrgCustomerObj.OrgCustomerID == id)
									.ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            if (finalOrgCustomers.Count > 0)
			{
				return finalOrgCustomers[0];
            }
			return null;
        }
		public async Task<OrgCustomer> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.OrgCustomerSet.AsNoTracking().FirstOrDefaultAsync(p => p.OrgCustomerID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var orgCustomersWithCodes = await BuildQuery()
                                        .Where(p => p.OrgCustomerObj.OrgCustomerID == id)
                                        .ToListAsync();
                List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalOrgCustomers.Count > 0)
                {
                    return finalOrgCustomers[0];
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
		public async Task<OrgCustomer> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(p => p.OrgCustomerObj.Code == code)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            if (finalOrgCustomers.Count > 0)
            {
                return finalOrgCustomers[0];
            }
            return null;
        }
		public async Task<OrgCustomer> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var orgCustomersWithCodes = await BuildQuery()
                                        .Where(p => p.OrgCustomerObj.Code == code)
                                        .ToListAsync();
                List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalOrgCustomers.Count > 0)
                {
                    return finalOrgCustomers[0];
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
		public async Task<IEnumerable<OrgCustomer>> GetAllAsync()
		{
            var orgCustomersWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
		public async Task<bool> UpdateAsync(OrgCustomer orgCustomerToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrgCustomerSet.Attach(orgCustomerToUpdate);
					_dbContext.Entry(orgCustomerToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(orgCustomerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					orgCustomerToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgCustomerToUpdate).State = EntityState.Detached;
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
					_dbContext.OrgCustomerSet.Attach(orgCustomerToUpdate);
					_dbContext.Entry(orgCustomerToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(orgCustomerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					orgCustomerToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgCustomerToUpdate).State = EntityState.Detached;
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
					var orgCustomer = await _dbContext.OrgCustomerSet.FirstOrDefaultAsync(p => p.OrgCustomerID == id);
					if (orgCustomer == null) return false;
					_dbContext.OrgCustomerSet.Remove(orgCustomer);
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
					var orgCustomer = await _dbContext.OrgCustomerSet.FirstOrDefaultAsync(p => p.OrgCustomerID == id);
					if (orgCustomer == null) return false;
					_dbContext.OrgCustomerSet.Remove(orgCustomer);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<OrgCustomer> orgCustomers)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(OrgCustomer.OrgCustomerID), nameof(OrgCustomer.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var orgCustomer in orgCustomers)
			{
				orgCustomer.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(orgCustomer);
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
					await _dbContext.BulkInsertAsync(orgCustomers, bulkConfig);
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
				await _dbContext.BulkInsertAsync(orgCustomers, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<OrgCustomer> updatedOrgCustomers)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(OrgCustomer.OrgCustomerID), nameof(OrgCustomer.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedOrgCustomers.Select(p => p.OrgCustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.OrgCustomerSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.OrgCustomerID))
			//	.Select(p => new { p.OrgCustomerID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.OrgCustomerID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedOrgCustomer in updatedOrgCustomers)
			{
				//	if (!existingTokens.TryGetValue(updatedOrgCustomer.OrgCustomerID, out var token) || token != updatedOrgCustomer.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedOrgCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.OrgCustomerSet.Attach(updatedOrgCustomer);
				//	_dbContext.Entry(updatedOrgCustomer).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedOrgCustomer);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.OrgCustomerSet.Attach(updatedOrgCustomer);
				//_dbContext.Entry(updatedOrgCustomer).State = EntityState.Modified;
				//_dbContext.Entry(orgCustomerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedOrgCustomer.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(orgCustomerToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedOrgCustomers, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedOrgCustomers, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<OrgCustomer> orgCustomersToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(OrgCustomer.OrgCustomerID), nameof(OrgCustomer.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = orgCustomersToDelete.Select(p => p.OrgCustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.OrgCustomerSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.OrgCustomerID))
				.Select(p => new { p.OrgCustomerID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.OrgCustomerID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedOrgCustomer in orgCustomersToDelete)
			{
				if (!existingTokens.TryGetValue(updatedOrgCustomer.OrgCustomerID, out var token) || token != updatedOrgCustomer.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedOrgCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(orgCustomersToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(orgCustomersToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from orgCustomer in _dbContext.OrgCustomerSet.AsNoTracking()
				   from customer in _dbContext.CustomerSet.AsNoTracking().Where(f => f.CustomerID == orgCustomer.CustomerID).DefaultIfEmpty() //CustomerID
				   from organization in _dbContext.OrganizationSet.AsNoTracking().Where(l => l.OrganizationID == orgCustomer.OrganizationID).DefaultIfEmpty() //OrganizationID
																														//ENDSET
				   select new QueryDTO
                   {
					   OrgCustomerObj = orgCustomer,
					   CustomerCode = customer.Code, //CustomerID
					   OrganizationCode = organization.Code, //OrganizationID
											 //ENDSET
				   };
        }
		private List<OrgCustomer> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.OrgCustomerObj.CustomerCodePeek = item.CustomerCode.Value; //CustomerID
                item.OrgCustomerObj.OrganizationCodePeek = item.OrganizationCode.Value; //OrganizationID
                //ENDSET
            }
            List<OrgCustomer> results = data.Select(r => r.OrgCustomerObj).ToList();
            return results;
        }
        //CustomerID
        public async Task<List<OrgCustomer>> GetByCustomerAsync(int id)
        {
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(p => p.OrgCustomerObj.CustomerID == id)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        //OrganizationID
        public async Task<List<OrgCustomer>> GetByOrganizationAsync(int id)
        {
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(p => p.OrgCustomerObj.OrganizationID == id)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
		//ENDSET
        private class QueryDTO
        {
            public OrgCustomer OrgCustomerObj { get; set; }
            public Guid? CustomerCode { get; set; } //CustomerID
            public Guid? OrganizationCode { get; set; } //OrganizationID
        }
    }
}
