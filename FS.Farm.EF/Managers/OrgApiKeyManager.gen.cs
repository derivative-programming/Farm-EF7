using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class OrgApiKeyManager
	{
		private readonly FarmDbContext _dbContext;

		public OrgApiKeyManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<OrgApiKey> AddAsync(OrgApiKey orgApiKey)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrgApiKeySet.Add(orgApiKey);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgApiKey).State = EntityState.Detached;

					await transaction.CommitAsync();

					return orgApiKey;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.OrgApiKeySet.Add(orgApiKey);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(orgApiKey).State = EntityState.Detached;

				return orgApiKey;

			}
		}

        public OrgApiKey Add(OrgApiKey orgApiKey)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrgApiKeySet.Add(orgApiKey);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(orgApiKey).State = EntityState.Detached;

                    transaction.Commit();

                    return orgApiKey;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.OrgApiKeySet.Add(orgApiKey);
                _dbContext.SaveChanges();
                _dbContext.Entry(orgApiKey).State = EntityState.Detached;

                return orgApiKey;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.OrgApiKeySet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.OrgApiKeySet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.OrgApiKeySet.AsNoTracking().MaxAsync(x => (int?)x.OrgApiKeyID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }
        public int? GetMaxId()
        {
            int? maxId = _dbContext.OrgApiKeySet.AsNoTracking().Max(x => (int?)x.OrgApiKeyID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<OrgApiKey> GetByIdAsync(int id)
		{

			var orgApiKeysWithCodes = await BuildQuery()
									.Where(x => x.OrgApiKeyObj.OrgApiKeyID == id)
									.ToListAsync();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            if (finalOrgApiKeys.Count > 0)
			{
				return finalOrgApiKeys[0];

            }

			return null;

        }

        public OrgApiKey GetById(int id)
        {

            var orgApiKeysWithCodes = BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.OrgApiKeyID == id)
                                    .ToList();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            if (finalOrgApiKeys.Count > 0)
            {
                return finalOrgApiKeys[0];

            }

            return null;

        }

        public async Task<OrgApiKey> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.OrgApiKeySet.AsNoTracking().FirstOrDefaultAsync(x => x.OrgApiKeyID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var orgApiKeysWithCodes = await BuildQuery()
                                        .Where(x => x.OrgApiKeyObj.OrgApiKeyID == id)
                                        .ToListAsync();

                List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalOrgApiKeys.Count > 0)
                {
                    return finalOrgApiKeys[0];

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

        public OrgApiKey DirtyGetById(int id) //to test
        {
            //return await _dbContext.OrgApiKeySet.AsNoTracking().FirstOrDefaultAsync(x => x.OrgApiKeyID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var orgApiKeysWithCodes = BuildQuery()
                                        .Where(x => x.OrgApiKeyObj.OrgApiKeyID == id)
                                        .ToList();

                List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalOrgApiKeys.Count > 0)
                {
                    return finalOrgApiKeys[0];

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
        public async Task<OrgApiKey> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var orgApiKeysWithCodes = await BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.Code == code)
                                    .ToListAsync();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            if (finalOrgApiKeys.Count > 0)
            {
                return finalOrgApiKeys[0];

            }

            return null;
        }

        public OrgApiKey GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var orgApiKeysWithCodes = BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.Code == code)
                                    .ToList();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            if (finalOrgApiKeys.Count > 0)
            {
                return finalOrgApiKeys[0];

            }

            return null;
        }

        public async Task<OrgApiKey> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var orgApiKeysWithCodes = await BuildQuery()
                                        .Where(x => x.OrgApiKeyObj.Code == code)
                                        .ToListAsync();

                List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalOrgApiKeys.Count > 0)
                {
                    return finalOrgApiKeys[0];

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

        public OrgApiKey DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var orgApiKeysWithCodes = BuildQuery()
                                        .Where(x => x.OrgApiKeyObj.Code == code)
                                        .ToList();

                List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalOrgApiKeys.Count > 0)
                {
                    return finalOrgApiKeys[0];

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

        public async Task<List<OrgApiKey>> GetAllAsync()
		{
            var orgApiKeysWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }
        public List<OrgApiKey> GetAll()
        {
            var orgApiKeysWithCodes = BuildQuery()
                                    .ToList();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }

        public async Task<bool> UpdateAsync(OrgApiKey orgApiKeyToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.OrgApiKeySet.Attach(orgApiKeyToUpdate);
					_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					orgApiKeyToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;

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
					_dbContext.OrgApiKeySet.Attach(orgApiKeyToUpdate);
					_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					orgApiKeyToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(OrgApiKey orgApiKeyToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.OrgApiKeySet.Attach(orgApiKeyToUpdate);
                    _dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    orgApiKeyToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;

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
                    _dbContext.OrgApiKeySet.Attach(orgApiKeyToUpdate);
                    _dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    orgApiKeyToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;

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
					var orgApiKey = await _dbContext.OrgApiKeySet.FirstOrDefaultAsync(x => x.OrgApiKeyID == id);
					if (orgApiKey == null) return false;

					_dbContext.OrgApiKeySet.Remove(orgApiKey);
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
					var orgApiKey = await _dbContext.OrgApiKeySet.FirstOrDefaultAsync(x => x.OrgApiKeyID == id);
					if (orgApiKey == null) return false;

					_dbContext.OrgApiKeySet.Remove(orgApiKey);
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
                    var orgApiKey = _dbContext.OrgApiKeySet.FirstOrDefault(x => x.OrgApiKeyID == id);
                    if (orgApiKey == null) return false;

                    _dbContext.OrgApiKeySet.Remove(orgApiKey);
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
                    var orgApiKey = _dbContext.OrgApiKeySet.FirstOrDefault(x => x.OrgApiKeyID == id);
                    if (orgApiKey == null) return false;

                    _dbContext.OrgApiKeySet.Remove(orgApiKey);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<OrgApiKey> orgApiKeys)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var orgApiKey in orgApiKeys)
			{
				orgApiKey.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(orgApiKey);
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
					await _dbContext.BulkInsertAsync(orgApiKeys, bulkConfig);

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
				await _dbContext.BulkInsertAsync(orgApiKeys, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<OrgApiKey> orgApiKeys)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var orgApiKey in orgApiKeys)
            {
                orgApiKey.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(orgApiKey);
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
                    _dbContext.BulkInsert(orgApiKeys, bulkConfig);

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
                _dbContext.BulkInsert(orgApiKeys, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<OrgApiKey> updatedOrgApiKeys)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedOrgApiKeys.Select(x => x.OrgApiKeyID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.OrgApiKeySet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.OrgApiKeyID))
			//	.Select(p => new { p.OrgApiKeyID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.OrgApiKeyID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedOrgApiKey in updatedOrgApiKeys)
			{
				//	if (!existingTokens.TryGetValue(updatedOrgApiKey.OrgApiKeyID, out var token) || token != updatedOrgApiKey.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedOrgApiKey.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.OrgApiKeySet.Attach(updatedOrgApiKey);
				//	_dbContext.Entry(updatedOrgApiKey).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedOrgApiKey);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.OrgApiKeySet.Attach(updatedOrgApiKey);
				//_dbContext.Entry(updatedOrgApiKey).State = EntityState.Modified;
				//_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedOrgApiKey.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedOrgApiKeys, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedOrgApiKeys, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<OrgApiKey> updatedOrgApiKeys)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedOrgApiKeys.Select(x => x.OrgApiKeyID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.OrgApiKeySet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.OrgApiKeyID))
            //	.Select(p => new { p.OrgApiKeyID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.OrgApiKeyID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedOrgApiKey in updatedOrgApiKeys)
            {
                //	if (!existingTokens.TryGetValue(updatedOrgApiKey.OrgApiKeyID, out var token) || token != updatedOrgApiKey.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedOrgApiKey.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.OrgApiKeySet.Attach(updatedOrgApiKey);
                //	_dbContext.Entry(updatedOrgApiKey).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedOrgApiKey);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.OrgApiKeySet.Attach(updatedOrgApiKey);
                //_dbContext.Entry(updatedOrgApiKey).State = EntityState.Modified;
                //_dbContext.Entry(orgApiKeyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedOrgApiKey.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(orgApiKeyToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedOrgApiKeys, bulkConfig);
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
                _dbContext.BulkUpdate(updatedOrgApiKeys, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<OrgApiKey> orgApiKeysToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = orgApiKeysToDelete.Select(x => x.OrgApiKeyID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.OrgApiKeySet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.OrgApiKeyID))
				.Select(p => new { p.OrgApiKeyID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.OrgApiKeyID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedOrgApiKey in orgApiKeysToDelete)
			{
				if (!existingTokens.TryGetValue(updatedOrgApiKey.OrgApiKeyID, out var token) || token != updatedOrgApiKey.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedOrgApiKey.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(orgApiKeysToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(orgApiKeysToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<OrgApiKey> orgApiKeysToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(OrgApiKey.OrgApiKeyID), nameof(OrgApiKey.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = orgApiKeysToDelete.Select(x => x.OrgApiKeyID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.OrgApiKeySet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.OrgApiKeyID))
                .Select(p => new { p.OrgApiKeyID, p.LastChangeCode })
                .ToDictionary(x => x.OrgApiKeyID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedOrgApiKey in orgApiKeysToDelete)
            {
                if (!existingTokens.TryGetValue(updatedOrgApiKey.OrgApiKeyID, out var token) || token != updatedOrgApiKey.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedOrgApiKey.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(orgApiKeysToDelete, bulkConfig);
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
                _dbContext.BulkDelete(orgApiKeysToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from orgApiKey in _dbContext.OrgApiKeySet.AsNoTracking()
				   from organization in _dbContext.OrganizationSet.AsNoTracking().Where(l => l.OrganizationID == orgApiKey.OrganizationID).DefaultIfEmpty() //OrganizationID
																																		   from orgCustomer in _dbContext.OrgCustomerSet.AsNoTracking().Where(f => f.OrgCustomerID == orgApiKey.OrgCustomerID).DefaultIfEmpty() //OrgCustomerID
				   select new QueryDTO
                   {
					   OrgApiKeyObj = orgApiKey,
					   OrganizationCode = organization.Code, //OrganizationID
											 					   OrgCustomerCode = orgCustomer.Code, //OrgCustomerID
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
                    var orgApiKey = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (orgApiKey != null)
                    {
                        found = true;
                        Delete(orgApiKey.OrgApiKeyID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var orgApiKey = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (orgApiKey != null)
                    {
                        found = true;
                        Delete(orgApiKey.OrgApiKeyID);
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
                    var orgApiKey = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (orgApiKey != null)
                    {
                        found = true;
                        Delete(orgApiKey.OrgApiKeyID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<OrgApiKey> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.OrgApiKeyObj.OrganizationCodePeek = item.OrganizationCode.Value; //OrganizationID
                item.OrgApiKeyObj.OrgCustomerCodePeek = item.OrgCustomerCode.Value; //OrgCustomerID
            }

            List<OrgApiKey> results = data.Select(r => r.OrgApiKeyObj).ToList();

            return results;
        }
        //OrganizationID
        public async Task<List<OrgApiKey>> GetByOrganizationIDAsync(int id)
        {
            var orgApiKeysWithCodes = await BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.OrganizationID == id)
                                    .ToListAsync();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }
        //OrgCustomerID
        public async Task<List<OrgApiKey>> GetByOrgCustomerIDAsync(int id)
        {
            var orgApiKeysWithCodes = await BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.OrgCustomerID == id)
                                    .ToListAsync();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }
        //OrganizationID
        public List<OrgApiKey> GetByOrganizationID(int id)
        {
            var orgApiKeysWithCodes = BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.OrganizationID == id)
                                    .ToList();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }
        //OrgCustomerID
        public List<OrgApiKey> GetByOrgCustomerID(int id)
        {
            var orgApiKeysWithCodes = BuildQuery()
                                    .Where(x => x.OrgApiKeyObj.OrgCustomerID == id)
                                    .ToList();

            List<OrgApiKey> finalOrgApiKeys = ProcessMappings(orgApiKeysWithCodes);

            return finalOrgApiKeys;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public OrgApiKey OrgApiKeyObj { get; set; }
            public Guid? OrganizationCode { get; set; } //OrganizationID
            public Guid? OrgCustomerCode { get; set; } //OrgCustomerID
        }

    }
}
