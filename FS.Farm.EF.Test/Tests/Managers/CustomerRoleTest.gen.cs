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
    public partial class CustomerRoleTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);
                var result = await manager.AddAsync(customerRole);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.CustomerRoleSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);
                var result = manager.Add(customerRole);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.CustomerRoleSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var customerRole = await CreateTestCustomerRoleAsync(context);
                    var result = await manager.AddAsync(customerRole);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.CustomerRoleSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var customerRole = CreateTestCustomerRole(context);
                    var result = manager.Add(customerRole);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.CustomerRoleSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoCustomerRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoCustomerRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithCustomerRoles_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add some customerRoles
                await manager.AddAsync(await CreateTestCustomerRoleAsync(context));
                await manager.AddAsync(await CreateTestCustomerRoleAsync(context));
                await manager.AddAsync(await CreateTestCustomerRoleAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithCustomerRoles_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add some customerRoles
                manager.Add(CreateTestCustomerRole(context));
                manager.Add(CreateTestCustomerRole(context));
                manager.Add(CreateTestCustomerRole(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoCustomerRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoCustomerRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithCustomerRoles_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add some customerRoles
                var customerRole1 = await CreateTestCustomerRoleAsync(context);
                var customerRole2 = await CreateTestCustomerRoleAsync(context);
                var customerRole3 = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRole1);
                await manager.AddAsync(customerRole2);
                await manager.AddAsync(customerRole3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { customerRole1.CustomerRoleID, customerRole2.CustomerRoleID, customerRole3.CustomerRoleID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithCustomerRoles_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add some customerRoles
                var customerRole1 = CreateTestCustomerRole(context);
                var customerRole2 = CreateTestCustomerRole(context);
                var customerRole3 = CreateTestCustomerRole(context);

                manager.Add(customerRole1);
                manager.Add(customerRole2);
                manager.Add(customerRole3);

                var result = manager.GetMaxId();

                var maxId = new[] { customerRole1.CustomerRoleID, customerRole2.CustomerRoleID, customerRole3.CustomerRoleID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingCustomerRole_ShouldReturnCorrectCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoleToAdd = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRoleToAdd);

                var fetchedCustomerRole = await manager.GetByIdAsync(customerRoleToAdd.CustomerRoleID);

                Assert.IsNotNull(fetchedCustomerRole);
                Assert.AreEqual(customerRoleToAdd.CustomerRoleID, fetchedCustomerRole.CustomerRoleID);
            }
        }

        [TestMethod]
        public void GetById_ExistingCustomerRole_ShouldReturnCorrectCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoleToAdd = CreateTestCustomerRole(context);

                manager.Add(customerRoleToAdd);

                var fetchedCustomerRole = manager.GetById(customerRoleToAdd.CustomerRoleID);

                Assert.IsNotNull(fetchedCustomerRole);
                Assert.AreEqual(customerRoleToAdd.CustomerRoleID, fetchedCustomerRole.CustomerRoleID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingCustomerRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRole = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedCustomerRole);
            }
        }
        [TestMethod]
        public void GetById_NonExistingCustomerRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRole = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedCustomerRole);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingCustomerRole_ShouldReturnCorrectCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoleToAdd = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRoleToAdd);

                var fetchedCustomerRole = await manager.GetByCodeAsync(customerRoleToAdd.Code.Value);

                Assert.IsNotNull(fetchedCustomerRole);
                Assert.AreEqual(customerRoleToAdd.Code, fetchedCustomerRole.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingCustomerRole_ShouldReturnCorrectCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoleToAdd = CreateTestCustomerRole(context);

                manager.Add(customerRoleToAdd);

                var fetchedCustomerRole = manager.GetByCode(customerRoleToAdd.Code.Value);

                Assert.IsNotNull(fetchedCustomerRole);
                Assert.AreEqual(customerRoleToAdd.Code, fetchedCustomerRole.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingCustomerRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRole = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedCustomerRole);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingCustomerRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRole = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedCustomerRole);
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
                var manager = new CustomerRoleManager(context);

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
                var manager = new CustomerRoleManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleCustomerRoles_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = await CreateTestCustomerRoleAsync(context);
                var customerRole2 = await CreateTestCustomerRoleAsync(context);
                var customerRole3 = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRole1);
                await manager.AddAsync(customerRole2);
                await manager.AddAsync(customerRole3);

                var fetchedCustomerRoles = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedCustomerRoles);
                Assert.AreEqual(3, fetchedCustomerRoles.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleCustomerRoles_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = CreateTestCustomerRole(context);
                var customerRole2 = CreateTestCustomerRole(context);
                var customerRole3 = CreateTestCustomerRole(context);

                manager.Add(customerRole1);
                manager.Add(customerRole2);
                manager.Add(customerRole3);

                var fetchedCustomerRoles = manager.GetAll();

                Assert.IsNotNull(fetchedCustomerRoles);
                Assert.AreEqual(3, fetchedCustomerRoles.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRoles = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedCustomerRoles);
                Assert.AreEqual(0, fetchedCustomerRoles.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var fetchedCustomerRoles = manager.GetAll();

                Assert.IsNotNull(fetchedCustomerRoles);
                Assert.AreEqual(0, fetchedCustomerRoles.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidCustomerRole_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRole);

                customerRole.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(customerRole);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(customerRole.Code, context.CustomerRoleSet.Find(customerRole.CustomerRoleID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidCustomerRole_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);

                manager.Add(customerRole);

                customerRole.Code = Guid.NewGuid();
                var updateResult = manager.Update(customerRole);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(customerRole.Code, context.CustomerRoleSet.Find(customerRole.CustomerRoleID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Arrange
                var customerRole = await CreateTestCustomerRoleAsync(context);
                await manager.AddAsync(customerRole);
                var firstInstance = await manager.GetByIdAsync(customerRole.CustomerRoleID);
                var secondInstance = await manager.GetByIdAsync(customerRole.CustomerRoleID);

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
                var manager = new CustomerRoleManager(context);

                // Arrange
                var customerRole = CreateTestCustomerRole(context);
                manager.Add(customerRole);
                var firstInstance = manager.GetById(customerRole.CustomerRoleID);
                var secondInstance = manager.GetById(customerRole.CustomerRoleID);

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
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);
                //context.CustomerRoleSet.Add(customerRole);
                //await context.SaveChangesAsync();
                await manager.AddAsync(customerRole);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    customerRole.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(customerRole);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomerRole = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                    Assert.AreNotEqual(customerRole.Code, freshCustomerRole.Code); // Because the transaction was not committed.
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
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);
                //context.CustomerRoleSet.Add(customerRole);
                //context.SaveChanges();
                manager.Add(customerRole);

                using (var transaction = context.Database.BeginTransaction())
                {
                    customerRole.Code = Guid.NewGuid();
                    var updateResult = manager.Update(customerRole);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomerRole = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                    Assert.AreNotEqual(customerRole.Code, freshCustomerRole.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRole);

                var deleteResult = await manager.DeleteAsync(customerRole.CustomerRoleID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.CustomerRoleSet.Find(customerRole.CustomerRoleID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteCustomerRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);

                manager.Add(customerRole);

                var deleteResult = manager.Delete(customerRole.CustomerRoleID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.CustomerRoleSet.Find(customerRole.CustomerRoleID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

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
                var manager = new CustomerRoleManager(context);

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
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);

                await manager.AddAsync(customerRole);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(customerRole.CustomerRoleID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomerRole = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                    Assert.IsNotNull(freshCustomerRole);  // Because the transaction was not committed.
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
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);

                manager.Add(customerRole);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(customerRole.CustomerRoleID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshCustomerRole = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                    Assert.IsNotNull(freshCustomerRole);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidCustomerRoles_ShouldInsertAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                await manager.BulkInsertAsync(customerRoles);

                Assert.AreEqual(customerRoles.Count, context.CustomerRoleSet.Count());
                foreach (var customerRole in customerRoles)
                {
                    Assert.IsNotNull(context.CustomerRoleSet.Find(customerRole.CustomerRoleID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidCustomerRoles_ShouldInsertAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                manager.BulkInsert(customerRoles);

                Assert.AreEqual(customerRoles.Count, context.CustomerRoleSet.Count());
                foreach (var customerRole in customerRoles)
                {
                    Assert.IsNotNull(context.CustomerRoleSet.Find(customerRole.CustomerRoleID));
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.CustomerRoleSet.Count());  // Because the transaction was not committed.
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.CustomerRoleSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add initial customerRoles
                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                var customerRolesToUpdate = new List<CustomerRole>();

                foreach (var customerRole in customerRoles)
                {
                    customerRolesToUpdate.Add(await manager.AddAsync(customerRole));
                }

                // Update customerRoles
                foreach (var customerRole in customerRolesToUpdate)
                {
                    customerRole.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(customerRolesToUpdate);

                // Verify updates
                foreach (var updatedCustomerRole in customerRolesToUpdate)
                {
                    var customerRoleFromDb = await manager.GetByIdAsync(updatedCustomerRole.CustomerRoleID);// context.CustomerRoleSet.Find(updatedCustomerRole.CustomerRoleID);
                    Assert.AreEqual(updatedCustomerRole.Code, customerRoleFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add initial customerRoles
                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                var customerRolesToUpdate = new List<CustomerRole>();

                foreach (var customerRole in customerRoles)
                {
                    customerRolesToUpdate.Add(manager.Add(customerRole));
                }

                // Update customerRoles
                foreach (var customerRole in customerRolesToUpdate)
                {
                    customerRole.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(customerRolesToUpdate);

                // Verify updates
                foreach (var updatedCustomerRole in customerRolesToUpdate)
                {
                    var customerRoleFromDb = manager.GetById(updatedCustomerRole.CustomerRoleID);// context.CustomerRoleSet.Find(updatedCustomerRole.CustomerRoleID);
                    Assert.AreEqual(updatedCustomerRole.Code, customerRoleFromDb.Code);
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
        //        var manager = new CustomerRoleManager(context);

        //        var customerRoles = new List<CustomerRole>
        //        {
        //            await CreateTestCustomerRoleAsync(context),
        //            await CreateTestCustomerRoleAsync(context),
        //            await CreateTestCustomerRoleAsync(context)
        //        };

        //        foreach (var customerRole in customerRoles)
        //        {
        //            await manager.AddAsync(customerRole);
        //        }

        //        foreach (var customerRole in customerRoles)
        //        {
        //            customerRole.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(customerRoles);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    await manager.AddAsync(customerRole);
                }

                foreach (var customerRole in customerRoles)
                {
                    customerRole.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customerRole in customerRoles)
                    {
                        var customerRoleFromDb = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                        Assert.AreNotEqual(customerRole.Code, customerRoleFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    manager.Add(customerRole);
                }

                foreach (var customerRole in customerRoles)
                {
                    customerRole.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customerRole in customerRoles)
                    {
                        var customerRoleFromDb = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                        Assert.AreNotEqual(customerRole.Code, customerRoleFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add initial customerRoles
                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    await manager.AddAsync(customerRole);
                }

                // Delete customerRoles
                await manager.BulkDeleteAsync(customerRoles);

                // Verify deletions
                foreach (var deletedCustomerRole in customerRoles)
                {
                    var customerRoleFromDb = context.CustomerRoleSet.Find(deletedCustomerRole.CustomerRoleID);
                    Assert.IsNull(customerRoleFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                // Add initial customerRoles
                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    manager.Add(customerRole);
                }

                // Delete customerRoles
                manager.BulkDelete(customerRoles);

                // Verify deletions
                foreach (var deletedCustomerRole in customerRoles)
                {
                    var customerRoleFromDb = context.CustomerRoleSet.Find(deletedCustomerRole.CustomerRoleID);
                    Assert.IsNull(customerRoleFromDb);
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    await manager.AddAsync(customerRole);
                }

                foreach (var customerRole in customerRoles)
                {
                    customerRole.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(customerRoles);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    manager.Add(customerRole);
                }

                foreach (var customerRole in customerRoles)
                {
                    customerRole.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(customerRoles);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context),
                    await CreateTestCustomerRoleAsync(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    await manager.AddAsync(customerRole);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customerRole in customerRoles)
                    {
                        var customerRoleFromDb = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                        Assert.IsNotNull(customerRoleFromDb);  // CustomerRole should still exist as the transaction wasn't committed.
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
                var manager = new CustomerRoleManager(context);

                var customerRoles = new List<CustomerRole>
                {
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context),
                    CreateTestCustomerRole(context)
                };

                foreach (var customerRole in customerRoles)
                {
                    manager.Add(customerRole);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(customerRoles);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var customerRole in customerRoles)
                    {
                        var customerRoleFromDb = freshContext.CustomerRoleSet.Find(customerRole.CustomerRoleID);
                        Assert.IsNotNull(customerRoleFromDb);  // CustomerRole should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//CustomerID
        public async Task GetByCustomerIdAsync_ValidCustomerId_ShouldReturnCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);
                //customerRole.CustomerID = 1;
                //context.CustomerRoleSet.Add(customerRole);
                //await context.SaveChangesAsync();
                await manager.AddAsync(customerRole);

                var result = await manager.GetByCustomerIDAsync(customerRole.CustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customerRole.CustomerRoleID, result.First().CustomerRoleID);
            }
        }
        [TestMethod]//RoleID
        public async Task GetByRoleAsync_ValidRoleID_ShouldReturnCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = await CreateTestCustomerRoleAsync(context);
                // customerRole.RoleID = 1;
                //context.CustomerRoleSet.Add(customerRole);
                //await context.SaveChangesAsync();
                await manager.AddAsync(customerRole);

                var result = await manager.GetByRoleIDAsync(customerRole.RoleID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customerRole.CustomerRoleID, result.First().CustomerRoleID);
            }
        }

        [TestMethod]//CustomerID
        public void GetByCustomerId_ValidCustomerId_ShouldReturnCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);
                //customerRole.CustomerID = 1;
                //context.CustomerRoleSet.Add(customerRole);
                //context.SaveChanges();
                manager.Add(customerRole);

                var result = manager.GetByCustomerID(customerRole.CustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customerRole.CustomerRoleID, result.First().CustomerRoleID);
            }
        }
        [TestMethod]//RoleID
        public void GetByRole_ValidRoleID_ShouldReturnCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole = CreateTestCustomerRole(context);
                // customerRole.RoleID = 1;
                //context.CustomerRoleSet.Add(customerRole);
                //context.SaveChanges();
                manager.Add(customerRole);

                var result = manager.GetByRoleID(customerRole.RoleID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(customerRole.CustomerRoleID, result.First().CustomerRoleID);
            }
        }

        [TestMethod] //CustomerID
        public async Task GetByCustomerIdAsync_InvalidCustomerId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = await manager.GetByCustomerIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//RoleID
        public async Task GetByRoleAsync_InvalidRoleID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = await manager.GetByRoleIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //CustomerID
        public void GetByCustomerId_InvalidCustomerId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = manager.GetByCustomerID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//RoleID
        public void GetByRole_InvalidRoleID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var result = manager.GetByRoleID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //CustomerID
        public async Task GetByCustomerIdAsync_MultipleCustomerRolesSameCustomerId_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = await CreateTestCustomerRoleAsync(context);
                var customerRole2 = await CreateTestCustomerRoleAsync(context);
                customerRole2.CustomerID = customerRole1.CustomerID;

                await manager.AddAsync(customerRole1);
                await manager.AddAsync(customerRole2);

                //context.CustomerRoleSet.AddRange(customerRole1, customerRole2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByCustomerIDAsync(customerRole1.CustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //RoleID
        public async Task GetByRoleAsync_MultipleCustomerRolesSameRoleID_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = await CreateTestCustomerRoleAsync(context);
                //  customerRole1.RoleID = 1;
                var customerRole2 = await CreateTestCustomerRoleAsync(context);
                customerRole2.RoleID = customerRole1.RoleID;

                //context.CustomerRoleSet.AddRange(customerRole1, customerRole2);
                //await context.SaveChangesAsync();

                await manager.AddAsync(customerRole1);
                await manager.AddAsync(customerRole2);

                var result = await manager.GetByRoleIDAsync(customerRole1.RoleID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //CustomerID
        public void GetByCustomerId_MultipleCustomerRolesSameCustomerId_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = CreateTestCustomerRole(context);
                var customerRole2 = CreateTestCustomerRole(context);
                customerRole2.CustomerID = customerRole1.CustomerID;

                manager.Add(customerRole1);
                manager.Add(customerRole2);

                //context.CustomerRoleSet.AddRange(customerRole1, customerRole2);
                //context.SaveChanges();

                var result = manager.GetByCustomerID(customerRole1.CustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //RoleID
        public void GetByRole_MultipleCustomerRolesSameRoleID_ShouldReturnAllCustomerRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new CustomerRoleManager(context);

                var customerRole1 = CreateTestCustomerRole(context);
                //  customerRole1.RoleID = 1;
                var customerRole2 = CreateTestCustomerRole(context);
                customerRole2.RoleID = customerRole1.RoleID;

                //context.CustomerRoleSet.AddRange(customerRole1, customerRole2);
                //context.SaveChanges();

                manager.Add(customerRole1);
                manager.Add(customerRole2);

                var result = manager.GetByRoleID(customerRole1.RoleID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<CustomerRole> CreateTestCustomerRoleAsync(FarmDbContext dbContext)
        {
            return await CustomerRoleFactory.CreateAsync(dbContext);
        }

        private CustomerRole CreateTestCustomerRole(FarmDbContext dbContext)
        {
            return CustomerRoleFactory.Create(dbContext);
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
