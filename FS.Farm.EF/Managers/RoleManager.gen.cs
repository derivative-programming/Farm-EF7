using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class RoleManager
	{
		private readonly FarmDbContext _dbContext;
		public RoleManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Role> AddAsync(Role role)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.RoleSet.Add(role);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(role).State = EntityState.Detached;
					await transaction.CommitAsync();
					return role;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.RoleSet.Add(role);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(role).State = EntityState.Detached;
				return role;
			}
		}
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.RoleSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.RoleSet.AsNoTracking().MaxAsync(p => (int?)p.RoleID);
		}
		public async Task<Role> GetByIdAsync(int id)
		{
			var rolesWithCodes = await BuildQuery()
									.Where(p => p.RoleObj.RoleID == id)
									.ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            if (finalRoles.Count > 0)
			{
				return finalRoles[0];
            }
			return null;
        }
		public async Task<Role> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.RoleSet.AsNoTracking().FirstOrDefaultAsync(p => p.RoleID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var rolesWithCodes = await BuildQuery()
                                        .Where(p => p.RoleObj.RoleID == id)
                                        .ToListAsync();
                List<Role> finalRoles = ProcessMappings(rolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalRoles.Count > 0)
                {
                    return finalRoles[0];
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
		public async Task<Role> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var rolesWithCodes = await BuildQuery()
                                    .Where(p => p.RoleObj.Code == code)
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            if (finalRoles.Count > 0)
            {
                return finalRoles[0];
            }
            return null;
        }
		public async Task<Role> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var rolesWithCodes = await BuildQuery()
                                        .Where(p => p.RoleObj.Code == code)
                                        .ToListAsync();
                List<Role> finalRoles = ProcessMappings(rolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalRoles.Count > 0)
                {
                    return finalRoles[0];
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
		public async Task<IEnumerable<Role>> GetAllAsync()
		{
            var rolesWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            return finalRoles;
        }
		public async Task<bool> UpdateAsync(Role roleToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.RoleSet.Attach(roleToUpdate);
					_dbContext.Entry(roleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(roleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					roleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(roleToUpdate).State = EntityState.Detached;
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
					_dbContext.RoleSet.Attach(roleToUpdate);
					_dbContext.Entry(roleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(roleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					roleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(roleToUpdate).State = EntityState.Detached;
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
					var role = await _dbContext.RoleSet.FirstOrDefaultAsync(p => p.RoleID == id);
					if (role == null) return false;
					_dbContext.RoleSet.Remove(role);
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
					var role = await _dbContext.RoleSet.FirstOrDefaultAsync(p => p.RoleID == id);
					if (role == null) return false;
					_dbContext.RoleSet.Remove(role);
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch
				{
					throw;
				}
			}
		}
		public async Task BulkInsertAsync(IEnumerable<Role> roles)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Role.RoleID), nameof(Role.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var role in roles)
			{
				role.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(role);
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
					await _dbContext.BulkInsertAsync(roles, bulkConfig);
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
				await _dbContext.BulkInsertAsync(roles, bulkConfig);
			}
		}
		public async Task BulkUpdateAsync(IEnumerable<Role> updatedRoles)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Role.RoleID), nameof(Role.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedRoles.Select(p => p.RoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.RoleSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.RoleID))
			//	.Select(p => new { p.RoleID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.RoleID, p => p.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedRole in updatedRoles)
			{
				//	if (!existingTokens.TryGetValue(updatedRole.RoleID, out var token) || token != updatedRole.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.RoleSet.Attach(updatedRole);
				//	_dbContext.Entry(updatedRole).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedRole);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.RoleSet.Attach(updatedRole);
				//_dbContext.Entry(updatedRole).State = EntityState.Modified;
				//_dbContext.Entry(roleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedRole.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(roleToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedRoles, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedRoles, bulkConfig);
			}
		}
		public async Task BulkDeleteAsync(IEnumerable<Role> rolesToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Role.RoleID), nameof(Role.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = rolesToDelete.Select(p => p.RoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.RoleSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.RoleID))
				.Select(p => new { p.RoleID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.RoleID, p => p.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedRole in rolesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedRole.RoleID, out var token) || token != updatedRole.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(rolesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(rolesToDelete, bulkConfig);
			}
		}
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from role in _dbContext.RoleSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == role.PacID).DefaultIfEmpty() //PacID
																														//ENDSET
				   select new QueryDTO
                   {
					   RoleObj = role,
					   PacCode = pac.Code, //PacID
											 //ENDSET
				   };
        }
		private List<Role> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.RoleObj.PacCodePeek = item.PacCode.Value; //PacID
                //ENDSET
            }
            List<Role> results = data.Select(r => r.RoleObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Role>> GetByPacAsync(int id)
        {
            var rolesWithCodes = await BuildQuery()
                                    .Where(p => p.RoleObj.PacID == id)
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            return finalRoles;
        }
		//ENDSET
        private class QueryDTO
        {
            public Role RoleObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
