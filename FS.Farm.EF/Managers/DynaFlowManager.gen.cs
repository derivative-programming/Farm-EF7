using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DynaFlowManager
	{
		private readonly FarmDbContext _dbContext;

		public DynaFlowManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DynaFlow> AddAsync(DynaFlow dynaFlow)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowSet.Add(dynaFlow);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlow).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dynaFlow;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DynaFlowSet.Add(dynaFlow);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dynaFlow).State = EntityState.Detached;

				return dynaFlow;

			}
		}

        public DynaFlow Add(DynaFlow dynaFlow)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowSet.Add(dynaFlow);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlow).State = EntityState.Detached;

                    transaction.Commit();

                    return dynaFlow;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DynaFlowSet.Add(dynaFlow);
                _dbContext.SaveChanges();
                _dbContext.Entry(dynaFlow).State = EntityState.Detached;

                return dynaFlow;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DynaFlowSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DynaFlowSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DynaFlowSet.AsNoTracking().MaxAsync(x => (int?)x.DynaFlowID);
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
            int? maxId = _dbContext.DynaFlowSet.AsNoTracking().Max(x => (int?)x.DynaFlowID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DynaFlow> GetByIdAsync(int id)
		{

			var dynaFlowsWithCodes = await BuildQuery()
									.Where(x => x.DynaFlowObj.DynaFlowID == id)
									.ToListAsync();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            if (finalDynaFlows.Count > 0)
			{
				return finalDynaFlows[0];

            }

			return null;

        }

        public DynaFlow GetById(int id)
        {

            var dynaFlowsWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowObj.DynaFlowID == id)
                                    .ToList();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            if (finalDynaFlows.Count > 0)
            {
                return finalDynaFlows[0];

            }

            return null;

        }

        public async Task<DynaFlow> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DynaFlowSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowsWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowObj.DynaFlowID == id)
                                        .ToListAsync();

                List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlows.Count > 0)
                {
                    return finalDynaFlows[0];

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

        public DynaFlow DirtyGetById(int id) //to test
        {
            //return await _dbContext.DynaFlowSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowsWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowObj.DynaFlowID == id)
                                        .ToList();

                List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlows.Count > 0)
                {
                    return finalDynaFlows[0];

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
        public async Task<DynaFlow> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowsWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowObj.Code == code)
                                    .ToListAsync();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            if (finalDynaFlows.Count > 0)
            {
                return finalDynaFlows[0];

            }

            return null;
        }

        public DynaFlow GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowsWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowObj.Code == code)
                                    .ToList();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            if (finalDynaFlows.Count > 0)
            {
                return finalDynaFlows[0];

            }

            return null;
        }

        public async Task<DynaFlow> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowsWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowObj.Code == code)
                                        .ToListAsync();

                List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlows.Count > 0)
                {
                    return finalDynaFlows[0];

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

        public DynaFlow DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowsWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowObj.Code == code)
                                        .ToList();

                List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlows.Count > 0)
                {
                    return finalDynaFlows[0];

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

        public async Task<List<DynaFlow>> GetAllAsync()
		{
            var dynaFlowsWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }
        public List<DynaFlow> GetAll()
        {
            var dynaFlowsWithCodes = BuildQuery()
                                    .ToList();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }

        public async Task<bool> UpdateAsync(DynaFlow dynaFlowToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowSet.Attach(dynaFlowToUpdate);
					_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;

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
					_dbContext.DynaFlowSet.Attach(dynaFlowToUpdate);
					_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DynaFlow dynaFlowToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowSet.Attach(dynaFlowToUpdate);
                    _dbContext.Entry(dynaFlowToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;

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
                    _dbContext.DynaFlowSet.Attach(dynaFlowToUpdate);
                    _dbContext.Entry(dynaFlowToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;

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
					var dynaFlow = await _dbContext.DynaFlowSet.FirstOrDefaultAsync(x => x.DynaFlowID == id);
					if (dynaFlow == null) return false;

					_dbContext.DynaFlowSet.Remove(dynaFlow);
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
					var dynaFlow = await _dbContext.DynaFlowSet.FirstOrDefaultAsync(x => x.DynaFlowID == id);
					if (dynaFlow == null) return false;

					_dbContext.DynaFlowSet.Remove(dynaFlow);
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
                    var dynaFlow = _dbContext.DynaFlowSet.FirstOrDefault(x => x.DynaFlowID == id);
                    if (dynaFlow == null) return false;

                    _dbContext.DynaFlowSet.Remove(dynaFlow);
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
                    var dynaFlow = _dbContext.DynaFlowSet.FirstOrDefault(x => x.DynaFlowID == id);
                    if (dynaFlow == null) return false;

                    _dbContext.DynaFlowSet.Remove(dynaFlow);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DynaFlow> dynaFlows)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dynaFlow in dynaFlows)
			{
				dynaFlow.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dynaFlow);
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
					await _dbContext.BulkInsertAsync(dynaFlows, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dynaFlows, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DynaFlow> dynaFlows)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dynaFlow in dynaFlows)
            {
                dynaFlow.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dynaFlow);
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
                    _dbContext.BulkInsert(dynaFlows, bulkConfig);

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
                _dbContext.BulkInsert(dynaFlows, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DynaFlow> updatedDynaFlows)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDynaFlows.Select(x => x.DynaFlowID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DynaFlowSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DynaFlowID))
			//	.Select(p => new { p.DynaFlowID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DynaFlowID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDynaFlow in updatedDynaFlows)
			{
				//	if (!existingTokens.TryGetValue(updatedDynaFlow.DynaFlowID, out var token) || token != updatedDynaFlow.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDynaFlow.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DynaFlowSet.Attach(updatedDynaFlow);
				//	_dbContext.Entry(updatedDynaFlow).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDynaFlow);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DynaFlowSet.Attach(updatedDynaFlow);
				//_dbContext.Entry(updatedDynaFlow).State = EntityState.Modified;
				//_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDynaFlow.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDynaFlows, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDynaFlows, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DynaFlow> updatedDynaFlows)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDynaFlows.Select(x => x.DynaFlowID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DynaFlowSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DynaFlowID))
            //	.Select(p => new { p.DynaFlowID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DynaFlowID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDynaFlow in updatedDynaFlows)
            {
                //	if (!existingTokens.TryGetValue(updatedDynaFlow.DynaFlowID, out var token) || token != updatedDynaFlow.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDynaFlow.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DynaFlowSet.Attach(updatedDynaFlow);
                //	_dbContext.Entry(updatedDynaFlow).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDynaFlow);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DynaFlowSet.Attach(updatedDynaFlow);
                //_dbContext.Entry(updatedDynaFlow).State = EntityState.Modified;
                //_dbContext.Entry(dynaFlowToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDynaFlow.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dynaFlowToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDynaFlows, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDynaFlows, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DynaFlow> dynaFlowsToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dynaFlowsToDelete.Select(x => x.DynaFlowID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DynaFlowSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DynaFlowID))
				.Select(p => new { p.DynaFlowID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DynaFlowID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDynaFlow in dynaFlowsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDynaFlow.DynaFlowID, out var token) || token != updatedDynaFlow.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDynaFlow.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dynaFlowsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dynaFlowsToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DynaFlow> dynaFlowsToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DynaFlow.DynaFlowID), nameof(DynaFlow.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dynaFlowsToDelete.Select(x => x.DynaFlowID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DynaFlowSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DynaFlowID))
                .Select(p => new { p.DynaFlowID, p.LastChangeCode })
                .ToDictionary(x => x.DynaFlowID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDynaFlow in dynaFlowsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDynaFlow.DynaFlowID, out var token) || token != updatedDynaFlow.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDynaFlow.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dynaFlowsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dynaFlowsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dynaFlow in _dbContext.DynaFlowSet.AsNoTracking()
				   from dynaFlowType in _dbContext.DynaFlowTypeSet.AsNoTracking().Where(f => f.DynaFlowTypeID == dynaFlow.DynaFlowTypeID).DefaultIfEmpty() //DynaFlowTypeID
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dynaFlow.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DynaFlowObj = dynaFlow,
					   DynaFlowTypeCode = dynaFlowType.Code, //DynaFlowTypeID
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
                    var dynaFlow = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dynaFlow != null)
                    {
                        found = true;
                        Delete(dynaFlow.DynaFlowID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dynaFlow = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dynaFlow != null)
                    {
                        found = true;
                        Delete(dynaFlow.DynaFlowID);
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
                    var dynaFlow = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dynaFlow != null)
                    {
                        found = true;
                        Delete(dynaFlow.DynaFlowID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DynaFlow> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DynaFlowObj.DynaFlowTypeCodePeek = item.DynaFlowTypeCode.Value; //DynaFlowTypeID
                item.DynaFlowObj.PacCodePeek = item.PacCode.Value; //PacID
            }

            List<DynaFlow> results = data.Select(r => r.DynaFlowObj).ToList();

            return results;
        }
        //DynaFlowTypeID
        public async Task<List<DynaFlow>> GetByDynaFlowTypeIDAsync(int id)
        {
            var dynaFlowsWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowObj.DynaFlowTypeID == id)
                                    .ToListAsync();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }
        //PacID
        public async Task<List<DynaFlow>> GetByPacIDAsync(int id)
        {
            var dynaFlowsWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowObj.PacID == id)
                                    .ToListAsync();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }
        //DynaFlowTypeID
        public List<DynaFlow> GetByDynaFlowTypeID(int id)
        {
            var dynaFlowsWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowObj.DynaFlowTypeID == id)
                                    .ToList();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }
        //PacID
        public List<DynaFlow> GetByPacID(int id)
        {
            var dynaFlowsWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowObj.PacID == id)
                                    .ToList();

            List<DynaFlow> finalDynaFlows = ProcessMappings(dynaFlowsWithCodes);

            return finalDynaFlows;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DynaFlow DynaFlowObj { get; set; }
            public Guid? DynaFlowTypeCode { get; set; } //DynaFlowTypeID
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
