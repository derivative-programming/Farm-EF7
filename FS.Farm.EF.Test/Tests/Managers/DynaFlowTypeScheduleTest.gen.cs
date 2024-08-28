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
    public partial class DynaFlowTypeScheduleTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                var result = await manager.AddAsync(dynaFlowTypeSchedule);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTypeScheduleSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                var result = manager.Add(dynaFlowTypeSchedule);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTypeScheduleSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                    var result = await manager.AddAsync(dynaFlowTypeSchedule);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTypeScheduleSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                    var result = manager.Add(dynaFlowTypeSchedule);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTypeScheduleSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDynaFlowTypeSchedules_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDynaFlowTypeSchedules_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDynaFlowTypeSchedules_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add some dynaFlowTypeSchedules
                await manager.AddAsync(await CreateTestDynaFlowTypeScheduleAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTypeScheduleAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTypeScheduleAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDynaFlowTypeSchedules_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add some dynaFlowTypeSchedules
                manager.Add(CreateTestDynaFlowTypeSchedule(context));
                manager.Add(CreateTestDynaFlowTypeSchedule(context));
                manager.Add(CreateTestDynaFlowTypeSchedule(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDynaFlowTypeSchedules_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDynaFlowTypeSchedules_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDynaFlowTypeSchedules_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add some dynaFlowTypeSchedules
                var dynaFlowTypeSchedule1 = await CreateTestDynaFlowTypeScheduleAsync(context);
                var dynaFlowTypeSchedule2 = await CreateTestDynaFlowTypeScheduleAsync(context);
                var dynaFlowTypeSchedule3 = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeSchedule1);
                await manager.AddAsync(dynaFlowTypeSchedule2);
                await manager.AddAsync(dynaFlowTypeSchedule3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dynaFlowTypeSchedule1.DynaFlowTypeScheduleID, dynaFlowTypeSchedule2.DynaFlowTypeScheduleID, dynaFlowTypeSchedule3.DynaFlowTypeScheduleID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDynaFlowTypeSchedules_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add some dynaFlowTypeSchedules
                var dynaFlowTypeSchedule1 = CreateTestDynaFlowTypeSchedule(context);
                var dynaFlowTypeSchedule2 = CreateTestDynaFlowTypeSchedule(context);
                var dynaFlowTypeSchedule3 = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeSchedule1);
                manager.Add(dynaFlowTypeSchedule2);
                manager.Add(dynaFlowTypeSchedule3);

                var result = manager.GetMaxId();

                var maxId = new[] { dynaFlowTypeSchedule1.DynaFlowTypeScheduleID, dynaFlowTypeSchedule2.DynaFlowTypeScheduleID, dynaFlowTypeSchedule3.DynaFlowTypeScheduleID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDynaFlowTypeSchedule_ShouldReturnCorrectDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeScheduleToAdd = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeScheduleToAdd);

                var fetchedDynaFlowTypeSchedule = await manager.GetByIdAsync(dynaFlowTypeScheduleToAdd.DynaFlowTypeScheduleID);

                Assert.IsNotNull(fetchedDynaFlowTypeSchedule);
                Assert.AreEqual(dynaFlowTypeScheduleToAdd.DynaFlowTypeScheduleID, fetchedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDynaFlowTypeSchedule_ShouldReturnCorrectDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeScheduleToAdd = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeScheduleToAdd);

                var fetchedDynaFlowTypeSchedule = manager.GetById(dynaFlowTypeScheduleToAdd.DynaFlowTypeScheduleID);

                Assert.IsNotNull(fetchedDynaFlowTypeSchedule);
                Assert.AreEqual(dynaFlowTypeScheduleToAdd.DynaFlowTypeScheduleID, fetchedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDynaFlowTypeSchedule_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedule = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTypeSchedule);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDynaFlowTypeSchedule_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedule = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTypeSchedule);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDynaFlowTypeSchedule_ShouldReturnCorrectDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeScheduleToAdd = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeScheduleToAdd);

                var fetchedDynaFlowTypeSchedule = await manager.GetByCodeAsync(dynaFlowTypeScheduleToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTypeSchedule);
                Assert.AreEqual(dynaFlowTypeScheduleToAdd.Code, fetchedDynaFlowTypeSchedule.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDynaFlowTypeSchedule_ShouldReturnCorrectDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeScheduleToAdd = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeScheduleToAdd);

                var fetchedDynaFlowTypeSchedule = manager.GetByCode(dynaFlowTypeScheduleToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTypeSchedule);
                Assert.AreEqual(dynaFlowTypeScheduleToAdd.Code, fetchedDynaFlowTypeSchedule.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDynaFlowTypeSchedule_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedule = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTypeSchedule);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDynaFlowTypeSchedule_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedule = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTypeSchedule);
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
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDynaFlowTypeSchedules_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = await CreateTestDynaFlowTypeScheduleAsync(context);
                var dynaFlowTypeSchedule2 = await CreateTestDynaFlowTypeScheduleAsync(context);
                var dynaFlowTypeSchedule3 = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeSchedule1);
                await manager.AddAsync(dynaFlowTypeSchedule2);
                await manager.AddAsync(dynaFlowTypeSchedule3);

                var fetchedDynaFlowTypeSchedules = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTypeSchedules);
                Assert.AreEqual(3, fetchedDynaFlowTypeSchedules.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDynaFlowTypeSchedules_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = CreateTestDynaFlowTypeSchedule(context);
                var dynaFlowTypeSchedule2 = CreateTestDynaFlowTypeSchedule(context);
                var dynaFlowTypeSchedule3 = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeSchedule1);
                manager.Add(dynaFlowTypeSchedule2);
                manager.Add(dynaFlowTypeSchedule3);

                var fetchedDynaFlowTypeSchedules = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTypeSchedules);
                Assert.AreEqual(3, fetchedDynaFlowTypeSchedules.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedules = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTypeSchedules);
                Assert.AreEqual(0, fetchedDynaFlowTypeSchedules.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var fetchedDynaFlowTypeSchedules = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTypeSchedules);
                Assert.AreEqual(0, fetchedDynaFlowTypeSchedules.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDynaFlowTypeSchedule_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeSchedule);

                dynaFlowTypeSchedule.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dynaFlowTypeSchedule);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTypeSchedule.Code, context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDynaFlowTypeSchedule_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeSchedule);

                dynaFlowTypeSchedule.Code = Guid.NewGuid();
                var updateResult = manager.Update(dynaFlowTypeSchedule);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTypeSchedule.Code, context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Arrange
                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                await manager.AddAsync(dynaFlowTypeSchedule);
                var firstInstance = await manager.GetByIdAsync(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                var secondInstance = await manager.GetByIdAsync(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

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
                var manager = new DynaFlowTypeScheduleManager(context);

                // Arrange
                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                manager.Add(dynaFlowTypeSchedule);
                var firstInstance = manager.GetById(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                var secondInstance = manager.GetById(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTypeSchedule);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dynaFlowTypeSchedule);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTypeSchedule = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.AreNotEqual(dynaFlowTypeSchedule.Code, freshDynaFlowTypeSchedule.Code); // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //context.SaveChanges();
                manager.Add(dynaFlowTypeSchedule);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dynaFlowTypeSchedule);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTypeSchedule = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.AreNotEqual(dynaFlowTypeSchedule.Code, freshDynaFlowTypeSchedule.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeSchedule);

                var deleteResult = await manager.DeleteAsync(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDynaFlowTypeSchedule()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeSchedule);

                var deleteResult = manager.Delete(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);

                await manager.AddAsync(dynaFlowTypeSchedule);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTypeSchedule = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.IsNotNull(freshDynaFlowTypeSchedule);  // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);

                manager.Add(dynaFlowTypeSchedule);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTypeSchedule = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.IsNotNull(freshDynaFlowTypeSchedule);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDynaFlowTypeSchedules_ShouldInsertAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                await manager.BulkInsertAsync(dynaFlowTypeSchedules);

                Assert.AreEqual(dynaFlowTypeSchedules.Count, context.DynaFlowTypeScheduleSet.Count());
                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    Assert.IsNotNull(context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDynaFlowTypeSchedules_ShouldInsertAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                manager.BulkInsert(dynaFlowTypeSchedules);

                Assert.AreEqual(dynaFlowTypeSchedules.Count, context.DynaFlowTypeScheduleSet.Count());
                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    Assert.IsNotNull(context.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID));
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTypeScheduleSet.Count());  // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTypeScheduleSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add initial dynaFlowTypeSchedules
                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                var dynaFlowTypeSchedulesToUpdate = new List<DynaFlowTypeSchedule>();

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedulesToUpdate.Add(await manager.AddAsync(dynaFlowTypeSchedule));
                }

                // Update dynaFlowTypeSchedules
                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedulesToUpdate)
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dynaFlowTypeSchedulesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTypeSchedule in dynaFlowTypeSchedulesToUpdate)
                {
                    var dynaFlowTypeScheduleFromDb = await manager.GetByIdAsync(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);// context.DynaFlowTypeScheduleSet.Find(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.AreEqual(updatedDynaFlowTypeSchedule.Code, dynaFlowTypeScheduleFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add initial dynaFlowTypeSchedules
                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                var dynaFlowTypeSchedulesToUpdate = new List<DynaFlowTypeSchedule>();

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedulesToUpdate.Add(manager.Add(dynaFlowTypeSchedule));
                }

                // Update dynaFlowTypeSchedules
                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedulesToUpdate)
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dynaFlowTypeSchedulesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTypeSchedule in dynaFlowTypeSchedulesToUpdate)
                {
                    var dynaFlowTypeScheduleFromDb = manager.GetById(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);// context.DynaFlowTypeScheduleSet.Find(updatedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.AreEqual(updatedDynaFlowTypeSchedule.Code, dynaFlowTypeScheduleFromDb.Code);
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
        //        var manager = new DynaFlowTypeScheduleManager(context);

        //        var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
        //        {
        //            await CreateTestDynaFlowTypeScheduleAsync(context),
        //            await CreateTestDynaFlowTypeScheduleAsync(context),
        //            await CreateTestDynaFlowTypeScheduleAsync(context)
        //        };

        //        foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
        //        {
        //            await manager.AddAsync(dynaFlowTypeSchedule);
        //        }

        //        foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
        //        {
        //            dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dynaFlowTypeSchedules);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    await manager.AddAsync(dynaFlowTypeSchedule);
                }

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                    {
                        var dynaFlowTypeScheduleFromDb = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        Assert.AreNotEqual(dynaFlowTypeSchedule.Code, dynaFlowTypeScheduleFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    manager.Add(dynaFlowTypeSchedule);
                }

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedule.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                    {
                        var dynaFlowTypeScheduleFromDb = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        Assert.AreNotEqual(dynaFlowTypeSchedule.Code, dynaFlowTypeScheduleFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add initial dynaFlowTypeSchedules
                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    await manager.AddAsync(dynaFlowTypeSchedule);
                }

                // Delete dynaFlowTypeSchedules
                await manager.BulkDeleteAsync(dynaFlowTypeSchedules);

                // Verify deletions
                foreach (var deletedDynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    var dynaFlowTypeScheduleFromDb = context.DynaFlowTypeScheduleSet.Find(deletedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.IsNull(dynaFlowTypeScheduleFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                // Add initial dynaFlowTypeSchedules
                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    manager.Add(dynaFlowTypeSchedule);
                }

                // Delete dynaFlowTypeSchedules
                manager.BulkDelete(dynaFlowTypeSchedules);

                // Verify deletions
                foreach (var deletedDynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    var dynaFlowTypeScheduleFromDb = context.DynaFlowTypeScheduleSet.Find(deletedDynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                    Assert.IsNull(dynaFlowTypeScheduleFromDb);
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    await manager.AddAsync(dynaFlowTypeSchedule);
                }

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dynaFlowTypeSchedules);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    manager.Add(dynaFlowTypeSchedule);
                }

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    dynaFlowTypeSchedule.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dynaFlowTypeSchedules);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context),
                    await CreateTestDynaFlowTypeScheduleAsync(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    await manager.AddAsync(dynaFlowTypeSchedule);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                    {
                        var dynaFlowTypeScheduleFromDb = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        Assert.IsNotNull(dynaFlowTypeScheduleFromDb);  // DynaFlowTypeSchedule should still exist as the transaction wasn't committed.
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
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedules = new List<DynaFlowTypeSchedule>
                {
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context),
                    CreateTestDynaFlowTypeSchedule(context)
                };

                foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                {
                    manager.Add(dynaFlowTypeSchedule);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dynaFlowTypeSchedules);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTypeSchedule in dynaFlowTypeSchedules)
                    {
                        var dynaFlowTypeScheduleFromDb = freshContext.DynaFlowTypeScheduleSet.Find(dynaFlowTypeSchedule.DynaFlowTypeScheduleID);
                        Assert.IsNotNull(dynaFlowTypeScheduleFromDb);  // DynaFlowTypeSchedule should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_ValidDynaFlowTypeID_ShouldReturnDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                // dynaFlowTypeSchedule.DynaFlowTypeID = 1;
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTypeSchedule);

                var result = await manager.GetByDynaFlowTypeIDAsync(dynaFlowTypeSchedule.DynaFlowTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTypeSchedule.DynaFlowTypeScheduleID, result.First().DynaFlowTypeScheduleID);
            }
        }

        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = await CreateTestDynaFlowTypeScheduleAsync(context);
                //dynaFlowTypeSchedule.PacID = 1;
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTypeSchedule);

                var result = await manager.GetByPacIDAsync(dynaFlowTypeSchedule.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTypeSchedule.DynaFlowTypeScheduleID, result.First().DynaFlowTypeScheduleID);
            }
        }
        [TestMethod]//DynaFlowTypeID
        public void GetByDynaFlowType_ValidDynaFlowTypeID_ShouldReturnDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                // dynaFlowTypeSchedule.DynaFlowTypeID = 1;
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //context.SaveChanges();
                manager.Add(dynaFlowTypeSchedule);

                var result = manager.GetByDynaFlowTypeID(dynaFlowTypeSchedule.DynaFlowTypeID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTypeSchedule.DynaFlowTypeScheduleID, result.First().DynaFlowTypeScheduleID);
            }
        }

        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule = CreateTestDynaFlowTypeSchedule(context);
                //dynaFlowTypeSchedule.PacID = 1;
                //context.DynaFlowTypeScheduleSet.Add(dynaFlowTypeSchedule);
                //context.SaveChanges();
                manager.Add(dynaFlowTypeSchedule);

                var result = manager.GetByPacID(dynaFlowTypeSchedule.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTypeSchedule.DynaFlowTypeScheduleID, result.First().DynaFlowTypeScheduleID);
            }
        }
        [TestMethod]//DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_InvalidDynaFlowTypeID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

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
                var manager = new DynaFlowTypeScheduleManager(context);

                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //DynaFlowTypeID
        public async Task GetByDynaFlowTypeAsync_MultipleDynaFlowTypeSchedulesSameDynaFlowTypeID_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = await CreateTestDynaFlowTypeScheduleAsync(context);
                //  dynaFlowTypeSchedule1.DynaFlowTypeID = 1;
                var dynaFlowTypeSchedule2 = await CreateTestDynaFlowTypeScheduleAsync(context);
                dynaFlowTypeSchedule2.DynaFlowTypeID = dynaFlowTypeSchedule1.DynaFlowTypeID;

                //context.DynaFlowTypeScheduleSet.AddRange(dynaFlowTypeSchedule1, dynaFlowTypeSchedule2);
                //await context.SaveChangesAsync();

                await manager.AddAsync(dynaFlowTypeSchedule1);
                await manager.AddAsync(dynaFlowTypeSchedule2);

                var result = await manager.GetByDynaFlowTypeIDAsync(dynaFlowTypeSchedule1.DynaFlowTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDynaFlowTypeSchedulesSamePacId_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = await CreateTestDynaFlowTypeScheduleAsync(context);
                var dynaFlowTypeSchedule2 = await CreateTestDynaFlowTypeScheduleAsync(context);
                dynaFlowTypeSchedule2.PacID = dynaFlowTypeSchedule1.PacID;

                await manager.AddAsync(dynaFlowTypeSchedule1);
                await manager.AddAsync(dynaFlowTypeSchedule2);

                //context.DynaFlowTypeScheduleSet.AddRange(dynaFlowTypeSchedule1, dynaFlowTypeSchedule2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByPacIDAsync(dynaFlowTypeSchedule1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //DynaFlowTypeID
        public void GetByDynaFlowType_MultipleDynaFlowTypeSchedulesSameDynaFlowTypeID_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = CreateTestDynaFlowTypeSchedule(context);
                //  dynaFlowTypeSchedule1.DynaFlowTypeID = 1;
                var dynaFlowTypeSchedule2 = CreateTestDynaFlowTypeSchedule(context);
                dynaFlowTypeSchedule2.DynaFlowTypeID = dynaFlowTypeSchedule1.DynaFlowTypeID;

                //context.DynaFlowTypeScheduleSet.AddRange(dynaFlowTypeSchedule1, dynaFlowTypeSchedule2);
                //context.SaveChanges();

                manager.Add(dynaFlowTypeSchedule1);
                manager.Add(dynaFlowTypeSchedule2);

                var result = manager.GetByDynaFlowTypeID(dynaFlowTypeSchedule1.DynaFlowTypeID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public void GetByPacId_MultipleDynaFlowTypeSchedulesSamePacId_ShouldReturnAllDynaFlowTypeSchedules()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeScheduleManager(context);

                var dynaFlowTypeSchedule1 = CreateTestDynaFlowTypeSchedule(context);
                var dynaFlowTypeSchedule2 = CreateTestDynaFlowTypeSchedule(context);
                dynaFlowTypeSchedule2.PacID = dynaFlowTypeSchedule1.PacID;

                manager.Add(dynaFlowTypeSchedule1);
                manager.Add(dynaFlowTypeSchedule2);

                //context.DynaFlowTypeScheduleSet.AddRange(dynaFlowTypeSchedule1, dynaFlowTypeSchedule2);
                //context.SaveChanges();

                var result = manager.GetByPacID(dynaFlowTypeSchedule1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DynaFlowTypeSchedule> CreateTestDynaFlowTypeScheduleAsync(FarmDbContext dbContext)
        {
            return await DynaFlowTypeScheduleFactory.CreateAsync(dbContext);
        }

        private DynaFlowTypeSchedule CreateTestDynaFlowTypeSchedule(FarmDbContext dbContext)
        {
            return DynaFlowTypeScheduleFactory.Create(dbContext);
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
