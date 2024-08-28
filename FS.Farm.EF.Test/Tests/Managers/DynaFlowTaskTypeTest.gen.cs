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
    public partial class DynaFlowTaskTypeTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);
                var result = await manager.AddAsync(dynaFlowTaskType);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTaskTypeSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);
                var result = manager.Add(dynaFlowTaskType);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTaskTypeSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);
                    var result = await manager.AddAsync(dynaFlowTaskType);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTaskTypeSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);
                    var result = manager.Add(dynaFlowTaskType);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTaskTypeSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDynaFlowTaskTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDynaFlowTaskTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDynaFlowTaskTypes_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add some dynaFlowTaskTypes
                await manager.AddAsync(await CreateTestDynaFlowTaskTypeAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTaskTypeAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTaskTypeAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDynaFlowTaskTypes_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add some dynaFlowTaskTypes
                manager.Add(CreateTestDynaFlowTaskType(context));
                manager.Add(CreateTestDynaFlowTaskType(context));
                manager.Add(CreateTestDynaFlowTaskType(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDynaFlowTaskTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDynaFlowTaskTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDynaFlowTaskTypes_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add some dynaFlowTaskTypes
                var dynaFlowTaskType1 = await CreateTestDynaFlowTaskTypeAsync(context);
                var dynaFlowTaskType2 = await CreateTestDynaFlowTaskTypeAsync(context);
                var dynaFlowTaskType3 = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskType1);
                await manager.AddAsync(dynaFlowTaskType2);
                await manager.AddAsync(dynaFlowTaskType3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dynaFlowTaskType1.DynaFlowTaskTypeID, dynaFlowTaskType2.DynaFlowTaskTypeID, dynaFlowTaskType3.DynaFlowTaskTypeID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDynaFlowTaskTypes_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add some dynaFlowTaskTypes
                var dynaFlowTaskType1 = CreateTestDynaFlowTaskType(context);
                var dynaFlowTaskType2 = CreateTestDynaFlowTaskType(context);
                var dynaFlowTaskType3 = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskType1);
                manager.Add(dynaFlowTaskType2);
                manager.Add(dynaFlowTaskType3);

                var result = manager.GetMaxId();

                var maxId = new[] { dynaFlowTaskType1.DynaFlowTaskTypeID, dynaFlowTaskType2.DynaFlowTaskTypeID, dynaFlowTaskType3.DynaFlowTaskTypeID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDynaFlowTaskType_ShouldReturnCorrectDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypeToAdd = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskTypeToAdd);

                var fetchedDynaFlowTaskType = await manager.GetByIdAsync(dynaFlowTaskTypeToAdd.DynaFlowTaskTypeID);

                Assert.IsNotNull(fetchedDynaFlowTaskType);
                Assert.AreEqual(dynaFlowTaskTypeToAdd.DynaFlowTaskTypeID, fetchedDynaFlowTaskType.DynaFlowTaskTypeID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDynaFlowTaskType_ShouldReturnCorrectDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypeToAdd = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskTypeToAdd);

                var fetchedDynaFlowTaskType = manager.GetById(dynaFlowTaskTypeToAdd.DynaFlowTaskTypeID);

                Assert.IsNotNull(fetchedDynaFlowTaskType);
                Assert.AreEqual(dynaFlowTaskTypeToAdd.DynaFlowTaskTypeID, fetchedDynaFlowTaskType.DynaFlowTaskTypeID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDynaFlowTaskType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskType = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTaskType);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDynaFlowTaskType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskType = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowTaskType);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDynaFlowTaskType_ShouldReturnCorrectDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypeToAdd = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskTypeToAdd);

                var fetchedDynaFlowTaskType = await manager.GetByCodeAsync(dynaFlowTaskTypeToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTaskType);
                Assert.AreEqual(dynaFlowTaskTypeToAdd.Code, fetchedDynaFlowTaskType.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDynaFlowTaskType_ShouldReturnCorrectDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypeToAdd = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskTypeToAdd);

                var fetchedDynaFlowTaskType = manager.GetByCode(dynaFlowTaskTypeToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowTaskType);
                Assert.AreEqual(dynaFlowTaskTypeToAdd.Code, fetchedDynaFlowTaskType.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDynaFlowTaskType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskType = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTaskType);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDynaFlowTaskType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskType = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowTaskType);
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
                var manager = new DynaFlowTaskTypeManager(context);

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
                var manager = new DynaFlowTaskTypeManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDynaFlowTaskTypes_ShouldReturnAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType1 = await CreateTestDynaFlowTaskTypeAsync(context);
                var dynaFlowTaskType2 = await CreateTestDynaFlowTaskTypeAsync(context);
                var dynaFlowTaskType3 = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskType1);
                await manager.AddAsync(dynaFlowTaskType2);
                await manager.AddAsync(dynaFlowTaskType3);

                var fetchedDynaFlowTaskTypes = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTaskTypes);
                Assert.AreEqual(3, fetchedDynaFlowTaskTypes.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDynaFlowTaskTypes_ShouldReturnAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType1 = CreateTestDynaFlowTaskType(context);
                var dynaFlowTaskType2 = CreateTestDynaFlowTaskType(context);
                var dynaFlowTaskType3 = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskType1);
                manager.Add(dynaFlowTaskType2);
                manager.Add(dynaFlowTaskType3);

                var fetchedDynaFlowTaskTypes = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTaskTypes);
                Assert.AreEqual(3, fetchedDynaFlowTaskTypes.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskTypes = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTaskTypes);
                Assert.AreEqual(0, fetchedDynaFlowTaskTypes.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var fetchedDynaFlowTaskTypes = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTaskTypes);
                Assert.AreEqual(0, fetchedDynaFlowTaskTypes.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDynaFlowTaskType_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskType);

                dynaFlowTaskType.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dynaFlowTaskType);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTaskType.Code, context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDynaFlowTaskType_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskType);

                dynaFlowTaskType.Code = Guid.NewGuid();
                var updateResult = manager.Update(dynaFlowTaskType);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowTaskType.Code, context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Arrange
                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);
                await manager.AddAsync(dynaFlowTaskType);
                var firstInstance = await manager.GetByIdAsync(dynaFlowTaskType.DynaFlowTaskTypeID);
                var secondInstance = await manager.GetByIdAsync(dynaFlowTaskType.DynaFlowTaskTypeID);

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
                var manager = new DynaFlowTaskTypeManager(context);

                // Arrange
                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);
                manager.Add(dynaFlowTaskType);
                var firstInstance = manager.GetById(dynaFlowTaskType.DynaFlowTaskTypeID);
                var secondInstance = manager.GetById(dynaFlowTaskType.DynaFlowTaskTypeID);

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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);
                //context.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTaskType);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dynaFlowTaskType);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTaskType = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.AreNotEqual(dynaFlowTaskType.Code, freshDynaFlowTaskType.Code); // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);
                //context.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                //context.SaveChanges();
                manager.Add(dynaFlowTaskType);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dynaFlowTaskType);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTaskType = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.AreNotEqual(dynaFlowTaskType.Code, freshDynaFlowTaskType.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskType);

                var deleteResult = await manager.DeleteAsync(dynaFlowTaskType.DynaFlowTaskTypeID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDynaFlowTaskType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskType);

                var deleteResult = manager.Delete(dynaFlowTaskType.DynaFlowTaskTypeID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

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
                var manager = new DynaFlowTaskTypeManager(context);

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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);

                await manager.AddAsync(dynaFlowTaskType);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dynaFlowTaskType.DynaFlowTaskTypeID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTaskType = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.IsNotNull(freshDynaFlowTaskType);  // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);

                manager.Add(dynaFlowTaskType);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dynaFlowTaskType.DynaFlowTaskTypeID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowTaskType = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.IsNotNull(freshDynaFlowTaskType);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDynaFlowTaskTypes_ShouldInsertAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                await manager.BulkInsertAsync(dynaFlowTaskTypes);

                Assert.AreEqual(dynaFlowTaskTypes.Count, context.DynaFlowTaskTypeSet.Count());
                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    Assert.IsNotNull(context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDynaFlowTaskTypes_ShouldInsertAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                manager.BulkInsert(dynaFlowTaskTypes);

                Assert.AreEqual(dynaFlowTaskTypes.Count, context.DynaFlowTaskTypeSet.Count());
                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    Assert.IsNotNull(context.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID));
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTaskTypeSet.Count());  // Because the transaction was not committed.
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTaskTypeSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add initial dynaFlowTaskTypes
                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                var dynaFlowTaskTypesToUpdate = new List<DynaFlowTaskType>();

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskTypesToUpdate.Add(await manager.AddAsync(dynaFlowTaskType));
                }

                // Update dynaFlowTaskTypes
                foreach (var dynaFlowTaskType in dynaFlowTaskTypesToUpdate)
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dynaFlowTaskTypesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTaskType in dynaFlowTaskTypesToUpdate)
                {
                    var dynaFlowTaskTypeFromDb = await manager.GetByIdAsync(updatedDynaFlowTaskType.DynaFlowTaskTypeID);// context.DynaFlowTaskTypeSet.Find(updatedDynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.AreEqual(updatedDynaFlowTaskType.Code, dynaFlowTaskTypeFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add initial dynaFlowTaskTypes
                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                var dynaFlowTaskTypesToUpdate = new List<DynaFlowTaskType>();

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskTypesToUpdate.Add(manager.Add(dynaFlowTaskType));
                }

                // Update dynaFlowTaskTypes
                foreach (var dynaFlowTaskType in dynaFlowTaskTypesToUpdate)
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dynaFlowTaskTypesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowTaskType in dynaFlowTaskTypesToUpdate)
                {
                    var dynaFlowTaskTypeFromDb = manager.GetById(updatedDynaFlowTaskType.DynaFlowTaskTypeID);// context.DynaFlowTaskTypeSet.Find(updatedDynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.AreEqual(updatedDynaFlowTaskType.Code, dynaFlowTaskTypeFromDb.Code);
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
        //        var manager = new DynaFlowTaskTypeManager(context);

        //        var dynaFlowTaskTypes = new List<DynaFlowTaskType>
        //        {
        //            await CreateTestDynaFlowTaskTypeAsync(context),
        //            await CreateTestDynaFlowTaskTypeAsync(context),
        //            await CreateTestDynaFlowTaskTypeAsync(context)
        //        };

        //        foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
        //        {
        //            await manager.AddAsync(dynaFlowTaskType);
        //        }

        //        foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
        //        {
        //            dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dynaFlowTaskTypes);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    await manager.AddAsync(dynaFlowTaskType);
                }

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                    {
                        var dynaFlowTaskTypeFromDb = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                        Assert.AreNotEqual(dynaFlowTaskType.Code, dynaFlowTaskTypeFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    manager.Add(dynaFlowTaskType);
                }

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskType.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                    {
                        var dynaFlowTaskTypeFromDb = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                        Assert.AreNotEqual(dynaFlowTaskType.Code, dynaFlowTaskTypeFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add initial dynaFlowTaskTypes
                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    await manager.AddAsync(dynaFlowTaskType);
                }

                // Delete dynaFlowTaskTypes
                await manager.BulkDeleteAsync(dynaFlowTaskTypes);

                // Verify deletions
                foreach (var deletedDynaFlowTaskType in dynaFlowTaskTypes)
                {
                    var dynaFlowTaskTypeFromDb = context.DynaFlowTaskTypeSet.Find(deletedDynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.IsNull(dynaFlowTaskTypeFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                // Add initial dynaFlowTaskTypes
                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    manager.Add(dynaFlowTaskType);
                }

                // Delete dynaFlowTaskTypes
                manager.BulkDelete(dynaFlowTaskTypes);

                // Verify deletions
                foreach (var deletedDynaFlowTaskType in dynaFlowTaskTypes)
                {
                    var dynaFlowTaskTypeFromDb = context.DynaFlowTaskTypeSet.Find(deletedDynaFlowTaskType.DynaFlowTaskTypeID);
                    Assert.IsNull(dynaFlowTaskTypeFromDb);
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    await manager.AddAsync(dynaFlowTaskType);
                }

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dynaFlowTaskTypes);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    manager.Add(dynaFlowTaskType);
                }

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    dynaFlowTaskType.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dynaFlowTaskTypes);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context),
                    await CreateTestDynaFlowTaskTypeAsync(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    await manager.AddAsync(dynaFlowTaskType);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                    {
                        var dynaFlowTaskTypeFromDb = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                        Assert.IsNotNull(dynaFlowTaskTypeFromDb);  // DynaFlowTaskType should still exist as the transaction wasn't committed.
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
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskTypes = new List<DynaFlowTaskType>
                {
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context),
                    CreateTestDynaFlowTaskType(context)
                };

                foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                {
                    manager.Add(dynaFlowTaskType);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dynaFlowTaskTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowTaskType in dynaFlowTaskTypes)
                    {
                        var dynaFlowTaskTypeFromDb = freshContext.DynaFlowTaskTypeSet.Find(dynaFlowTaskType.DynaFlowTaskTypeID);
                        Assert.IsNotNull(dynaFlowTaskTypeFromDb);  // DynaFlowTaskType should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = await CreateTestDynaFlowTaskTypeAsync(context);
                //dynaFlowTaskType.PacID = 1;
                //context.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowTaskType);

                var result = await manager.GetByPacIDAsync(dynaFlowTaskType.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTaskType.DynaFlowTaskTypeID, result.First().DynaFlowTaskTypeID);
            }
        }

        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType = CreateTestDynaFlowTaskType(context);
                //dynaFlowTaskType.PacID = 1;
                //context.DynaFlowTaskTypeSet.Add(dynaFlowTaskType);
                //context.SaveChanges();
                manager.Add(dynaFlowTaskType);

                var result = manager.GetByPacID(dynaFlowTaskType.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowTaskType.DynaFlowTaskTypeID, result.First().DynaFlowTaskTypeID);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

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
                var manager = new DynaFlowTaskTypeManager(context);

                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDynaFlowTaskTypesSamePacId_ShouldReturnAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType1 = await CreateTestDynaFlowTaskTypeAsync(context);
                var dynaFlowTaskType2 = await CreateTestDynaFlowTaskTypeAsync(context);
                dynaFlowTaskType2.PacID = dynaFlowTaskType1.PacID;

                await manager.AddAsync(dynaFlowTaskType1);
                await manager.AddAsync(dynaFlowTaskType2);

                //context.DynaFlowTaskTypeSet.AddRange(dynaFlowTaskType1, dynaFlowTaskType2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByPacIDAsync(dynaFlowTaskType1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public void GetByPacId_MultipleDynaFlowTaskTypesSamePacId_ShouldReturnAllDynaFlowTaskTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTaskTypeManager(context);

                var dynaFlowTaskType1 = CreateTestDynaFlowTaskType(context);
                var dynaFlowTaskType2 = CreateTestDynaFlowTaskType(context);
                dynaFlowTaskType2.PacID = dynaFlowTaskType1.PacID;

                manager.Add(dynaFlowTaskType1);
                manager.Add(dynaFlowTaskType2);

                //context.DynaFlowTaskTypeSet.AddRange(dynaFlowTaskType1, dynaFlowTaskType2);
                //context.SaveChanges();

                var result = manager.GetByPacID(dynaFlowTaskType1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DynaFlowTaskType> CreateTestDynaFlowTaskTypeAsync(FarmDbContext dbContext)
        {
            return await DynaFlowTaskTypeFactory.CreateAsync(dbContext);
        }

        private DynaFlowTaskType CreateTestDynaFlowTaskType(FarmDbContext dbContext)
        {
            return DynaFlowTaskTypeFactory.Create(dbContext);
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
