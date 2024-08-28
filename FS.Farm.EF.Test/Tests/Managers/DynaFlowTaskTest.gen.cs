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
    public partial class DynaFlowTaskTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                var result = await manager.AddAsync(dynaFlowTask);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTaskSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);
                var result = manager.Add(dynaFlowTask);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTaskSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                    var result = await manager.AddAsync(dynaFlowTask);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTaskSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dynaFlowTask = CreateTestDynaFlowTask(context);
                    var result = manager.Add(dynaFlowTask);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTaskSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDynaFlowTasks_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDynaFlowTasks_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDynaFlowTasks_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add some dynaFlowTasks
                await manager.AddAsync(await CreateTestDynaFlowTaskAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTaskAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTaskAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDynaFlowTasks_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add some dynaFlowTasks
                manager.Add(CreateTestDynaFlowTask(context));
                manager.Add(CreateTestDynaFlowTask(context));
                manager.Add(CreateTestDynaFlowTask(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDynaFlowTasks_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDynaFlowTasks_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDynaFlowTasks_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add some dynaFlowTasks
                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);
                var dynaFlowTask2 = await CreateTestDynaFlowTaskAsync(context);
                var dynaFlowTask3 = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTask1);
                await manager.AddAsync(dynaFlowTask2);
                await manager.AddAsync(dynaFlowTask3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dynaFlowTask1.DynaFlowTaskID, dynaFlowTask2.DynaFlowTaskID, dynaFlowTask3.DynaFlowTaskID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDynaFlowTasks_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add some dynaFlowTasks
                var dynaFlowTask1 = CreateTestDynaFlowTask(context);
                var dynaFlowTask2 = CreateTestDynaFlowTask(context);
                var dynaFlowTask3 = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTask1);
                manager.Add(dynaFlowTask2);
                manager.Add(dynaFlowTask3);

                var result = manager.GetMaxId();

                var maxId = new[] { dynaFlowTask1.DynaFlowTaskID, dynaFlowTask2.DynaFlowTaskID, dynaFlowTask3.DynaFlowTaskID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDynaFlowTask_ShouldReturnCorrectDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTaskToAdd = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTaskToAdd);

                var fetchedDynaFlowTask = await manager.GetByIdAsync(dynaFlowTaskToAdd.DynaFlowTaskID);

                Assert.IsNotNull(fetchedDynaFlowTask);
                Assert.AreEqual(dynaFlowTaskToAdd.DynaFlowTaskID, fetchedDynaFlowTask.DynaFlowTaskID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDynaFlowTask_ShouldReturnCorrectDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTaskToAdd = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTaskToAdd);

                var fetchedDynaFlowTask = manager.GetById(dynaFlowTaskToAdd.DynaFlowTaskID);

                Assert.IsNotNull(fetchedDynaFlowTask);
                Assert.AreEqual(dynaFlowTaskToAdd.DynaFlowTaskID, fetchedDynaFlowTask.DynaFlowTaskID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDynaFlowTask_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTask = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTask);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDynaFlowTask_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTask = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTask);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDynaFlowTask_ShouldReturnCorrectDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTaskToAdd = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTaskToAdd);

                var fetchedDynaFlowTask = await manager.GetByCodeAsync(dynaFlowTaskToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTask);
                Assert.AreEqual(dynaFlowTaskToAdd.Code, fetchedDynaFlowTask.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDynaFlowTask_ShouldReturnCorrectDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTaskToAdd = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTaskToAdd);

                var fetchedDynaFlowTask = manager.GetByCode(dynaFlowTaskToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTask);
                Assert.AreEqual(dynaFlowTaskToAdd.Code, fetchedDynaFlowTask.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDynaFlowTask_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTask = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTask);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDynaFlowTask_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTask = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTask);
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
                var manager = new DynaFlowTaskManager(context);

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
                var manager = new DynaFlowTaskManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDynaFlowTasks_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);
                var dynaFlowTask2 = await CreateTestDynaFlowTaskAsync(context);
                var dynaFlowTask3 = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTask1);
                await manager.AddAsync(dynaFlowTask2);
                await manager.AddAsync(dynaFlowTask3);

                var fetchedDynaFlowTasks = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTasks);
                Assert.AreEqual(3, fetchedDynaFlowTasks.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDynaFlowTasks_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);
                var dynaFlowTask2 = CreateTestDynaFlowTask(context);
                var dynaFlowTask3 = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTask1);
                manager.Add(dynaFlowTask2);
                manager.Add(dynaFlowTask3);

                var fetchedDynaFlowTasks = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTasks);
                Assert.AreEqual(3, fetchedDynaFlowTasks.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTasks = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTasks);
                Assert.AreEqual(0, fetchedDynaFlowTasks.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var fetchedDynaFlowTasks = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTasks);
                Assert.AreEqual(0, fetchedDynaFlowTasks.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDynaFlowTask_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTask);

                dynaFlowTask.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dynaFlowTask);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTask.Code, context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDynaFlowTask_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTask);

                dynaFlowTask.Code = Guid.NewGuid();
                var updateResult = manager.Update(dynaFlowTask);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTask.Code, context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Arrange
                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                await manager.AddAsync(dynaFlowTask);
                var firstInstance = await manager.GetByIdAsync(dynaFlowTask.DynaFlowTaskID);
                var secondInstance = await manager.GetByIdAsync(dynaFlowTask.DynaFlowTaskID);

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
                var manager = new DynaFlowTaskManager(context);

                // Arrange
                var dynaFlowTask = CreateTestDynaFlowTask(context);
                manager.Add(dynaFlowTask);
                var firstInstance = manager.GetById(dynaFlowTask.DynaFlowTaskID);
                var secondInstance = manager.GetById(dynaFlowTask.DynaFlowTaskID);

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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTask);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dynaFlowTask);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTask = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                    Assert.AreNotEqual(dynaFlowTask.Code, freshDynaFlowTask.Code); // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //context.SaveChanges();
                manager.Add(dynaFlowTask);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dynaFlowTask);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTask = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                    Assert.AreNotEqual(dynaFlowTask.Code, freshDynaFlowTask.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTask);

                var deleteResult = await manager.DeleteAsync(dynaFlowTask.DynaFlowTaskID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDynaFlowTask()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTask);

                var deleteResult = manager.Delete(dynaFlowTask.DynaFlowTaskID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

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
                var manager = new DynaFlowTaskManager(context);

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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);

                await manager.AddAsync(dynaFlowTask);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dynaFlowTask.DynaFlowTaskID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTask = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                    Assert.IsNotNull(freshDynaFlowTask);  // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);

                manager.Add(dynaFlowTask);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dynaFlowTask.DynaFlowTaskID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTask = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                    Assert.IsNotNull(freshDynaFlowTask);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDynaFlowTasks_ShouldInsertAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                await manager.BulkInsertAsync(dynaFlowTasks);

                Assert.AreEqual(dynaFlowTasks.Count, context.DynaFlowTaskSet.Count());
                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    Assert.IsNotNull(context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDynaFlowTasks_ShouldInsertAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                manager.BulkInsert(dynaFlowTasks);

                Assert.AreEqual(dynaFlowTasks.Count, context.DynaFlowTaskSet.Count());
                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    Assert.IsNotNull(context.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID));
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTaskSet.Count());  // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTaskSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add initial dynaFlowTasks
                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                var dynaFlowTasksToUpdate = new List<DynaFlowTask>();

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTasksToUpdate.Add(await manager.AddAsync(dynaFlowTask));
                }

                // Update dynaFlowTasks
                foreach (var dynaFlowTask in dynaFlowTasksToUpdate)
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dynaFlowTasksToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTask in dynaFlowTasksToUpdate)
                {
                    var dynaFlowTaskFromDb = await manager.GetByIdAsync(updatedDynaFlowTask.DynaFlowTaskID);// context.DynaFlowTaskSet.Find(updatedDynaFlowTask.DynaFlowTaskID);
                    Assert.AreEqual(updatedDynaFlowTask.Code, dynaFlowTaskFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add initial dynaFlowTasks
                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                var dynaFlowTasksToUpdate = new List<DynaFlowTask>();

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTasksToUpdate.Add(manager.Add(dynaFlowTask));
                }

                // Update dynaFlowTasks
                foreach (var dynaFlowTask in dynaFlowTasksToUpdate)
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dynaFlowTasksToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTask in dynaFlowTasksToUpdate)
                {
                    var dynaFlowTaskFromDb = manager.GetById(updatedDynaFlowTask.DynaFlowTaskID);// context.DynaFlowTaskSet.Find(updatedDynaFlowTask.DynaFlowTaskID);
                    Assert.AreEqual(updatedDynaFlowTask.Code, dynaFlowTaskFromDb.Code);
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
        //        var manager = new DynaFlowTaskManager(context);

        //        var dynaFlowTasks = new List<DynaFlowTask>
        //        {
        //            await CreateTestDynaFlowTaskAsync(context),
        //            await CreateTestDynaFlowTaskAsync(context),
        //            await CreateTestDynaFlowTaskAsync(context)
        //        };

        //        foreach (var dynaFlowTask in dynaFlowTasks)
        //        {
        //            await manager.AddAsync(dynaFlowTask);
        //        }

        //        foreach (var dynaFlowTask in dynaFlowTasks)
        //        {
        //            dynaFlowTask.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dynaFlowTasks);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    await manager.AddAsync(dynaFlowTask);
                }

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTask in dynaFlowTasks)
                    {
                        var dynaFlowTaskFromDb = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                        Assert.AreNotEqual(dynaFlowTask.Code, dynaFlowTaskFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    manager.Add(dynaFlowTask);
                }

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTask.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTask in dynaFlowTasks)
                    {
                        var dynaFlowTaskFromDb = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                        Assert.AreNotEqual(dynaFlowTask.Code, dynaFlowTaskFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add initial dynaFlowTasks
                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    await manager.AddAsync(dynaFlowTask);
                }

                // Delete dynaFlowTasks
                await manager.BulkDeleteAsync(dynaFlowTasks);

                // Verify deletions
                foreach (var deletedDynaFlowTask in dynaFlowTasks)
                {
                    var dynaFlowTaskFromDb = context.DynaFlowTaskSet.Find(deletedDynaFlowTask.DynaFlowTaskID);
                    Assert.IsNull(dynaFlowTaskFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                // Add initial dynaFlowTasks
                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    manager.Add(dynaFlowTask);
                }

                // Delete dynaFlowTasks
                manager.BulkDelete(dynaFlowTasks);

                // Verify deletions
                foreach (var deletedDynaFlowTask in dynaFlowTasks)
                {
                    var dynaFlowTaskFromDb = context.DynaFlowTaskSet.Find(deletedDynaFlowTask.DynaFlowTaskID);
                    Assert.IsNull(dynaFlowTaskFromDb);
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    await manager.AddAsync(dynaFlowTask);
                }

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTask.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dynaFlowTasks);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    manager.Add(dynaFlowTask);
                }

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    dynaFlowTask.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dynaFlowTasks);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context),
                    await CreateTestDynaFlowTaskAsync(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    await manager.AddAsync(dynaFlowTask);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTask in dynaFlowTasks)
                    {
                        var dynaFlowTaskFromDb = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                        Assert.IsNotNull(dynaFlowTaskFromDb);  // DynaFlowTask should still exist as the transaction wasn't committed.
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
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTasks = new List<DynaFlowTask>
                {
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context),
                    CreateTestDynaFlowTask(context)
                };

                foreach (var dynaFlowTask in dynaFlowTasks)
                {
                    manager.Add(dynaFlowTask);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dynaFlowTasks);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTask in dynaFlowTasks)
                    {
                        var dynaFlowTaskFromDb = freshContext.DynaFlowTaskSet.Find(dynaFlowTask.DynaFlowTaskID);
                        Assert.IsNotNull(dynaFlowTaskFromDb);  // DynaFlowTask should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//DynaFlowID
        public async Task GetByDynaFlowIdAsync_ValidDynaFlowId_ShouldReturnDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                //dynaFlowTask.DynaFlowID = 1;
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTask);

                var result = await manager.GetByDynaFlowIDAsync(dynaFlowTask.DynaFlowID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTask.DynaFlowTaskID, result.First().DynaFlowTaskID);
            }
        }
        [TestMethod]//DynaFlowTaskTypeID
        public async Task GetByDynaFlowTaskTypeAsync_ValidDynaFlowTaskTypeID_ShouldReturnDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = await CreateTestDynaFlowTaskAsync(context);
                // dynaFlowTask.DynaFlowTaskTypeID = 1;
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTask);

                var result = await manager.GetByDynaFlowTaskTypeIDAsync(dynaFlowTask.DynaFlowTaskTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTask.DynaFlowTaskID, result.First().DynaFlowTaskID);
            }
        }

        [TestMethod]//DynaFlowID
        public void GetByDynaFlowId_ValidDynaFlowId_ShouldReturnDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);
                //dynaFlowTask.DynaFlowID = 1;
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //context.SaveChanges();
                manager.Add(dynaFlowTask);

                var result = manager.GetByDynaFlowID(dynaFlowTask.DynaFlowID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTask.DynaFlowTaskID, result.First().DynaFlowTaskID);
            }
        }
        [TestMethod]//DynaFlowTaskTypeID
        public void GetByDynaFlowTaskType_ValidDynaFlowTaskTypeID_ShouldReturnDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask = CreateTestDynaFlowTask(context);
                // dynaFlowTask.DynaFlowTaskTypeID = 1;
                //context.DynaFlowTaskSet.Add(dynaFlowTask);
                //context.SaveChanges();
                manager.Add(dynaFlowTask);

                var result = manager.GetByDynaFlowTaskTypeID(dynaFlowTask.DynaFlowTaskTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTask.DynaFlowTaskID, result.First().DynaFlowTaskID);
            }
        }

        [TestMethod] //DynaFlowID
        public async Task GetByDynaFlowIdAsync_InvalidDynaFlowId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = await manager.GetByDynaFlowIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//DynaFlowTaskTypeID
        public async Task GetByDynaFlowTaskTypeAsync_InvalidDynaFlowTaskTypeID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = await manager.GetByDynaFlowTaskTypeIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //DynaFlowID
        public void GetByDynaFlowId_InvalidDynaFlowId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = manager.GetByDynaFlowID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//DynaFlowTaskTypeID
        public void GetByDynaFlowTaskType_InvalidDynaFlowTaskTypeID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var result = manager.GetByDynaFlowTaskTypeID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //DynaFlowID
        public async Task GetByDynaFlowIdAsync_MultipleDynaFlowTasksSameDynaFlowId_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);
                var dynaFlowTask2 = await CreateTestDynaFlowTaskAsync(context);
                dynaFlowTask2.DynaFlowID = dynaFlowTask1.DynaFlowID;

                await manager.AddAsync(dynaFlowTask1);
                await manager.AddAsync(dynaFlowTask2);

                //context.DynaFlowTaskSet.AddRange(dynaFlowTask1, dynaFlowTask2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByDynaFlowIDAsync(dynaFlowTask1.DynaFlowID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //DynaFlowTaskTypeID
        public async Task GetByDynaFlowTaskTypeAsync_MultipleDynaFlowTasksSameDynaFlowTaskTypeID_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);
                //  dynaFlowTask1.DynaFlowTaskTypeID = 1;
                var dynaFlowTask2 = await CreateTestDynaFlowTaskAsync(context);
                dynaFlowTask2.DynaFlowTaskTypeID = dynaFlowTask1.DynaFlowTaskTypeID;

                //context.DynaFlowTaskSet.AddRange(dynaFlowTask1, dynaFlowTask2);
                //await context.SaveChangesAsync();

                await manager.AddAsync(dynaFlowTask1);
                await manager.AddAsync(dynaFlowTask2);

                var result = await manager.GetByDynaFlowTaskTypeIDAsync(dynaFlowTask1.DynaFlowTaskTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //DynaFlowID
        public void GetByDynaFlowId_MultipleDynaFlowTasksSameDynaFlowId_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);
                var dynaFlowTask2 = CreateTestDynaFlowTask(context);
                dynaFlowTask2.DynaFlowID = dynaFlowTask1.DynaFlowID;

                manager.Add(dynaFlowTask1);
                manager.Add(dynaFlowTask2);

                //context.DynaFlowTaskSet.AddRange(dynaFlowTask1, dynaFlowTask2);
                //context.SaveChanges();

                var result = manager.GetByDynaFlowID(dynaFlowTask1.DynaFlowID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //DynaFlowTaskTypeID
        public void GetByDynaFlowTaskType_MultipleDynaFlowTasksSameDynaFlowTaskTypeID_ShouldReturnAllDynaFlowTasks()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskManager(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);
                //  dynaFlowTask1.DynaFlowTaskTypeID = 1;
                var dynaFlowTask2 = CreateTestDynaFlowTask(context);
                dynaFlowTask2.DynaFlowTaskTypeID = dynaFlowTask1.DynaFlowTaskTypeID;

                //context.DynaFlowTaskSet.AddRange(dynaFlowTask1, dynaFlowTask2);
                //context.SaveChanges();

                manager.Add(dynaFlowTask1);
                manager.Add(dynaFlowTask2);

                var result = manager.GetByDynaFlowTaskTypeID(dynaFlowTask1.DynaFlowTaskTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DynaFlowTask> CreateTestDynaFlowTaskAsync(FarmDbContext dbContext)
        {
            return await DynaFlowTaskFactory.CreateAsync(dbContext);
        }

        private DynaFlowTask CreateTestDynaFlowTask(FarmDbContext dbContext)
        {
            return DynaFlowTaskFactory.Create(dbContext);
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
