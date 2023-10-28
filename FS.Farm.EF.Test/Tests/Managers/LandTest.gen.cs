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
    public partial class LandTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                var result = await manager.AddAsync(land);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.LandSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                var result = manager.Add(land);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.LandSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var land = await CreateTestLandAsync(context);
                    var result = await manager.AddAsync(land);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.LandSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var land = CreateTestLand(context);
                    var result = manager.Add(land);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.LandSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoLands_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoLands_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithLands_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add some lands
                await manager.AddAsync(await CreateTestLandAsync(context));
                await manager.AddAsync(await CreateTestLandAsync(context));
                await manager.AddAsync(await CreateTestLandAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithLands_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add some lands
                manager.Add(CreateTestLand(context));
                manager.Add(CreateTestLand(context));
                manager.Add(CreateTestLand(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoLands_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoLands_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var result = manager.GetMaxId();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithLands_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add some lands
                var land1 = await CreateTestLandAsync(context);
                var land2 = await CreateTestLandAsync(context);
                var land3 = await CreateTestLandAsync(context);
                await manager.AddAsync(land1);
                await manager.AddAsync(land2);
                await manager.AddAsync(land3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { land1.LandID, land2.LandID, land3.LandID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithLands_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add some lands
                var land1 = CreateTestLand(context);
                var land2 = CreateTestLand(context);
                var land3 = CreateTestLand(context);
                manager.Add(land1);
                manager.Add(land2);
                manager.Add(land3);
                var result = manager.GetMaxId();
                var maxId = new[] { land1.LandID, land2.LandID, land3.LandID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingLand_ShouldReturnCorrectLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var landToAdd = await CreateTestLandAsync(context);
                await manager.AddAsync(landToAdd);
                var fetchedLand = await manager.GetByIdAsync(landToAdd.LandID);
                Assert.IsNotNull(fetchedLand);
                Assert.AreEqual(landToAdd.LandID, fetchedLand.LandID);
            }
        }
        [TestMethod]
        public void GetById_ExistingLand_ShouldReturnCorrectLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var landToAdd = CreateTestLand(context);
                manager.Add(landToAdd);
                var fetchedLand = manager.GetById(landToAdd.LandID);
                Assert.IsNotNull(fetchedLand);
                Assert.AreEqual(landToAdd.LandID, fetchedLand.LandID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingLand_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLand = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedLand);
            }
        }
        [TestMethod]
        public void GetById_NonExistingLand_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLand = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedLand);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingLand_ShouldReturnCorrectLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var landToAdd = await CreateTestLandAsync(context);
                await manager.AddAsync(landToAdd);
                var fetchedLand = await manager.GetByCodeAsync(landToAdd.Code.Value);
                Assert.IsNotNull(fetchedLand);
                Assert.AreEqual(landToAdd.Code, fetchedLand.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingLand_ShouldReturnCorrectLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var landToAdd = CreateTestLand(context);
                manager.Add(landToAdd);
                var fetchedLand = manager.GetByCode(landToAdd.Code.Value);
                Assert.IsNotNull(fetchedLand);
                Assert.AreEqual(landToAdd.Code, fetchedLand.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingLand_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLand = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedLand);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingLand_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLand = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedLand);
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
                var manager = new LandManager(context);
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
                var manager = new LandManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleLands_ShouldReturnAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land1 = await CreateTestLandAsync(context);
                var land2 = await CreateTestLandAsync(context);
                var land3 = await CreateTestLandAsync(context);
                await manager.AddAsync(land1);
                await manager.AddAsync(land2);
                await manager.AddAsync(land3);
                var fetchedLands = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedLands);
                Assert.AreEqual(3, fetchedLands.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleLands_ShouldReturnAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land1 = CreateTestLand(context);
                var land2 = CreateTestLand(context);
                var land3 = CreateTestLand(context);
                manager.Add(land1);
                manager.Add(land2);
                manager.Add(land3);
                var fetchedLands = manager.GetAll();
                Assert.IsNotNull(fetchedLands);
                Assert.AreEqual(3, fetchedLands.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLands = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedLands);
                Assert.AreEqual(0, fetchedLands.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var fetchedLands = manager.GetAll();
                Assert.IsNotNull(fetchedLands);
                Assert.AreEqual(0, fetchedLands.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidLand_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                await manager.AddAsync(land);
                land.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(land);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(land.Code, context.LandSet.Find(land.LandID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidLand_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                manager.Add(land);
                land.Code = Guid.NewGuid();
                var updateResult = manager.Update(land);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(land.Code, context.LandSet.Find(land.LandID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Arrange
                var land = await CreateTestLandAsync(context);
                await manager.AddAsync(land);
                var firstInstance = await manager.GetByIdAsync(land.LandID);
                var secondInstance = await manager.GetByIdAsync(land.LandID);
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
                var manager = new LandManager(context);
                // Arrange
                var land = CreateTestLand(context);
                manager.Add(land);
                var firstInstance = manager.GetById(land.LandID);
                var secondInstance = manager.GetById(land.LandID);
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
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                //context.LandSet.Add(land);
                //await context.SaveChangesAsync();
                await manager.AddAsync(land);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    land.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(land);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshLand = freshContext.LandSet.Find(land.LandID);
                    Assert.AreNotEqual(land.Code, freshLand.Code); // Because the transaction was not committed.
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
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                //context.LandSet.Add(land);
                //context.SaveChanges();
                manager.Add(land);
                using (var transaction = context.Database.BeginTransaction())
                {
                    land.Code = Guid.NewGuid();
                    var updateResult = manager.Update(land);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshLand = freshContext.LandSet.Find(land.LandID);
                    Assert.AreNotEqual(land.Code, freshLand.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                await manager.AddAsync(land);
                var deleteResult = await manager.DeleteAsync(land.LandID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.LandSet.Find(land.LandID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteLand()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                manager.Add(land);
                var deleteResult = manager.Delete(land.LandID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.LandSet.Find(land.LandID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
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
                var manager = new LandManager(context);
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
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                await manager.AddAsync(land);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(land.LandID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshLand = freshContext.LandSet.Find(land.LandID);
                    Assert.IsNotNull(freshLand);  // Because the transaction was not committed.
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
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                manager.Add(land);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(land.LandID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshLand = freshContext.LandSet.Find(land.LandID);
                    Assert.IsNotNull(freshLand);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidLands_ShouldInsertAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                await manager.BulkInsertAsync(lands);
                Assert.AreEqual(lands.Count, context.LandSet.Count());
                foreach (var land in lands)
                {
                    Assert.IsNotNull(context.LandSet.Find(land.LandID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidLands_ShouldInsertAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                manager.BulkInsert(lands);
                Assert.AreEqual(lands.Count, context.LandSet.Count());
                foreach (var land in lands)
                {
                    Assert.IsNotNull(context.LandSet.Find(land.LandID));
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.LandSet.Count());  // Because the transaction was not committed.
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.LandSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add initial lands
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                var landsToUpdate = new List<Land>();
                foreach (var land in lands)
                {
                    landsToUpdate.Add(await manager.AddAsync(land));
                }
                // Update lands
                foreach (var land in landsToUpdate)
                {
                    land.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(landsToUpdate);
                // Verify updates
                foreach (var updatedLand in landsToUpdate)
                {
                    var landFromDb = await manager.GetByIdAsync(updatedLand.LandID);// context.LandSet.Find(updatedLand.LandID);
                    Assert.AreEqual(updatedLand.Code, landFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add initial lands
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                var landsToUpdate = new List<Land>();
                foreach (var land in lands)
                {
                    landsToUpdate.Add(manager.Add(land));
                }
                // Update lands
                foreach (var land in landsToUpdate)
                {
                    land.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(landsToUpdate);
                // Verify updates
                foreach (var updatedLand in landsToUpdate)
                {
                    var landFromDb = manager.GetById(updatedLand.LandID);// context.LandSet.Find(updatedLand.LandID);
                    Assert.AreEqual(updatedLand.Code, landFromDb.Code);
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
        //        var manager = new LandManager(context);
        //        var lands = new List<Land>
        //        {
        //            await CreateTestLandAsync(context),
        //            await CreateTestLandAsync(context),
        //            await CreateTestLandAsync(context)
        //        };
        //        foreach (var land in lands)
        //        {
        //            await manager.AddAsync(land);
        //        }
        //        foreach (var land in lands)
        //        {
        //            land.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(lands);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                foreach (var land in lands)
                {
                    await manager.AddAsync(land);
                }
                foreach (var land in lands)
                {
                    land.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var land in lands)
                    {
                        var landFromDb = freshContext.LandSet.Find(land.LandID);
                        Assert.AreNotEqual(land.Code, landFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                foreach (var land in lands)
                {
                    manager.Add(land);
                }
                foreach (var land in lands)
                {
                    land.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var land in lands)
                    {
                        var landFromDb = freshContext.LandSet.Find(land.LandID);
                        Assert.AreNotEqual(land.Code, landFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add initial lands
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                foreach (var land in lands)
                {
                    await manager.AddAsync(land);
                }
                // Delete lands
                await manager.BulkDeleteAsync(lands);
                // Verify deletions
                foreach (var deletedLand in lands)
                {
                    var landFromDb = context.LandSet.Find(deletedLand.LandID);
                    Assert.IsNull(landFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                // Add initial lands
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                foreach (var land in lands)
                {
                    manager.Add(land);
                }
                // Delete lands
                manager.BulkDelete(lands);
                // Verify deletions
                foreach (var deletedLand in lands)
                {
                    var landFromDb = context.LandSet.Find(deletedLand.LandID);
                    Assert.IsNull(landFromDb);
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                foreach (var land in lands)
                {
                    await manager.AddAsync(land);
                }
                foreach (var land in lands)
                {
                    land.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(lands);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                foreach (var land in lands)
                {
                    manager.Add(land);
                }
                foreach (var land in lands)
                {
                    land.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(lands);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context),
                    await CreateTestLandAsync(context)
                };
                foreach (var land in lands)
                {
                    await manager.AddAsync(land);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var land in lands)
                    {
                        var landFromDb = freshContext.LandSet.Find(land.LandID);
                        Assert.IsNotNull(landFromDb);  // Land should still exist as the transaction wasn't committed.
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
                var manager = new LandManager(context);
                var lands = new List<Land>
                {
                    CreateTestLand(context),
                    CreateTestLand(context),
                    CreateTestLand(context)
                };
                foreach (var land in lands)
                {
                    manager.Add(land);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(lands);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var land in lands)
                    {
                        var landFromDb = freshContext.LandSet.Find(land.LandID);
                        Assert.IsNotNull(landFromDb);  // Land should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = await CreateTestLandAsync(context);
                //land.PacID = 1;
                //context.LandSet.Add(land);
                //await context.SaveChangesAsync();
                await manager.AddAsync(land);
                var result = await manager.GetByPacIDAsync(land.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(land.LandID, result.First().LandID);
            }
        }
        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land = CreateTestLand(context);
                //land.PacID = 1;
                //context.LandSet.Add(land);
                //context.SaveChanges();
                manager.Add(land);
                var result = manager.GetByPacID(land.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(land.LandID, result.First().LandID);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var result = await manager.GetByPacIDAsync(100);  // ID 100 is not added to the database
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
                var manager = new LandManager(context);
                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleLandsSamePacId_ShouldReturnAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land1 = await CreateTestLandAsync(context);
                var land2 = await CreateTestLandAsync(context);
                land2.PacID = land1.PacID;
                await manager.AddAsync(land1);
                await manager.AddAsync(land2);
                //context.LandSet.AddRange(land1, land2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacIDAsync(land1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_MultipleLandsSamePacId_ShouldReturnAllLands()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new LandManager(context);
                var land1 = CreateTestLand(context);
                var land2 = CreateTestLand(context);
                land2.PacID = land1.PacID;
                manager.Add(land1);
                manager.Add(land2);
                //context.LandSet.AddRange(land1, land2);
                //context.SaveChanges();
                var result = manager.GetByPacID(land1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<Land> CreateTestLandAsync(FarmDbContext dbContext)
        {
            return await LandFactory.CreateAsync(dbContext);
        }
        private Land CreateTestLand(FarmDbContext dbContext)
        {
            return LandFactory.Create(dbContext);
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
