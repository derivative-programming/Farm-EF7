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
    public class FlavorTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                var result = await manager.AddAsync(flavor);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.FlavorSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                var result = manager.Add(flavor);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.FlavorSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var flavor = await CreateTestFlavorAsync(context);
                    var result = await manager.AddAsync(flavor);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.FlavorSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var flavor = CreateTestFlavor(context);
                    var result = manager.Add(flavor);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.FlavorSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoFlavors_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoFlavors_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithFlavors_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add some flavors
                await manager.AddAsync(await CreateTestFlavorAsync(context));
                await manager.AddAsync(await CreateTestFlavorAsync(context));
                await manager.AddAsync(await CreateTestFlavorAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithFlavors_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add some flavors
                manager.Add(CreateTestFlavor(context));
                manager.Add(CreateTestFlavor(context));
                manager.Add(CreateTestFlavor(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoFlavors_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoFlavors_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = manager.GetMaxId();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithFlavors_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add some flavors
                var flavor1 = await CreateTestFlavorAsync(context);
                var flavor2 = await CreateTestFlavorAsync(context);
                var flavor3 = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor1);
                await manager.AddAsync(flavor2);
                await manager.AddAsync(flavor3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { flavor1.FlavorID, flavor2.FlavorID, flavor3.FlavorID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithFlavors_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add some flavors
                var flavor1 = CreateTestFlavor(context);
                var flavor2 = CreateTestFlavor(context);
                var flavor3 = CreateTestFlavor(context);
                manager.Add(flavor1);
                manager.Add(flavor2);
                manager.Add(flavor3);
                var result = manager.GetMaxId();
                var maxId = new[] { flavor1.FlavorID, flavor2.FlavorID, flavor3.FlavorID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingFlavor_ShouldReturnCorrectFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavorToAdd = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavorToAdd);
                var fetchedFlavor = await manager.GetByIdAsync(flavorToAdd.FlavorID);
                Assert.IsNotNull(fetchedFlavor);
                Assert.AreEqual(flavorToAdd.FlavorID, fetchedFlavor.FlavorID);
            }
        }
        [TestMethod]
        public void GetById_ExistingFlavor_ShouldReturnCorrectFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavorToAdd = CreateTestFlavor(context);
                manager.Add(flavorToAdd);
                var fetchedFlavor = manager.GetById(flavorToAdd.FlavorID);
                Assert.IsNotNull(fetchedFlavor);
                Assert.AreEqual(flavorToAdd.FlavorID, fetchedFlavor.FlavorID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingFlavor_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavor = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedFlavor);
            }
        }
        [TestMethod]
        public void GetById_NonExistingFlavor_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavor = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedFlavor);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingFlavor_ShouldReturnCorrectFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavorToAdd = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavorToAdd);
                var fetchedFlavor = await manager.GetByCodeAsync(flavorToAdd.Code.Value);
                Assert.IsNotNull(fetchedFlavor);
                Assert.AreEqual(flavorToAdd.Code, fetchedFlavor.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingFlavor_ShouldReturnCorrectFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavorToAdd = CreateTestFlavor(context);
                manager.Add(flavorToAdd);
                var fetchedFlavor = manager.GetByCode(flavorToAdd.Code.Value);
                Assert.IsNotNull(fetchedFlavor);
                Assert.AreEqual(flavorToAdd.Code, fetchedFlavor.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingFlavor_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavor = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedFlavor);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingFlavor_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavor = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedFlavor);
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
                var manager = new FlavorManager(context);
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
                var manager = new FlavorManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleFlavors_ShouldReturnAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor1 = await CreateTestFlavorAsync(context);
                var flavor2 = await CreateTestFlavorAsync(context);
                var flavor3 = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor1);
                await manager.AddAsync(flavor2);
                await manager.AddAsync(flavor3);
                var fetchedFlavors = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedFlavors);
                Assert.AreEqual(3, fetchedFlavors.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleFlavors_ShouldReturnAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor1 = CreateTestFlavor(context);
                var flavor2 = CreateTestFlavor(context);
                var flavor3 = CreateTestFlavor(context);
                manager.Add(flavor1);
                manager.Add(flavor2);
                manager.Add(flavor3);
                var fetchedFlavors = manager.GetAll();
                Assert.IsNotNull(fetchedFlavors);
                Assert.AreEqual(3, fetchedFlavors.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavors = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedFlavors);
                Assert.AreEqual(0, fetchedFlavors.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var fetchedFlavors = manager.GetAll();
                Assert.IsNotNull(fetchedFlavors);
                Assert.AreEqual(0, fetchedFlavors.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidFlavor_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor);
                flavor.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(flavor);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(flavor.Code, context.FlavorSet.Find(flavor.FlavorID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidFlavor_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                manager.Add(flavor);
                flavor.Code = Guid.NewGuid();
                var updateResult = manager.Update(flavor);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(flavor.Code, context.FlavorSet.Find(flavor.FlavorID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Arrange
                var flavor = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor);
                var firstInstance = await manager.GetByIdAsync(flavor.FlavorID);
                var secondInstance = await manager.GetByIdAsync(flavor.FlavorID);
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
                var manager = new FlavorManager(context);
                // Arrange
                var flavor = CreateTestFlavor(context);
                manager.Add(flavor);
                var firstInstance = manager.GetById(flavor.FlavorID);
                var secondInstance = manager.GetById(flavor.FlavorID);
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
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                //context.FlavorSet.Add(flavor);
                //await context.SaveChangesAsync();
                await manager.AddAsync(flavor);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    flavor.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(flavor);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshFlavor = freshContext.FlavorSet.Find(flavor.FlavorID);
                    Assert.AreNotEqual(flavor.Code, freshFlavor.Code); // Because the transaction was not committed.
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
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                //context.FlavorSet.Add(flavor);
                //context.SaveChanges();
                manager.Add(flavor);
                using (var transaction = context.Database.BeginTransaction())
                {
                    flavor.Code = Guid.NewGuid();
                    var updateResult = manager.Update(flavor);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshFlavor = freshContext.FlavorSet.Find(flavor.FlavorID);
                    Assert.AreNotEqual(flavor.Code, freshFlavor.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor);
                var deleteResult = await manager.DeleteAsync(flavor.FlavorID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.FlavorSet.Find(flavor.FlavorID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteFlavor()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                manager.Add(flavor);
                var deleteResult = manager.Delete(flavor.FlavorID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.FlavorSet.Find(flavor.FlavorID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
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
                var manager = new FlavorManager(context);
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
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                await manager.AddAsync(flavor);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(flavor.FlavorID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshFlavor = freshContext.FlavorSet.Find(flavor.FlavorID);
                    Assert.IsNotNull(freshFlavor);  // Because the transaction was not committed.
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
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                manager.Add(flavor);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(flavor.FlavorID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshFlavor = freshContext.FlavorSet.Find(flavor.FlavorID);
                    Assert.IsNotNull(freshFlavor);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidFlavors_ShouldInsertAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                await manager.BulkInsertAsync(flavors);
                Assert.AreEqual(flavors.Count, context.FlavorSet.Count());
                foreach (var flavor in flavors)
                {
                    Assert.IsNotNull(context.FlavorSet.Find(flavor.FlavorID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidFlavors_ShouldInsertAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                manager.BulkInsert(flavors);
                Assert.AreEqual(flavors.Count, context.FlavorSet.Count());
                foreach (var flavor in flavors)
                {
                    Assert.IsNotNull(context.FlavorSet.Find(flavor.FlavorID));
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.FlavorSet.Count());  // Because the transaction was not committed.
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.FlavorSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add initial flavors
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                var flavorsToUpdate = new List<Flavor>();
                foreach (var flavor in flavors)
                {
                    flavorsToUpdate.Add(await manager.AddAsync(flavor));
                }
                // Update flavors
                foreach (var flavor in flavorsToUpdate)
                {
                    flavor.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(flavorsToUpdate);
                // Verify updates
                foreach (var updatedFlavor in flavorsToUpdate)
                {
                    var flavorFromDb = await manager.GetByIdAsync(updatedFlavor.FlavorID);// context.FlavorSet.Find(updatedFlavor.FlavorID);
                    Assert.AreEqual(updatedFlavor.Code, flavorFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add initial flavors
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                var flavorsToUpdate = new List<Flavor>();
                foreach (var flavor in flavors)
                {
                    flavorsToUpdate.Add(manager.Add(flavor));
                }
                // Update flavors
                foreach (var flavor in flavorsToUpdate)
                {
                    flavor.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(flavorsToUpdate);
                // Verify updates
                foreach (var updatedFlavor in flavorsToUpdate)
                {
                    var flavorFromDb = manager.GetById(updatedFlavor.FlavorID);// context.FlavorSet.Find(updatedFlavor.FlavorID);
                    Assert.AreEqual(updatedFlavor.Code, flavorFromDb.Code);
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
        //        var manager = new FlavorManager(context);
        //        var flavors = new List<Flavor>
        //        {
        //            await CreateTestFlavorAsync(context),
        //            await CreateTestFlavorAsync(context),
        //            await CreateTestFlavorAsync(context)
        //        };
        //        foreach (var flavor in flavors)
        //        {
        //            await manager.AddAsync(flavor);
        //        }
        //        foreach (var flavor in flavors)
        //        {
        //            flavor.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(flavors);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                foreach (var flavor in flavors)
                {
                    await manager.AddAsync(flavor);
                }
                foreach (var flavor in flavors)
                {
                    flavor.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var flavor in flavors)
                    {
                        var flavorFromDb = freshContext.FlavorSet.Find(flavor.FlavorID);
                        Assert.AreNotEqual(flavor.Code, flavorFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                foreach (var flavor in flavors)
                {
                    manager.Add(flavor);
                }
                foreach (var flavor in flavors)
                {
                    flavor.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var flavor in flavors)
                    {
                        var flavorFromDb = freshContext.FlavorSet.Find(flavor.FlavorID);
                        Assert.AreNotEqual(flavor.Code, flavorFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add initial flavors
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                foreach (var flavor in flavors)
                {
                    await manager.AddAsync(flavor);
                }
                // Delete flavors
                await manager.BulkDeleteAsync(flavors);
                // Verify deletions
                foreach (var deletedFlavor in flavors)
                {
                    var flavorFromDb = context.FlavorSet.Find(deletedFlavor.FlavorID);
                    Assert.IsNull(flavorFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                // Add initial flavors
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                foreach (var flavor in flavors)
                {
                    manager.Add(flavor);
                }
                // Delete flavors
                manager.BulkDelete(flavors);
                // Verify deletions
                foreach (var deletedFlavor in flavors)
                {
                    var flavorFromDb = context.FlavorSet.Find(deletedFlavor.FlavorID);
                    Assert.IsNull(flavorFromDb);
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                foreach (var flavor in flavors)
                {
                    await manager.AddAsync(flavor);
                }
                foreach (var flavor in flavors)
                {
                    flavor.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(flavors);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                foreach (var flavor in flavors)
                {
                    manager.Add(flavor);
                }
                foreach (var flavor in flavors)
                {
                    flavor.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(flavors);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context),
                    await CreateTestFlavorAsync(context)
                };
                foreach (var flavor in flavors)
                {
                    await manager.AddAsync(flavor);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var flavor in flavors)
                    {
                        var flavorFromDb = freshContext.FlavorSet.Find(flavor.FlavorID);
                        Assert.IsNotNull(flavorFromDb);  // Flavor should still exist as the transaction wasn't committed.
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
                var manager = new FlavorManager(context);
                var flavors = new List<Flavor>
                {
                    CreateTestFlavor(context),
                    CreateTestFlavor(context),
                    CreateTestFlavor(context)
                };
                foreach (var flavor in flavors)
                {
                    manager.Add(flavor);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(flavors);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var flavor in flavors)
                    {
                        var flavorFromDb = freshContext.FlavorSet.Find(flavor.FlavorID);
                        Assert.IsNotNull(flavorFromDb);  // Flavor should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = await CreateTestFlavorAsync(context);
                //flavor.PacID = 1;
                //context.FlavorSet.Add(flavor);
                //await context.SaveChangesAsync();
                await manager.AddAsync(flavor);
                var result = await manager.GetByPacAsync(flavor.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(flavor.FlavorID, result.First().FlavorID);
            }
        }
        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor = CreateTestFlavor(context);
                //flavor.PacID = 1;
                //context.FlavorSet.Add(flavor);
                //context.SaveChanges();
                manager.Add(flavor);
                var result = manager.GetByPac(flavor.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(flavor.FlavorID, result.First().FlavorID);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = await manager.GetByPacAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var result = manager.GetByPac(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleFlavorsSamePacId_ShouldReturnAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor1 = await CreateTestFlavorAsync(context);
                var flavor2 = await CreateTestFlavorAsync(context);
                flavor2.PacID = flavor1.PacID;
                await manager.AddAsync(flavor1);
                await manager.AddAsync(flavor2);
                //context.FlavorSet.AddRange(flavor1, flavor2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacAsync(flavor1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_MultipleFlavorsSamePacId_ShouldReturnAllFlavors()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new FlavorManager(context);
                var flavor1 = CreateTestFlavor(context);
                var flavor2 = CreateTestFlavor(context);
                flavor2.PacID = flavor1.PacID;
                manager.Add(flavor1);
                manager.Add(flavor2);
                //context.FlavorSet.AddRange(flavor1, flavor2);
                //context.SaveChanges();
                var result = manager.GetByPac(flavor1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<Flavor> CreateTestFlavorAsync(FarmDbContext dbContext)
        {
            return await FlavorFactory.CreateAsync(dbContext);
        }
        private Flavor CreateTestFlavor(FarmDbContext dbContext)
        {
            return FlavorFactory.Create(dbContext);
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
