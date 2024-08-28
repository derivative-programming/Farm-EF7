using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Test.Factory;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using FS.Common.Diagnostics.Loggers;

namespace FS.Farm.EF.Test.Tests.Managers
{
    [TestClass]
    public partial class CustomerTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                var result = await manager.AddAsync(customer);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.CustomerSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                var result = manager.Add(customer);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.CustomerSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var customer = await CreateTestCustomerAsync(context);
                    var result = await manager.AddAsync(customer);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.CustomerSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var customer = CreateTestCustomer(context);
                    var result = manager.Add(customer);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.CustomerSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoCustomers_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoCustomers_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithCustomers_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add some customers
                await manager.AddAsync(await CreateTestCustomerAsync(context));
                await manager.AddAsync(await CreateTestCustomerAsync(context));
                await manager.AddAsync(await CreateTestCustomerAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithCustomers_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add some customers
                manager.Add(CreateTestCustomer(context));
                manager.Add(CreateTestCustomer(context));
                manager.Add(CreateTestCustomer(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoCustomers_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoCustomers_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithCustomers_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add some customers
                var customer1 = await CreateTestCustomerAsync(context);
                var customer2 = await CreateTestCustomerAsync(context);
                var customer3 = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customer1);
                await manager.AddAsync(customer2);
                await manager.AddAsync(customer3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { customer1.CustomerID, customer2.CustomerID, customer3.CustomerID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithCustomers_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add some customers
                var customer1 = CreateTestCustomer(context);
                var customer2 = CreateTestCustomer(context);
                var customer3 = CreateTestCustomer(context);

                manager.Add(customer1);
                manager.Add(customer2);
                manager.Add(customer3);

                var result = manager.GetMaxId();

                var maxId = new[] { customer1.CustomerID, customer2.CustomerID, customer3.CustomerID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingCustomer_ShouldReturnCorrectCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customerToAdd = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customerToAdd);

                var fetchedCustomer = await manager.GetByIdAsync(customerToAdd.CustomerID);

                Assert.IsNotNull(fetchedCustomer);
                Assert.AreEqual(customerToAdd.CustomerID, fetchedCustomer.CustomerID);
            }
        }

        [TestMethod]
        public void GetById_ExistingCustomer_ShouldReturnCorrectCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customerToAdd = CreateTestCustomer(context);

                manager.Add(customerToAdd);

                var fetchedCustomer = manager.GetById(customerToAdd.CustomerID);

                Assert.IsNotNull(fetchedCustomer);
                Assert.AreEqual(customerToAdd.CustomerID, fetchedCustomer.CustomerID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomer = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedCustomer);
            }
        }
        [TestMethod]
        public void GetById_NonExistingCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomer = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedCustomer);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingCustomer_ShouldReturnCorrectCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customerToAdd = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customerToAdd);

                var fetchedCustomer = await manager.GetByCodeAsync(customerToAdd.Code.Value);

                Assert.IsNotNull(fetchedCustomer);
                Assert.AreEqual(customerToAdd.Code, fetchedCustomer.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingCustomer_ShouldReturnCorrectCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customerToAdd = CreateTestCustomer(context);

                manager.Add(customerToAdd);

                var fetchedCustomer = manager.GetByCode(customerToAdd.Code.Value);

                Assert.IsNotNull(fetchedCustomer);
                Assert.AreEqual(customerToAdd.Code, fetchedCustomer.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomer = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedCustomer);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomer = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedCustomer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetByCodeAsync_EmptyGuid_ShouldThrowArgumentException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetByCode_EmptyGuid_ShouldThrowArgumentException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleCustomers_ShouldReturnAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer1 = await CreateTestCustomerAsync(context);
                var customer2 = await CreateTestCustomerAsync(context);
                var customer3 = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customer1);
                await manager.AddAsync(customer2);
                await manager.AddAsync(customer3);

                var fetchedCustomers = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedCustomers);
                Assert.AreEqual(3, fetchedCustomers.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleCustomers_ShouldReturnAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer1 = CreateTestCustomer(context);
                var customer2 = CreateTestCustomer(context);
                var customer3 = CreateTestCustomer(context);

                manager.Add(customer1);
                manager.Add(customer2);
                manager.Add(customer3);

                var fetchedCustomers = manager.GetAll();

                Assert.IsNotNull(fetchedCustomers);
                Assert.AreEqual(3, fetchedCustomers.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomers = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedCustomers);
                Assert.AreEqual(0, fetchedCustomers.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var fetchedCustomers = manager.GetAll();

                Assert.IsNotNull(fetchedCustomers);
                Assert.AreEqual(0, fetchedCustomers.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidCustomer_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customer);

                customer.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(customer);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(customer.Code, context.CustomerSet.Find(customer.CustomerID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidCustomer_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);

                manager.Add(customer);

                customer.Code = Guid.NewGuid();
                var updateResult = manager.Update(customer);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(customer.Code, context.CustomerSet.Find(customer.CustomerID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Arrange
                var customer = await CreateTestCustomerAsync(context);
                await manager.AddAsync(customer);
                var firstInstance = await manager.GetByIdAsync(customer.CustomerID);
                var secondInstance = await manager.GetByIdAsync(customer.CustomerID);

                firstInstance.Code = Guid.NewGuid();
                await manager.UpdateAsync(firstInstance);

                // Act
                secondInstance.Code = Guid.NewGuid();
                var result = await manager.UpdateAsync(secondInstance);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void Update_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Arrange
                var customer = CreateTestCustomer(context);
                manager.Add(customer);
                var firstInstance = manager.GetById(customer.CustomerID);
                var secondInstance = manager.GetById(customer.CustomerID);

                firstInstance.Code = Guid.NewGuid();
                manager.Update(firstInstance);

                // Act
                secondInstance.Code = Guid.NewGuid();
                var result = manager.Update(secondInstance);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                //context.CustomerSet.Add(customer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(customer);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    customer.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(customer);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomer = freshContext.CustomerSet.Find(customer.CustomerID);
                    Assert.AreNotEqual(customer.Code, freshCustomer.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public void Update_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                //context.CustomerSet.Add(customer);
                //context.SaveChanges();
                manager.Add(customer);

                using (var transaction = context.Database.BeginTransaction())
                {
                    customer.Code = Guid.NewGuid();
                    var updateResult = manager.Update(customer);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomer = freshContext.CustomerSet.Find(customer.CustomerID);
                    Assert.AreNotEqual(customer.Code, freshCustomer.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customer);

                var deleteResult = await manager.DeleteAsync(customer.CustomerID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.CustomerSet.Find(customer.CustomerID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);

                manager.Add(customer);

                var deleteResult = manager.Delete(customer.CustomerID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.CustomerSet.Find(customer.CustomerID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var deleteResult = await manager.DeleteAsync(-1);  // Non-existing ID

                Assert.IsFalse(deleteResult);
            }
        }

        [TestMethod]
        public void Delete_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var deleteResult = manager.Delete(-1);  // Non-existing ID

                Assert.IsFalse(deleteResult);
            }
        }

        [TestMethod]
        public async Task DeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);

                await manager.AddAsync(customer);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(customer.CustomerID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomer = freshContext.CustomerSet.Find(customer.CustomerID);
                    Assert.IsNotNull(freshCustomer);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public void Delete_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);

                manager.Add(customer);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(customer.CustomerID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomer = freshContext.CustomerSet.Find(customer.CustomerID);
                    Assert.IsNotNull(freshCustomer);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidCustomers_ShouldInsertAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                await manager.BulkInsertAsync(customers);

                Assert.AreEqual(customers.Count, context.CustomerSet.Count());
                foreach (var customer in customers)
                {
                    Assert.IsNotNull(context.CustomerSet.Find(customer.CustomerID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidCustomers_ShouldInsertAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                manager.BulkInsert(customers);

                Assert.AreEqual(customers.Count, context.CustomerSet.Count());
                foreach (var customer in customers)
                {
                    Assert.IsNotNull(context.CustomerSet.Find(customer.CustomerID));
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.CustomerSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public void BulkInsert_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.CustomerSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add initial customers
                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                var customersToUpdate = new List<Customer>();

                foreach (var customer in customers)
                {
                    customersToUpdate.Add(await manager.AddAsync(customer));
                }

                // Update customers
                foreach (var customer in customersToUpdate)
                {
                    customer.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(customersToUpdate);

                // Verify updates
                foreach (var updatedCustomer in customersToUpdate)
                {
                    var customerFromDb = await manager.GetByIdAsync(updatedCustomer.CustomerID);// context.CustomerSet.Find(updatedCustomer.CustomerID);
                    Assert.AreEqual(updatedCustomer.Code, customerFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add initial customers
                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                var customersToUpdate = new List<Customer>();

                foreach (var customer in customers)
                {
                    customersToUpdate.Add(manager.Add(customer));
                }

                // Update customers
                foreach (var customer in customersToUpdate)
                {
                    customer.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(customersToUpdate);

                // Verify updates
                foreach (var updatedCustomer in customersToUpdate)
                {
                    var customerFromDb = manager.GetById(updatedCustomer.CustomerID);// context.CustomerSet.Find(updatedCustomer.CustomerID);
                    Assert.AreEqual(updatedCustomer.Code, customerFromDb.Code);
                }
            }
        }

        //[TestMethod]
        //[ExpectedException(typeof(DbUpdateConcurrencyException))]
        //public async Task BulkUpdateAsync_ConcurrencyMismatch_ShouldThrowConcurrencyException()
        //{
        //    var options = CreateSQLiteInMemoryDbContextOptions();

        //    using (var context = new FarmDbContext(options))
        //    {
        //        context.Database.EnsureCreated();
        //        var manager = new CustomerManager(context);

        //        var customers = new List<Customer>
        //        {
        //            await CreateTestCustomerAsync(context),
        //            await CreateTestCustomerAsync(context),
        //            await CreateTestCustomerAsync(context)
        //        };

        //        foreach (var customer in customers)
        //        {
        //            await manager.AddAsync(customer);
        //        }

        //        foreach (var customer in customers)
        //        {
        //            customer.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(customers);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                foreach (var customer in customers)
                {
                    await manager.AddAsync(customer);
                }

                foreach (var customer in customers)
                {
                    customer.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customer in customers)
                    {
                        var customerFromDb = freshContext.CustomerSet.Find(customer.CustomerID);
                        Assert.AreNotEqual(customer.Code, customerFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public void BulkUpdate_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                foreach (var customer in customers)
                {
                    manager.Add(customer);
                }

                foreach (var customer in customers)
                {
                    customer.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customer in customers)
                    {
                        var customerFromDb = freshContext.CustomerSet.Find(customer.CustomerID);
                        Assert.AreNotEqual(customer.Code, customerFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add initial customers
                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                foreach (var customer in customers)
                {
                    await manager.AddAsync(customer);
                }

                // Delete customers
                await manager.BulkDeleteAsync(customers);

                // Verify deletions
                foreach (var deletedCustomer in customers)
                {
                    var customerFromDb = context.CustomerSet.Find(deletedCustomer.CustomerID);
                    Assert.IsNull(customerFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                // Add initial customers
                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                foreach (var customer in customers)
                {
                    manager.Add(customer);
                }

                // Delete customers
                manager.BulkDelete(customers);

                // Verify deletions
                foreach (var deletedCustomer in customers)
                {
                    var customerFromDb = context.CustomerSet.Find(deletedCustomer.CustomerID);
                    Assert.IsNull(customerFromDb);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public async Task BulkDeleteAsync_ConcurrencyMismatch_ShouldThrowConcurrencyException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                foreach (var customer in customers)
                {
                    await manager.AddAsync(customer);
                }

                foreach (var customer in customers)
                {
                    customer.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(customers);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void BulkDelete_ConcurrencyMismatch_ShouldThrowConcurrencyException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                foreach (var customer in customers)
                {
                    manager.Add(customer);
                }

                foreach (var customer in customers)
                {
                    customer.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(customers);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context),
                    await CreateTestCustomerAsync(context)
                };

                foreach (var customer in customers)
                {
                    await manager.AddAsync(customer);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customer in customers)
                    {
                        var customerFromDb = freshContext.CustomerSet.Find(customer.CustomerID);
                        Assert.IsNotNull(customerFromDb);  // Customer should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public void BulkDelete_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customers = new List<Customer>
                {
                    CreateTestCustomer(context),
                    CreateTestCustomer(context),
                    CreateTestCustomer(context)
                };

                foreach (var customer in customers)
                {
                    manager.Add(customer);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(customers);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customer in customers)
                    {
                        var customerFromDb = freshContext.CustomerSet.Find(customer.CustomerID);
                        Assert.IsNotNull(customerFromDb);  // Customer should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//TacID
        public async Task GetByTacIdAsync_ValidTacId_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                //customer.TacID = 1;
                //context.CustomerSet.Add(customer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(customer);

                var result = await manager.GetByTacIDAsync(customer.TacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        [TestMethod]//TacID
        public void GetByTacId_ValidTacId_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                //customer.TacID = 1;
                //context.CustomerSet.Add(customer);
                //context.SaveChanges();
                manager.Add(customer);

                var result = manager.GetByTacID(customer.TacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        [TestMethod] //TacID
        public async Task GetByTacIdAsync_InvalidTacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = await manager.GetByTacIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //TacID
        public void GetByTacId_InvalidTacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var result = manager.GetByTacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //TacID
        public async Task GetByTacIdAsync_MultipleCustomersSameTacId_ShouldReturnAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer1 = await CreateTestCustomerAsync(context);
                var customer2 = await CreateTestCustomerAsync(context);
                customer2.TacID = customer1.TacID;

                await manager.AddAsync(customer1);
                await manager.AddAsync(customer2);

                //context.CustomerSet.AddRange(customer1, customer2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByTacIDAsync(customer1.TacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //TacID
        public void GetByTacId_MultipleCustomersSameTacId_ShouldReturnAllCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer1 = CreateTestCustomer(context);
                var customer2 = CreateTestCustomer(context);
                customer2.TacID = customer1.TacID;

                manager.Add(customer1);
                manager.Add(customer2);

                //context.CustomerSet.AddRange(customer1, customer2);
                //context.SaveChanges();

                var result = manager.GetByTacID(customer1.TacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod]
        public async Task GetByEmailAsync_ValidEmail_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                await manager.AddAsync(customer);

                var result = await manager.GetByEmailAsync(customer.Email);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        [TestMethod]
        public void GetByEmail_ValidEmail_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                manager.Add(customer);

                var result = manager.GetByEmail(customer.Email);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }
        [TestMethod]
        public async Task GetByForgotPasswordKeyValueAsync_ValidForgotPasswordKeyValue_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                await manager.AddAsync(customer);

                var result = await manager.GetByForgotPasswordKeyValueAsync(customer.ForgotPasswordKeyValue);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        [TestMethod]
        public void GetByForgotPasswordKeyValue_ValidForgotPasswordKeyValue_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                manager.Add(customer);

                var result = manager.GetByForgotPasswordKeyValue(customer.ForgotPasswordKeyValue);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }
        [TestMethod]
        public async Task GetByFSUserCodeValueAsync_ValidFSUserCodeValue_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = await CreateTestCustomerAsync(context);
                await manager.AddAsync(customer);

                var result = await manager.GetByFSUserCodeValueAsync(customer.FSUserCodeValue.Value);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        [TestMethod]
        public void GetByFSUserCodeValue_ValidFSUserCodeValue_ShouldReturnCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerManager(context);

                var customer = CreateTestCustomer(context);
                manager.Add(customer);

                var result = manager.GetByFSUserCodeValue(customer.FSUserCodeValue.Value);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customer.CustomerID, result.First().CustomerID);
            }
        }

        private async Task<Customer> CreateTestCustomerAsync(FarmDbContext dbContext)
        {
            return await CustomerFactory.CreateAsync(dbContext);
        }

        private Customer CreateTestCustomer(FarmDbContext dbContext)
        {
            return CustomerFactory.Create(dbContext);
        }

        private DbContextOptions<FarmDbContext> CreateInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<FarmDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private DbContextOptions<FarmDbContext> CreateSQLiteInMemoryDbContextOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return new DbContextOptionsBuilder<FarmDbContext>()
                .UseSqlite(connection)
                .Options;
        }
    }

}
