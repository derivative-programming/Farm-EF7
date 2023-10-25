using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class CustomerRoleManager
	{
		private readonly FarmDbContext _dbContext;
		public CustomerRoleManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<CustomerRole> AddAsync(CustomerRole customerRole)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.CustomerRoleSet.Add(customerRole);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customerRole).State = EntityState.Detached;
					await transaction.CommitAsync();
					return customerRole;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.CustomerRoleSet.Add(customerRole);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(customerRole).State = EntityState.Detached;
				return customerRole;
			}
		}
        public CustomerRole Add(CustomerRole customerRole)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.CustomerRoleSet.Add(customerRole);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customerRole).State = EntityState.Detached;
                    transaction.Commit();
                    return customerRole;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.CustomerRoleSet.Add(customerRole);
                _dbContext.SaveChanges();
                _dbContext.Entry(customerRole).State = EntityState.Detached;
                return customerRole;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.CustomerRoleSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.CustomerRoleSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.CustomerRoleSet.AsNoTracking().MaxAsync(x => (int?)x.CustomerRoleID);
        }
        public int? GetMaxId()
        {
            return _dbContext.CustomerRoleSet.AsNoTracking().Max(x => (int?)x.CustomerRoleID);
        }
        public async Task<CustomerRole> GetByIdAsync(int id)
		{
			var customerRolesWithCodes = await BuildQuery()
									.Where(x => x.CustomerRoleObj.CustomerRoleID == id)
									.ToListAsync();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            if (finalCustomerRoles.Count > 0)
			{
				return finalCustomerRoles[0];
            }
			return null;
        }
        public CustomerRole GetById(int id)
        {
            var customerRolesWithCodes = BuildQuery()
                                    .Where(x => x.CustomerRoleObj.CustomerRoleID == id)
                                    .ToList();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            if (finalCustomerRoles.Count > 0)
            {
                return finalCustomerRoles[0];
            }
            return null;
        }
        public async Task<CustomerRole> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.CustomerRoleSet.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerRoleID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var customerRolesWithCodes = await BuildQuery()
                                        .Where(x => x.CustomerRoleObj.CustomerRoleID == id)
                                        .ToListAsync();
                List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalCustomerRoles.Count > 0)
                {
                    return finalCustomerRoles[0];
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
        public CustomerRole DirtyGetById(int id) //to test
        {
            //return await _dbContext.CustomerRoleSet.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerRoleID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var customerRolesWithCodes = BuildQuery()
                                        .Where(x => x.CustomerRoleObj.CustomerRoleID == id)
                                        .ToList();
                List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalCustomerRoles.Count > 0)
                {
                    return finalCustomerRoles[0];
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
        public async Task<CustomerRole> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var customerRolesWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerRoleObj.Code == code)
                                    .ToListAsync();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            if (finalCustomerRoles.Count > 0)
            {
                return finalCustomerRoles[0];
            }
            return null;
        }
        public CustomerRole GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var customerRolesWithCodes = BuildQuery()
                                    .Where(x => x.CustomerRoleObj.Code == code)
                                    .ToList();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            if (finalCustomerRoles.Count > 0)
            {
                return finalCustomerRoles[0];
            }
            return null;
        }
        public async Task<CustomerRole> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var customerRolesWithCodes = await BuildQuery()
                                        .Where(x => x.CustomerRoleObj.Code == code)
                                        .ToListAsync();
                List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalCustomerRoles.Count > 0)
                {
                    return finalCustomerRoles[0];
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
        public CustomerRole DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var customerRolesWithCodes = BuildQuery()
                                        .Where(x => x.CustomerRoleObj.Code == code)
                                        .ToList();
                List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalCustomerRoles.Count > 0)
                {
                    return finalCustomerRoles[0];
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
        public async Task<List<CustomerRole>> GetAllAsync()
		{
            var customerRolesWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        public List<CustomerRole> GetAll()
        {
            var customerRolesWithCodes = BuildQuery()
                                    .ToList();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        public async Task<bool> UpdateAsync(CustomerRole customerRoleToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.CustomerRoleSet.Attach(customerRoleToUpdate);
					_dbContext.Entry(customerRoleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					customerRoleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
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
					_dbContext.CustomerRoleSet.Attach(customerRoleToUpdate);
					_dbContext.Entry(customerRoleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					customerRoleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}
        public bool Update(CustomerRole customerRoleToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.CustomerRoleSet.Attach(customerRoleToUpdate);
                    _dbContext.Entry(customerRoleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    customerRoleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
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
                    _dbContext.CustomerRoleSet.Attach(customerRoleToUpdate);
                    _dbContext.Entry(customerRoleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    customerRoleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
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
					var customerRole = await _dbContext.CustomerRoleSet.FirstOrDefaultAsync(x => x.CustomerRoleID == id);
					if (customerRole == null) return false;
					_dbContext.CustomerRoleSet.Remove(customerRole);
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
					var customerRole = await _dbContext.CustomerRoleSet.FirstOrDefaultAsync(x => x.CustomerRoleID == id);
					if (customerRole == null) return false;
					_dbContext.CustomerRoleSet.Remove(customerRole);
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
                    var customerRole = _dbContext.CustomerRoleSet.FirstOrDefault(x => x.CustomerRoleID == id);
                    if (customerRole == null) return false;
                    _dbContext.CustomerRoleSet.Remove(customerRole);
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
                    var customerRole = _dbContext.CustomerRoleSet.FirstOrDefault(x => x.CustomerRoleID == id);
                    if (customerRole == null) return false;
                    _dbContext.CustomerRoleSet.Remove(customerRole);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task BulkInsertAsync(IEnumerable<CustomerRole> customerRoles)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var customerRole in customerRoles)
			{
				customerRole.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(customerRole);
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
					await _dbContext.BulkInsertAsync(customerRoles, bulkConfig);
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
				await _dbContext.BulkInsertAsync(customerRoles, bulkConfig);
			}
		}
        public void BulkInsert(IEnumerable<CustomerRole> customerRoles)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var customerRole in customerRoles)
            {
                customerRole.LastChangeCode = Guid.NewGuid();
                var entry = _dbContext.Entry(customerRole);
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
                    _dbContext.BulkInsert(customerRoles, bulkConfig);
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
                _dbContext.BulkInsert(customerRoles, bulkConfig);
            }
        }
        public async Task BulkUpdateAsync(IEnumerable<CustomerRole> updatedCustomerRoles)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedCustomerRoles.Select(x => x.CustomerRoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.CustomerRoleSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.CustomerRoleID))
			//	.Select(p => new { p.CustomerRoleID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.CustomerRoleID, x => x.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedCustomerRole in updatedCustomerRoles)
			{
				//	if (!existingTokens.TryGetValue(updatedCustomerRole.CustomerRoleID, out var token) || token != updatedCustomerRole.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedCustomerRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.CustomerRoleSet.Attach(updatedCustomerRole);
				//	_dbContext.Entry(updatedCustomerRole).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedCustomerRole);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.CustomerRoleSet.Attach(updatedCustomerRole);
				//_dbContext.Entry(updatedCustomerRole).State = EntityState.Modified;
				//_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedCustomerRole.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedCustomerRoles, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedCustomerRoles, bulkConfig);
			}
		}
        public void BulkUpdate(IEnumerable<CustomerRole> updatedCustomerRoles)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedCustomerRoles.Select(x => x.CustomerRoleID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.CustomerRoleSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.CustomerRoleID))
            //	.Select(p => new { p.CustomerRoleID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.CustomerRoleID, x => x.LastChangeCode);
            //// Check concurrency conflicts
            foreach (var updatedCustomerRole in updatedCustomerRoles)
            {
                //	if (!existingTokens.TryGetValue(updatedCustomerRole.CustomerRoleID, out var token) || token != updatedCustomerRole.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedCustomerRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
                //	_dbContext.CustomerRoleSet.Attach(updatedCustomerRole);
                //	_dbContext.Entry(updatedCustomerRole).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedCustomerRole);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                //_dbContext.CustomerRoleSet.Attach(updatedCustomerRole);
                //_dbContext.Entry(updatedCustomerRole).State = EntityState.Modified;
                //_dbContext.Entry(customerRoleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedCustomerRole.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(customerRoleToUpdate).State = EntityState.Detached;
            }
            //TODO concurrency token check
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedCustomerRoles, bulkConfig);
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
                _dbContext.BulkUpdate(updatedCustomerRoles, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<CustomerRole> customerRolesToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = customerRolesToDelete.Select(x => x.CustomerRoleID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.CustomerRoleSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.CustomerRoleID))
				.Select(p => new { p.CustomerRoleID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.CustomerRoleID, x => x.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedCustomerRole in customerRolesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedCustomerRole.CustomerRoleID, out var token) || token != updatedCustomerRole.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedCustomerRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(customerRolesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(customerRolesToDelete, bulkConfig);
			}
		}
        public void BulkDelete(IEnumerable<CustomerRole> customerRolesToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(CustomerRole.CustomerRoleID), nameof(CustomerRole.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = customerRolesToDelete.Select(x => x.CustomerRoleID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.CustomerRoleSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.CustomerRoleID))
                .Select(p => new { p.CustomerRoleID, p.LastChangeCode })
                .ToDictionary(x => x.CustomerRoleID, x => x.LastChangeCode);
            // Check concurrency conflicts
            foreach (var updatedCustomerRole in customerRolesToDelete)
            {
                if (!existingTokens.TryGetValue(updatedCustomerRole.CustomerRoleID, out var token) || token != updatedCustomerRole.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedCustomerRole.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(customerRolesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(customerRolesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from customerRole in _dbContext.CustomerRoleSet.AsNoTracking()
				   from customer in _dbContext.CustomerSet.AsNoTracking().Where(l => l.CustomerID == customerRole.CustomerID).DefaultIfEmpty() //CustomerID
				   from role in _dbContext.RoleSet.AsNoTracking().Where(f => f.RoleID == customerRole.RoleID).DefaultIfEmpty() //RoleID
				   select new QueryDTO
                   {
					   CustomerRoleObj = customerRole,
					   CustomerCode = customer.Code, //CustomerID
					   RoleCode = role.Code, //RoleID
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
                    var customerRole = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (customerRole != null)
                    {
                        found = true;
                        Delete(customerRole.CustomerRoleID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var customerRole = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (customerRole != null)
                    {
                        found = true;
                        Delete(customerRole.CustomerRoleID);
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
                    var customerRole = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (customerRole != null)
                    {
                        found = true;
                        Delete(customerRole.CustomerRoleID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<CustomerRole> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.CustomerRoleObj.CustomerCodePeek = item.CustomerCode.Value; //CustomerID
                item.CustomerRoleObj.RoleCodePeek = item.RoleCode.Value; //RoleID
            }
            List<CustomerRole> results = data.Select(r => r.CustomerRoleObj).ToList();
            return results;
        }
        //CustomerID
        public async Task<List<CustomerRole>> GetByCustomerAsync(int id)
        {
            var customerRolesWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerRoleObj.CustomerID == id)
                                    .ToListAsync();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        //RoleID
        public async Task<List<CustomerRole>> GetByRoleAsync(int id)
        {
            var customerRolesWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerRoleObj.RoleID == id)
                                    .ToListAsync();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        public List<CustomerRole> GetByRole(int id)
        {
            var customerRolesWithCodes = BuildQuery()
                                    .Where(x => x.CustomerRoleObj.RoleID == id)
                                    .ToList();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        public List<CustomerRole> GetByCustomer(int id)
        {
            var customerRolesWithCodes = BuildQuery()
                                    .Where(x => x.CustomerRoleObj.CustomerID == id)
                                    .ToList();
            List<CustomerRole> finalCustomerRoles = ProcessMappings(customerRolesWithCodes);
            return finalCustomerRoles;
        }
        private class QueryDTO
        {
            public CustomerRole CustomerRoleObj { get; set; }
            public Guid? CustomerCode { get; set; } //CustomerID
            public Guid? RoleCode { get; set; } //RoleID
        }
    }
}
