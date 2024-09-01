using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DFMaintenanceManager
	{
		private readonly FarmDbContext _dbContext;

		public DFMaintenanceManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DFMaintenance> AddAsync(DFMaintenance dFMaintenance)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DFMaintenanceSet.Add(dFMaintenance);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFMaintenance).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dFMaintenance;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DFMaintenanceSet.Add(dFMaintenance);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dFMaintenance).State = EntityState.Detached;

				return dFMaintenance;

			}
		}

        public DFMaintenance Add(DFMaintenance dFMaintenance)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DFMaintenanceSet.Add(dFMaintenance);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFMaintenance).State = EntityState.Detached;

                    transaction.Commit();

                    return dFMaintenance;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DFMaintenanceSet.Add(dFMaintenance);
                _dbContext.SaveChanges();
                _dbContext.Entry(dFMaintenance).State = EntityState.Detached;

                return dFMaintenance;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DFMaintenanceSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DFMaintenanceSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DFMaintenanceSet.AsNoTracking().MaxAsync(x => (int?)x.DFMaintenanceID);
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
            int? maxId = _dbContext.DFMaintenanceSet.AsNoTracking().Max(x => (int?)x.DFMaintenanceID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DFMaintenance> GetByIdAsync(int id)
		{

			var dFMaintenancesWithCodes = await BuildQuery()
									.Where(x => x.DFMaintenanceObj.DFMaintenanceID == id)
									.ToListAsync();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            if (finalDFMaintenances.Count > 0)
			{
				return finalDFMaintenances[0];

            }

			return null;

        }

        public DFMaintenance GetById(int id)
        {

            var dFMaintenancesWithCodes = BuildQuery()
                                    .Where(x => x.DFMaintenanceObj.DFMaintenanceID == id)
                                    .ToList();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            if (finalDFMaintenances.Count > 0)
            {
                return finalDFMaintenances[0];

            }

            return null;

        }

        public async Task<DFMaintenance> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DFMaintenanceSet.AsNoTracking().FirstOrDefaultAsync(x => x.DFMaintenanceID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dFMaintenancesWithCodes = await BuildQuery()
                                        .Where(x => x.DFMaintenanceObj.DFMaintenanceID == id)
                                        .ToListAsync();

                List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDFMaintenances.Count > 0)
                {
                    return finalDFMaintenances[0];

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

        public DFMaintenance DirtyGetById(int id) //to test
        {
            //return await _dbContext.DFMaintenanceSet.AsNoTracking().FirstOrDefaultAsync(x => x.DFMaintenanceID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dFMaintenancesWithCodes = BuildQuery()
                                        .Where(x => x.DFMaintenanceObj.DFMaintenanceID == id)
                                        .ToList();

                List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDFMaintenances.Count > 0)
                {
                    return finalDFMaintenances[0];

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
        public async Task<DFMaintenance> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dFMaintenancesWithCodes = await BuildQuery()
                                    .Where(x => x.DFMaintenanceObj.Code == code)
                                    .ToListAsync();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            if (finalDFMaintenances.Count > 0)
            {
                return finalDFMaintenances[0];

            }

            return null;
        }

        public DFMaintenance GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dFMaintenancesWithCodes = BuildQuery()
                                    .Where(x => x.DFMaintenanceObj.Code == code)
                                    .ToList();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            if (finalDFMaintenances.Count > 0)
            {
                return finalDFMaintenances[0];

            }

            return null;
        }

        public async Task<DFMaintenance> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dFMaintenancesWithCodes = await BuildQuery()
                                        .Where(x => x.DFMaintenanceObj.Code == code)
                                        .ToListAsync();

                List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDFMaintenances.Count > 0)
                {
                    return finalDFMaintenances[0];

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

        public DFMaintenance DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dFMaintenancesWithCodes = BuildQuery()
                                        .Where(x => x.DFMaintenanceObj.Code == code)
                                        .ToList();

                List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDFMaintenances.Count > 0)
                {
                    return finalDFMaintenances[0];

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

        public async Task<List<DFMaintenance>> GetAllAsync()
		{
            var dFMaintenancesWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            return finalDFMaintenances;
        }
        public List<DFMaintenance> GetAll()
        {
            var dFMaintenancesWithCodes = BuildQuery()
                                    .ToList();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            return finalDFMaintenances;
        }

        public async Task<bool> UpdateAsync(DFMaintenance dFMaintenanceToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DFMaintenanceSet.Attach(dFMaintenanceToUpdate);
					_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dFMaintenanceToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;

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
					_dbContext.DFMaintenanceSet.Attach(dFMaintenanceToUpdate);
					_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dFMaintenanceToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DFMaintenance dFMaintenanceToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DFMaintenanceSet.Attach(dFMaintenanceToUpdate);
                    _dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dFMaintenanceToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;

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
                    _dbContext.DFMaintenanceSet.Attach(dFMaintenanceToUpdate);
                    _dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dFMaintenanceToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;

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
					var dFMaintenance = await _dbContext.DFMaintenanceSet.FirstOrDefaultAsync(x => x.DFMaintenanceID == id);
					if (dFMaintenance == null) return false;

					_dbContext.DFMaintenanceSet.Remove(dFMaintenance);
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
					var dFMaintenance = await _dbContext.DFMaintenanceSet.FirstOrDefaultAsync(x => x.DFMaintenanceID == id);
					if (dFMaintenance == null) return false;

					_dbContext.DFMaintenanceSet.Remove(dFMaintenance);
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
                    var dFMaintenance = _dbContext.DFMaintenanceSet.FirstOrDefault(x => x.DFMaintenanceID == id);
                    if (dFMaintenance == null) return false;

                    _dbContext.DFMaintenanceSet.Remove(dFMaintenance);
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
                    var dFMaintenance = _dbContext.DFMaintenanceSet.FirstOrDefault(x => x.DFMaintenanceID == id);
                    if (dFMaintenance == null) return false;

                    _dbContext.DFMaintenanceSet.Remove(dFMaintenance);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DFMaintenance> dFMaintenances)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dFMaintenance in dFMaintenances)
			{
				dFMaintenance.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dFMaintenance);
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
					await _dbContext.BulkInsertAsync(dFMaintenances, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dFMaintenances, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DFMaintenance> dFMaintenances)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dFMaintenance in dFMaintenances)
            {
                dFMaintenance.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dFMaintenance);
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
                    _dbContext.BulkInsert(dFMaintenances, bulkConfig);

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
                _dbContext.BulkInsert(dFMaintenances, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DFMaintenance> updatedDFMaintenances)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDFMaintenances.Select(x => x.DFMaintenanceID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DFMaintenanceSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DFMaintenanceID))
			//	.Select(p => new { p.DFMaintenanceID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DFMaintenanceID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDFMaintenance in updatedDFMaintenances)
			{
				//	if (!existingTokens.TryGetValue(updatedDFMaintenance.DFMaintenanceID, out var token) || token != updatedDFMaintenance.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDFMaintenance.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DFMaintenanceSet.Attach(updatedDFMaintenance);
				//	_dbContext.Entry(updatedDFMaintenance).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDFMaintenance);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DFMaintenanceSet.Attach(updatedDFMaintenance);
				//_dbContext.Entry(updatedDFMaintenance).State = EntityState.Modified;
				//_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDFMaintenance.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDFMaintenances, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDFMaintenances, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DFMaintenance> updatedDFMaintenances)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDFMaintenances.Select(x => x.DFMaintenanceID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DFMaintenanceSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DFMaintenanceID))
            //	.Select(p => new { p.DFMaintenanceID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DFMaintenanceID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDFMaintenance in updatedDFMaintenances)
            {
                //	if (!existingTokens.TryGetValue(updatedDFMaintenance.DFMaintenanceID, out var token) || token != updatedDFMaintenance.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDFMaintenance.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DFMaintenanceSet.Attach(updatedDFMaintenance);
                //	_dbContext.Entry(updatedDFMaintenance).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDFMaintenance);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DFMaintenanceSet.Attach(updatedDFMaintenance);
                //_dbContext.Entry(updatedDFMaintenance).State = EntityState.Modified;
                //_dbContext.Entry(dFMaintenanceToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDFMaintenance.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dFMaintenanceToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDFMaintenances, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDFMaintenances, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DFMaintenance> dFMaintenancesToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dFMaintenancesToDelete.Select(x => x.DFMaintenanceID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DFMaintenanceSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DFMaintenanceID))
				.Select(p => new { p.DFMaintenanceID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DFMaintenanceID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDFMaintenance in dFMaintenancesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDFMaintenance.DFMaintenanceID, out var token) || token != updatedDFMaintenance.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDFMaintenance.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dFMaintenancesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dFMaintenancesToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DFMaintenance> dFMaintenancesToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DFMaintenance.DFMaintenanceID), nameof(DFMaintenance.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dFMaintenancesToDelete.Select(x => x.DFMaintenanceID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DFMaintenanceSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DFMaintenanceID))
                .Select(p => new { p.DFMaintenanceID, p.LastChangeCode })
                .ToDictionary(x => x.DFMaintenanceID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDFMaintenance in dFMaintenancesToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDFMaintenance.DFMaintenanceID, out var token) || token != updatedDFMaintenance.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDFMaintenance.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dFMaintenancesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dFMaintenancesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dFMaintenance in _dbContext.DFMaintenanceSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dFMaintenance.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DFMaintenanceObj = dFMaintenance,
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
                    var dFMaintenance = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dFMaintenance != null)
                    {
                        found = true;
                        Delete(dFMaintenance.DFMaintenanceID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dFMaintenance = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dFMaintenance != null)
                    {
                        found = true;
                        Delete(dFMaintenance.DFMaintenanceID);
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
                    var dFMaintenance = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dFMaintenance != null)
                    {
                        found = true;
                        Delete(dFMaintenance.DFMaintenanceID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DFMaintenance> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DFMaintenanceObj.PacCodePeek = item.PacCode.Value; //PacID
            }

            List<DFMaintenance> results = data.Select(r => r.DFMaintenanceObj).ToList();

            return results;
        }
        //PacID
        public async Task<List<DFMaintenance>> GetByPacIDAsync(int id)
        {
            var dFMaintenancesWithCodes = await BuildQuery()
                                    .Where(x => x.DFMaintenanceObj.PacID == id)
                                    .ToListAsync();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            return finalDFMaintenances;
        }
        //PacID
        public List<DFMaintenance> GetByPacID(int id)
        {
            var dFMaintenancesWithCodes = BuildQuery()
                                    .Where(x => x.DFMaintenanceObj.PacID == id)
                                    .ToList();

            List<DFMaintenance> finalDFMaintenances = ProcessMappings(dFMaintenancesWithCodes);

            return finalDFMaintenances;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DFMaintenance DFMaintenanceObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
