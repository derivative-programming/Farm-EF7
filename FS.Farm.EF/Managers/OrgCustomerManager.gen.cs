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
        public OrgCustomer Add(OrgCustomer orgCustomer)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrgCustomerSet.Add(orgCustomer);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(orgCustomer).State = EntityState.Detached;
                    transaction.Commit();
                    return orgCustomer;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.OrgCustomerSet.Add(orgCustomer);
                _dbContext.SaveChanges();
                _dbContext.Entry(orgCustomer).State = EntityState.Detached;
                return orgCustomer;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.OrgCustomerSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.OrgCustomerSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.OrgCustomerSet.AsNoTracking().MaxAsync(x => (int?)x.OrgCustomerID);
        }
        public int? GetMaxId()
        {
            return _dbContext.OrgCustomerSet.AsNoTracking().Max(x => (int?)x.OrgCustomerID);
        }
        public async Task<OrgCustomer> GetByIdAsync(int id)
		{
			var orgCustomersWithCodes = await BuildQuery()
									.Where(x => x.OrgCustomerObj.OrgCustomerID == id)
									.ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            if (finalOrgCustomers.Count > 0)
			{
				return finalOrgCustomers[0];
            }
			return null;
        }
        public OrgCustomer GetById(int id)
        {
            var orgCustomersWithCodes = BuildQuery()
                                    .Where(x => x.OrgCustomerObj.OrgCustomerID == id)
                                    .ToList();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            if (finalOrgCustomers.Count > 0)
            {
                return finalOrgCustomers[0];
            }
            return null;
        }
        public async Task<OrgCustomer> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.OrgCustomerSet.AsNoTracking().FirstOrDefaultAsync(x => x.OrgCustomerID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var orgCustomersWithCodes = await BuildQuery()
                                        .Where(x => x.OrgCustomerObj.OrgCustomerID == id)
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
        public OrgCustomer DirtyGetById(int id) //to test
        {
            //return await _dbContext.OrgCustomerSet.AsNoTracking().FirstOrDefaultAsync(x => x.OrgCustomerID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var orgCustomersWithCodes = BuildQuery()
                                        .Where(x => x.OrgCustomerObj.OrgCustomerID == id)
                                        .ToList();
                List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalOrgCustomers.Count > 0)
                {
                    return finalOrgCustomers[0];
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
        public async Task<OrgCustomer> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(x => x.OrgCustomerObj.Code == code)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            if (finalOrgCustomers.Count > 0)
            {
                return finalOrgCustomers[0];
            }
            return null;
        }
        public OrgCustomer GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var orgCustomersWithCodes = BuildQuery()
                                    .Where(x => x.OrgCustomerObj.Code == code)
                                    .ToList();
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
                                        .Where(x => x.OrgCustomerObj.Code == code)
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
        public OrgCustomer DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var orgCustomersWithCodes = BuildQuery()
                                        .Where(x => x.OrgCustomerObj.Code == code)
                                        .ToList();
                List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalOrgCustomers.Count > 0)
                {
                    return finalOrgCustomers[0];
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
        public async Task<List<OrgCustomer>> GetAllAsync()
		{
            var orgCustomersWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        public List<OrgCustomer> GetAll()
        {
            var orgCustomersWithCodes = BuildQuery()
                                    .ToList();
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
        public bool Update(OrgCustomer orgCustomerToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrgCustomerSet.Attach(orgCustomerToUpdate);
                    _dbContext.Entry(orgCustomerToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(orgCustomerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    orgCustomerToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(orgCustomerToUpdate).State = EntityState.Detached;
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
                    _dbContext.OrgCustomerSet.Attach(orgCustomerToUpdate);
                    _dbContext.Entry(orgCustomerToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(orgCustomerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    orgCustomerToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
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
					var orgCustomer = await _dbContext.OrgCustomerSet.FirstOrDefaultAsync(x => x.OrgCustomerID == id);
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
					var orgCustomer = await _dbContext.OrgCustomerSet.FirstOrDefaultAsync(x => x.OrgCustomerID == id);
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
        public bool Delete(int id)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    var orgCustomer = _dbContext.OrgCustomerSet.FirstOrDefault(x => x.OrgCustomerID == id);
                    if (orgCustomer == null) return false;
                    _dbContext.OrgCustomerSet.Remove(orgCustomer);
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
                    var orgCustomer = _dbContext.OrgCustomerSet.FirstOrDefault(x => x.OrgCustomerID == id);
                    if (orgCustomer == null) return false;
                    _dbContext.OrgCustomerSet.Remove(orgCustomer);
                    _dbContext.SaveChanges();
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
        public void BulkInsert(IEnumerable<OrgCustomer> orgCustomers)
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
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkInsert(orgCustomers, bulkConfig);
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
                _dbContext.BulkInsert(orgCustomers, bulkConfig);
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
			var idsToUpdate = updatedOrgCustomers.Select(x => x.OrgCustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.OrgCustomerSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.OrgCustomerID))
			//	.Select(p => new { p.OrgCustomerID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.OrgCustomerID, x => x.LastChangeCode);
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
        public void BulkUpdate(IEnumerable<OrgCustomer> updatedOrgCustomers)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(OrgCustomer.OrgCustomerID), nameof(OrgCustomer.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedOrgCustomers.Select(x => x.OrgCustomerID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.OrgCustomerSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.OrgCustomerID))
            //	.Select(p => new { p.OrgCustomerID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.OrgCustomerID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedOrgCustomers, bulkConfig);
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
                _dbContext.BulkUpdate(updatedOrgCustomers, bulkConfig);
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
			var idsToUpdate = orgCustomersToDelete.Select(x => x.OrgCustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.OrgCustomerSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.OrgCustomerID))
				.Select(p => new { p.OrgCustomerID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.OrgCustomerID, x => x.LastChangeCode);
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
        public void BulkDelete(IEnumerable<OrgCustomer> orgCustomersToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(OrgCustomer.OrgCustomerID), nameof(OrgCustomer.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = orgCustomersToDelete.Select(x => x.OrgCustomerID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.OrgCustomerSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.OrgCustomerID))
                .Select(p => new { p.OrgCustomerID, p.LastChangeCode })
                .ToDictionary(x => x.OrgCustomerID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(orgCustomersToDelete, bulkConfig);
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
                _dbContext.BulkDelete(orgCustomersToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from orgCustomer in _dbContext.OrgCustomerSet.AsNoTracking()
				   from customer in _dbContext.CustomerSet.AsNoTracking().Where(f => f.CustomerID == orgCustomer.CustomerID).DefaultIfEmpty() //CustomerID
				   from organization in _dbContext.OrganizationSet.AsNoTracking().Where(l => l.OrganizationID == orgCustomer.OrganizationID).DefaultIfEmpty() //OrganizationID
				   select new QueryDTO
                   {
					   OrgCustomerObj = orgCustomer,
					   CustomerCode = customer.Code, //CustomerID
					   OrganizationCode = organization.Code, //OrganizationID
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
                    var orgCustomer = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (orgCustomer != null)
                    {
                        found = true;
                        Delete(orgCustomer.OrgCustomerID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var orgCustomer = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (orgCustomer != null)
                    {
                        found = true;
                        Delete(orgCustomer.OrgCustomerID);
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
                    var orgCustomer = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (orgCustomer != null)
                    {
                        found = true;
                        Delete(orgCustomer.OrgCustomerID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<OrgCustomer> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.OrgCustomerObj.CustomerCodePeek = item.CustomerCode.Value; //CustomerID
                item.OrgCustomerObj.OrganizationCodePeek = item.OrganizationCode.Value; //OrganizationID
            }
            List<OrgCustomer> results = data.Select(r => r.OrgCustomerObj).ToList();
            return results;
        }
        //CustomerID
        public async Task<List<OrgCustomer>> GetByCustomerAsync(int id)
        {
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(x => x.OrgCustomerObj.CustomerID == id)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        //OrganizationID
        public async Task<List<OrgCustomer>> GetByOrganizationAsync(int id)
        {
            var orgCustomersWithCodes = await BuildQuery()
                                    .Where(x => x.OrgCustomerObj.OrganizationID == id)
                                    .ToListAsync();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        //CustomerID
        public List<OrgCustomer> GetByCustomer(int id)
        {
            var orgCustomersWithCodes = BuildQuery()
                                    .Where(x => x.OrgCustomerObj.CustomerID == id)
                                    .ToList();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        //OrganizationID
        public List<OrgCustomer> GetByOrganization(int id)
        {
            var orgCustomersWithCodes = BuildQuery()
                                    .Where(x => x.OrgCustomerObj.OrganizationID == id)
                                    .ToList();
            List<OrgCustomer> finalOrgCustomers = ProcessMappings(orgCustomersWithCodes);
            return finalOrgCustomers;
        }
        private class QueryDTO
        {
            public OrgCustomer OrgCustomerObj { get; set; }
            public Guid? CustomerCode { get; set; } //CustomerID
            public Guid? OrganizationCode { get; set; } //OrganizationID
        }
    }
}
