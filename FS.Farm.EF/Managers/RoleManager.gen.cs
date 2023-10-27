using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
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
        public Role Add(Role role)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.RoleSet.Add(role);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(role).State = EntityState.Detached;
                    transaction.Commit();
                    return role;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.RoleSet.Add(role);
                _dbContext.SaveChanges();
                _dbContext.Entry(role).State = EntityState.Detached;
                return role;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.RoleSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.RoleSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.RoleSet.AsNoTracking().MaxAsync(x => (int?)x.RoleID);
        }
        public int? GetMaxId()
        {
            return _dbContext.RoleSet.AsNoTracking().Max(x => (int?)x.RoleID);
        }
        public async Task<Role> GetByIdAsync(int id)
		{
			var rolesWithCodes = await BuildQuery()
									.Where(x => x.RoleObj.RoleID == id)
									.ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            if (finalRoles.Count > 0)
			{
				return finalRoles[0];
            }
			return null;
        }
        public Role GetById(int id)
        {
            var rolesWithCodes = BuildQuery()
                                    .Where(x => x.RoleObj.RoleID == id)
                                    .ToList();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            if (finalRoles.Count > 0)
            {
                return finalRoles[0];
            }
            return null;
        }
        public async Task<Role> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.RoleSet.AsNoTracking().FirstOrDefaultAsync(x => x.RoleID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var rolesWithCodes = await BuildQuery()
                                        .Where(x => x.RoleObj.RoleID == id)
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
        public Role DirtyGetById(int id) //to test
        {
            //return await _dbContext.RoleSet.AsNoTracking().FirstOrDefaultAsync(x => x.RoleID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var rolesWithCodes = BuildQuery()
                                        .Where(x => x.RoleObj.RoleID == id)
                                        .ToList();
                List<Role> finalRoles = ProcessMappings(rolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalRoles.Count > 0)
                {
                    return finalRoles[0];
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
        public async Task<Role> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var rolesWithCodes = await BuildQuery()
                                    .Where(x => x.RoleObj.Code == code)
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            if (finalRoles.Count > 0)
            {
                return finalRoles[0];
            }
            return null;
        }
        public Role GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var rolesWithCodes = BuildQuery()
                                    .Where(x => x.RoleObj.Code == code)
                                    .ToList();
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
                                        .Where(x => x.RoleObj.Code == code)
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
        public Role DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var rolesWithCodes = BuildQuery()
                                        .Where(x => x.RoleObj.Code == code)
                                        .ToList();
                List<Role> finalRoles = ProcessMappings(rolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalRoles.Count > 0)
                {
                    return finalRoles[0];
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
        public async Task<List<Role>> GetAllAsync()
		{
            var rolesWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            return finalRoles;
        }
        public List<Role> GetAll()
        {
            var rolesWithCodes = BuildQuery()
                                    .ToList();
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
        public bool Update(Role roleToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.RoleSet.Attach(roleToUpdate);
                    _dbContext.Entry(roleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(roleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    roleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(roleToUpdate).State = EntityState.Detached;
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
                    _dbContext.RoleSet.Attach(roleToUpdate);
                    _dbContext.Entry(roleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(roleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    roleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
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
					var role = await _dbContext.RoleSet.FirstOrDefaultAsync(x => x.RoleID == id);
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
					var role = await _dbContext.RoleSet.FirstOrDefaultAsync(x => x.RoleID == id);
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
        public bool Delete(int id)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    var role = _dbContext.RoleSet.FirstOrDefault(x => x.RoleID == id);
                    if (role == null) return false;
                    _dbContext.RoleSet.Remove(role);
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
                    var role = _dbContext.RoleSet.FirstOrDefault(x => x.RoleID == id);
                    if (role == null) return false;
                    _dbContext.RoleSet.Remove(role);
                    _dbContext.SaveChanges();
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
        public void BulkInsert(IEnumerable<Role> roles)
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
                    _dbContext.BulkInsert(roles, bulkConfig);
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
                _dbContext.BulkInsert(roles, bulkConfig);
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
			var idsToUpdate = updatedRoles.Select(x => x.RoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.RoleSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.RoleID))
			//	.Select(p => new { p.RoleID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.RoleID, x => x.LastChangeCode);
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
        public void BulkUpdate(IEnumerable<Role> updatedRoles)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Role.RoleID), nameof(Role.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedRoles.Select(x => x.RoleID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.RoleSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.RoleID))
            //	.Select(p => new { p.RoleID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.RoleID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedRoles, bulkConfig);
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
                _dbContext.BulkUpdate(updatedRoles, bulkConfig);
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
			var idsToUpdate = rolesToDelete.Select(x => x.RoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.RoleSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.RoleID))
				.Select(p => new { p.RoleID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.RoleID, x => x.LastChangeCode);
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
        public void BulkDelete(IEnumerable<Role> rolesToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Role.RoleID), nameof(Role.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = rolesToDelete.Select(x => x.RoleID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.RoleSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.RoleID))
                .Select(p => new { p.RoleID, p.LastChangeCode })
                .ToDictionary(x => x.RoleID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(rolesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(rolesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from role in _dbContext.RoleSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == role.PacID).DefaultIfEmpty() //PacID
				   select new QueryDTO
                   {
					   RoleObj = role,
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
                    var role = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (role != null)
                    {
                        found = true;
                        Delete(role.RoleID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var role = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (role != null)
                    {
                        found = true;
                        Delete(role.RoleID);
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
                    var role = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (role != null)
                    {
                        found = true;
                        Delete(role.RoleID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<Role> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.RoleObj.PacCodePeek = item.PacCode.Value; //PacID
            }
            List<Role> results = data.Select(r => r.RoleObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Role>> GetByPacIDAsync(int id)
        {
            var rolesWithCodes = await BuildQuery()
                                    .Where(x => x.RoleObj.PacID == id)
                                    .ToListAsync();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            return finalRoles;
        }
        //PacID
        public List<Role> GetByPacID(int id)
        {
            var rolesWithCodes = BuildQuery()
                                    .Where(x => x.RoleObj.PacID == id)
                                    .ToList();
            List<Role> finalRoles = ProcessMappings(rolesWithCodes);
            return finalRoles;
        }
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        private class QueryDTO
        {
            public Role RoleObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
