using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
namespace FS.Farm.EF.Managers
{
	public class CustomerManager
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
		public async Task<int> GetTotalCountAsync()
		{
			return await _dbContext.CustomerSet.AsNoTracking().CountAsync();
		}
		public async Task<int?> GetMaxIdAsync()
		{
			return await _dbContext.CustomerSet.AsNoTracking().MaxAsync(p => (int?)p.CustomerID);
		}
		public async Task<Customer> GetByIdAsync(int id)
		{
			var customersWithCodes = await BuildQuery()
									.Where(p => p.CustomerObj.CustomerID == id)
									.ToListAsync();
            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);
            if (finalCustomers.Count > 0)
			{
				return finalCustomers[0];
            }
			return null;
        }
		public async Task<Customer> DirtyGetByIdAsync(int id) //to test
		{
			//return await _dbContext.CustomerSet.AsNoTracking().FirstOrDefaultAsync(p => p.CustomerID == id);
			// Begin a new transaction with the READ UNCOMMITTED isolation level
			using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
			try
			{
                var customersWithCodes = await BuildQuery()
                                        .Where(p => p.CustomerObj.CustomerID == id)
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
		public async Task<Customer> GetByCodeAsync(Guid code)
		{
			if (code == Guid.Empty)
				throw new ArgumentException("Code must not be an empty GUID.", nameof(code));
            var customersWithCodes = await BuildQuery()
                                    .Where(p => p.CustomerObj.Code == code)
                                    .ToListAsync();
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
                                        .Where(p => p.CustomerObj.Code == code)
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
		public async Task<IEnumerable<Customer>> GetAllAsync()
		{
            var customersWithCodes = await BuildQuery()
                                    .ToListAsync();
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
		public async Task<bool> DeleteAsync(int id)
		{
			var existingTransaction = _dbContext.Database.CurrentTransaction;
			if (existingTransaction == null)
			{
				using var transaction = existingTransaction ?? await _dbContext.Database.BeginTransactionAsync();
				try
				{
					var customer = await _dbContext.CustomerSet.FirstOrDefaultAsync(p => p.CustomerID == id);
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
					var customer = await _dbContext.CustomerSet.FirstOrDefaultAsync(p => p.CustomerID == id);
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
			var idsToUpdate = updatedCustomers.Select(p => p.CustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			//var existingTokens = await _dbContext.CustomerSet.AsNoTracking()
			//	.Where(p => idsToUpdate.Contains(p.CustomerID))
			//	.Select(p => new { p.CustomerID, p.LastChangeCode })
			//	.ToDictionaryAsync(p => p.CustomerID, p => p.LastChangeCode);
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
			var idsToUpdate = customersToDelete.Select(p => p.CustomerID).ToList();
			// Fetch only IDs and ConcurrencyToken values for existing entities
			var existingTokens = await _dbContext.CustomerSet.AsNoTracking()
				.Where(p => idsToUpdate.Contains(p.CustomerID))
				.Select(p => new { p.CustomerID, p.LastChangeCode })
				.ToDictionaryAsync(p => p.CustomerID, p => p.LastChangeCode);
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
		private IQueryable<QueryDTO> BuildQuery()
		{
			return from customer in _dbContext.CustomerSet.AsNoTracking()
				   from tac in _dbContext.TacSet.AsNoTracking().Where(l => l.TacID == customer.TacID).DefaultIfEmpty() //TacID
																														//ENDSET
				   select new QueryDTO
                   {
					   CustomerObj = customer,
					   TacCode = tac.Code, //TacID
											 //ENDSET
				   };
        }
		private List<Customer> ProcessMappings(List<QueryDTO> data)
		{
            foreach (var item in data)
            {
                item.CustomerObj.TacCodePeek = item.TacCode.Value; //TacID
                //ENDSET
            }
            List<Customer> results = data.Select(r => r.CustomerObj).ToList();
            return results;
        }
        //TacID
        public async Task<List<Customer>> GetByTacAsync(int id)
        {
            var customersWithCodes = await BuildQuery()
                                    .Where(p => p.CustomerObj.TacID == id)
                                    .ToListAsync();
            List<Customer> finalCustomers = ProcessMappings(customersWithCodes);
            return finalCustomers;
        }
		//ENDSET
        private class QueryDTO
        {
            public Customer CustomerObj { get; set; }
            public Guid? TacCode { get; set; } //TacID
        }
    }
}
