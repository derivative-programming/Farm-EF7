using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DynaFlowTaskTypeManager
	{
		private readonly FarmDbContext _dbContext;

		public DynaFlowTaskTypeManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DynaFlowTaskType> AddAsync(DynaFlowTaskType dynaFlowTaskType)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTaskType).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dynaFlowTaskType;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dynaFlowTaskType).State = EntityState.Detached;

				return dynaFlowTaskType;

			}
		}

        public DynaFlowTaskType Add(DynaFlowTaskType dynaFlowTaskType)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTaskType).State = EntityState.Detached;

                    transaction.Commit();

                    return dynaFlowTaskType;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                _dbContext.SaveChanges();
                _dbContext.Entry(dynaFlowTaskType).State = EntityState.Detached;

                return dynaFlowTaskType;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DynaFlowTaskTypeSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DynaFlowTaskTypeSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DynaFlowTaskTypeSet.AsNoTracking().MaxAsync(x => (int?)x.DynaFlowTaskTypeID);
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
            int? maxId = _dbContext.DynaFlowTaskTypeSet.AsNoTracking().Max(x => (int?)x.DynaFlowTaskTypeID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DynaFlowTaskType> GetByIdAsync(int id)
		{

			var dynaFlowTaskTypesWithCodes = await BuildQuery()
									.Where(x => x.DynaFlowTaskTypeObj.DynaFlowTaskTypeID == id)
									.ToListAsync();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            if (finalDynaFlowTaskTypes.Count > 0)
			{
				return finalDynaFlowTaskTypes[0];

            }

			return null;

        }

        public DynaFlowTaskType GetById(int id)
        {

            var dynaFlowTaskTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskTypeObj.DynaFlowTaskTypeID == id)
                                    .ToList();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            if (finalDynaFlowTaskTypes.Count > 0)
            {
                return finalDynaFlowTaskTypes[0];

            }

            return null;

        }

        public async Task<DynaFlowTaskType> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DynaFlowTaskTypeSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTaskTypeID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTaskTypesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTaskTypeObj.DynaFlowTaskTypeID == id)
                                        .ToListAsync();

                List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTaskTypes.Count > 0)
                {
                    return finalDynaFlowTaskTypes[0];

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

        public DynaFlowTaskType DirtyGetById(int id) //to test
        {
            //return await _dbContext.DynaFlowTaskTypeSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTaskTypeID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTaskTypesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTaskTypeObj.DynaFlowTaskTypeID == id)
                                        .ToList();

                List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTaskTypes.Count > 0)
                {
                    return finalDynaFlowTaskTypes[0];

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
        public async Task<DynaFlowTaskType> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTaskTypesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTaskTypeObj.Code == code)
                                    .ToListAsync();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            if (finalDynaFlowTaskTypes.Count > 0)
            {
                return finalDynaFlowTaskTypes[0];

            }

            return null;
        }

        public DynaFlowTaskType GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTaskTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskTypeObj.Code == code)
                                    .ToList();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            if (finalDynaFlowTaskTypes.Count > 0)
            {
                return finalDynaFlowTaskTypes[0];

            }

            return null;
        }

        public async Task<DynaFlowTaskType> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTaskTypesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTaskTypeObj.Code == code)
                                        .ToListAsync();

                List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTaskTypes.Count > 0)
                {
                    return finalDynaFlowTaskTypes[0];

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

        public DynaFlowTaskType DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTaskTypesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTaskTypeObj.Code == code)
                                        .ToList();

                List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTaskTypes.Count > 0)
                {
                    return finalDynaFlowTaskTypes[0];

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

        public async Task<List<DynaFlowTaskType>> GetAllAsync()
		{
            var dynaFlowTaskTypesWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            return finalDynaFlowTaskTypes;
        }
        public List<DynaFlowTaskType> GetAll()
        {
            var dynaFlowTaskTypesWithCodes = BuildQuery()
                                    .ToList();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            return finalDynaFlowTaskTypes;
        }

        public async Task<bool> UpdateAsync(DynaFlowTaskType dynaFlowTaskTypeToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTaskTypeSet.Attach(dynaFlowTaskTypeToUpdate);
					_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTaskTypeToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;

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
					_dbContext.DynaFlowTaskTypeSet.Attach(dynaFlowTaskTypeToUpdate);
					_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTaskTypeToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DynaFlowTaskType dynaFlowTaskTypeToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTaskTypeSet.Attach(dynaFlowTaskTypeToUpdate);
                    _dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTaskTypeToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;

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
                    _dbContext.DynaFlowTaskTypeSet.Attach(dynaFlowTaskTypeToUpdate);
                    _dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTaskTypeToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;

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
					var dynaFlowTaskType = await _dbContext.DynaFlowTaskTypeSet.FirstOrDefaultAsync(x => x.DynaFlowTaskTypeID == id);
					if (dynaFlowTaskType == null) return false;

					_dbContext.DynaFlowTaskTypeSet.Remove(dynaFlowTaskType);
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
					var dynaFlowTaskType = await _dbContext.DynaFlowTaskTypeSet.FirstOrDefaultAsync(x => x.DynaFlowTaskTypeID == id);
					if (dynaFlowTaskType == null) return false;

					_dbContext.DynaFlowTaskTypeSet.Remove(dynaFlowTaskType);
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
                    var dynaFlowTaskType = _dbContext.DynaFlowTaskTypeSet.FirstOrDefault(x => x.DynaFlowTaskTypeID == id);
                    if (dynaFlowTaskType == null) return false;

                    _dbContext.DynaFlowTaskTypeSet.Remove(dynaFlowTaskType);
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
                    var dynaFlowTaskType = _dbContext.DynaFlowTaskTypeSet.FirstOrDefault(x => x.DynaFlowTaskTypeID == id);
                    if (dynaFlowTaskType == null) return false;

                    _dbContext.DynaFlowTaskTypeSet.Remove(dynaFlowTaskType);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DynaFlowTaskType> dynaFlowTaskTypes)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
			{
				dynaFlowTaskType.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dynaFlowTaskType);
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
					await _dbContext.BulkInsertAsync(dynaFlowTaskTypes, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dynaFlowTaskTypes, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DynaFlowTaskType> dynaFlowTaskTypes)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
            {
                dynaFlowTaskType.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dynaFlowTaskType);
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
                    _dbContext.BulkInsert(dynaFlowTaskTypes, bulkConfig);

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
                _dbContext.BulkInsert(dynaFlowTaskTypes, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DynaFlowTaskType> updatedDynaFlowTaskTypes)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDynaFlowTaskTypes.Select(x => x.DynaFlowTaskTypeID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DynaFlowTaskTypeSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DynaFlowTaskTypeID))
			//	.Select(p => new { p.DynaFlowTaskTypeID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DynaFlowTaskTypeID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDynaFlowTaskType in updatedDynaFlowTaskTypes)
			{
				//	if (!existingTokens.TryGetValue(updatedDynaFlowTaskType.DynaFlowTaskTypeID, out var token) || token != updatedDynaFlowTaskType.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DynaFlowTaskTypeSet.Attach(updatedDynaFlowTaskType);
				//	_dbContext.Entry(updatedDynaFlowTaskType).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDynaFlowTaskType);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DynaFlowTaskTypeSet.Attach(updatedDynaFlowTaskType);
				//_dbContext.Entry(updatedDynaFlowTaskType).State = EntityState.Modified;
				//_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDynaFlowTaskTypes, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDynaFlowTaskTypes, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DynaFlowTaskType> updatedDynaFlowTaskTypes)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDynaFlowTaskTypes.Select(x => x.DynaFlowTaskTypeID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DynaFlowTaskTypeSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DynaFlowTaskTypeID))
            //	.Select(p => new { p.DynaFlowTaskTypeID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DynaFlowTaskTypeID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDynaFlowTaskType in updatedDynaFlowTaskTypes)
            {
                //	if (!existingTokens.TryGetValue(updatedDynaFlowTaskType.DynaFlowTaskTypeID, out var token) || token != updatedDynaFlowTaskType.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DynaFlowTaskTypeSet.Attach(updatedDynaFlowTaskType);
                //	_dbContext.Entry(updatedDynaFlowTaskType).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDynaFlowTaskType);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DynaFlowTaskTypeSet.Attach(updatedDynaFlowTaskType);
                //_dbContext.Entry(updatedDynaFlowTaskType).State = EntityState.Modified;
                //_dbContext.Entry(dynaFlowTaskTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dynaFlowTaskTypeToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDynaFlowTaskTypes, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDynaFlowTaskTypes, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DynaFlowTaskType> dynaFlowTaskTypesToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dynaFlowTaskTypesToDelete.Select(x => x.DynaFlowTaskTypeID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DynaFlowTaskTypeSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DynaFlowTaskTypeID))
				.Select(p => new { p.DynaFlowTaskTypeID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DynaFlowTaskTypeID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDynaFlowTaskType in dynaFlowTaskTypesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDynaFlowTaskType.DynaFlowTaskTypeID, out var token) || token != updatedDynaFlowTaskType.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dynaFlowTaskTypesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dynaFlowTaskTypesToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DynaFlowTaskType> dynaFlowTaskTypesToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DynaFlowTaskType.DynaFlowTaskTypeID), nameof(DynaFlowTaskType.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dynaFlowTaskTypesToDelete.Select(x => x.DynaFlowTaskTypeID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DynaFlowTaskTypeSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DynaFlowTaskTypeID))
                .Select(p => new { p.DynaFlowTaskTypeID, p.LastChangeCode })
                .ToDictionary(x => x.DynaFlowTaskTypeID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDynaFlowTaskType in dynaFlowTaskTypesToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDynaFlowTaskType.DynaFlowTaskTypeID, out var token) || token != updatedDynaFlowTaskType.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDynaFlowTaskType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dynaFlowTaskTypesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dynaFlowTaskTypesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dynaFlowTaskType in _dbContext.DynaFlowTaskTypeSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dynaFlowTaskType.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DynaFlowTaskTypeObj = dynaFlowTaskType,
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
                    var dynaFlowTaskType = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dynaFlowTaskType != null)
                    {
                        found = true;
                        Delete(dynaFlowTaskType.DynaFlowTaskTypeID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dynaFlowTaskType = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dynaFlowTaskType != null)
                    {
                        found = true;
                        Delete(dynaFlowTaskType.DynaFlowTaskTypeID);
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
                    var dynaFlowTaskType = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dynaFlowTaskType != null)
                    {
                        found = true;
                        Delete(dynaFlowTaskType.DynaFlowTaskTypeID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DynaFlowTaskType> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DynaFlowTaskTypeObj.PacCodePeek = item.PacCode.Value; //PacID
                            }

            List<DynaFlowTaskType> results = data.Select(r => r.DynaFlowTaskTypeObj).ToList();

            return results;
        }
        //PacID
        public async Task<List<DynaFlowTaskType>> GetByPacIDAsync(int id)
        {
            var dynaFlowTaskTypesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTaskTypeObj.PacID == id)
                                    .ToListAsync();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            return finalDynaFlowTaskTypes;
        }
        //PacID
        public List<DynaFlowTaskType> GetByPacID(int id)
        {
            var dynaFlowTaskTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTaskTypeObj.PacID == id)
                                    .ToList();

            List<DynaFlowTaskType> finalDynaFlowTaskTypes = ProcessMappings(dynaFlowTaskTypesWithCodes);

            return finalDynaFlowTaskTypes;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DynaFlowTaskType DynaFlowTaskTypeObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
