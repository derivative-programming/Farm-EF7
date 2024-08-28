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
    public partial class DFTDependencyTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);
                var result = await manager.AddAsync(dFTDependency);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DFTDependencySet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);
                var result = manager.Add(dFTDependency);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DFTDependencySet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dFTDependency = await CreateTestDFTDependencyAsync(context);
                    var result = await manager.AddAsync(dFTDependency);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DFTDependencySet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dFTDependency = CreateTestDFTDependency(context);
                    var result = manager.Add(dFTDependency);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DFTDependencySet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDFTDependencys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDFTDependencys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDFTDependencys_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add some dFTDependencys
                await manager.AddAsync(await CreateTestDFTDependencyAsync(context));
                await manager.AddAsync(await CreateTestDFTDependencyAsync(context));
                await manager.AddAsync(await CreateTestDFTDependencyAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDFTDependencys_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add some dFTDependencys
                manager.Add(CreateTestDFTDependency(context));
                manager.Add(CreateTestDFTDependency(context));
                manager.Add(CreateTestDFTDependency(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDFTDependencys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDFTDependencys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDFTDependencys_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add some dFTDependencys
                var dFTDependency1 = await CreateTestDFTDependencyAsync(context);
                var dFTDependency2 = await CreateTestDFTDependencyAsync(context);
                var dFTDependency3 = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependency1);
                await manager.AddAsync(dFTDependency2);
                await manager.AddAsync(dFTDependency3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dFTDependency1.DFTDependencyID, dFTDependency2.DFTDependencyID, dFTDependency3.DFTDependencyID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDFTDependencys_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add some dFTDependencys
                var dFTDependency1 = CreateTestDFTDependency(context);
                var dFTDependency2 = CreateTestDFTDependency(context);
                var dFTDependency3 = CreateTestDFTDependency(context);

                manager.Add(dFTDependency1);
                manager.Add(dFTDependency2);
                manager.Add(dFTDependency3);

                var result = manager.GetMaxId();

                var maxId = new[] { dFTDependency1.DFTDependencyID, dFTDependency2.DFTDependencyID, dFTDependency3.DFTDependencyID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDFTDependency_ShouldReturnCorrectDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencyToAdd = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependencyToAdd);

                var fetchedDFTDependency = await manager.GetByIdAsync(dFTDependencyToAdd.DFTDependencyID);

                Assert.IsNotNull(fetchedDFTDependency);
                Assert.AreEqual(dFTDependencyToAdd.DFTDependencyID, fetchedDFTDependency.DFTDependencyID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDFTDependency_ShouldReturnCorrectDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencyToAdd = CreateTestDFTDependency(context);

                manager.Add(dFTDependencyToAdd);

                var fetchedDFTDependency = manager.GetById(dFTDependencyToAdd.DFTDependencyID);

                Assert.IsNotNull(fetchedDFTDependency);
                Assert.AreEqual(dFTDependencyToAdd.DFTDependencyID, fetchedDFTDependency.DFTDependencyID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDFTDependency_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependency = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDFTDependency);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDFTDependency_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependency = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDFTDependency);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDFTDependency_ShouldReturnCorrectDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencyToAdd = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependencyToAdd);

                var fetchedDFTDependency = await manager.GetByCodeAsync(dFTDependencyToAdd.Code.Value);

                Assert.IsNotNull(fetchedDFTDependency);
                Assert.AreEqual(dFTDependencyToAdd.Code, fetchedDFTDependency.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDFTDependency_ShouldReturnCorrectDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencyToAdd = CreateTestDFTDependency(context);

                manager.Add(dFTDependencyToAdd);

                var fetchedDFTDependency = manager.GetByCode(dFTDependencyToAdd.Code.Value);

                Assert.IsNotNull(fetchedDFTDependency);
                Assert.AreEqual(dFTDependencyToAdd.Code, fetchedDFTDependency.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDFTDependency_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependency = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDFTDependency);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDFTDependency_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependency = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDFTDependency);
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
                var manager = new DFTDependencyManager(context);

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
                var manager = new DFTDependencyManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDFTDependencys_ShouldReturnAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency1 = await CreateTestDFTDependencyAsync(context);
                var dFTDependency2 = await CreateTestDFTDependencyAsync(context);
                var dFTDependency3 = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependency1);
                await manager.AddAsync(dFTDependency2);
                await manager.AddAsync(dFTDependency3);

                var fetchedDFTDependencys = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDFTDependencys);
                Assert.AreEqual(3, fetchedDFTDependencys.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDFTDependencys_ShouldReturnAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency1 = CreateTestDFTDependency(context);
                var dFTDependency2 = CreateTestDFTDependency(context);
                var dFTDependency3 = CreateTestDFTDependency(context);

                manager.Add(dFTDependency1);
                manager.Add(dFTDependency2);
                manager.Add(dFTDependency3);

                var fetchedDFTDependencys = manager.GetAll();

                Assert.IsNotNull(fetchedDFTDependencys);
                Assert.AreEqual(3, fetchedDFTDependencys.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependencys = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDFTDependencys);
                Assert.AreEqual(0, fetchedDFTDependencys.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var fetchedDFTDependencys = manager.GetAll();

                Assert.IsNotNull(fetchedDFTDependencys);
                Assert.AreEqual(0, fetchedDFTDependencys.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDFTDependency_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependency);

                dFTDependency.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dFTDependency);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dFTDependency.Code, context.DFTDependencySet.Find(dFTDependency.DFTDependencyID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDFTDependency_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);

                manager.Add(dFTDependency);

                dFTDependency.Code = Guid.NewGuid();
                var updateResult = manager.Update(dFTDependency);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dFTDependency.Code, context.DFTDependencySet.Find(dFTDependency.DFTDependencyID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Arrange
                var dFTDependency = await CreateTestDFTDependencyAsync(context);
                await manager.AddAsync(dFTDependency);
                var firstInstance = await manager.GetByIdAsync(dFTDependency.DFTDependencyID);
                var secondInstance = await manager.GetByIdAsync(dFTDependency.DFTDependencyID);

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
                var manager = new DFTDependencyManager(context);

                // Arrange
                var dFTDependency = CreateTestDFTDependency(context);
                manager.Add(dFTDependency);
                var firstInstance = manager.GetById(dFTDependency.DFTDependencyID);
                var secondInstance = manager.GetById(dFTDependency.DFTDependencyID);

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
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);
                //context.DFTDependencySet.Add(dFTDependency);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dFTDependency);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dFTDependency.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dFTDependency);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFTDependency = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                    Assert.AreNotEqual(dFTDependency.Code, freshDFTDependency.Code); // Because the transaction was not committed.
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
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);
                //context.DFTDependencySet.Add(dFTDependency);
                //context.SaveChanges();
                manager.Add(dFTDependency);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dFTDependency.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dFTDependency);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFTDependency = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                    Assert.AreNotEqual(dFTDependency.Code, freshDFTDependency.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependency);

                var deleteResult = await manager.DeleteAsync(dFTDependency.DFTDependencyID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DFTDependencySet.Find(dFTDependency.DFTDependencyID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDFTDependency()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);

                manager.Add(dFTDependency);

                var deleteResult = manager.Delete(dFTDependency.DFTDependencyID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DFTDependencySet.Find(dFTDependency.DFTDependencyID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

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
                var manager = new DFTDependencyManager(context);

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
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);

                await manager.AddAsync(dFTDependency);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dFTDependency.DFTDependencyID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFTDependency = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                    Assert.IsNotNull(freshDFTDependency);  // Because the transaction was not committed.
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
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);

                manager.Add(dFTDependency);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dFTDependency.DFTDependencyID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFTDependency = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                    Assert.IsNotNull(freshDFTDependency);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDFTDependencys_ShouldInsertAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                await manager.BulkInsertAsync(dFTDependencys);

                Assert.AreEqual(dFTDependencys.Count, context.DFTDependencySet.Count());
                foreach (var dFTDependency in dFTDependencys)
                {
                    Assert.IsNotNull(context.DFTDependencySet.Find(dFTDependency.DFTDependencyID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDFTDependencys_ShouldInsertAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                manager.BulkInsert(dFTDependencys);

                Assert.AreEqual(dFTDependencys.Count, context.DFTDependencySet.Count());
                foreach (var dFTDependency in dFTDependencys)
                {
                    Assert.IsNotNull(context.DFTDependencySet.Find(dFTDependency.DFTDependencyID));
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DFTDependencySet.Count());  // Because the transaction was not committed.
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DFTDependencySet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add initial dFTDependencys
                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                var dFTDependencysToUpdate = new List<DFTDependency>();

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependencysToUpdate.Add(await manager.AddAsync(dFTDependency));
                }

                // Update dFTDependencys
                foreach (var dFTDependency in dFTDependencysToUpdate)
                {
                    dFTDependency.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dFTDependencysToUpdate);

                // Verify updates
                foreach (var updatedDFTDependency in dFTDependencysToUpdate)
                {
                    var dFTDependencyFromDb = await manager.GetByIdAsync(updatedDFTDependency.DFTDependencyID);// context.DFTDependencySet.Find(updatedDFTDependency.DFTDependencyID);
                    Assert.AreEqual(updatedDFTDependency.Code, dFTDependencyFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add initial dFTDependencys
                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                var dFTDependencysToUpdate = new List<DFTDependency>();

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependencysToUpdate.Add(manager.Add(dFTDependency));
                }

                // Update dFTDependencys
                foreach (var dFTDependency in dFTDependencysToUpdate)
                {
                    dFTDependency.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dFTDependencysToUpdate);

                // Verify updates
                foreach (var updatedDFTDependency in dFTDependencysToUpdate)
                {
                    var dFTDependencyFromDb = manager.GetById(updatedDFTDependency.DFTDependencyID);// context.DFTDependencySet.Find(updatedDFTDependency.DFTDependencyID);
                    Assert.AreEqual(updatedDFTDependency.Code, dFTDependencyFromDb.Code);
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
        //        var manager = new DFTDependencyManager(context);

        //        var dFTDependencys = new List<DFTDependency>
        //        {
        //            await CreateTestDFTDependencyAsync(context),
        //            await CreateTestDFTDependencyAsync(context),
        //            await CreateTestDFTDependencyAsync(context)
        //        };

        //        foreach (var dFTDependency in dFTDependencys)
        //        {
        //            await manager.AddAsync(dFTDependency);
        //        }

        //        foreach (var dFTDependency in dFTDependencys)
        //        {
        //            dFTDependency.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dFTDependencys);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    await manager.AddAsync(dFTDependency);
                }

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependency.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFTDependency in dFTDependencys)
                    {
                        var dFTDependencyFromDb = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                        Assert.AreNotEqual(dFTDependency.Code, dFTDependencyFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    manager.Add(dFTDependency);
                }

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependency.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFTDependency in dFTDependencys)
                    {
                        var dFTDependencyFromDb = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                        Assert.AreNotEqual(dFTDependency.Code, dFTDependencyFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add initial dFTDependencys
                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    await manager.AddAsync(dFTDependency);
                }

                // Delete dFTDependencys
                await manager.BulkDeleteAsync(dFTDependencys);

                // Verify deletions
                foreach (var deletedDFTDependency in dFTDependencys)
                {
                    var dFTDependencyFromDb = context.DFTDependencySet.Find(deletedDFTDependency.DFTDependencyID);
                    Assert.IsNull(dFTDependencyFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                // Add initial dFTDependencys
                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    manager.Add(dFTDependency);
                }

                // Delete dFTDependencys
                manager.BulkDelete(dFTDependencys);

                // Verify deletions
                foreach (var deletedDFTDependency in dFTDependencys)
                {
                    var dFTDependencyFromDb = context.DFTDependencySet.Find(deletedDFTDependency.DFTDependencyID);
                    Assert.IsNull(dFTDependencyFromDb);
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    await manager.AddAsync(dFTDependency);
                }

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependency.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dFTDependencys);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    manager.Add(dFTDependency);
                }

                foreach (var dFTDependency in dFTDependencys)
                {
                    dFTDependency.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dFTDependencys);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context),
                    await CreateTestDFTDependencyAsync(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    await manager.AddAsync(dFTDependency);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFTDependency in dFTDependencys)
                    {
                        var dFTDependencyFromDb = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                        Assert.IsNotNull(dFTDependencyFromDb);  // DFTDependency should still exist as the transaction wasn't committed.
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
                var manager = new DFTDependencyManager(context);

                var dFTDependencys = new List<DFTDependency>
                {
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context),
                    CreateTestDFTDependency(context)
                };

                foreach (var dFTDependency in dFTDependencys)
                {
                    manager.Add(dFTDependency);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dFTDependencys);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFTDependency in dFTDependencys)
                    {
                        var dFTDependencyFromDb = freshContext.DFTDependencySet.Find(dFTDependency.DFTDependencyID);
                        Assert.IsNotNull(dFTDependencyFromDb);  // DFTDependency should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//DynaFlowTaskID
        public async Task GetByDynaFlowTaskIdAsync_ValidDynaFlowTaskId_ShouldReturnDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = await CreateTestDFTDependencyAsync(context);
                //dFTDependency.DynaFlowTaskID = 1;
                //context.DFTDependencySet.Add(dFTDependency);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dFTDependency);

                var result = await manager.GetByDynaFlowTaskIDAsync(dFTDependency.DynaFlowTaskID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dFTDependency.DFTDependencyID, result.First().DFTDependencyID);
            }
        }

        [TestMethod]//DynaFlowTaskID
        public void GetByDynaFlowTaskId_ValidDynaFlowTaskId_ShouldReturnDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency = CreateTestDFTDependency(context);
                //dFTDependency.DynaFlowTaskID = 1;
                //context.DFTDependencySet.Add(dFTDependency);
                //context.SaveChanges();
                manager.Add(dFTDependency);

                var result = manager.GetByDynaFlowTaskID(dFTDependency.DynaFlowTaskID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dFTDependency.DFTDependencyID, result.First().DFTDependencyID);
            }
        }

        [TestMethod] //DynaFlowTaskID
        public async Task GetByDynaFlowTaskIdAsync_InvalidDynaFlowTaskId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = await manager.GetByDynaFlowTaskIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //DynaFlowTaskID
        public void GetByDynaFlowTaskId_InvalidDynaFlowTaskId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var result = manager.GetByDynaFlowTaskID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //DynaFlowTaskID
        public async Task GetByDynaFlowTaskIdAsync_MultipleDFTDependencysSameDynaFlowTaskId_ShouldReturnAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency1 = await CreateTestDFTDependencyAsync(context);
                var dFTDependency2 = await CreateTestDFTDependencyAsync(context);
                dFTDependency2.DynaFlowTaskID = dFTDependency1.DynaFlowTaskID;

                await manager.AddAsync(dFTDependency1);
                await manager.AddAsync(dFTDependency2);

                //context.DFTDependencySet.AddRange(dFTDependency1, dFTDependency2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByDynaFlowTaskIDAsync(dFTDependency1.DynaFlowTaskID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //DynaFlowTaskID
        public void GetByDynaFlowTaskId_MultipleDFTDependencysSameDynaFlowTaskId_ShouldReturnAllDFTDependencys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFTDependencyManager(context);

                var dFTDependency1 = CreateTestDFTDependency(context);
                var dFTDependency2 = CreateTestDFTDependency(context);
                dFTDependency2.DynaFlowTaskID = dFTDependency1.DynaFlowTaskID;

                manager.Add(dFTDependency1);
                manager.Add(dFTDependency2);

                //context.DFTDependencySet.AddRange(dFTDependency1, dFTDependency2);
                //context.SaveChanges();

                var result = manager.GetByDynaFlowTaskID(dFTDependency1.DynaFlowTaskID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DFTDependency> CreateTestDFTDependencyAsync(FarmDbContext dbContext)
        {
            return await DFTDependencyFactory.CreateAsync(dbContext);
        }

        private DFTDependency CreateTestDFTDependency(FarmDbContext dbContext)
        {
            return DFTDependencyFactory.Create(dbContext);
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
