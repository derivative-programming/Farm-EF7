using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DynaFlowTypeManager
	{
		private readonly FarmDbContext _dbContext;

		public DynaFlowTypeManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DynaFlowType> AddAsync(DynaFlowType dynaFlowType)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTypeSet.Add(dynaFlowType);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowType).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dynaFlowType;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DynaFlowTypeSet.Add(dynaFlowType);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dynaFlowType).State = EntityState.Detached;

				return dynaFlowType;

			}
		}

        public DynaFlowType Add(DynaFlowType dynaFlowType)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTypeSet.Add(dynaFlowType);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowType).State = EntityState.Detached;

                    transaction.Commit();

                    return dynaFlowType;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DynaFlowTypeSet.Add(dynaFlowType);
                _dbContext.SaveChanges();
                _dbContext.Entry(dynaFlowType).State = EntityState.Detached;

                return dynaFlowType;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DynaFlowTypeSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DynaFlowTypeSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DynaFlowTypeSet.AsNoTracking().MaxAsync(x => (int?)x.DynaFlowTypeID);
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
            int? maxId = _dbContext.DynaFlowTypeSet.AsNoTracking().Max(x => (int?)x.DynaFlowTypeID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DynaFlowType> GetByIdAsync(int id)
		{

			var dynaFlowTypesWithCodes = await BuildQuery()
									.Where(x => x.DynaFlowTypeObj.DynaFlowTypeID == id)
									.ToListAsync();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            if (finalDynaFlowTypes.Count > 0)
			{
				return finalDynaFlowTypes[0];

            }

			return null;

        }

        public DynaFlowType GetById(int id)
        {

            var dynaFlowTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeObj.DynaFlowTypeID == id)
                                    .ToList();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            if (finalDynaFlowTypes.Count > 0)
            {
                return finalDynaFlowTypes[0];

            }

            return null;

        }

        public async Task<DynaFlowType> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DynaFlowTypeSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTypeID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTypesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTypeObj.DynaFlowTypeID == id)
                                        .ToListAsync();

                List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTypes.Count > 0)
                {
                    return finalDynaFlowTypes[0];

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

        public DynaFlowType DirtyGetById(int id) //to test
        {
            //return await _dbContext.DynaFlowTypeSet.AsNoTracking().FirstOrDefaultAsync(x => x.DynaFlowTypeID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTypesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTypeObj.DynaFlowTypeID == id)
                                        .ToList();

                List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTypes.Count > 0)
                {
                    return finalDynaFlowTypes[0];

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
        public async Task<DynaFlowType> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTypesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTypeObj.Code == code)
                                    .ToListAsync();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            if (finalDynaFlowTypes.Count > 0)
            {
                return finalDynaFlowTypes[0];

            }

            return null;
        }

        public DynaFlowType GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dynaFlowTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeObj.Code == code)
                                    .ToList();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            if (finalDynaFlowTypes.Count > 0)
            {
                return finalDynaFlowTypes[0];

            }

            return null;
        }

        public async Task<DynaFlowType> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dynaFlowTypesWithCodes = await BuildQuery()
                                        .Where(x => x.DynaFlowTypeObj.Code == code)
                                        .ToListAsync();

                List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDynaFlowTypes.Count > 0)
                {
                    return finalDynaFlowTypes[0];

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

        public DynaFlowType DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dynaFlowTypesWithCodes = BuildQuery()
                                        .Where(x => x.DynaFlowTypeObj.Code == code)
                                        .ToList();

                List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDynaFlowTypes.Count > 0)
                {
                    return finalDynaFlowTypes[0];

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

        public async Task<List<DynaFlowType>> GetAllAsync()
		{
            var dynaFlowTypesWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            return finalDynaFlowTypes;
        }
        public List<DynaFlowType> GetAll()
        {
            var dynaFlowTypesWithCodes = BuildQuery()
                                    .ToList();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            return finalDynaFlowTypes;
        }

        public async Task<bool> UpdateAsync(DynaFlowType dynaFlowTypeToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DynaFlowTypeSet.Attach(dynaFlowTypeToUpdate);
					_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTypeToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;

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
					_dbContext.DynaFlowTypeSet.Attach(dynaFlowTypeToUpdate);
					_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dynaFlowTypeToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DynaFlowType dynaFlowTypeToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DynaFlowTypeSet.Attach(dynaFlowTypeToUpdate);
                    _dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTypeToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;

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
                    _dbContext.DynaFlowTypeSet.Attach(dynaFlowTypeToUpdate);
                    _dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dynaFlowTypeToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;

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
					var dynaFlowType = await _dbContext.DynaFlowTypeSet.FirstOrDefaultAsync(x => x.DynaFlowTypeID == id);
					if (dynaFlowType == null) return false;

					_dbContext.DynaFlowTypeSet.Remove(dynaFlowType);
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
					var dynaFlowType = await _dbContext.DynaFlowTypeSet.FirstOrDefaultAsync(x => x.DynaFlowTypeID == id);
					if (dynaFlowType == null) return false;

					_dbContext.DynaFlowTypeSet.Remove(dynaFlowType);
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
                    var dynaFlowType = _dbContext.DynaFlowTypeSet.FirstOrDefault(x => x.DynaFlowTypeID == id);
                    if (dynaFlowType == null) return false;

                    _dbContext.DynaFlowTypeSet.Remove(dynaFlowType);
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
                    var dynaFlowType = _dbContext.DynaFlowTypeSet.FirstOrDefault(x => x.DynaFlowTypeID == id);
                    if (dynaFlowType == null) return false;

                    _dbContext.DynaFlowTypeSet.Remove(dynaFlowType);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DynaFlowType> dynaFlowTypes)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dynaFlowType in dynaFlowTypes)
			{
				dynaFlowType.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dynaFlowType);
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
					await _dbContext.BulkInsertAsync(dynaFlowTypes, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dynaFlowTypes, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DynaFlowType> dynaFlowTypes)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dynaFlowType in dynaFlowTypes)
            {
                dynaFlowType.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dynaFlowType);
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
                    _dbContext.BulkInsert(dynaFlowTypes, bulkConfig);

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
                _dbContext.BulkInsert(dynaFlowTypes, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DynaFlowType> updatedDynaFlowTypes)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDynaFlowTypes.Select(x => x.DynaFlowTypeID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DynaFlowTypeSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DynaFlowTypeID))
			//	.Select(p => new { p.DynaFlowTypeID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DynaFlowTypeID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDynaFlowType in updatedDynaFlowTypes)
			{
				//	if (!existingTokens.TryGetValue(updatedDynaFlowType.DynaFlowTypeID, out var token) || token != updatedDynaFlowType.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDynaFlowType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DynaFlowTypeSet.Attach(updatedDynaFlowType);
				//	_dbContext.Entry(updatedDynaFlowType).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDynaFlowType);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DynaFlowTypeSet.Attach(updatedDynaFlowType);
				//_dbContext.Entry(updatedDynaFlowType).State = EntityState.Modified;
				//_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDynaFlowType.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDynaFlowTypes, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDynaFlowTypes, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DynaFlowType> updatedDynaFlowTypes)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDynaFlowTypes.Select(x => x.DynaFlowTypeID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DynaFlowTypeSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DynaFlowTypeID))
            //	.Select(p => new { p.DynaFlowTypeID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DynaFlowTypeID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDynaFlowType in updatedDynaFlowTypes)
            {
                //	if (!existingTokens.TryGetValue(updatedDynaFlowType.DynaFlowTypeID, out var token) || token != updatedDynaFlowType.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDynaFlowType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DynaFlowTypeSet.Attach(updatedDynaFlowType);
                //	_dbContext.Entry(updatedDynaFlowType).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDynaFlowType);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DynaFlowTypeSet.Attach(updatedDynaFlowType);
                //_dbContext.Entry(updatedDynaFlowType).State = EntityState.Modified;
                //_dbContext.Entry(dynaFlowTypeToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDynaFlowType.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dynaFlowTypeToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDynaFlowTypes, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDynaFlowTypes, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DynaFlowType> dynaFlowTypesToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dynaFlowTypesToDelete.Select(x => x.DynaFlowTypeID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DynaFlowTypeSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DynaFlowTypeID))
				.Select(p => new { p.DynaFlowTypeID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DynaFlowTypeID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDynaFlowType in dynaFlowTypesToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDynaFlowType.DynaFlowTypeID, out var token) || token != updatedDynaFlowType.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDynaFlowType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dynaFlowTypesToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dynaFlowTypesToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DynaFlowType> dynaFlowTypesToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DynaFlowType.DynaFlowTypeID), nameof(DynaFlowType.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dynaFlowTypesToDelete.Select(x => x.DynaFlowTypeID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DynaFlowTypeSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DynaFlowTypeID))
                .Select(p => new { p.DynaFlowTypeID, p.LastChangeCode })
                .ToDictionary(x => x.DynaFlowTypeID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDynaFlowType in dynaFlowTypesToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDynaFlowType.DynaFlowTypeID, out var token) || token != updatedDynaFlowType.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDynaFlowType.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dynaFlowTypesToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dynaFlowTypesToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dynaFlowType in _dbContext.DynaFlowTypeSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dynaFlowType.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DynaFlowTypeObj = dynaFlowType,
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
                    var dynaFlowType = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dynaFlowType != null)
                    {
                        found = true;
                        Delete(dynaFlowType.DynaFlowTypeID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dynaFlowType = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dynaFlowType != null)
                    {
                        found = true;
                        Delete(dynaFlowType.DynaFlowTypeID);
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
                    var dynaFlowType = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dynaFlowType != null)
                    {
                        found = true;
                        Delete(dynaFlowType.DynaFlowTypeID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DynaFlowType> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DynaFlowTypeObj.PacCodePeek = item.PacCode.Value; //PacID
            }

            List<DynaFlowType> results = data.Select(r => r.DynaFlowTypeObj).ToList();

            return results;
        }
        //PacID
        public async Task<List<DynaFlowType>> GetByPacIDAsync(int id)
        {
            var dynaFlowTypesWithCodes = await BuildQuery()
                                    .Where(x => x.DynaFlowTypeObj.PacID == id)
                                    .ToListAsync();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            return finalDynaFlowTypes;
        }
        //PacID
        public List<DynaFlowType> GetByPacID(int id)
        {
            var dynaFlowTypesWithCodes = BuildQuery()
                                    .Where(x => x.DynaFlowTypeObj.PacID == id)
                                    .ToList();

            List<DynaFlowType> finalDynaFlowTypes = ProcessMappings(dynaFlowTypesWithCodes);

            return finalDynaFlowTypes;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DynaFlowType DynaFlowTypeObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
