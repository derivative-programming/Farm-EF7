using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class LandManager
	{
		private readonly FarmDbContext _dbContext;

		public LandManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Land> AddAsync(Land land)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.LandSet.Add(land);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(land).State = EntityState.Detached;

					await transaction.CommitAsync();

					return land;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.LandSet.Add(land);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(land).State = EntityState.Detached;

				return land;

			}
		}

        public Land Add(Land land)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.LandSet.Add(land);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(land).State = EntityState.Detached;

                    transaction.Commit();

                    return land;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.LandSet.Add(land);
                _dbContext.SaveChanges();
                _dbContext.Entry(land).State = EntityState.Detached;

                return land;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.LandSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.LandSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.LandSet.AsNoTracking().MaxAsync(x => (int?)x.LandID);
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
            int? maxId = _dbContext.LandSet.AsNoTracking().Max(x => (int?)x.LandID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<Land> GetByIdAsync(int id)
		{

			var landsWithCodes = await BuildQuery()
									.Where(x => x.LandObj.LandID == id)
									.ToListAsync();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            if (finalLands.Count > 0)
			{
				return finalLands[0];

            }

			return null;

        }

        public Land GetById(int id)
        {

            var landsWithCodes = BuildQuery()
                                    .Where(x => x.LandObj.LandID == id)
                                    .ToList();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            if (finalLands.Count > 0)
            {
                return finalLands[0];

            }

            return null;

        }

        public async Task<Land> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.LandSet.AsNoTracking().FirstOrDefaultAsync(x => x.LandID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var landsWithCodes = await BuildQuery()
                                        .Where(x => x.LandObj.LandID == id)
                                        .ToListAsync();

                List<Land> finalLands = ProcessMappings(landsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalLands.Count > 0)
                {
                    return finalLands[0];

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

        public Land DirtyGetById(int id) //to test
        {
            //return await _dbContext.LandSet.AsNoTracking().FirstOrDefaultAsync(x => x.LandID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var landsWithCodes = BuildQuery()
                                        .Where(x => x.LandObj.LandID == id)
                                        .ToList();

                List<Land> finalLands = ProcessMappings(landsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalLands.Count > 0)
                {
                    return finalLands[0];

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
        public async Task<Land> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var landsWithCodes = await BuildQuery()
                                    .Where(x => x.LandObj.Code == code)
                                    .ToListAsync();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            if (finalLands.Count > 0)
            {
                return finalLands[0];

            }

            return null;
        }

        public Land GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var landsWithCodes = BuildQuery()
                                    .Where(x => x.LandObj.Code == code)
                                    .ToList();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            if (finalLands.Count > 0)
            {
                return finalLands[0];

            }

            return null;
        }

        public async Task<Land> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var landsWithCodes = await BuildQuery()
                                        .Where(x => x.LandObj.Code == code)
                                        .ToListAsync();

                List<Land> finalLands = ProcessMappings(landsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalLands.Count > 0)
                {
                    return finalLands[0];

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

        public Land DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var landsWithCodes = BuildQuery()
                                        .Where(x => x.LandObj.Code == code)
                                        .ToList();

                List<Land> finalLands = ProcessMappings(landsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalLands.Count > 0)
                {
                    return finalLands[0];

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

        public async Task<List<Land>> GetAllAsync()
		{
            var landsWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            return finalLands;
        }
        public List<Land> GetAll()
        {
            var landsWithCodes = BuildQuery()
                                    .ToList();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            return finalLands;
        }

        public async Task<bool> UpdateAsync(Land landToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.LandSet.Attach(landToUpdate);
					_dbContext.Entry(landToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					landToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(landToUpdate).State = EntityState.Detached;

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
					_dbContext.LandSet.Attach(landToUpdate);
					_dbContext.Entry(landToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					landToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(landToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(Land landToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.LandSet.Attach(landToUpdate);
                    _dbContext.Entry(landToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    landToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(landToUpdate).State = EntityState.Detached;

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
                    _dbContext.LandSet.Attach(landToUpdate);
                    _dbContext.Entry(landToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    landToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(landToUpdate).State = EntityState.Detached;

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
					var land = await _dbContext.LandSet.FirstOrDefaultAsync(x => x.LandID == id);
					if (land == null) return false;

					_dbContext.LandSet.Remove(land);
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
					var land = await _dbContext.LandSet.FirstOrDefaultAsync(x => x.LandID == id);
					if (land == null) return false;

					_dbContext.LandSet.Remove(land);
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
                    var land = _dbContext.LandSet.FirstOrDefault(x => x.LandID == id);
                    if (land == null) return false;

                    _dbContext.LandSet.Remove(land);
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
                    var land = _dbContext.LandSet.FirstOrDefault(x => x.LandID == id);
                    if (land == null) return false;

                    _dbContext.LandSet.Remove(land);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<Land> lands)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var land in lands)
			{
				land.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(land);
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
					await _dbContext.BulkInsertAsync(lands, bulkConfig);

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
				await _dbContext.BulkInsertAsync(lands, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<Land> lands)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var land in lands)
            {
                land.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(land);
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
                    _dbContext.BulkInsert(lands, bulkConfig);

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
                _dbContext.BulkInsert(lands, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<Land> updatedLands)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedLands.Select(x => x.LandID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.LandSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.LandID))
			//	.Select(p => new { p.LandID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.LandID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedLand in updatedLands)
			{
				//	if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.LandSet.Attach(updatedLand);
				//	_dbContext.Entry(updatedLand).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedLand);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.LandSet.Attach(updatedLand);
				//_dbContext.Entry(updatedLand).State = EntityState.Modified;
				//_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedLand.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(landToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedLands, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedLands, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<Land> updatedLands)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedLands.Select(x => x.LandID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.LandSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.LandID))
            //	.Select(p => new { p.LandID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.LandID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedLand in updatedLands)
            {
                //	if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.LandSet.Attach(updatedLand);
                //	_dbContext.Entry(updatedLand).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedLand);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.LandSet.Attach(updatedLand);
                //_dbContext.Entry(updatedLand).State = EntityState.Modified;
                //_dbContext.Entry(landToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedLand.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(landToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedLands, bulkConfig);
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
                _dbContext.BulkUpdate(updatedLands, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Land> landsToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = landsToDelete.Select(x => x.LandID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.LandSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.LandID))
				.Select(p => new { p.LandID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.LandID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedLand in landsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(landsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(landsToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<Land> landsToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Land.LandID), nameof(Land.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = landsToDelete.Select(x => x.LandID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.LandSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.LandID))
                .Select(p => new { p.LandID, p.LastChangeCode })
                .ToDictionary(x => x.LandID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedLand in landsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedLand.LandID, out var token) || token != updatedLand.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedLand.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(landsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(landsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from land in _dbContext.LandSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == land.PacID).DefaultIfEmpty() //PacID
																																		   select new QueryDTO
                   {
					   LandObj = land,
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
                    var land = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (land != null)
                    {
                        found = true;
                        Delete(land.LandID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var land = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (land != null)
                    {
                        found = true;
                        Delete(land.LandID);
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
                    var land = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (land != null)
                    {
                        found = true;
                        Delete(land.LandID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<Land> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.LandObj.PacCodePeek = item.PacCode.Value; //PacID
                            }

            List<Land> results = data.Select(r => r.LandObj).ToList();

            return results;
        }
        //PacID
        public async Task<List<Land>> GetByPacIDAsync(int id)
        {
            var landsWithCodes = await BuildQuery()
                                    .Where(x => x.LandObj.PacID == id)
                                    .ToListAsync();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            return finalLands;
        }
        //PacID
        public List<Land> GetByPacID(int id)
        {
            var landsWithCodes = BuildQuery()
                                    .Where(x => x.LandObj.PacID == id)
                                    .ToList();

            List<Land> finalLands = ProcessMappings(landsWithCodes);

            return finalLands;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public Land LandObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }

    }
}
