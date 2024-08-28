using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DynaFlowTaskManager
	{
		private readonly FarmDbContext _dbContext;

		public DynaFlowTaskManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DynaFlowTask> AddAsync(DynaFlowTask dynaFlowTask)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTaskSet.Add(dynaFlowTask);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTask).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dynaFlowTask;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DynaFlowTaskSet.Add(dynaFlowTask);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dynaFlowTask).State = EntityState.Detached;

				return dynaFlowTask;

			}
		}

        public DynaFlowTask Add(DynaFlowTask dynaFlowTask)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTaskSet.Add(dynaFlowTask);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTask).State = EntityState.Detached;

                    transaction.Commit();

                    return dynaFlowTask;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DynaFlowTaskSet.Add(dynaFlowTask);
                _dbContext.SaveChanges();
                _dbContext.Entry(dynaFlowTask).State = EntityState.Detached;

                return dynaFlowTask;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DynaFlowTaskSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DynaFlowTaskSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DynaFlowTaskSet.AsNoTracking().MaxAsync(x => (int?)x.DynaFlowTaskID);
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
            int? maxId = _dbContext.DynaFlowTaskSet.AsNoTracking().Max(x => (int?)x.DynaFlowTaskID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DynaFlowTask> GetByIdAsync(int id)
		{

			var dynaFlowTasksWithCodes = await BuildQuery()
									.Where(x => x.DynaFlowTaskObj.DynaFlowTaskID == id)
									.ToListAsync();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            if (finalDynaFlowTasks.Count > 0)
			{
				return finalDynaFlowTasks[0];

            }

			return null;

        }

        public DynaFlowTask GetById(int id)
        {

            var dynaFlowTasksWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.DynaFlowTaskID == id)
                                    .ToList();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            if (finalDynaFlowTasks.Count > 0)
            {
                return finalDynaFlowTasks[0];

            }

            return null;

        }

        public async Task<DynaFlowTask> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DynaFlowTaskSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTaskID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTasksWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTaskObj.DynaFlowTaskID == id)
                                        .ToListAsync();

                List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTasks.Count > 0)
                {
                    return finalDynaFlowTasks[0];

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

        public DynaFlowTask DirtyGetById(int id) //to test
        {
            //return await _dbContext.DynaFlowTaskSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTaskID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTasksWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTaskObj.DynaFlowTaskID == id)
                                        .ToList();

                List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTasks.Count > 0)
                {
                    return finalDynaFlowTasks[0];

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
        public async Task<DynaFlowTask> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTasksWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.Code == code)
                                    .ToListAsync();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            if (finalDynaFlowTasks.Count > 0)
            {
                return finalDynaFlowTasks[0];

            }

            return null;
        }

        public DynaFlowTask GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTasksWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.Code == code)
                                    .ToList();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            if (finalDynaFlowTasks.Count > 0)
            {
                return finalDynaFlowTasks[0];

            }

            return null;
        }

        public async Task<DynaFlowTask> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTasksWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTaskObj.Code == code)
                                        .ToListAsync();

                List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTasks.Count > 0)
                {
                    return finalDynaFlowTasks[0];

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

        public DynaFlowTask DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTasksWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTaskObj.Code == code)
                                        .ToList();

                List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTasks.Count > 0)
                {
                    return finalDynaFlowTasks[0];

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

        public async Task<List<DynaFlowTask>> GetAllAsync()
		{
            var dynaFlowTasksWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }
        public List<DynaFlowTask> GetAll()
        {
            var dynaFlowTasksWithCodes = BuildQuery()
                                    .ToList();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }

        public async Task<bool> UpdateAsync(DynaFlowTask dynaFlowTaskToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTaskSet.Attach(dynaFlowTaskToUpdate);
					_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTaskToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;

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
					_dbContext.DynaFlowTaskSet.Attach(dynaFlowTaskToUpdate);
					_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTaskToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DynaFlowTask dynaFlowTaskToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTaskSet.Attach(dynaFlowTaskToUpdate);
                    _dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTaskToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;

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
                    _dbContext.DynaFlowTaskSet.Attach(dynaFlowTaskToUpdate);
                    _dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTaskToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;

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
					var dynaFlowTask = await _dbContext.DynaFlowTaskSet.FirstOrDefaultAsync(x => x.DynaFlowTaskID == id);
					if (dynaFlowTask == null) return false;

					_dbContext.DynaFlowTaskSet.Remove(dynaFlowTask);
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
					var dynaFlowTask = await _dbContext.DynaFlowTaskSet.FirstOrDefaultAsync(x => x.DynaFlowTaskID == id);
					if (dynaFlowTask == null) return false;

					_dbContext.DynaFlowTaskSet.Remove(dynaFlowTask);
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
                    var dynaFlowTask = _dbContext.DynaFlowTaskSet.FirstOrDefault(x => x.DynaFlowTaskID == id);
                    if (dynaFlowTask == null) return false;

                    _dbContext.DynaFlowTaskSet.Remove(dynaFlowTask);
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
                    var dynaFlowTask = _dbContext.DynaFlowTaskSet.FirstOrDefault(x => x.DynaFlowTaskID == id);
                    if (dynaFlowTask == null) return false;

                    _dbContext.DynaFlowTaskSet.Remove(dynaFlowTask);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DynaFlowTask> dynaFlowTasks)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dynaFlowTask in dynaFlowTasks)
			{
				dynaFlowTask.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dynaFlowTask);
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
					await _dbContext.BulkInsertAsync(dynaFlowTasks, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dynaFlowTasks, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DynaFlowTask> dynaFlowTasks)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dynaFlowTask in dynaFlowTasks)
            {
                dynaFlowTask.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dynaFlowTask);
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
                    _dbContext.BulkInsert(dynaFlowTasks, bulkConfig);

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
                _dbContext.BulkInsert(dynaFlowTasks, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DynaFlowTask> updatedDynaFlowTasks)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDynaFlowTasks.Select(x => x.DynaFlowTaskID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DynaFlowTaskSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DynaFlowTaskID))
			//	.Select(p => new { p.DynaFlowTaskID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DynaFlowTaskID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDynaFlowTask in updatedDynaFlowTasks)
			{
				//	if (!existingTokens.TryGetValue(updatedDynaFlowTask.DynaFlowTaskID, out var token) || token != updatedDynaFlowTask.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDynaFlowTask.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DynaFlowTaskSet.Attach(updatedDynaFlowTask);
				//	_dbContext.Entry(updatedDynaFlowTask).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDynaFlowTask);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DynaFlowTaskSet.Attach(updatedDynaFlowTask);
				//_dbContext.Entry(updatedDynaFlowTask).State = EntityState.Modified;
				//_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDynaFlowTask.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDynaFlowTasks, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDynaFlowTasks, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DynaFlowTask> updatedDynaFlowTasks)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDynaFlowTasks.Select(x => x.DynaFlowTaskID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DynaFlowTaskSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DynaFlowTaskID))
            //	.Select(p => new { p.DynaFlowTaskID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DynaFlowTaskID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDynaFlowTask in updatedDynaFlowTasks)
            {
                //	if (!existingTokens.TryGetValue(updatedDynaFlowTask.DynaFlowTaskID, out var token) || token != updatedDynaFlowTask.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDynaFlowTask.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DynaFlowTaskSet.Attach(updatedDynaFlowTask);
                //	_dbContext.Entry(updatedDynaFlowTask).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDynaFlowTask);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DynaFlowTaskSet.Attach(updatedDynaFlowTask);
                //_dbContext.Entry(updatedDynaFlowTask).State = EntityState.Modified;
                //_dbContext.Entry(dynaFlowTaskToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDynaFlowTask.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dynaFlowTaskToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDynaFlowTasks, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDynaFlowTasks, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DynaFlowTask> dynaFlowTasksToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dynaFlowTasksToDelete.Select(x => x.DynaFlowTaskID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DynaFlowTaskSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DynaFlowTaskID))
				.Select(p => new { p.DynaFlowTaskID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DynaFlowTaskID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDynaFlowTask in dynaFlowTasksToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDynaFlowTask.DynaFlowTaskID, out var token) || token != updatedDynaFlowTask.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDynaFlowTask.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dynaFlowTasksToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dynaFlowTasksToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DynaFlowTask> dynaFlowTasksToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DynaFlowTask.DynaFlowTaskID), nameof(DynaFlowTask.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dynaFlowTasksToDelete.Select(x => x.DynaFlowTaskID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DynaFlowTaskSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DynaFlowTaskID))
                .Select(p => new { p.DynaFlowTaskID, p.LastChangeCode })
                .ToDictionary(x => x.DynaFlowTaskID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDynaFlowTask in dynaFlowTasksToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDynaFlowTask.DynaFlowTaskID, out var token) || token != updatedDynaFlowTask.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDynaFlowTask.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dynaFlowTasksToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dynaFlowTasksToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dynaFlowTask in _dbContext.DynaFlowTaskSet.AsNoTracking()
				   from dynaFlow in _dbContext.DynaFlowSet.AsNoTracking().Where(l => l.DynaFlowID == dynaFlowTask.DynaFlowID).DefaultIfEmpty() //DynaFlowID
																																		   from dynaFlowTaskType in _dbContext.DynaFlowTaskTypeSet.AsNoTracking().Where(f => f.DynaFlowTaskTypeID == dynaFlowTask.DynaFlowTaskTypeID).DefaultIfEmpty() //DynaFlowTaskTypeID
				   select new QueryDTO
                   {
					   DynaFlowTaskObj = dynaFlowTask,
					   DynaFlowCode = dynaFlow.Code, //DynaFlowID
											 					   DynaFlowTaskTypeCode = dynaFlowTaskType.Code, //DynaFlowTaskTypeID
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
                    var dynaFlowTask = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dynaFlowTask != null)
                    {
                        found = true;
                        Delete(dynaFlowTask.DynaFlowTaskID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dynaFlowTask = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dynaFlowTask != null)
                    {
                        found = true;
                        Delete(dynaFlowTask.DynaFlowTaskID);
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
                    var dynaFlowTask = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dynaFlowTask != null)
                    {
                        found = true;
                        Delete(dynaFlowTask.DynaFlowTaskID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DynaFlowTask> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DynaFlowTaskObj.DynaFlowCodePeek = item.DynaFlowCode.Value; //DynaFlowID
                item.DynaFlowTaskObj.DynaFlowTaskTypeCodePeek = item.DynaFlowTaskTypeCode.Value; //DynaFlowTaskTypeID
                            }

            List<DynaFlowTask> results = data.Select(r => r.DynaFlowTaskObj).ToList();

            return results;
        }
        //DynaFlowID
        public async Task<List<DynaFlowTask>> GetByDynaFlowIDAsync(int id)
        {
            var dynaFlowTasksWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.DynaFlowID == id)
                                    .ToListAsync();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }
        //DynaFlowTaskTypeID
        public async Task<List<DynaFlowTask>> GetByDynaFlowTaskTypeIDAsync(int id)
        {
            var dynaFlowTasksWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.DynaFlowTaskTypeID == id)
                                    .ToListAsync();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }
        //DynaFlowID
        public List<DynaFlowTask> GetByDynaFlowID(int id)
        {
            var dynaFlowTasksWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.DynaFlowID == id)
                                    .ToList();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }
        //DynaFlowTaskTypeID
        public List<DynaFlowTask> GetByDynaFlowTaskTypeID(int id)
        {
            var dynaFlowTasksWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskObj.DynaFlowTaskTypeID == id)
                                    .ToList();

            List<DynaFlowTask> finalDynaFlowTasks = ProcessMappings(dynaFlowTasksWithCodes);

            return finalDynaFlowTasks;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DynaFlowTask DynaFlowTaskObj { get; set; }
            public Guid? DynaFlowCode { get; set; } //DynaFlowID
            public Guid? DynaFlowTaskTypeCode { get; set; } //DynaFlowTaskTypeID
        }

    }
}
