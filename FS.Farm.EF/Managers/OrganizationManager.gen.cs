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
        public Organization Add(Organization organization)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrganizationSet.Add(organization);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(organization).State = EntityState.Detached;
                    transaction.Commit();
                    return organization;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.OrganizationSet.Add(organization);
                _dbContext.SaveChanges();
                _dbContext.Entry(organization).State = EntityState.Detached;
                return organization;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.OrganizationSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.OrganizationSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.OrganizationSet.AsNoTracking().MaxAsync(x => (int?)x.OrganizationID);
        }
        public int? GetMaxId()
        {
            return _dbContext.OrganizationSet.AsNoTracking().Max(x => (int?)x.OrganizationID);
        }
        public async Task<Organization> GetByIdAsync(int id)
		{
			var organizationsWithCodes = await BuildQuery()
									.Where(x => x.OrganizationObj.OrganizationID == id)
									.ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            if (finalOrganizations.Count > 0)
			{
				return finalOrganizations[0];
            }
			return null;
        }
        public Organization GetById(int id)
        {
            var organizationsWithCodes = BuildQuery()
                                    .Where(x => x.OrganizationObj.OrganizationID == id)
                                    .ToList();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            if (finalOrganizations.Count > 0)
            {
                return finalOrganizations[0];
            }
            return null;
        }
        public async Task<Organization> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.OrganizationSet.AsNoTracking().FirstOrDefaultAsync(x => x.OrganizationID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var organizationsWithCodes = await BuildQuery()
                                        .Where(x => x.OrganizationObj.OrganizationID == id)
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
        public Organization DirtyGetById(int id) //to test
        {
            //return await _dbContext.OrganizationSet.AsNoTracking().FirstOrDefaultAsync(x => x.OrganizationID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var organizationsWithCodes = BuildQuery()
                                        .Where(x => x.OrganizationObj.OrganizationID == id)
                                        .ToList();
                List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalOrganizations.Count > 0)
                {
                    return finalOrganizations[0];
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
        public async Task<Organization> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var organizationsWithCodes = await BuildQuery()
                                    .Where(x => x.OrganizationObj.Code == code)
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            if (finalOrganizations.Count > 0)
            {
                return finalOrganizations[0];
            }
            return null;
        }
        public Organization GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var organizationsWithCodes = BuildQuery()
                                    .Where(x => x.OrganizationObj.Code == code)
                                    .ToList();
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
                                        .Where(x => x.OrganizationObj.Code == code)
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
        public Organization DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var organizationsWithCodes = BuildQuery()
                                        .Where(x => x.OrganizationObj.Code == code)
                                        .ToList();
                List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalOrganizations.Count > 0)
                {
                    return finalOrganizations[0];
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
        public async Task<List<Organization>> GetAllAsync()
		{
            var organizationsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            return finalOrganizations;
        }
        public List<Organization> GetAll()
        {
            var organizationsWithCodes = BuildQuery()
                                    .ToList();
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
        public bool Update(Organization organizationToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrganizationSet.Attach(organizationToUpdate);
                    _dbContext.Entry(organizationToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(organizationToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    organizationToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(organizationToUpdate).State = EntityState.Detached;
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
                    _dbContext.OrganizationSet.Attach(organizationToUpdate);
                    _dbContext.Entry(organizationToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(organizationToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    organizationToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
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
					var organization = await _dbContext.OrganizationSet.FirstOrDefaultAsync(x => x.OrganizationID == id);
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
					var organization = await _dbContext.OrganizationSet.FirstOrDefaultAsync(x => x.OrganizationID == id);
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
        public bool Delete(int id)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    var organization = _dbContext.OrganizationSet.FirstOrDefault(x => x.OrganizationID == id);
                    if (organization == null) return false;
                    _dbContext.OrganizationSet.Remove(organization);
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
                    var organization = _dbContext.OrganizationSet.FirstOrDefault(x => x.OrganizationID == id);
                    if (organization == null) return false;
                    _dbContext.OrganizationSet.Remove(organization);
                    _dbContext.SaveChanges();
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
        public void BulkInsert(IEnumerable<Organization> organizations)
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
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkInsert(organizations, bulkConfig);
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
                _dbContext.BulkInsert(organizations, bulkConfig);
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
			var idsToUpdate = updatedOrganizations.Select(x => x.OrganizationID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.OrganizationSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.OrganizationID))
			//	.Select(p => new { p.OrganizationID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.OrganizationID, x => x.LastChangeCode);
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
        public void BulkUpdate(IEnumerable<Organization> updatedOrganizations)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Organization.OrganizationID), nameof(Organization.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedOrganizations.Select(x => x.OrganizationID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.OrganizationSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.OrganizationID))
            //	.Select(p => new { p.OrganizationID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.OrganizationID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedOrganizations, bulkConfig);
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
                _dbContext.BulkUpdate(updatedOrganizations, bulkConfig);
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
			var idsToUpdate = organizationsToDelete.Select(x => x.OrganizationID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.OrganizationSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.OrganizationID))
				.Select(p => new { p.OrganizationID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.OrganizationID, x => x.LastChangeCode);
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
        public void BulkDelete(IEnumerable<Organization> organizationsToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Organization.OrganizationID), nameof(Organization.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = organizationsToDelete.Select(x => x.OrganizationID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.OrganizationSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.OrganizationID))
                .Select(p => new { p.OrganizationID, p.LastChangeCode })
                .ToDictionary(x => x.OrganizationID, x => x.LastChangeCode);
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
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(organizationsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(organizationsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from organization in _dbContext.OrganizationSet.AsNoTracking()
				   from tac in _dbContext.TacSet.AsNoTracking().Where(l => l.TacID == organization.TacID).DefaultIfEmpty() //TacID
				   select new QueryDTO
                   {
					   OrganizationObj = organization,
					   TacCode = tac.Code, //TacID
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
                    var organization = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (organization != null)
                    {
                        found = true;
                        Delete(organization.OrganizationID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var organization = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (organization != null)
                    {
                        found = true;
                        Delete(organization.OrganizationID);
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
                    var organization = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (organization != null)
                    {
                        found = true;
                        Delete(organization.OrganizationID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<Organization> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.OrganizationObj.TacCodePeek = item.TacCode.Value; //TacID
            }
            List<Organization> results = data.Select(r => r.OrganizationObj).ToList();
            return results;
        }
        //TacID
        public async Task<List<Organization>> GetByTacAsync(int id)
        {
            var organizationsWithCodes = await BuildQuery()
                                    .Where(x => x.OrganizationObj.TacID == id)
                                    .ToListAsync();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            return finalOrganizations;
        }
        //TacID
        public List<Organization> GetByTac(int id)
        {
            var organizationsWithCodes = BuildQuery()
                                    .Where(x => x.OrganizationObj.TacID == id)
                                    .ToList();
            List<Organization> finalOrganizations = ProcessMappings(organizationsWithCodes);
            return finalOrganizations;
        }
        private class QueryDTO
        {
            public Organization OrganizationObj { get; set; }
            public Guid? TacCode { get; set; } //TacID
        }
    }
}
