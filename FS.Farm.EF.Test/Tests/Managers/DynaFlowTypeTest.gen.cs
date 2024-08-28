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
    public partial class DynaFlowTypeTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);
                var result = await manager.AddAsync(dynaFlowType);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTypeSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);
                var result = manager.Add(dynaFlowType);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DynaFlowTypeSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);
                    var result = await manager.AddAsync(dynaFlowType);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTypeSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dynaFlowType = CreateTestDynaFlowType(context);
                    var result = manager.Add(dynaFlowType);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DynaFlowTypeSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDynaFlowTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDynaFlowTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDynaFlowTypes_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add some dynaFlowTypes
                await manager.AddAsync(await CreateTestDynaFlowTypeAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTypeAsync(context));
                await manager.AddAsync(await CreateTestDynaFlowTypeAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDynaFlowTypes_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add some dynaFlowTypes
                manager.Add(CreateTestDynaFlowType(context));
                manager.Add(CreateTestDynaFlowType(context));
                manager.Add(CreateTestDynaFlowType(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDynaFlowTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDynaFlowTypes_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDynaFlowTypes_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add some dynaFlowTypes
                var dynaFlowType1 = await CreateTestDynaFlowTypeAsync(context);
                var dynaFlowType2 = await CreateTestDynaFlowTypeAsync(context);
                var dynaFlowType3 = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowType1);
                await manager.AddAsync(dynaFlowType2);
                await manager.AddAsync(dynaFlowType3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dynaFlowType1.DynaFlowTypeID, dynaFlowType2.DynaFlowTypeID, dynaFlowType3.DynaFlowTypeID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDynaFlowTypes_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add some dynaFlowTypes
                var dynaFlowType1 = CreateTestDynaFlowType(context);
                var dynaFlowType2 = CreateTestDynaFlowType(context);
                var dynaFlowType3 = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowType1);
                manager.Add(dynaFlowType2);
                manager.Add(dynaFlowType3);

                var result = manager.GetMaxId();

                var maxId = new[] { dynaFlowType1.DynaFlowTypeID, dynaFlowType2.DynaFlowTypeID, dynaFlowType3.DynaFlowTypeID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDynaFlowType_ShouldReturnCorrectDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypeToAdd = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowTypeToAdd);

                var fetchedDynaFlowType = await manager.GetByIdAsync(dynaFlowTypeToAdd.DynaFlowTypeID);

                Assert.IsNotNull(fetchedDynaFlowType);
                Assert.AreEqual(dynaFlowTypeToAdd.DynaFlowTypeID, fetchedDynaFlowType.DynaFlowTypeID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDynaFlowType_ShouldReturnCorrectDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypeToAdd = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowTypeToAdd);

                var fetchedDynaFlowType = manager.GetById(dynaFlowTypeToAdd.DynaFlowTypeID);

                Assert.IsNotNull(fetchedDynaFlowType);
                Assert.AreEqual(dynaFlowTypeToAdd.DynaFlowTypeID, fetchedDynaFlowType.DynaFlowTypeID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDynaFlowType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowType = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowType);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDynaFlowType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowType = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDynaFlowType);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDynaFlowType_ShouldReturnCorrectDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypeToAdd = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowTypeToAdd);

                var fetchedDynaFlowType = await manager.GetByCodeAsync(dynaFlowTypeToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowType);
                Assert.AreEqual(dynaFlowTypeToAdd.Code, fetchedDynaFlowType.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDynaFlowType_ShouldReturnCorrectDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypeToAdd = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowTypeToAdd);

                var fetchedDynaFlowType = manager.GetByCode(dynaFlowTypeToAdd.Code.Value);

                Assert.IsNotNull(fetchedDynaFlowType);
                Assert.AreEqual(dynaFlowTypeToAdd.Code, fetchedDynaFlowType.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDynaFlowType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowType = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowType);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDynaFlowType_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowType = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDynaFlowType);
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
                var manager = new DynaFlowTypeManager(context);

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
                var manager = new DynaFlowTypeManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDynaFlowTypes_ShouldReturnAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType1 = await CreateTestDynaFlowTypeAsync(context);
                var dynaFlowType2 = await CreateTestDynaFlowTypeAsync(context);
                var dynaFlowType3 = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowType1);
                await manager.AddAsync(dynaFlowType2);
                await manager.AddAsync(dynaFlowType3);

                var fetchedDynaFlowTypes = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTypes);
                Assert.AreEqual(3, fetchedDynaFlowTypes.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDynaFlowTypes_ShouldReturnAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType1 = CreateTestDynaFlowType(context);
                var dynaFlowType2 = CreateTestDynaFlowType(context);
                var dynaFlowType3 = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowType1);
                manager.Add(dynaFlowType2);
                manager.Add(dynaFlowType3);

                var fetchedDynaFlowTypes = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTypes);
                Assert.AreEqual(3, fetchedDynaFlowTypes.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowTypes = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDynaFlowTypes);
                Assert.AreEqual(0, fetchedDynaFlowTypes.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var fetchedDynaFlowTypes = manager.GetAll();

                Assert.IsNotNull(fetchedDynaFlowTypes);
                Assert.AreEqual(0, fetchedDynaFlowTypes.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDynaFlowType_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowType);

                dynaFlowType.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dynaFlowType);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowType.Code, context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDynaFlowType_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowType);

                dynaFlowType.Code = Guid.NewGuid();
                var updateResult = manager.Update(dynaFlowType);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dynaFlowType.Code, context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Arrange
                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);
                await manager.AddAsync(dynaFlowType);
                var firstInstance = await manager.GetByIdAsync(dynaFlowType.DynaFlowTypeID);
                var secondInstance = await manager.GetByIdAsync(dynaFlowType.DynaFlowTypeID);

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
                var manager = new DynaFlowTypeManager(context);

                // Arrange
                var dynaFlowType = CreateTestDynaFlowType(context);
                manager.Add(dynaFlowType);
                var firstInstance = manager.GetById(dynaFlowType.DynaFlowTypeID);
                var secondInstance = manager.GetById(dynaFlowType.DynaFlowTypeID);

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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);
                //context.DynaFlowTypeSet.Add(dynaFlowType);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowType);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dynaFlowType.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dynaFlowType);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowType = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                    Assert.AreNotEqual(dynaFlowType.Code, freshDynaFlowType.Code); // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);
                //context.DynaFlowTypeSet.Add(dynaFlowType);
                //context.SaveChanges();
                manager.Add(dynaFlowType);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dynaFlowType.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dynaFlowType);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowType = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                    Assert.AreNotEqual(dynaFlowType.Code, freshDynaFlowType.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowType);

                var deleteResult = await manager.DeleteAsync(dynaFlowType.DynaFlowTypeID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDynaFlowType()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowType);

                var deleteResult = manager.Delete(dynaFlowType.DynaFlowTypeID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

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
                var manager = new DynaFlowTypeManager(context);

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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);

                await manager.AddAsync(dynaFlowType);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dynaFlowType.DynaFlowTypeID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowType = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                    Assert.IsNotNull(freshDynaFlowType);  // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);

                manager.Add(dynaFlowType);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dynaFlowType.DynaFlowTypeID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDynaFlowType = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                    Assert.IsNotNull(freshDynaFlowType);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDynaFlowTypes_ShouldInsertAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                await manager.BulkInsertAsync(dynaFlowTypes);

                Assert.AreEqual(dynaFlowTypes.Count, context.DynaFlowTypeSet.Count());
                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    Assert.IsNotNull(context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDynaFlowTypes_ShouldInsertAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                manager.BulkInsert(dynaFlowTypes);

                Assert.AreEqual(dynaFlowTypes.Count, context.DynaFlowTypeSet.Count());
                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    Assert.IsNotNull(context.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID));
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTypeSet.Count());  // Because the transaction was not committed.
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DynaFlowTypeSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add initial dynaFlowTypes
                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                var dynaFlowTypesToUpdate = new List<DynaFlowType>();

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowTypesToUpdate.Add(await manager.AddAsync(dynaFlowType));
                }

                // Update dynaFlowTypes
                foreach (var dynaFlowType in dynaFlowTypesToUpdate)
                {
                    dynaFlowType.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dynaFlowTypesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowType in dynaFlowTypesToUpdate)
                {
                    var dynaFlowTypeFromDb = await manager.GetByIdAsync(updatedDynaFlowType.DynaFlowTypeID);// context.DynaFlowTypeSet.Find(updatedDynaFlowType.DynaFlowTypeID);
                    Assert.AreEqual(updatedDynaFlowType.Code, dynaFlowTypeFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add initial dynaFlowTypes
                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                var dynaFlowTypesToUpdate = new List<DynaFlowType>();

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowTypesToUpdate.Add(manager.Add(dynaFlowType));
                }

                // Update dynaFlowTypes
                foreach (var dynaFlowType in dynaFlowTypesToUpdate)
                {
                    dynaFlowType.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dynaFlowTypesToUpdate);

                // Verify updates
                foreach (var updatedDynaFlowType in dynaFlowTypesToUpdate)
                {
                    var dynaFlowTypeFromDb = manager.GetById(updatedDynaFlowType.DynaFlowTypeID);// context.DynaFlowTypeSet.Find(updatedDynaFlowType.DynaFlowTypeID);
                    Assert.AreEqual(updatedDynaFlowType.Code, dynaFlowTypeFromDb.Code);
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
        //        var manager = new DynaFlowTypeManager(context);

        //        var dynaFlowTypes = new List<DynaFlowType>
        //        {
        //            await CreateTestDynaFlowTypeAsync(context),
        //            await CreateTestDynaFlowTypeAsync(context),
        //            await CreateTestDynaFlowTypeAsync(context)
        //        };

        //        foreach (var dynaFlowType in dynaFlowTypes)
        //        {
        //            await manager.AddAsync(dynaFlowType);
        //        }

        //        foreach (var dynaFlowType in dynaFlowTypes)
        //        {
        //            dynaFlowType.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dynaFlowTypes);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    await manager.AddAsync(dynaFlowType);
                }

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowType.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowType in dynaFlowTypes)
                    {
                        var dynaFlowTypeFromDb = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                        Assert.AreNotEqual(dynaFlowType.Code, dynaFlowTypeFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    manager.Add(dynaFlowType);
                }

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowType.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowType in dynaFlowTypes)
                    {
                        var dynaFlowTypeFromDb = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                        Assert.AreNotEqual(dynaFlowType.Code, dynaFlowTypeFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add initial dynaFlowTypes
                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    await manager.AddAsync(dynaFlowType);
                }

                // Delete dynaFlowTypes
                await manager.BulkDeleteAsync(dynaFlowTypes);

                // Verify deletions
                foreach (var deletedDynaFlowType in dynaFlowTypes)
                {
                    var dynaFlowTypeFromDb = context.DynaFlowTypeSet.Find(deletedDynaFlowType.DynaFlowTypeID);
                    Assert.IsNull(dynaFlowTypeFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                // Add initial dynaFlowTypes
                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    manager.Add(dynaFlowType);
                }

                // Delete dynaFlowTypes
                manager.BulkDelete(dynaFlowTypes);

                // Verify deletions
                foreach (var deletedDynaFlowType in dynaFlowTypes)
                {
                    var dynaFlowTypeFromDb = context.DynaFlowTypeSet.Find(deletedDynaFlowType.DynaFlowTypeID);
                    Assert.IsNull(dynaFlowTypeFromDb);
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    await manager.AddAsync(dynaFlowType);
                }

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowType.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dynaFlowTypes);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    manager.Add(dynaFlowType);
                }

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    dynaFlowType.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dynaFlowTypes);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context),
                    await CreateTestDynaFlowTypeAsync(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    await manager.AddAsync(dynaFlowType);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowType in dynaFlowTypes)
                    {
                        var dynaFlowTypeFromDb = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                        Assert.IsNotNull(dynaFlowTypeFromDb);  // DynaFlowType should still exist as the transaction wasn't committed.
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
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowTypes = new List<DynaFlowType>
                {
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context),
                    CreateTestDynaFlowType(context)
                };

                foreach (var dynaFlowType in dynaFlowTypes)
                {
                    manager.Add(dynaFlowType);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dynaFlowTypes);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dynaFlowType in dynaFlowTypes)
                    {
                        var dynaFlowTypeFromDb = freshContext.DynaFlowTypeSet.Find(dynaFlowType.DynaFlowTypeID);
                        Assert.IsNotNull(dynaFlowTypeFromDb);  // DynaFlowType should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = await CreateTestDynaFlowTypeAsync(context);
                //dynaFlowType.PacID = 1;
                //context.DynaFlowTypeSet.Add(dynaFlowType);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dynaFlowType);

                var result = await manager.GetByPacIDAsync(dynaFlowType.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowType.DynaFlowTypeID, result.First().DynaFlowTypeID);
            }
        }

        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType = CreateTestDynaFlowType(context);
                //dynaFlowType.PacID = 1;
                //context.DynaFlowTypeSet.Add(dynaFlowType);
                //context.SaveChanges();
                manager.Add(dynaFlowType);

                var result = manager.GetByPacID(dynaFlowType.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dynaFlowType.DynaFlowTypeID, result.First().DynaFlowTypeID);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

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
                var manager = new DynaFlowTypeManager(context);

                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDynaFlowTypesSamePacId_ShouldReturnAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType1 = await CreateTestDynaFlowTypeAsync(context);
                var dynaFlowType2 = await CreateTestDynaFlowTypeAsync(context);
                dynaFlowType2.PacID = dynaFlowType1.PacID;

                await manager.AddAsync(dynaFlowType1);
                await manager.AddAsync(dynaFlowType2);

                //context.DynaFlowTypeSet.AddRange(dynaFlowType1, dynaFlowType2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByPacIDAsync(dynaFlowType1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public void GetByPacId_MultipleDynaFlowTypesSamePacId_ShouldReturnAllDynaFlowTypes()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DynaFlowTypeManager(context);

                var dynaFlowType1 = CreateTestDynaFlowType(context);
                var dynaFlowType2 = CreateTestDynaFlowType(context);
                dynaFlowType2.PacID = dynaFlowType1.PacID;

                manager.Add(dynaFlowType1);
                manager.Add(dynaFlowType2);

                //context.DynaFlowTypeSet.AddRange(dynaFlowType1, dynaFlowType2);
                //context.SaveChanges();

                var result = manager.GetByPacID(dynaFlowType1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DynaFlowType> CreateTestDynaFlowTypeAsync(FarmDbContext dbContext)
        {
            return await DynaFlowTypeFactory.CreateAsync(dbContext);
        }

        private DynaFlowType CreateTestDynaFlowType(FarmDbContext dbContext)
        {
            return DynaFlowTypeFactory.Create(dbContext);
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
