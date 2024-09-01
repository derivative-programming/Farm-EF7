using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DynaFlowTypeScheduleManager
	{
		private readonly FarmDbContext _dbContext;

		public DynaFlowTypeScheduleManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DynaFlowTypeSchedule> AddAsync(DynaFlowTypeSchedule dynaFlowTypeSchedule)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTypeSchedule).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dynaFlowTypeSchedule;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dynaFlowTypeSchedule).State = EntityState.Detached;

				return dynaFlowTypeSchedule;

			}
		}

        public DynaFlowTypeSchedule Add(DynaFlowTypeSchedule dynaFlowTypeSchedule)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTypeSchedule).State = EntityState.Detached;

                    transaction.Commit();

                    return dynaFlowTypeSchedule;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                _dbContext.SaveChanges();
                _dbContext.Entry(dynaFlowTypeSchedule).State = EntityState.Detached;

                return dynaFlowTypeSchedule;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().MaxAsync(x => (int?)x.DynaFlowTypeScheduleID);
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
            int? maxId = _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().Max(x => (int?)x.DynaFlowTypeScheduleID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DynaFlowTypeSchedule> GetByIdAsync(int id)
		{

			var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
									.Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeScheduleID == id)
									.ToListAsync();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            if (finalDynaFlowTypeSchedules.Count > 0)
			{
				return finalDynaFlowTypeSchedules[0];

            }

			return null;

        }

        public DynaFlowTypeSchedule GetById(int id)
        {

            var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeScheduleID == id)
                                    .ToList();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            if (finalDynaFlowTypeSchedules.Count > 0)
            {
                return finalDynaFlowTypeSchedules[0];

            }

            return null;

        }

        public async Task<DynaFlowTypeSchedule> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTypeScheduleID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeScheduleID == id)
                                        .ToListAsync();

                List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTypeSchedules.Count > 0)
                {
                    return finalDynaFlowTypeSchedules[0];

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

        public DynaFlowTypeSchedule DirtyGetById(int id) //to test
        {
            //return await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTypeScheduleID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeScheduleID == id)
                                        .ToList();

                List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTypeSchedules.Count > 0)
                {
                    return finalDynaFlowTypeSchedules[0];

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
        public async Task<DynaFlowTypeSchedule> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.Code == code)
                                    .ToListAsync();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            if (finalDynaFlowTypeSchedules.Count > 0)
            {
                return finalDynaFlowTypeSchedules[0];

            }

            return null;
        }

        public DynaFlowTypeSchedule GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.Code == code)
                                    .ToList();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            if (finalDynaFlowTypeSchedules.Count > 0)
            {
                return finalDynaFlowTypeSchedules[0];

            }

            return null;
        }

        public async Task<DynaFlowTypeSchedule> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTypeScheduleObj.Code == code)
                                        .ToListAsync();

                List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTypeSchedules.Count > 0)
                {
                    return finalDynaFlowTypeSchedules[0];

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

        public DynaFlowTypeSchedule DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTypeScheduleObj.Code == code)
                                        .ToList();

                List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTypeSchedules.Count > 0)
                {
                    return finalDynaFlowTypeSchedules[0];

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

        public async Task<List<DynaFlowTypeSchedule>> GetAllAsync()
		{
            var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }
        public List<DynaFlowTypeSchedule> GetAll()
        {
            var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                    .ToList();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }

        public async Task<bool> UpdateAsync(DynaFlowTypeSchedule dynaFlowTypeScheduleToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTypeScheduleSet.Attach(dynaFlowTypeScheduleToUpdate);
					_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTypeScheduleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;

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
					_dbContext.DynaFlowTypeScheduleSet.Attach(dynaFlowTypeScheduleToUpdate);
					_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTypeScheduleToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DynaFlowTypeSchedule dynaFlowTypeScheduleToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTypeScheduleSet.Attach(dynaFlowTypeScheduleToUpdate);
                    _dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTypeScheduleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;

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
                    _dbContext.DynaFlowTypeScheduleSet.Attach(dynaFlowTypeScheduleToUpdate);
                    _dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTypeScheduleToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;

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
					var dynaFlowTypeSchedule = await _dbContext.DynaFlowTypeScheduleSet.FirstOrDefaultAsync(x => x.DynaFlowTypeScheduleID == id);
					if (dynaFlowTypeSchedule == null) return false;

					_dbContext.DynaFlowTypeScheduleSet.Remove(dynaFlowTypeSchedule);
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
					var dynaFlowTypeSchedule = await _dbContext.DynaFlowTypeScheduleSet.FirstOrDefaultAsync(x => x.DynaFlowTypeScheduleID == id);
					if (dynaFlowTypeSchedule == null) return false;

					_dbContext.DynaFlowTypeScheduleSet.Remove(dynaFlowTypeSchedule);
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
                    var dynaFlowTypeSchedule = _dbContext.DynaFlowTypeScheduleSet.FirstOrDefault(x => x.DynaFlowTypeScheduleID == id);
                    if (dynaFlowTypeSchedule == null) return false;

                    _dbContext.DynaFlowTypeScheduleSet.Remove(dynaFlowTypeSchedule);
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
                    var dynaFlowTypeSchedule = _dbContext.DynaFlowTypeScheduleSet.FirstOrDefault(x => x.DynaFlowTypeScheduleID == id);
                    if (dynaFlowTypeSchedule == null) return false;

                    _dbContext.DynaFlowTypeScheduleSet.Remove(dynaFlowTypeSchedule);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DynaFlowTypeSchedule> dynaFlowTypeSchedules)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
			{
				dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dynaFlowTypeSchedule);
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
					await _dbContext.BulkInsertAsync(dynaFlowTypeSchedules, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dynaFlowTypeSchedules, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DynaFlowTypeSchedule> dynaFlowTypeSchedules)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
            {
                dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dynaFlowTypeSchedule);
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
                    _dbContext.BulkInsert(dynaFlowTypeSchedules, bulkConfig);

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
                _dbContext.BulkInsert(dynaFlowTypeSchedules, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DynaFlowTypeSchedule> updatedDynaFlowTypeSchedules)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDynaFlowTypeSchedules.Select(x => x.DynaFlowTypeScheduleID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DynaFlowTypeScheduleID))
			//	.Select(p => new { p.DynaFlowTypeScheduleID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DynaFlowTypeScheduleID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDynaFlowTypeSchedule in updatedDynaFlowTypeSchedules)
			{
				//	if (!existingTokens.TryGetValue(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID, out var token) || token != updatedDynaFlowTypeSchedule.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DynaFlowTypeScheduleSet.Attach(updatedDynaFlowTypeSchedule);
				//	_dbContext.Entry(updatedDynaFlowTypeSchedule).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDynaFlowTypeSchedule);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DynaFlowTypeScheduleSet.Attach(updatedDynaFlowTypeSchedule);
				//_dbContext.Entry(updatedDynaFlowTypeSchedule).State = EntityState.Modified;
				//_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDynaFlowTypeSchedules, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDynaFlowTypeSchedules, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DynaFlowTypeSchedule> updatedDynaFlowTypeSchedules)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDynaFlowTypeSchedules.Select(x => x.DynaFlowTypeScheduleID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DynaFlowTypeScheduleID))
            //	.Select(p => new { p.DynaFlowTypeScheduleID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DynaFlowTypeScheduleID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDynaFlowTypeSchedule in updatedDynaFlowTypeSchedules)
            {
                //	if (!existingTokens.TryGetValue(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID, out var token) || token != updatedDynaFlowTypeSchedule.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DynaFlowTypeScheduleSet.Attach(updatedDynaFlowTypeSchedule);
                //	_dbContext.Entry(updatedDynaFlowTypeSchedule).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDynaFlowTypeSchedule);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DynaFlowTypeScheduleSet.Attach(updatedDynaFlowTypeSchedule);
                //_dbContext.Entry(updatedDynaFlowTypeSchedule).State = EntityState.Modified;
                //_dbContext.Entry(dynaFlowTypeScheduleToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dynaFlowTypeScheduleToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDynaFlowTypeSchedules, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDynaFlowTypeSchedules, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DynaFlowTypeSchedule> dynaFlowTypeSchedulesToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dynaFlowTypeSchedulesToDelete.Select(x => x.DynaFlowTypeScheduleID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DynaFlowTypeScheduleSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DynaFlowTypeScheduleID))
				.Select(p => new { p.DynaFlowTypeScheduleID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DynaFlowTypeScheduleID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDynaFlowTypeSchedule in dynaFlowTypeSchedulesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID, out var token) || token != updatedDynaFlowTypeSchedule.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dynaFlowTypeSchedulesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dynaFlowTypeSchedulesToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DynaFlowTypeSchedule> dynaFlowTypeSchedulesToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DynaFlowTypeSchedule.DynaFlowTypeScheduleID), nameof(DynaFlowTypeSchedule.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dynaFlowTypeSchedulesToDelete.Select(x => x.DynaFlowTypeScheduleID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DynaFlowTypeScheduleSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DynaFlowTypeScheduleID))
                .Select(p => new { p.DynaFlowTypeScheduleID, p.LastChangeCode })
                .ToDictionary(x => x.DynaFlowTypeScheduleID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDynaFlowTypeSchedule in dynaFlowTypeSchedulesToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID, out var token) || token != updatedDynaFlowTypeSchedule.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dynaFlowTypeSchedulesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dynaFlowTypeSchedulesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dynaFlowTypeSchedule in _dbContext.DynaFlowTypeScheduleSet.AsNoTracking()
				   from dynaFlowType in _dbContext.DynaFlowTypeSet.AsNoTracking().Where(f => f.DynaFlowTypeID == dynaFlowTypeSchedule.DynaFlowTypeID).DefaultIfEmpty() //DynaFlowTypeID
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dynaFlowTypeSchedule.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DynaFlowTypeScheduleObj = dynaFlowTypeSchedule,
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
                    var dynaFlowTypeSchedule = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dynaFlowTypeSchedule != null)
                    {
                        found = true;
                        Delete(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dynaFlowTypeSchedule = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dynaFlowTypeSchedule != null)
                    {
                        found = true;
                        Delete(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
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
                    var dynaFlowTypeSchedule = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dynaFlowTypeSchedule != null)
                    {
                        found = true;
                        Delete(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DynaFlowTypeSchedule> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DynaFlowTypeScheduleObj.DynaFlowTypeCodePeek = item.DynaFlowTypeCode.Value; //DynaFlowTypeID
                item.DynaFlowTypeScheduleObj.PacCodePeek = item.PacCode.Value; //PacID
            }

            List<DynaFlowTypeSchedule> results = data.Select(r => r.DynaFlowTypeScheduleObj).ToList();

            return results;
        }
        //DynaFlowTypeID
        public async Task<List<DynaFlowTypeSchedule>> GetByDynaFlowTypeIDAsync(int id)
        {
            var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeID == id)
                                    .ToListAsync();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }
        //PacID
        public async Task<List<DynaFlowTypeSchedule>> GetByPacIDAsync(int id)
        {
            var dynaFlowTypeSchedulesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.PacID == id)
                                    .ToListAsync();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }
        //DynaFlowTypeID
        public List<DynaFlowTypeSchedule> GetByDynaFlowTypeID(int id)
        {
            var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.DynaFlowTypeID == id)
                                    .ToList();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }
        //PacID
        public List<DynaFlowTypeSchedule> GetByPacID(int id)
        {
            var dynaFlowTypeSchedulesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeScheduleObj.PacID == id)
                                    .ToList();

            List<DynaFlowTypeSchedule> finalDynaFlowTypeSchedules = ProcessMappings(dynaFlowTypeSchedulesWithCodes);

            return finalDynaFlowTypeSchedules;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DynaFlowTypeSchedule DynaFlowTypeScheduleObj { get; set; }
            public Guid? DynaFlowTypeCode { get; set; } //DynaFlowTypeID
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
