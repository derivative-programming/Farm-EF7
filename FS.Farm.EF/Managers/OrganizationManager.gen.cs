using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class OrganizationManager
	{
		private readonly FarmDbContext _dbContext;
		public OrganizationManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Organization> AddAsync(Organization organization)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrganizationSet.Add(organization);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(organization).State = EntityState.Detached;
					await transaction.CommitAsync();
					return organization;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.OrganizationSet.Add(organization);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(organization).State = EntityState.Detached;
				return organization;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.OrganizationSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.OrganizationSet.AsNoTracking().MaxAsync(p => (int?)p.OrganizationID);
		}
		public async Task<Organization> GetByIdAsync(int id)
		{
			var organizationsWithCodes = await BuildQuery()
									.Where(p => p.OrganizationObj.OrganizationID == id)
									.ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            if (finalOrganizations.Count > 0)
			{
				return finalOrganizations[0];
            }
			return null;
        }
		public async Task<Organization> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.OrganizationSet.AsNoTracking().FirstOrDefaultAsync(p => p.OrganizationID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var organizationsWithCodes = await BuildQuery()
                                        .Where(p => p.OrganizationObj.OrganizationID == id)
                                        .ToListAsync();
                List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalOrganizations.Count > 0)
                {
                    return finalOrganizations[0];
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
		public async Task<Organization> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var organizationsWithCodes = await BuildQuery()
                                    .Where(p => p.OrganizationObj.Code == code)
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            if (finalOrganizations.Count > 0)
            {
                return finalOrganizations[0];
            }
            return null;
        }
		public async Task<Organization> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var organizationsWithCodes = await BuildQuery()
                                        .Where(p => p.OrganizationObj.Code == code)
                                        .ToListAsync();
                List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalOrganizations.Count > 0)
                {
                    return finalOrganizations[0];
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
		public async Task<IEnumerable<Organization>> GetAllAsync()
		{
            var organizationsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            return finalOrganizations;
        }
		public async Task<bool> UpdateAsync(Organization organizationToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrganizationSet.Attach(organizationToUpdate);
					_dbContext.Entry(organizationToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(organizationToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					organizationToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(organizationToUpdate).State = EntityState.Detached;
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
					_dbContext.OrganizationSet.Attach(organizationToUpdate);
					_dbContext.Entry(organizationToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(organizationToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					organizationToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(organizationToUpdate).State = EntityState.Detached;
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
					var organization = await _dbContext.OrganizationSet.FirstOrDefaultAsync(p => p.OrganizationID == id);
					if (organization == null) return false;
					_dbContext.OrganizationSet.Remove(organization);
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
					var organization = await _dbContext.OrganizationSet.FirstOrDefaultAsync(p => p.OrganizationID == id);
					if (organization == null) return false;
					_dbContext.OrganizationSet.Remove(organization);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<Organization> organizations)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Organization.OrganizationID), nameof(Organization.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var organization in organizations)
			{
				organization.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(organization);
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
					await _dbContext.BulkInsertAsync(organizations, bulkConfig);
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
				await _dbContext.BulkInsertAsync(organizations, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<Organization> updatedOrganizations)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Organization.OrganizationID), nameof(Organization.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedOrganizations.Select(p => p.OrganizationID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.OrganizationSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.OrganizationID))
			//	.Select(p => new { p.OrganizationID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.OrganizationID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedOrganization in updatedOrganizations)
			{
				//	if (!existingTokens.TryGetValue(updatedOrganization.OrganizationID, out var token) || token != updatedOrganization.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedOrganization.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.OrganizationSet.Attach(updatedOrganization);
				//	_dbContext.Entry(updatedOrganization).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedOrganization);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.OrganizationSet.Attach(updatedOrganization);
				//_dbContext.Entry(updatedOrganization).State = EntityState.Modified;
				//_dbContext.Entry(organizationToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedOrganization.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(organizationToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedOrganizations, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedOrganizations, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<Organization> organizationsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Organization.OrganizationID), nameof(Organization.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = organizationsToDelete.Select(p => p.OrganizationID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.OrganizationSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.OrganizationID))
				.Select(p => new { p.OrganizationID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.OrganizationID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedOrganization in organizationsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedOrganization.OrganizationID, out var token) || token != updatedOrganization.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedOrganization.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(organizationsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(organizationsToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from organization in _dbContext.OrganizationSet.AsNoTracking()
				   from tac in _dbContext.TacSet.AsNoTracking().Where(l => l.TacID == organization.TacID).DefaultIfEmpty() //TacID
																														//ENDSET
				   select new QueryDTO
                   {
					   OrganizationObj = organization,
					   TacCode = tac.Code, //TacID
											 //ENDSET
				   };
        }
		private List<Organization> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.OrganizationObj.TacCodePeek = item.TacCode.Value; //TacID
                //ENDSET
            }
            List<Organization> results = data.Select(r => r.OrganizationObj).ToList();
            return results;
        }
        //TacID
        public async Task<List<Organization>> GetByTacAsync(int id)
        {
            var organizationsWithCodes = await BuildQuery()
                                    .Where(p => p.OrganizationObj.TacID == id)
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            return finalOrganizations;
        }
		//ENDSET
        private class QueryDTO
        {
            public Organization OrganizationObj { get; set; }
            public Guid? TacCode { get; set; } //TacID
        }
    }
}
