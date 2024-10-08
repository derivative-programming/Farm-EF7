using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;

namespace FS.Farm.EF.Managers
{
	public partial class CustomerManager
	{
		private readonly FarmDbContext _dbContext;

		public CustomerManager(FarmDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Customer> AddAsync(Customer customer)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.CustomerSet.Add(customer);
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customer).State = EntityState.Detached;

					await transaction.CommitAsync();

					return customer;
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
			}
			else
			{
				_dbContext.CustomerSet.Add(customer);
				await _dbContext.SaveChangesAsync();
				_dbContext.Entry(customer).State = EntityState.Detached;

				return customer;

			}
		}

        public Customer Add(Customer customer)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.CustomerSet.Add(customer);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customer).State = EntityState.Detached;

                    transaction.Commit();

                    return customer;
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
            else
            {
                _dbContext.CustomerSet.Add(customer);
                _dbContext.SaveChanges();
                _dbContext.Entry(customer).State = EntityState.Detached;

                return customer;

            }
        }
        public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.CustomerSet.AsNoTracking().CountAsync();
		}

        public int GetTotalCount()
        {
            return _dbContext.CustomerSet.AsNoTracking().Count();
        }

        public async Task<int?> GetMaxIdAsync()
        {
            int? maxId = await _dbContext.CustomerSet.AsNoTracking().MaxAsync(x => (int?)x.CustomerID);
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
            int? maxId = _dbContext.CustomerSet.AsNoTracking().Max(x => (int?)x.CustomerID);
            if (maxId == null)
            {
                return 0;
            }
            else
            {
                return maxId.Value;
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
		{

			var customersWithCodes = await BuildQuery()
									.Where(x => x.CustomerObj.CustomerID == id)
									.ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            if (finalCustomers.Count > 0)
			{
				return finalCustomers[0];

            }

			return null;

        }

        public Customer GetById(int id)
        {

            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.CustomerID == id)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            if (finalCustomers.Count > 0)
            {
                return finalCustomers[0];

            }

            return null;

        }

        public async Task<Customer> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.CustomerSet.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var customersWithCodes = await BuildQuery()
                                        .Where(x => x.CustomerObj.CustomerID == id)
                                        .ToListAsync();

                List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalCustomers.Count > 0)
                {
                    return finalCustomers[0];

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

        public Customer DirtyGetById(int id) //to test
        {
            //return await _dbContext.CustomerSet.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerID == id);
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var customersWithCodes = BuildQuery()
                                        .Where(x => x.CustomerObj.CustomerID == id)
                                        .ToList();

                List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalCustomers.Count > 0)
                {
                    return finalCustomers[0];

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
        public async Task<Customer> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var customersWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerObj.Code == code)
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            if (finalCustomers.Count > 0)
            {
                return finalCustomers[0];

            }

            return null;
        }

        public Customer GetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));

            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.Code == code)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            if (finalCustomers.Count > 0)
            {
                return finalCustomers[0];

            }

            return null;
        }

        public async Task<Customer> DirtyGetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

			try
			{
                var customersWithCodes = await BuildQuery()
                                        .Where(x => x.CustomerObj.Code == code)
                                        .ToListAsync();

                List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                await transaction.CommitAsync();

                if (finalCustomers.Count > 0)
                {
                    return finalCustomers[0];

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

        public Customer DirtyGetByCode(Guid code)
        {
            if (code == Guid.Empty)
                throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            // Begin a new transaction with the READ UNCOMMITTED isolation level
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                var customersWithCodes = BuildQuery()
                                        .Where(x => x.CustomerObj.Code == code)
                                        .ToList();

                List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

                // Commit transaction (this essentially just ends it since we're only reading data)
                transaction.Commit();

                if (finalCustomers.Count > 0)
                {
                    return finalCustomers[0];

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

        public async Task<List<Customer>> GetAllAsync()
		{
            var customersWithCodes = await BuildQuery()
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }
        public List<Customer> GetAll()
        {
            var customersWithCodes = BuildQuery()
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        public async Task<bool> UpdateAsync(Customer customerToUpdate)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{

				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					_dbContext.CustomerSet.Attach(customerToUpdate);
					_dbContext.Entry(customerToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					customerToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customerToUpdate).State = EntityState.Detached;

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
					_dbContext.CustomerSet.Attach(customerToUpdate);
					_dbContext.Entry(customerToUpdate).State = EntityState.Modified;
					//_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
					customerToUpdate.LastChangeCode = Guid.NewGuid();
					await _dbContext.SaveChangesAsync();
					_dbContext.Entry(customerToUpdate).State = EntityState.Detached;

					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

        public bool Update(Customer customerToUpdate)
        {
            var existingTransaction = _dbContext.Database.CurrentTransaction;
            if (existingTransaction == null)
            {

                using var transaction = existingTransaction ?? _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.CustomerSet.Attach(customerToUpdate);
                    _dbContext.Entry(customerToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    customerToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customerToUpdate).State = EntityState.Detached;

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
                    _dbContext.CustomerSet.Attach(customerToUpdate);
                    _dbContext.Entry(customerToUpdate).State = EntityState.Modified;
                    //_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                    customerToUpdate.LastChangeCode = Guid.NewGuid();
                    _dbContext.SaveChanges();
                    _dbContext.Entry(customerToUpdate).State = EntityState.Detached;

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
					var customer = await _dbContext.CustomerSet.FirstOrDefaultAsync(x => x.CustomerID == id);
					if (customer == null) return false;

					_dbContext.CustomerSet.Remove(customer);
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
					var customer = await _dbContext.CustomerSet.FirstOrDefaultAsync(x => x.CustomerID == id);
					if (customer == null) return false;

					_dbContext.CustomerSet.Remove(customer);
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
                    var customer = _dbContext.CustomerSet.FirstOrDefault(x => x.CustomerID == id);
                    if (customer == null) return false;

                    _dbContext.CustomerSet.Remove(customer);
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
                    var customer = _dbContext.CustomerSet.FirstOrDefault(x => x.CustomerID == id);
                    if (customer == null) return false;

                    _dbContext.CustomerSet.Remove(customer);
                    _dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public async Task BulkInsertAsync(IEnumerable<Customer> customers)
		{
			var bulkConfig = new BulkConfig
			{
				//	UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
				SetOutputIdentity = true,
				PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};
			foreach (var customer in customers)
			{
				customer.LastChangeCode = Guid.NewGuid();

				var entry = _dbContext.Entry(customer);
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
					await _dbContext.BulkInsertAsync(customers, bulkConfig);

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
				await _dbContext.BulkInsertAsync(customers, bulkConfig);

			}
		}

        public void BulkInsert(IEnumerable<Customer> customers)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
                SetOutputIdentity = true,
                PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };
            foreach (var customer in customers)
            {
                customer.LastChangeCode = Guid.NewGuid();

                var entry = _dbContext.Entry(customer);
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
                    _dbContext.BulkInsert(customers, bulkConfig);

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
                _dbContext.BulkInsert(customers, bulkConfig);

            }
        }

        public async Task BulkUpdateAsync(IEnumerable<Customer> updatedCustomers)
		{
			var bulkConfig = new BulkConfig
			{
			//	UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
			//	SetOutputIdentity = true,
			//	PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = updatedCustomers.Select(x => x.CustomerID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.CustomerSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.CustomerID))
			//	.Select(p => new { p.CustomerID, p.LastChangeCode })
			//	.ToDictionaryAsync(x => x.CustomerID, x => x.LastChangeCode);

			//// Check concurrency conflicts
			foreach (var updatedCustomer in updatedCustomers)
			{
				//	if (!existingTokens.TryGetValue(updatedCustomer.CustomerID, out var token) || token != updatedCustomer.LastChangeCode)
				//	{
				//		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				//	}
				//	updatedCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

				//	_dbContext.CustomerSet.Attach(updatedCustomer);
				//	_dbContext.Entry(updatedCustomer).State = EntityState.Modified;
				//	var entry = _dbContext.Entry(updatedCustomer);
				//	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

				//_dbContext.CustomerSet.Attach(updatedCustomer);
				//_dbContext.Entry(updatedCustomer).State = EntityState.Modified;
				//_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
				//updatedCustomer.LastChangeCode = Guid.NewGuid();
				//await _dbContext.SaveChangesAsync();
				//_dbContext.Entry(customerToUpdate).State = EntityState.Detached;
			}

			//TODO concurrency token check

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkUpdateAsync(updatedCustomers, bulkConfig);
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
				await _dbContext.BulkUpdateAsync(updatedCustomers, bulkConfig);
			}
		}

        public void BulkUpdate(IEnumerable<Customer> updatedCustomers)
        {
            var bulkConfig = new BulkConfig
            {
                //	UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
                //	SetOutputIdentity = true,
                //	PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = updatedCustomers.Select(x => x.CustomerID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            //var existingTokens = await _dbContext.CustomerSet.AsNoTracking()
            //	.Where(p => idsToUpdate.Contains(p.CustomerID))
            //	.Select(p => new { p.CustomerID, p.LastChangeCode })
            //	.ToDictionaryAsync(x => x.CustomerID, x => x.LastChangeCode);

            //// Check concurrency conflicts
            foreach (var updatedCustomer in updatedCustomers)
            {
                //	if (!existingTokens.TryGetValue(updatedCustomer.CustomerID, out var token) || token != updatedCustomer.LastChangeCode)
                //	{
                //		throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                //	}
                //	updatedCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.

                //	_dbContext.CustomerSet.Attach(updatedCustomer);
                //	_dbContext.Entry(updatedCustomer).State = EntityState.Modified;
                //	var entry = _dbContext.Entry(updatedCustomer);
                //	entry.Property("LastUpdatedUtcDateTime").CurrentValue = DateTime.UtcNow;

                //_dbContext.CustomerSet.Attach(updatedCustomer);
                //_dbContext.Entry(updatedCustomer).State = EntityState.Modified;
                //_dbContext.Entry(customerToUpdate).Property("LastChangeCode").CurrentValue = Guid.NewGuid();
                //updatedCustomer.LastChangeCode = Guid.NewGuid();
                //await _dbContext.SaveChangesAsync();
                //_dbContext.Entry(customerToUpdate).State = EntityState.Detached;
            }

            //TODO concurrency token check

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkUpdate(updatedCustomers, bulkConfig);
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
                _dbContext.BulkUpdate(updatedCustomers, bulkConfig);
            }
        }
        public async Task BulkDeleteAsync(IEnumerable<Customer> customersToDelete)
		{

			var bulkConfig = new BulkConfig
			{
				//UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
				//SetOutputIdentity = true,
				//PreserveInsertOrder = true,
				BatchSize = 5000,
				EnableShadowProperties = true
			};

			var idsToUpdate = customersToDelete.Select(x => x.CustomerID).ToList();

			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.CustomerSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.CustomerID))
				.Select(p => new { p.CustomerID, p.LastChangeCode })
				.ToDictionaryAsync(x => x.CustomerID, x => x.LastChangeCode);

			// Check concurrency conflicts
			foreach (var updatedCustomer in customersToDelete)
			{
				if (!existingTokens.TryGetValue(updatedCustomer.CustomerID, out var token) || token != updatedCustomer.LastChangeCode)
				{
					throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
				}
				updatedCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
			}

			var existingTransaction = _dbContext.Database.CurrentTransaction;

			if (existingTransaction == null)
			{
				using var transaction = await _dbContext.Database.BeginTransactionAsync();
				try
				{
					await _dbContext.BulkDeleteAsync(customersToDelete, bulkConfig);
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
				await _dbContext.BulkDeleteAsync(customersToDelete, bulkConfig);
			}
		}

        public void BulkDelete(IEnumerable<Customer> customersToDelete)
        {

            var bulkConfig = new BulkConfig
            {
                //UpdateByProperties = new List<string> { nameof(Customer.CustomerID), nameof(Customer.LastChangeCode) },
                //SetOutputIdentity = true,
                //PreserveInsertOrder = true,
                BatchSize = 5000,
                EnableShadowProperties = true
            };

            var idsToUpdate = customersToDelete.Select(x => x.CustomerID).ToList();

            // Fetch only IDs and ConcurrencyToken values for existing entities
            var existingTokens = _dbContext.CustomerSet.AsNoTracking()
                .Where(p => idsToUpdate.Contains(p.CustomerID))
                .Select(p => new { p.CustomerID, p.LastChangeCode })
                .ToDictionary(x => x.CustomerID, x => x.LastChangeCode);

            // Check concurrency conflicts
            foreach (var updatedCustomer in customersToDelete)
            {
                if (!existingTokens.TryGetValue(updatedCustomer.CustomerID, out var token) || token != updatedCustomer.LastChangeCode)
                {
                    throw new DbUpdateConcurrencyException("Concurrency conflict detected during bulk update.");
                }
                updatedCustomer.LastChangeCode = Guid.NewGuid(); // Renew the token for each update.
            }

            var existingTransaction = _dbContext.Database.CurrentTransaction;

            if (existingTransaction == null)
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    _dbContext.BulkDelete(customersToDelete, bulkConfig);
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
                _dbContext.BulkDelete(customersToDelete, bulkConfig);
            }
        }
        private IQueryable<QueryDTO> BuildQuery()
		{
			return from customer in _dbContext.CustomerSet.AsNoTracking()
				   from tac in _dbContext.TacSet.AsNoTracking().Where(l => l.TacID == customer.TacID).DefaultIfEmpty() //TacID
																																		   select new QueryDTO
                   {
					   CustomerObj = customer,
					   TacCode = tac.Code, //TacID
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
                    var customer = GetByCode(Guid.Parse("11111111-1111-1111-1111-111111111111"));
                    if (customer != null)
                    {
                        found = true;
                        Delete(customer.CustomerID);
                        delCount++;
                    }
                }

                while (found)
                {
                    found = false;
                    var customer = GetByCode(Guid.Parse("22222222-2222-2222-2222-222222222222"));
                    if (customer != null)
                    {
                        found = true;
                        Delete(customer.CustomerID);
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
                    var customer = GetByCode(Guid.Parse("99999999-9999-9999-9999-999999999999"));
                    if (customer != null)
                    {
                        found = true;
                        Delete(customer.CustomerID);
                        delCount++;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return delCount;
        }

        private List<Customer> ProcessMappings(List<QueryDTO> data)
		{

            foreach (var item in data)
            {
                item.CustomerObj.TacCodePeek = item.TacCode.Value; //TacID
            }

            List<Customer> results = data.Select(r => r.CustomerObj).ToList();

            return results;
        }
        //TacID
        public async Task<List<Customer>> GetByTacIDAsync(int id)
        {
            var customersWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerObj.TacID == id)
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }
        //TacID
        public List<Customer> GetByTacID(int id)
        {
            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.TacID == id)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        public async Task<List<Customer>> GetByEmailAsync(String email)
        {
            var customersWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerObj.Email == email)
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        public List<Customer> GetByEmail(String email)
        {
            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.Email == email)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }
        public async Task<List<Customer>> GetByForgotPasswordKeyValueAsync(String forgotPasswordKeyValue)
        {
            var customersWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerObj.ForgotPasswordKeyValue == forgotPasswordKeyValue)
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        public List<Customer> GetByForgotPasswordKeyValue(String forgotPasswordKeyValue)
        {
            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.ForgotPasswordKeyValue == forgotPasswordKeyValue)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }
        public async Task<List<Customer>> GetByFSUserCodeValueAsync(Guid fSUserCodeValue)
        {
            var customersWithCodes = await BuildQuery()
                                    .Where(x => x.CustomerObj.FSUserCodeValue == fSUserCodeValue)
                                    .ToListAsync();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        public List<Customer> GetByFSUserCodeValue(Guid fSUserCodeValue)
        {
            var customersWithCodes = BuildQuery()
                                    .Where(x => x.CustomerObj.FSUserCodeValue == fSUserCodeValue)
                                    .ToList();

            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);

            return finalCustomers;
        }

        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        private class QueryDTO
        {
            public Customer CustomerObj { get; set; }
            public Guid? TacCode { get; set; } //TacID
        }

    }
}
