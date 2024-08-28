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
    public partial class DynaFlowTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);
                var result = await manager.AddAsync(dynaFlow);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);
                var result = manager.Add(dynaFlow);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dynaFlow = await CreateTestDynaFlowAsync(context);
                    var result = await manager.AddAsync(dynaFlow);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dynaFlow = CreateTestDynaFlow(context);
                    var result = manager.Add(dynaFlow);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDynaFlows_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDynaFlows_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDynaFlows_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add some dynaFlows
                await manager.AddAsync(await CreateTestDynaFlowAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDynaFlows_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add some dynaFlows
                manager.Add(CreateTestDynaFlow(context));
                manager.Add(CreateTestDynaFlow(context));
                manager.Add(CreateTestDynaFlow(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDynaFlows_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDynaFlows_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDynaFlows_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add some dynaFlows
                var dynaFlow1 = await CreateTestDynaFlowAsync(context);
                var dynaFlow2 = await CreateTestDynaFlowAsync(context);
                var dynaFlow3 = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlow1);
                await manager.AddAsync(dynaFlow2);
                await manager.AddAsync(dynaFlow3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dynaFlow1.DynaFlowID, dynaFlow2.DynaFlowID, dynaFlow3.DynaFlowID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDynaFlows_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add some dynaFlows
                var dynaFlow1 = CreateTestDynaFlow(context);
                var dynaFlow2 = CreateTestDynaFlow(context);
                var dynaFlow3 = CreateTestDynaFlow(context);

                manager.Add(dynaFlow1);
                manager.Add(dynaFlow2);
                manager.Add(dynaFlow3);

                var result = manager.GetMaxId();

                var maxId = new[] { dynaFlow1.DynaFlowID, dynaFlow2.DynaFlowID, dynaFlow3.DynaFlowID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDynaFlow_ShouldReturnCorrectDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlowToAdd = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlowToAdd);

                var fetchedDynaFlow = await manager.GetByIdAsync(dynaFlowToAdd.DynaFlowID);

                Assert.IsNotNull(fetchedDynaFlow);
                Assert.AreEqual(dynaFlowToAdd.DynaFlowID, fetchedDynaFlow.DynaFlowID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDynaFlow_ShouldReturnCorrectDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlowToAdd = CreateTestDynaFlow(context);

                manager.Add(dynaFlowToAdd);

                var fetchedDynaFlow = manager.GetById(dynaFlowToAdd.DynaFlowID);

                Assert.IsNotNull(fetchedDynaFlow);
                Assert.AreEqual(dynaFlowToAdd.DynaFlowID, fetchedDynaFlow.DynaFlowID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDynaFlow_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlow = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlow);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDynaFlow_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlow = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlow);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDynaFlow_ShouldReturnCorrectDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlowToAdd = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlowToAdd);

                var fetchedDynaFlow = await manager.GetByCodeAsync(dynaFlowToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlow);
                Assert.AreEqual(dynaFlowToAdd.Code, fetchedDynaFlow.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDynaFlow_ShouldReturnCorrectDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlowToAdd = CreateTestDynaFlow(context);

                manager.Add(dynaFlowToAdd);

                var fetchedDynaFlow = manager.GetByCode(dynaFlowToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlow);
                Assert.AreEqual(dynaFlowToAdd.Code, fetchedDynaFlow.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDynaFlow_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlow = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlow);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDynaFlow_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlow = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlow);
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
                var manager = new DynaFlowManager(context);

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
                var manager = new DynaFlowManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDynaFlows_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);
                var dynaFlow2 = await CreateTestDynaFlowAsync(context);
                var dynaFlow3 = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlow1);
                await manager.AddAsync(dynaFlow2);
                await manager.AddAsync(dynaFlow3);

                var fetchedDynaFlows = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlows);
                Assert.AreEqual(3, fetchedDynaFlows.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDynaFlows_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = CreateTestDynaFlow(context);
                var dynaFlow2 = CreateTestDynaFlow(context);
                var dynaFlow3 = CreateTestDynaFlow(context);

                manager.Add(dynaFlow1);
                manager.Add(dynaFlow2);
                manager.Add(dynaFlow3);

                var fetchedDynaFlows = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlows);
                Assert.AreEqual(3, fetchedDynaFlows.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlows = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlows);
                Assert.AreEqual(0, fetchedDynaFlows.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var fetchedDynaFlows = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlows);
                Assert.AreEqual(0, fetchedDynaFlows.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDynaFlow_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlow);

                dynaFlow.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dynaFlow);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlow.Code, context.DynaFlowSet.Find(dynaFlow.DynaFlowID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDynaFlow_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);

                manager.Add(dynaFlow);

                dynaFlow.Code = Guid.NewGuid();
                var updateResult = manager.Update(dynaFlow);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlow.Code, context.DynaFlowSet.Find(dynaFlow.DynaFlowID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Arrange
                var dynaFlow = await CreateTestDynaFlowAsync(context);
                await manager.AddAsync(dynaFlow);
                var firstInstance = await manager.GetByIdAsync(dynaFlow.DynaFlowID);
                var secondInstance = await manager.GetByIdAsync(dynaFlow.DynaFlowID);

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
                var manager = new DynaFlowManager(context);

                // Arrange
                var dynaFlow = CreateTestDynaFlow(context);
                manager.Add(dynaFlow);
                var firstInstance = manager.GetById(dynaFlow.DynaFlowID);
                var secondInstance = manager.GetById(dynaFlow.DynaFlowID);

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
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);
                //context.DynaFlowSet.Add(dynaFlow);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlow);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dynaFlow.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dynaFlow);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlow = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                    Assert.AreNotEqual(dynaFlow.Code, freshDynaFlow.Code); // Because the transaction was not committed.
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
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);
                //context.DynaFlowSet.Add(dynaFlow);
                //context.SaveChanges();
                manager.Add(dynaFlow);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dynaFlow.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dynaFlow);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlow = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                    Assert.AreNotEqual(dynaFlow.Code, freshDynaFlow.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlow);

                var deleteResult = await manager.DeleteAsync(dynaFlow.DynaFlowID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowSet.Find(dynaFlow.DynaFlowID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDynaFlow()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);

                manager.Add(dynaFlow);

                var deleteResult = manager.Delete(dynaFlow.DynaFlowID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowSet.Find(dynaFlow.DynaFlowID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

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
                var manager = new DynaFlowManager(context);

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
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);

                await manager.AddAsync(dynaFlow);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dynaFlow.DynaFlowID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlow = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                    Assert.IsNotNull(freshDynaFlow);  // Because the transaction was not committed.
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
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);

                manager.Add(dynaFlow);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dynaFlow.DynaFlowID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlow = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                    Assert.IsNotNull(freshDynaFlow);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDynaFlows_ShouldInsertAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                await manager.BulkInsertAsync(dynaFlows);

                Assert.AreEqual(dynaFlows.Count, context.DynaFlowSet.Count());
                foreach (var dynaFlow in dynaFlows)
                {
                    Assert.IsNotNull(context.DynaFlowSet.Find(dynaFlow.DynaFlowID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDynaFlows_ShouldInsertAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                manager.BulkInsert(dynaFlows);

                Assert.AreEqual(dynaFlows.Count, context.DynaFlowSet.Count());
                foreach (var dynaFlow in dynaFlows)
                {
                    Assert.IsNotNull(context.DynaFlowSet.Find(dynaFlow.DynaFlowID));
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowSet.Count());  // Because the transaction was not committed.
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add initial dynaFlows
                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                var dynaFlowsToUpdate = new List<DynaFlow>();

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlowsToUpdate.Add(await manager.AddAsync(dynaFlow));
                }

                // Update dynaFlows
                foreach (var dynaFlow in dynaFlowsToUpdate)
                {
                    dynaFlow.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dynaFlowsToUpdate);

                // Verify updates
                foreach (var updatedDynaFlow in dynaFlowsToUpdate)
                {
                    var dynaFlowFromDb = await manager.GetByIdAsync(updatedDynaFlow.DynaFlowID);// context.DynaFlowSet.Find(updatedDynaFlow.DynaFlowID);
                    Assert.AreEqual(updatedDynaFlow.Code, dynaFlowFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add initial dynaFlows
                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                var dynaFlowsToUpdate = new List<DynaFlow>();

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlowsToUpdate.Add(manager.Add(dynaFlow));
                }

                // Update dynaFlows
                foreach (var dynaFlow in dynaFlowsToUpdate)
                {
                    dynaFlow.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dynaFlowsToUpdate);

                // Verify updates
                foreach (var updatedDynaFlow in dynaFlowsToUpdate)
                {
                    var dynaFlowFromDb = manager.GetById(updatedDynaFlow.DynaFlowID);// context.DynaFlowSet.Find(updatedDynaFlow.DynaFlowID);
                    Assert.AreEqual(updatedDynaFlow.Code, dynaFlowFromDb.Code);
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
        //        var manager = new DynaFlowManager(context);

        //        var dynaFlows = new List<DynaFlow>
        //        {
        //            await CreateTestDynaFlowAsync(context),
        //            await CreateTestDynaFlowAsync(context),
        //            await CreateTestDynaFlowAsync(context)
        //        };

        //        foreach (var dynaFlow in dynaFlows)
        //        {
        //            await manager.AddAsync(dynaFlow);
        //        }

        //        foreach (var dynaFlow in dynaFlows)
        //        {
        //            dynaFlow.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dynaFlows);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    await manager.AddAsync(dynaFlow);
                }

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlow.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlow in dynaFlows)
                    {
                        var dynaFlowFromDb = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                        Assert.AreNotEqual(dynaFlow.Code, dynaFlowFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    manager.Add(dynaFlow);
                }

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlow.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlow in dynaFlows)
                    {
                        var dynaFlowFromDb = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                        Assert.AreNotEqual(dynaFlow.Code, dynaFlowFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add initial dynaFlows
                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    await manager.AddAsync(dynaFlow);
                }

                // Delete dynaFlows
                await manager.BulkDeleteAsync(dynaFlows);

                // Verify deletions
                foreach (var deletedDynaFlow in dynaFlows)
                {
                    var dynaFlowFromDb = context.DynaFlowSet.Find(deletedDynaFlow.DynaFlowID);
                    Assert.IsNull(dynaFlowFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                // Add initial dynaFlows
                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    manager.Add(dynaFlow);
                }

                // Delete dynaFlows
                manager.BulkDelete(dynaFlows);

                // Verify deletions
                foreach (var deletedDynaFlow in dynaFlows)
                {
                    var dynaFlowFromDb = context.DynaFlowSet.Find(deletedDynaFlow.DynaFlowID);
                    Assert.IsNull(dynaFlowFromDb);
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    await manager.AddAsync(dynaFlow);
                }

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlow.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dynaFlows);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    manager.Add(dynaFlow);
                }

                foreach (var dynaFlow in dynaFlows)
                {
                    dynaFlow.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dynaFlows);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context),
                    await CreateTestDynaFlowAsync(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    await manager.AddAsync(dynaFlow);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlow in dynaFlows)
                    {
                        var dynaFlowFromDb = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                        Assert.IsNotNull(dynaFlowFromDb);  // DynaFlow should still exist as the transaction wasn't committed.
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
                var manager = new DynaFlowManager(context);

                var dynaFlows = new List<DynaFlow>
                {
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context),
                    CreateTestDynaFlow(context)
                };

                foreach (var dynaFlow in dynaFlows)
                {
                    manager.Add(dynaFlow);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dynaFlows);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlow in dynaFlows)
                    {
                        var dynaFlowFromDb = freshContext.DynaFlowSet.Find(dynaFlow.DynaFlowID);
                        Assert.IsNotNull(dynaFlowFromDb);  // DynaFlow should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_ValidDynaFlowTypeID_ShouldReturnDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);
                // dynaFlow.DynaFlowTypeID = 1;
                //context.DynaFlowSet.Add(dynaFlow);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlow);

                var result = await manager.GetByDynaFlowTypeIDAsync(dynaFlow.DynaFlowTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlow.DynaFlowID, result.First().DynaFlowID);
            }
        }

        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = await CreateTestDynaFlowAsync(context);
                //dynaFlow.PacID = 1;
                //context.DynaFlowSet.Add(dynaFlow);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlow);

                var result = await manager.GetByPacIDAsync(dynaFlow.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlow.DynaFlowID, result.First().DynaFlowID);
            }
        }
        [TestMethod]//DynaFlowTypeID
        public void GetByDynaFlowType_ValidDynaFlowTypeID_ShouldReturnDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);
                // dynaFlow.DynaFlowTypeID = 1;
                //context.DynaFlowSet.Add(dynaFlow);
                //context.SaveChanges();
                manager.Add(dynaFlow);

                var result = manager.GetByDynaFlowTypeID(dynaFlow.DynaFlowTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlow.DynaFlowID, result.First().DynaFlowID);
            }
        }

        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow = CreateTestDynaFlow(context);
                //dynaFlow.PacID = 1;
                //context.DynaFlowSet.Add(dynaFlow);
                //context.SaveChanges();
                manager.Add(dynaFlow);

                var result = manager.GetByPacID(dynaFlow.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlow.DynaFlowID, result.First().DynaFlowID);
            }
        }
        [TestMethod]//DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_InvalidDynaFlowTypeID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = await manager.GetByDynaFlowTypeIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = await manager.GetByPacIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//DynaFlowTypeID
        public void GetByDynaFlowType_InvalidDynaFlowTypeID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var result = manager.GetByDynaFlowTypeID(100);  // ID 100 is not added to the database
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
                var manager = new DynaFlowManager(context);

                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_MultipleDynaFlowsSameDynaFlowTypeID_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);
                //  dynaFlow1.DynaFlowTypeID = 1;
                var dynaFlow2 = await CreateTestDynaFlowAsync(context);
                dynaFlow2.DynaFlowTypeID = dynaFlow1.DynaFlowTypeID;

                //context.DynaFlowSet.AddRange(dynaFlow1, dynaFlow2);
                //await context.SaveChangesAsync();

                await manager.AddAsync(dynaFlow1);
                await manager.AddAsync(dynaFlow2);

                var result = await manager.GetByDynaFlowTypeIDAsync(dynaFlow1.DynaFlowTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDynaFlowsSamePacId_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);
                var dynaFlow2 = await CreateTestDynaFlowAsync(context);
                dynaFlow2.PacID = dynaFlow1.PacID;

                await manager.AddAsync(dynaFlow1);
                await manager.AddAsync(dynaFlow2);

                //context.DynaFlowSet.AddRange(dynaFlow1, dynaFlow2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByPacIDAsync(dynaFlow1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //DynaFlowTypeID
        public void GetByDynaFlowType_MultipleDynaFlowsSameDynaFlowTypeID_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = CreateTestDynaFlow(context);
                //  dynaFlow1.DynaFlowTypeID = 1;
                var dynaFlow2 = CreateTestDynaFlow(context);
                dynaFlow2.DynaFlowTypeID = dynaFlow1.DynaFlowTypeID;

                //context.DynaFlowSet.AddRange(dynaFlow1, dynaFlow2);
                //context.SaveChanges();

                manager.Add(dynaFlow1);
                manager.Add(dynaFlow2);

                var result = manager.GetByDynaFlowTypeID(dynaFlow1.DynaFlowTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public void GetByPacId_MultipleDynaFlowsSamePacId_ShouldReturnAllDynaFlows()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowManager(context);

                var dynaFlow1 = CreateTestDynaFlow(context);
                var dynaFlow2 = CreateTestDynaFlow(context);
                dynaFlow2.PacID = dynaFlow1.PacID;

                manager.Add(dynaFlow1);
                manager.Add(dynaFlow2);

                //context.DynaFlowSet.AddRange(dynaFlow1, dynaFlow2);
                //context.SaveChanges();

                var result = manager.GetByPacID(dynaFlow1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DynaFlow> CreateTestDynaFlowAsync(FarmDbContext dbContext)
        {
            return await DynaFlowFactory.CreateAsync(dbContext);
        }

        private DynaFlow CreateTestDynaFlow(FarmDbContext dbContext)
        {
            return DynaFlowFactory.Create(dbContext);
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
