using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DFTDependencyManager
	{
		private readonly FarmDbContext _dbContext;

		public DFTDependencyManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DFTDependency> AddAsync(DFTDependency dFTDependency)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DFTDependencySet.Add(dFTDependency);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFTDependency).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dFTDependency;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DFTDependencySet.Add(dFTDependency);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dFTDependency).State = EntityState.Detached;

				return dFTDependency;

			}
		}

        public DFTDependency Add(DFTDependency dFTDependency)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DFTDependencySet.Add(dFTDependency);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFTDependency).State = EntityState.Detached;

                    transaction.Commit();

                    return dFTDependency;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DFTDependencySet.Add(dFTDependency);
                _dbContext.SaveChanges();
                _dbContext.Entry(dFTDependency).State = EntityState.Detached;

                return dFTDependency;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DFTDependencySet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DFTDependencySet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DFTDependencySet.AsNoTracking().MaxAsync(x => (int?)x.DFTDependencyID);
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
            int? maxId = _dbContext.DFTDependencySet.AsNoTracking().Max(x => (int?)x.DFTDependencyID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DFTDependency> GetByIdAsync(int id)
		{

			var dFTDependencysWithCodes = await BuildQuery()
									.Where(x => x.DFTDependencyObj.DFTDependencyID == id)
									.ToListAsync();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            if (finalDFTDependencys.Count > 0)
			{
				return finalDFTDependencys[0];

            }

			return null;

        }

        public DFTDependency GetById(int id)
        {

            var dFTDependencysWithCodes = BuildQuery()
                                    .Where(x => x.DFTDependencyObj.DFTDependencyID == id)
                                    .ToList();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            if (finalDFTDependencys.Count > 0)
            {
                return finalDFTDependencys[0];

            }

            return null;

        }

        public async Task<DFTDependency> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DFTDependencySet.AsNoTracking().FirstOrDefaultAsync(x => x.DFTDependencyID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dFTDependencysWithCodes = await BuildQuery()
                                        .Where(x => x.DFTDependencyObj.DFTDependencyID == id)
                                        .ToListAsync();

                List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDFTDependencys.Count > 0)
                {
                    return finalDFTDependencys[0];

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

        public DFTDependency DirtyGetById(int id) //to test
        {
            //return await _dbContext.DFTDependencySet.AsNoTracking().FirstOrDefaultAsync(x => x.DFTDependencyID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dFTDependencysWithCodes = BuildQuery()
                                        .Where(x => x.DFTDependencyObj.DFTDependencyID == id)
                                        .ToList();

                List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDFTDependencys.Count > 0)
                {
                    return finalDFTDependencys[0];

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
        public async Task<DFTDependency> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dFTDependencysWithCodes = await BuildQuery()
                                    .Where(x => x.DFTDependencyObj.Code == code)
                                    .ToListAsync();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            if (finalDFTDependencys.Count > 0)
            {
                return finalDFTDependencys[0];

            }

            return null;
        }

        public DFTDependency GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dFTDependencysWithCodes = BuildQuery()
                                    .Where(x => x.DFTDependencyObj.Code == code)
                                    .ToList();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            if (finalDFTDependencys.Count > 0)
            {
                return finalDFTDependencys[0];

            }

            return null;
        }

        public async Task<DFTDependency> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dFTDependencysWithCodes = await BuildQuery()
                                        .Where(x => x.DFTDependencyObj.Code == code)
                                        .ToListAsync();

                List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDFTDependencys.Count > 0)
                {
                    return finalDFTDependencys[0];

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

        public DFTDependency DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dFTDependencysWithCodes = BuildQuery()
                                        .Where(x => x.DFTDependencyObj.Code == code)
                                        .ToList();

                List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDFTDependencys.Count > 0)
                {
                    return finalDFTDependencys[0];

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

        public async Task<List<DFTDependency>> GetAllAsync()
		{
            var dFTDependencysWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            return finalDFTDependencys;
        }
        public List<DFTDependency> GetAll()
        {
            var dFTDependencysWithCodes = BuildQuery()
                                    .ToList();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            return finalDFTDependencys;
        }

        public async Task<bool> UpdateAsync(DFTDependency dFTDependencyToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DFTDependencySet.Attach(dFTDependencyToUpdate);
					_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dFTDependencyToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;

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
					_dbContext.DFTDependencySet.Attach(dFTDependencyToUpdate);
					_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dFTDependencyToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DFTDependency dFTDependencyToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DFTDependencySet.Attach(dFTDependencyToUpdate);
                    _dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dFTDependencyToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;

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
                    _dbContext.DFTDependencySet.Attach(dFTDependencyToUpdate);
                    _dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dFTDependencyToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;

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
					var dFTDependency = await _dbContext.DFTDependencySet.FirstOrDefaultAsync(x => x.DFTDependencyID == id);
					if (dFTDependency == null) return false;

					_dbContext.DFTDependencySet.Remove(dFTDependency);
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
					var dFTDependency = await _dbContext.DFTDependencySet.FirstOrDefaultAsync(x => x.DFTDependencyID == id);
					if (dFTDependency == null) return false;

					_dbContext.DFTDependencySet.Remove(dFTDependency);
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
                    var dFTDependency = _dbContext.DFTDependencySet.FirstOrDefault(x => x.DFTDependencyID == id);
                    if (dFTDependency == null) return false;

                    _dbContext.DFTDependencySet.Remove(dFTDependency);
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
                    var dFTDependency = _dbContext.DFTDependencySet.FirstOrDefault(x => x.DFTDependencyID == id);
                    if (dFTDependency == null) return false;

                    _dbContext.DFTDependencySet.Remove(dFTDependency);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DFTDependency> dFTDependencys)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dFTDependency in dFTDependencys)
			{
				dFTDependency.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dFTDependency);
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
					await _dbContext.BulkInsertAsync(dFTDependencys, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dFTDependencys, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DFTDependency> dFTDependencys)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dFTDependency in dFTDependencys)
            {
                dFTDependency.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dFTDependency);
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
                    _dbContext.BulkInsert(dFTDependencys, bulkConfig);

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
                _dbContext.BulkInsert(dFTDependencys, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DFTDependency> updatedDFTDependencys)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDFTDependencys.Select(x => x.DFTDependencyID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DFTDependencySet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DFTDependencyID))
			//	.Select(p => new { p.DFTDependencyID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DFTDependencyID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDFTDependency in updatedDFTDependencys)
			{
				//	if (!existingTokens.TryGetValue(updatedDFTDependency.DFTDependencyID, out var token) || token != updatedDFTDependency.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDFTDependency.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DFTDependencySet.Attach(updatedDFTDependency);
				//	_dbContext.Entry(updatedDFTDependency).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDFTDependency);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DFTDependencySet.Attach(updatedDFTDependency);
				//_dbContext.Entry(updatedDFTDependency).State = EntityState.Modified;
				//_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDFTDependency.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDFTDependencys, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDFTDependencys, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DFTDependency> updatedDFTDependencys)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDFTDependencys.Select(x => x.DFTDependencyID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DFTDependencySet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DFTDependencyID))
            //	.Select(p => new { p.DFTDependencyID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DFTDependencyID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDFTDependency in updatedDFTDependencys)
            {
                //	if (!existingTokens.TryGetValue(updatedDFTDependency.DFTDependencyID, out var token) || token != updatedDFTDependency.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDFTDependency.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DFTDependencySet.Attach(updatedDFTDependency);
                //	_dbContext.Entry(updatedDFTDependency).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDFTDependency);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DFTDependencySet.Attach(updatedDFTDependency);
                //_dbContext.Entry(updatedDFTDependency).State = EntityState.Modified;
                //_dbContext.Entry(dFTDependencyToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDFTDependency.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dFTDependencyToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDFTDependencys, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDFTDependencys, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DFTDependency> dFTDependencysToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dFTDependencysToDelete.Select(x => x.DFTDependencyID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DFTDependencySet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DFTDependencyID))
				.Select(p => new { p.DFTDependencyID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DFTDependencyID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDFTDependency in dFTDependencysToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDFTDependency.DFTDependencyID, out var token) || token != updatedDFTDependency.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDFTDependency.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dFTDependencysToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dFTDependencysToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DFTDependency> dFTDependencysToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DFTDependency.DFTDependencyID), nameof(DFTDependency.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dFTDependencysToDelete.Select(x => x.DFTDependencyID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DFTDependencySet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DFTDependencyID))
                .Select(p => new { p.DFTDependencyID, p.LastChangeCode })
                .ToDictionary(x => x.DFTDependencyID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDFTDependency in dFTDependencysToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDFTDependency.DFTDependencyID, out var token) || token != updatedDFTDependency.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDFTDependency.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dFTDependencysToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dFTDependencysToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dFTDependency in _dbContext.DFTDependencySet.AsNoTracking()
				   from dynaFlowTask in _dbContext.DynaFlowTaskSet.AsNoTracking().Where(l => l.DynaFlowTaskID == dFTDependency.DynaFlowTaskID).DefaultIfEmpty() //DynaFlowTaskID
																																		   select new QueryDTO
                   {
					   DFTDependencyObj = dFTDependency,
					   DynaFlowTaskCode = dynaFlowTask.Code, //DynaFlowTaskID
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
                    var dFTDependency = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dFTDependency != null)
                    {
                        found = true;
                        Delete(dFTDependency.DFTDependencyID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dFTDependency = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dFTDependency != null)
                    {
                        found = true;
                        Delete(dFTDependency.DFTDependencyID);
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
                    var dFTDependency = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dFTDependency != null)
                    {
                        found = true;
                        Delete(dFTDependency.DFTDependencyID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DFTDependency> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DFTDependencyObj.DynaFlowTaskCodePeek = item.DynaFlowTaskCode.Value; //DynaFlowTaskID
                            }

            List<DFTDependency> results = data.Select(r => r.DFTDependencyObj).ToList();

            return results;
        }
        //DynaFlowTaskID
        public async Task<List<DFTDependency>> GetByDynaFlowTaskIDAsync(int id)
        {
            var dFTDependencysWithCodes = await BuildQuery()
                                    .Where(x => x.DFTDependencyObj.DynaFlowTaskID == id)
                                    .ToListAsync();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            return finalDFTDependencys;
        }
        //DynaFlowTaskID
        public List<DFTDependency> GetByDynaFlowTaskID(int id)
        {
            var dFTDependencysWithCodes = BuildQuery()
                                    .Where(x => x.DFTDependencyObj.DynaFlowTaskID == id)
                                    .ToList();

            List<DFTDependency> finalDFTDependencys = ProcessMappings(dFTDependencysWithCodes);

            return finalDFTDependencys;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DFTDependency DFTDependencyObj { get; set; }
            public Guid? DynaFlowTaskCode { get; set; } //DynaFlowTaskID
        }

    }
}
