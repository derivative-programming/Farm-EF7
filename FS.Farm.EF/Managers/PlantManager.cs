using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;

namespace FS.Farm.EF.Managers
{
	public class PlantManager
	{
		private readonly FarmDbContext _dbContext;

		public PlantManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Plant> AddAsync(Plant plant)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{ 
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.PlantSet.Add(plant);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(plant).State = EntityState.Detached;

					await transaction.CommitAsync();

					return plant;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.PlantSet.Add(plant);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(plant).State = EntityState.Detached;

				return plant;

			}
		}


        public Plant Add(Plant plant)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.PlantSet.Add(plant);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(plant).State = EntityState.Detached;

                    transaction.Commit();

                    return plant;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.PlantSet.Add(plant);
                _dbContext.SaveChanges();
                _dbContext.Entry(plant).State = EntityState.Detached;

                return plant;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.PlantSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.PlantSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.PlantSet.AsNoTracking().MaxAsync(x => (int?)x.PlantID);
        }
        public int? GetMaxId()
        {
            return _dbContext.PlantSet.AsNoTracking().Max(x => (int?)x.PlantID);
        }

        public async Task<Plant> GetByIdAsync(int id)
		{ 

			var plantsWithCodes = await BuildQuery()
									.Where(x => x.PlantObj.PlantID == id)
									.ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            if (finalPlants.Count > 0)
			{
				return finalPlants[0];

            }

			return null;

        }

        public Plant GetById(int id)
        {

            var plantsWithCodes = BuildQuery()
                                    .Where(x => x.PlantObj.PlantID == id)
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            if (finalPlants.Count > 0)
            {
                return finalPlants[0];

            }

            return null;

        }

        public async Task<Plant> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.PlantSet.AsNoTracking().FirstOrDefaultAsync(x => x.PlantID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{ 
                var plantsWithCodes = await BuildQuery()
                                        .Where(x => x.PlantObj.PlantID == id)
                                        .ToListAsync();

                List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();
				 
                if (finalPlants.Count > 0)
                {
                    return finalPlants[0];

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

        public Plant DirtyGetById(int id) //to test
        {
            //return await _dbContext.PlantSet.AsNoTracking().FirstOrDefaultAsync(x => x.PlantID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var plantsWithCodes = BuildQuery()
                                        .Where(x => x.PlantObj.PlantID == id)
                                        .ToList();

                List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalPlants.Count > 0)
                {
                    return finalPlants[0];

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
        public async Task<Plant> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			 
            var plantsWithCodes = await BuildQuery()
                                    .Where(x => x.PlantObj.Code == code)
                                    .ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            if (finalPlants.Count > 0)
            {
                return finalPlants[0];

            }

            return null;
        }

        public Plant GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var plantsWithCodes = BuildQuery()
                                    .Where(x => x.PlantObj.Code == code)
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            if (finalPlants.Count > 0)
            {
                return finalPlants[0];

            }

            return null;
        }

        public async Task<Plant> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{ 
                var plantsWithCodes = await BuildQuery()
                                        .Where(x => x.PlantObj.Code == code)
                                        .ToListAsync();

                List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalPlants.Count > 0)
                {
                    return finalPlants[0];

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

        public Plant DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var plantsWithCodes = BuildQuery()
                                        .Where(x => x.PlantObj.Code == code)
                                        .ToList();

                List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalPlants.Count > 0)
                {
                    return finalPlants[0];

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

        public async Task<List<Plant>> GetAllAsync()
		{ 
            var plantsWithCodes = await BuildQuery() 
                                    .ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }
        public List<Plant> GetAll()
        {
            var plantsWithCodes = BuildQuery()
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }

        public async Task<bool> UpdateAsync(Plant plantToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.PlantSet.Attach(plantToUpdate);
					_dbContext.Entry(plantToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					plantToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(plantToUpdate).State = EntityState.Detached;

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
					_dbContext.PlantSet.Attach(plantToUpdate);
					_dbContext.Entry(plantToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					plantToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(plantToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{ 
					return false;
				}
			}
		}

        public bool Update(Plant plantToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.PlantSet.Attach(plantToUpdate);
                    _dbContext.Entry(plantToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    plantToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(plantToUpdate).State = EntityState.Detached;

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
                    _dbContext.PlantSet.Attach(plantToUpdate);
                    _dbContext.Entry(plantToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    plantToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(plantToUpdate).State = EntityState.Detached;

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
					var plant = await _dbContext.PlantSet.FirstOrDefaultAsync(x => x.PlantID == id);
					if (plant == null) return false;

					_dbContext.PlantSet.Remove(plant);
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
					var plant = await _dbContext.PlantSet.FirstOrDefaultAsync(x => x.PlantID == id);
					if (plant == null) return false;

					_dbContext.PlantSet.Remove(plant);
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
                    var plant = _dbContext.PlantSet.FirstOrDefault(x => x.PlantID == id);
                    if (plant == null) return false;

                    _dbContext.PlantSet.Remove(plant);
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
                    var plant = _dbContext.PlantSet.FirstOrDefault(x => x.PlantID == id);
                    if (plant == null) return false;

                    _dbContext.PlantSet.Remove(plant);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<Plant> plants)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true, 
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var plant in plants)
			{
				plant.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(plant);
				if (entry.State == EntityState.Added || entry.State == EntityState.Detached)
				{
					entry.Property("InsertUtcDateTime").CurrentValue = DateTime.UtcNow;
					entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
				}
			} 

			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();

				try
				{
					await _dbContext.BulkInsertAsync(plants, bulkConfig);

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
				await _dbContext.BulkInsertAsync(plants, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<Plant> plants)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var plant in plants)
            {
                plant.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(plant);
                if (entry.State == EntityState.Added || entry.State == EntityState.Detached)
                {
                    entry.Property("InsertUtcDateTime").CurrentValue = DateTime.UtcNow;
                    entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;
                }
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {
                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();

                try
                {
                    _dbContext.BulkInsert(plants, bulkConfig);

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
                _dbContext.BulkInsert(plants, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<Plant> updatedPlants)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true, 
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedPlants.Select(x => x.PlantID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.PlantSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.PlantID))
			//	.Select(p => new { p.PlantID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.PlantID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedPlant in updatedPlants)
			{
				//	if (!existingTokens.TryGetValue(updatedPlant.PlantID, out var token) || token != updatedPlant.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedPlant.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.PlantSet.Attach(updatedPlant);
				//	_dbContext.Entry(updatedPlant).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedPlant);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.PlantSet.Attach(updatedPlant);
				//_dbContext.Entry(updatedPlant).State = EntityState.Modified;
				//_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedPlant.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(plantToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedPlants, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedPlants, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<Plant> updatedPlants)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true, 
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedPlants.Select(x => x.PlantID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.PlantSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.PlantID))
            //	.Select(p => new { p.PlantID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.PlantID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedPlant in updatedPlants)
            {
                //	if (!existingTokens.TryGetValue(updatedPlant.PlantID, out var token) || token != updatedPlant.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedPlant.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.PlantSet.Attach(updatedPlant);
                //	_dbContext.Entry(updatedPlant).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedPlant);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.PlantSet.Attach(updatedPlant);
                //_dbContext.Entry(updatedPlant).State = EntityState.Modified;
                //_dbContext.Entry(plantToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedPlant.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(plantToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedPlants, bulkConfig);
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
                _dbContext.BulkUpdate(updatedPlants, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Plant> plantsToDelete)
		{ 


			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = plantsToDelete.Select(x => x.PlantID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.PlantSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.PlantID))
				.Select(p => new { p.PlantID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.PlantID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedPlant in plantsToDelete)
			{
				if (!existingTokens.TryGetValue(updatedPlant.PlantID, out var token) || token != updatedPlant.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedPlant.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(plantsToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(plantsToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<Plant> plantsToDelete)
        {


            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Plant.PlantID), nameof(Plant.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = plantsToDelete.Select(x => x.PlantID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.PlantSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.PlantID))
                .Select(p => new { p.PlantID, p.LastChangeCode })
                .ToDictionary(x => x.PlantID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedPlant in plantsToDelete)
            {
                if (!existingTokens.TryGetValue(updatedPlant.PlantID, out var token) || token != updatedPlant.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedPlant.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(plantsToDelete, bulkConfig);
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
                _dbContext.BulkDelete(plantsToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from plant in _dbContext.PlantSet.AsNoTracking()
				   from flavor in _dbContext.FlavorSet.AsNoTracking().Where(f => f.FlavorID == plant.FlvrForeignKeyID).DefaultIfEmpty() //FlvrForeignKeyID
				   from land in _dbContext.LandSet.AsNoTracking().Where(l => l.LandID == plant.LandID).DefaultIfEmpty() //LandID
																														//ENDSET  
				   select new QueryDTO
                   {
					   PlantObj = plant,
					   FlvrForeignKeyCode = flavor.Code, //FlvrForeignKeyID
					   LandCode = land.Code, //LandID
											 //ENDSET
				   }; 
        }

        public int ClearTestObjects()
        {
            int delCount = 0;
            bool found = false;

            try
            { 
                while (found)
                {
                    found = false;
                    var plant = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (plant != null)
                    {
                        found = true;
                        Delete(plant.PlantID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var plant = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (plant != null)
                    {
                        found = true;
                        Delete(plant.PlantID);
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
            bool found = false;

            try
            { 
                while (found)
                {
                    found = false;
                    var plant = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (plant != null)
                    {
                        found = true;
                        Delete(plant.PlantID);
                        delCount++;
                    }
                } 
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<Plant> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.PlantObj.FlvrForeignKeyCodePeek = item.FlvrForeignKeyCode.Value; //FlvrForeignKeyID
                item.PlantObj.LandCodePeek = item.LandCode.Value; //LandID
                //ENDSET
            }

            List<Plant> results = data.Select(r => r.PlantObj).ToList();

            return results;
        }



        //FlvrForeignKeyID
        public async Task<List<Plant>> GetByFlvrForeignKeyAsync(int id) 
        {  
            var plantsWithCodes = await BuildQuery()
                                    .Where(x => x.PlantObj.FlvrForeignKeyID == id)
                                    .ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }

        //LandID
        public async Task<List<Plant>> GetByLandAsync(int id) 
        {
            var plantsWithCodes = await BuildQuery()
                                    .Where(x => x.PlantObj.LandID == id)
                                    .ToListAsync();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }


        //FlvrForeignKeyID
        public List<Plant> GetByFlvrForeignKey(int id)
        {
            var plantsWithCodes = BuildQuery()
                                    .Where(x => x.PlantObj.FlvrForeignKeyID == id)
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }

        //LandID
        public List<Plant> GetByLand(int id)
        {
            var plantsWithCodes = BuildQuery()
                                    .Where(x => x.PlantObj.LandID == id)
                                    .ToList();

            List<Plant> finalPlants = ProcessMappings(plantsWithCodes);

            return finalPlants;
        }

        //ENDSET

        private class QueryDTO
        {
            public Plant PlantObj { get; set; }
            public Guid? FlvrForeignKeyCode { get; set; } //FlvrForeignKeyID
            public Guid? LandCode { get; set; } //LandID
        } 
    }
}