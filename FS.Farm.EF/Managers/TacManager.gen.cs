using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
namespace FS.Farm.EF.Managers
{
	public partial class TacManager
	{
		private readonly FarmDbContext _dbContext;
		public TacManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Tac> AddAsync(Tac tac)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TacSet.Add(tac);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tac).State = EntityState.Detached;
					await transaction.CommitAsync();
					return tac;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			else
			{
				_dbContext.TacSet.Add(tac);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(tac).State = EntityState.Detached;
				return tac;
			}
		}
        public Tac Add(Tac tac)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.TacSet.Add(tac);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(tac).State = EntityState.Detached;
                    transaction.Commit();
                    return tac;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            else
            {
                _dbContext.TacSet.Add(tac);
                _dbContext.SaveChanges();
                _dbContext.Entry(tac).State = EntityState.Detached;
                return tac;
            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.TacSet.AsNoTracking().CountAsync();
		}
        public int GetTotalCount()
        {
            return _dbContext.TacSet.AsNoTracking().Count();
        }
        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.TacSet.AsNoTracking().MaxAsync(x => (int?)x.TacID);
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
            int? maxId = _dbContext.TacSet.AsNoTracking().Max(x => (int?)x.TacID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }
        public async Task<Tac> GetByIdAsync(int id)
		{
			var tacsWithCodes = await BuildQuery()
									.Where(x => x.TacObj.TacID == id)
									.ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
			{
				return finalTacs[0];
            }
			return null;
        }
        public Tac GetById(int id)
        {
            var tacsWithCodes = BuildQuery()
                                    .Where(x => x.TacObj.TacID == id)
                                    .ToList();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
            {
                return finalTacs[0];
            }
            return null;
        }
        public async Task<Tac> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.TacSet.AsNoTracking().FirstOrDefaultAsync(x => x.TacID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var tacsWithCodes = await BuildQuery()
                                        .Where(x => x.TacObj.TacID == id)
                                        .ToListAsync();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
        public Tac DirtyGetById(int id) //to test
        {
            //return await _dbContext.TacSet.AsNoTracking().FirstOrDefaultAsync(x => x.TacID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var tacsWithCodes = BuildQuery()
                                        .Where(x => x.TacObj.TacID == id)
                                        .ToList();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
        public async Task<Tac> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var tacsWithCodes = await BuildQuery()
                                    .Where(x => x.TacObj.Code == code)
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
            {
                return finalTacs[0];
            }
            return null;
        }
        public Tac GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var tacsWithCodes = BuildQuery()
                                    .Where(x => x.TacObj.Code == code)
                                    .ToList();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            if (finalTacs.Count > 0)
            {
                return finalTacs[0];
            }
            return null;
        }
        public async Task<Tac> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var tacsWithCodes = await BuildQuery()
                                        .Where(x => x.TacObj.Code == code)
                                        .ToListAsync();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
        public Tac DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                var tacsWithCodes = BuildQuery()
                                        .Where(x => x.TacObj.Code == code)
                                        .ToList();
                List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();
                if (finalTacs.Count > 0)
                {
                    return finalTacs[0];
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
        public async Task<List<Tac>> GetAllAsync()
		{
            var tacsWithCodes = await BuildQuery()
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
        public List<Tac> GetAll()
        {
            var tacsWithCodes = BuildQuery()
                                    .ToList();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
        public async Task<bool> UpdateAsync(Tac tacToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.TacSet.Attach(tacToUpdate);
					_dbContext.Entry(tacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					tacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
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
					_dbContext.TacSet.Attach(tacToUpdate);
					_dbContext.Entry(tacToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					tacToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}
        public bool Update(Tac tacToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.TacSet.Attach(tacToUpdate);
                    _dbContext.Entry(tacToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    tacToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(tacToUpdate).State = EntityState.Detached;
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
                    _dbContext.TacSet.Attach(tacToUpdate);
                    _dbContext.Entry(tacToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    tacToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(tacToUpdate).State = EntityState.Detached;
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
					var tac = await _dbContext.TacSet.FirstOrDefaultAsync(x => x.TacID == id);
					if (tac == null) return false;
					_dbContext.TacSet.Remove(tac);
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
					var tac = await _dbContext.TacSet.FirstOrDefaultAsync(x => x.TacID == id);
					if (tac == null) return false;
					_dbContext.TacSet.Remove(tac);
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
                    var tac = _dbContext.TacSet.FirstOrDefault(x => x.TacID == id);
                    if (tac == null) return false;
                    _dbContext.TacSet.Remove(tac);
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
                    var tac = _dbContext.TacSet.FirstOrDefault(x => x.TacID == id);
                    if (tac == null) return false;
                    _dbContext.TacSet.Remove(tac);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task BulkInsertAsync(IEnumerable<Tac> tacs)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var tac in tacs)
			{
				tac.LastChangeCode = Guid.NewGuid();
				var entry = _dbContext.Entry(tac);
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
					await _dbContext.BulkInsertAsync(tacs, bulkConfig);
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
				await _dbContext.BulkInsertAsync(tacs, bulkConfig);
			}
		}
        public void BulkInsert(IEnumerable<Tac> tacs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var tac in tacs)
            {
                tac.LastChangeCode = Guid.NewGuid();
                var entry = _dbContext.Entry(tac);
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
                    _dbContext.BulkInsert(tacs, bulkConfig);
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
                _dbContext.BulkInsert(tacs, bulkConfig);
            }
        }
        public async Task BulkUpdateAsync(IEnumerable<Tac> updatedTacs)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = updatedTacs.Select(x => x.TacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.TacSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.TacID))
			//	.Select(p => new { p.TacID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.TacID, x => x.LastChangeCode);
			//// Check concurrency conflicts
			foreach (var updatedTac in updatedTacs)
			{
				//	if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
				//	_dbContext.TacSet.Attach(updatedTac);
				//	_dbContext.Entry(updatedTac).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedTac);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				//_dbContext.TacSet.Attach(updatedTac);
				//_dbContext.Entry(updatedTac).State = EntityState.Modified;
				//_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedTac.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
			}
			//TODO concurrency token check
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedTacs, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedTacs, bulkConfig);
			}
		}
        public void BulkUpdate(IEnumerable<Tac> updatedTacs)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = updatedTacs.Select(x => x.TacID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.TacSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.TacID))
            //	.Select(p => new { p.TacID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.TacID, x => x.LastChangeCode);
            //// Check concurrency conflicts
            foreach (var updatedTac in updatedTacs)
            {
                //	if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
                //	_dbContext.TacSet.Attach(updatedTac);
                //	_dbContext.Entry(updatedTac).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedTac);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                //_dbContext.TacSet.Attach(updatedTac);
                //_dbContext.Entry(updatedTac).State = EntityState.Modified;
                //_dbContext.Entry(tacToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedTac.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(tacToUpdate).State = EntityState.Detached;
            }
            //TODO concurrency token check
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedTacs, bulkConfig);
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
                _dbContext.BulkUpdate(updatedTacs, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Tac> tacsToDelete)
		{
			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			var idsToUpdate = tacsToDelete.Select(x => x.TacID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.TacSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.TacID))
				.Select(p => new { p.TacID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.TacID, x => x.LastChangeCode);
			// Check concurrency conflicts
			foreach (var updatedTac in tacsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(tacsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(tacsToDelete, bulkConfig);
			}
		}
        public void BulkDelete(IEnumerable<Tac> tacsToDelete)
        {
            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Tac.TacID), nameof(Tac.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            var idsToUpdate = tacsToDelete.Select(x => x.TacID).ToList();
            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.TacSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.TacID))
                .Select(p => new { p.TacID, p.LastChangeCode })
                .ToDictionary(x => x.TacID, x => x.LastChangeCode);
            // Check concurrency conflicts
            foreach (var updatedTac in tacsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedTac.TacID, out var token) || token != updatedTac.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedTac.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(tacsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(tacsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from tac in _dbContext.TacSet.AsNoTracking()
				   from pac in _dbContext.PacSet.AsNoTracking().Where(l => l.PacID == tac.PacID).DefaultIfEmpty() //PacID
				   select new QueryDTO
                   {
					   TacObj = tac,
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
                    var tac = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (tac != null)
                    {
                        found = true;
                        Delete(tac.TacID);
                        delCount++;
                    }
                }
                while (found)
                {
                    found = false;
                    var tac = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (tac != null)
                    {
                        found = true;
                        Delete(tac.TacID);
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
                    var tac = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (tac != null)
                    {
                        found = true;
                        Delete(tac.TacID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }
        private List<Tac> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.TacObj.PacCodePeek = item.PacCode.Value; //PacID
            }
            List<Tac> results = data.Select(r => r.TacObj).ToList();
            return results;
        }
        //PacID
        public async Task<List<Tac>> GetByPacIDAsync(int id)
        {
            var tacsWithCodes = await BuildQuery()
                                    .Where(x => x.TacObj.PacID == id)
                                    .ToListAsync();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
        //PacID
        public List<Tac> GetByPacID(int id)
        {
            var tacsWithCodes = BuildQuery()
                                    .Where(x => x.TacObj.PacID == id)
                                    .ToList();
            List<Tac> finalTacs = ProcessMappings(tacsWithCodes);
            return finalTacs;
        }
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        private class QueryDTO
        {
            public Tac TacObj { get; set; }
            public Guid? PacCode { get; set; } //PacID
        }
    }
}
