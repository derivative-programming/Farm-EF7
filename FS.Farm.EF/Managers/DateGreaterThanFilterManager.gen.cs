using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class DateGreaterThanFilterManager
	{
		private readonly FarmDbContext _dbContext;

		public DateGreaterThanFilterManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DateGreaterThanFilter> AddAsync(DateGreaterThanFilter dateGreaterThanFilter)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dateGreaterThanFilter).State = EntityState.Detached;

					await transaction.CommitAsync();

					return dateGreaterThanFilter;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(dateGreaterThanFilter).State = EntityState.Detached;

				return dateGreaterThanFilter;

			}
		}

        public DateGreaterThanFilter Add(DateGreaterThanFilter dateGreaterThanFilter)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dateGreaterThanFilter).State = EntityState.Detached;

                    transaction.Commit();

                    return dateGreaterThanFilter;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                _dbContext.SaveChanges();
                _dbContext.Entry(dateGreaterThanFilter).State = EntityState.Detached;

                return dateGreaterThanFilter;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.DateGreaterThanFilterSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.DateGreaterThanFilterSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.DateGreaterThanFilterSet.AsNoTracking().MaxAsync(x => (int?)x.DateGreaterThanFilterID);
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
            int? maxId = _dbContext.DateGreaterThanFilterSet.AsNoTracking().Max(x => (int?)x.DateGreaterThanFilterID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<DateGreaterThanFilter> GetByIdAsync(int id)
		{

			var dateGreaterThanFiltersWithCodes = await BuildQuery()
									.Where(x => x.DateGreaterThanFilterObj.DateGreaterThanFilterID == id)
									.ToListAsync();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            if (finalDateGreaterThanFilters.Count > 0)
			{
				return finalDateGreaterThanFilters[0];

            }

			return null;

        }

        public DateGreaterThanFilter GetById(int id)
        {

            var dateGreaterThanFiltersWithCodes = BuildQuery()
                                    .Where(x => x.DateGreaterThanFilterObj.DateGreaterThanFilterID == id)
                                    .ToList();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            if (finalDateGreaterThanFilters.Count > 0)
            {
                return finalDateGreaterThanFilters[0];

            }

            return null;

        }

        public async Task<DateGreaterThanFilter> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.DateGreaterThanFilterSet.AsNoTracking().FirstOrDefaultAsync(x => x.DateGreaterThanFilterID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dateGreaterThanFiltersWithCodes = await BuildQuery()
                                        .Where(x => x.DateGreaterThanFilterObj.DateGreaterThanFilterID == id)
                                        .ToListAsync();

                List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDateGreaterThanFilters.Count > 0)
                {
                    return finalDateGreaterThanFilters[0];

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

        public DateGreaterThanFilter DirtyGetById(int id) //to test
        {
            //return await _dbContext.DateGreaterThanFilterSet.AsNoTracking().FirstOrDefaultAsync(x => x.DateGreaterThanFilterID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dateGreaterThanFiltersWithCodes = BuildQuery()
                                        .Where(x => x.DateGreaterThanFilterObj.DateGreaterThanFilterID == id)
                                        .ToList();

                List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDateGreaterThanFilters.Count > 0)
                {
                    return finalDateGreaterThanFilters[0];

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
        public async Task<DateGreaterThanFilter> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dateGreaterThanFiltersWithCodes = await BuildQuery()
                                    .Where(x => x.DateGreaterThanFilterObj.Code == code)
                                    .ToListAsync();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            if (finalDateGreaterThanFilters.Count > 0)
            {
                return finalDateGreaterThanFilters[0];

            }

            return null;
        }

        public DateGreaterThanFilter GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var dateGreaterThanFiltersWithCodes = BuildQuery()
                                    .Where(x => x.DateGreaterThanFilterObj.Code == code)
                                    .ToList();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            if (finalDateGreaterThanFilters.Count > 0)
            {
                return finalDateGreaterThanFilters[0];

            }

            return null;
        }

        public async Task<DateGreaterThanFilter> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var dateGreaterThanFiltersWithCodes = await BuildQuery()
                                        .Where(x => x.DateGreaterThanFilterObj.Code == code)
                                        .ToListAsync();

                List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalDateGreaterThanFilters.Count > 0)
                {
                    return finalDateGreaterThanFilters[0];

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

        public DateGreaterThanFilter DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var dateGreaterThanFiltersWithCodes = BuildQuery()
                                        .Where(x => x.DateGreaterThanFilterObj.Code == code)
                                        .ToList();

                List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalDateGreaterThanFilters.Count > 0)
                {
                    return finalDateGreaterThanFilters[0];

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

        public async Task<List<DateGreaterThanFilter>> GetAllAsync()
		{
            var dateGreaterThanFiltersWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            return finalDateGreaterThanFilters;
        }
        public List<DateGreaterThanFilter> GetAll()
        {
            var dateGreaterThanFiltersWithCodes = BuildQuery()
                                    .ToList();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            return finalDateGreaterThanFilters;
        }

        public async Task<bool> UpdateAsync(DateGreaterThanFilter dateGreaterThanFilterToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.DateGreaterThanFilterSet.Attach(dateGreaterThanFilterToUpdate);
					_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dateGreaterThanFilterToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;

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
					_dbContext.DateGreaterThanFilterSet.Attach(dateGreaterThanFilterToUpdate);
					_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					dateGreaterThanFilterToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(DateGreaterThanFilter dateGreaterThanFilterToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.DateGreaterThanFilterSet.Attach(dateGreaterThanFilterToUpdate);
                    _dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dateGreaterThanFilterToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;

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
                    _dbContext.DateGreaterThanFilterSet.Attach(dateGreaterThanFilterToUpdate);
                    _dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    dateGreaterThanFilterToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;

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
					var dateGreaterThanFilter = await _dbContext.DateGreaterThanFilterSet.FirstOrDefaultAsync(x => x.DateGreaterThanFilterID == id);
					if (dateGreaterThanFilter == null) return false;

					_dbContext.DateGreaterThanFilterSet.Remove(dateGreaterThanFilter);
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
					var dateGreaterThanFilter = await _dbContext.DateGreaterThanFilterSet.FirstOrDefaultAsync(x => x.DateGreaterThanFilterID == id);
					if (dateGreaterThanFilter == null) return false;

					_dbContext.DateGreaterThanFilterSet.Remove(dateGreaterThanFilter);
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
                    var dateGreaterThanFilter = _dbContext.DateGreaterThanFilterSet.FirstOrDefault(x => x.DateGreaterThanFilterID == id);
                    if (dateGreaterThanFilter == null) return false;

                    _dbContext.DateGreaterThanFilterSet.Remove(dateGreaterThanFilter);
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
                    var dateGreaterThanFilter = _dbContext.DateGreaterThanFilterSet.FirstOrDefault(x => x.DateGreaterThanFilterID == id);
                    if (dateGreaterThanFilter == null) return false;

                    _dbContext.DateGreaterThanFilterSet.Remove(dateGreaterThanFilter);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<DateGreaterThanFilter> dateGreaterThanFilters)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
			{
				dateGreaterThanFilter.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(dateGreaterThanFilter);
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
					await _dbContext.BulkInsertAsync(dateGreaterThanFilters, bulkConfig);

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
				await _dbContext.BulkInsertAsync(dateGreaterThanFilters, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<DateGreaterThanFilter> dateGreaterThanFilters)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
            {
                dateGreaterThanFilter.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(dateGreaterThanFilter);
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
                    _dbContext.BulkInsert(dateGreaterThanFilters, bulkConfig);

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
                _dbContext.BulkInsert(dateGreaterThanFilters, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<DateGreaterThanFilter> updatedDateGreaterThanFilters)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedDateGreaterThanFilters.Select(x => x.DateGreaterThanFilterID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.DateGreaterThanFilterSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.DateGreaterThanFilterID))
			//	.Select(p => new { p.DateGreaterThanFilterID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.DateGreaterThanFilterID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedDateGreaterThanFilter in updatedDateGreaterThanFilters)
			{
				//	if (!existingTokens.TryGetValue(updatedDateGreaterThanFilter.DateGreaterThanFilterID, out var token) || token != updatedDateGreaterThanFilter.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.DateGreaterThanFilterSet.Attach(updatedDateGreaterThanFilter);
				//	_dbContext.Entry(updatedDateGreaterThanFilter).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedDateGreaterThanFilter);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.DateGreaterThanFilterSet.Attach(updatedDateGreaterThanFilter);
				//_dbContext.Entry(updatedDateGreaterThanFilter).State = EntityState.Modified;
				//_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedDateGreaterThanFilters, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedDateGreaterThanFilters, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<DateGreaterThanFilter> updatedDateGreaterThanFilters)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedDateGreaterThanFilters.Select(x => x.DateGreaterThanFilterID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.DateGreaterThanFilterSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.DateGreaterThanFilterID))
            //	.Select(p => new { p.DateGreaterThanFilterID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.DateGreaterThanFilterID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedDateGreaterThanFilter in updatedDateGreaterThanFilters)
            {
                //	if (!existingTokens.TryGetValue(updatedDateGreaterThanFilter.DateGreaterThanFilterID, out var token) || token != updatedDateGreaterThanFilter.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.DateGreaterThanFilterSet.Attach(updatedDateGreaterThanFilter);
                //	_dbContext.Entry(updatedDateGreaterThanFilter).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedDateGreaterThanFilter);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.DateGreaterThanFilterSet.Attach(updatedDateGreaterThanFilter);
                //_dbContext.Entry(updatedDateGreaterThanFilter).State = EntityState.Modified;
                //_dbContext.Entry(dateGreaterThanFilterToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(dateGreaterThanFilterToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedDateGreaterThanFilters, bulkConfig);
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
                _dbContext.BulkUpdate(updatedDateGreaterThanFilters, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<DateGreaterThanFilter> dateGreaterThanFiltersToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = dateGreaterThanFiltersToDelete.Select(x => x.DateGreaterThanFilterID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.DateGreaterThanFilterSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.DateGreaterThanFilterID))
				.Select(p => new { p.DateGreaterThanFilterID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.DateGreaterThanFilterID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedDateGreaterThanFilter in dateGreaterThanFiltersToDelete)
			{
				if (!existingTokens.TryGetValue(updatedDateGreaterThanFilter.DateGreaterThanFilterID, out var token) || token != updatedDateGreaterThanFilter.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(dateGreaterThanFiltersToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(dateGreaterThanFiltersToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<DateGreaterThanFilter> dateGreaterThanFiltersToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(DateGreaterThanFilter.DateGreaterThanFilterID), nameof(DateGreaterThanFilter.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = dateGreaterThanFiltersToDelete.Select(x => x.DateGreaterThanFilterID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.DateGreaterThanFilterSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.DateGreaterThanFilterID))
                .Select(p => new { p.DateGreaterThanFilterID, p.LastChangeCode })
                .ToDictionary(x => x.DateGreaterThanFilterID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedDateGreaterThanFilter in dateGreaterThanFiltersToDelete)
            {
                if (!existingTokens.TryGetValue(updatedDateGreaterThanFilter.DateGreaterThanFilterID, out var token) || token != updatedDateGreaterThanFilter.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedDateGreaterThanFilter.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(dateGreaterThanFiltersToDelete, bulkConfig);
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
                _dbContext.BulkDelete(dateGreaterThanFiltersToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from dateGreaterThanFilter in _dbContext.DateGreaterThanFilterSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == dateGreaterThanFilter.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   DateGreaterThanFilterObj = dateGreaterThanFilter,
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
                    var dateGreaterThanFilter = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (dateGreaterThanFilter != null)
                    {
                        found = true;
                        Delete(dateGreaterThanFilter.DateGreaterThanFilterID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var dateGreaterThanFilter = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (dateGreaterThanFilter != null)
                    {
                        found = true;
                        Delete(dateGreaterThanFilter.DateGreaterThanFilterID);
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
                    var dateGreaterThanFilter = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (dateGreaterThanFilter != null)
                    {
                        found = true;
                        Delete(dateGreaterThanFilter.DateGreaterThanFilterID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<DateGreaterThanFilter> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.DateGreaterThanFilterObj.PacCodePeek = item.PacCode.Value; //PacID
                            }

            List<DateGreaterThanFilter> results = data.Select(r => r.DateGreaterThanFilterObj).ToList();

            return results;
        }
        //PacID
        public async Task<List<DateGreaterThanFilter>> GetByPacIDAsync(int id)
        {
            var dateGreaterThanFiltersWithCodes = await BuildQuery()
                                    .Where(x => x.DateGreaterThanFilterObj.PacID == id)
                                    .ToListAsync();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            return finalDateGreaterThanFilters;
        }
        //PacID
        public List<DateGreaterThanFilter> GetByPacID(int id)
        {
            var dateGreaterThanFiltersWithCodes = BuildQuery()
                                    .Where(x => x.DateGreaterThanFilterObj.PacID == id)
                                    .ToList();

            List<DateGreaterThanFilter> finalDateGreaterThanFilters = ProcessMappings(dateGreaterThanFiltersWithCodes);

            return finalDateGreaterThanFilters;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public DateGreaterThanFilter DateGreaterThanFilterObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
