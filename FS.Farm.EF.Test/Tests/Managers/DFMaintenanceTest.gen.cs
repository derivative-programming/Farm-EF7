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
    public partial class DFMaintenanceTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);
                var result = await manager.AddAsync(dFMaintenance);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DFMaintenanceSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);
                var result = manager.Add(dFMaintenance);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DFMaintenanceSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dFMaintenance = await CreateTestDFMaintenanceAsync(context);
                    var result = await manager.AddAsync(dFMaintenance);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DFMaintenanceSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var dFMaintenance = CreateTestDFMaintenance(context);
                    var result = manager.Add(dFMaintenance);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DFMaintenanceSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoDFMaintenances_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoDFMaintenances_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_WithDFMaintenances_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add some dFMaintenances
                await manager.AddAsync(await CreateTestDFMaintenanceAsync(context));
                await manager.AddAsync(await CreateTestDFMaintenanceAsync(context));
                await manager.AddAsync(await CreateTestDFMaintenanceAsync(context));

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithDFMaintenances_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add some dFMaintenances
                manager.Add(CreateTestDFMaintenance(context));
                manager.Add(CreateTestDFMaintenance(context));
                manager.Add(CreateTestDFMaintenance(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoDFMaintenances_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoDFMaintenances_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var result = manager.GetMaxId();

                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithDFMaintenances_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add some dFMaintenances
                var dFMaintenance1 = await CreateTestDFMaintenanceAsync(context);
                var dFMaintenance2 = await CreateTestDFMaintenanceAsync(context);
                var dFMaintenance3 = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenance1);
                await manager.AddAsync(dFMaintenance2);
                await manager.AddAsync(dFMaintenance3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { dFMaintenance1.DFMaintenanceID, dFMaintenance2.DFMaintenanceID, dFMaintenance3.DFMaintenanceID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithDFMaintenances_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add some dFMaintenances
                var dFMaintenance1 = CreateTestDFMaintenance(context);
                var dFMaintenance2 = CreateTestDFMaintenance(context);
                var dFMaintenance3 = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenance1);
                manager.Add(dFMaintenance2);
                manager.Add(dFMaintenance3);

                var result = manager.GetMaxId();

                var maxId = new[] { dFMaintenance1.DFMaintenanceID, dFMaintenance2.DFMaintenanceID, dFMaintenance3.DFMaintenanceID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingDFMaintenance_ShouldReturnCorrectDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenanceToAdd = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenanceToAdd);

                var fetchedDFMaintenance = await manager.GetByIdAsync(dFMaintenanceToAdd.DFMaintenanceID);

                Assert.IsNotNull(fetchedDFMaintenance);
                Assert.AreEqual(dFMaintenanceToAdd.DFMaintenanceID, fetchedDFMaintenance.DFMaintenanceID);
            }
        }

        [TestMethod]
        public void GetById_ExistingDFMaintenance_ShouldReturnCorrectDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenanceToAdd = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenanceToAdd);

                var fetchedDFMaintenance = manager.GetById(dFMaintenanceToAdd.DFMaintenanceID);

                Assert.IsNotNull(fetchedDFMaintenance);
                Assert.AreEqual(dFMaintenanceToAdd.DFMaintenanceID, fetchedDFMaintenance.DFMaintenanceID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingDFMaintenance_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenance = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDFMaintenance);
            }
        }
        [TestMethod]
        public void GetById_NonExistingDFMaintenance_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenance = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedDFMaintenance);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingDFMaintenance_ShouldReturnCorrectDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenanceToAdd = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenanceToAdd);

                var fetchedDFMaintenance = await manager.GetByCodeAsync(dFMaintenanceToAdd.Code.Value);

                Assert.IsNotNull(fetchedDFMaintenance);
                Assert.AreEqual(dFMaintenanceToAdd.Code, fetchedDFMaintenance.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingDFMaintenance_ShouldReturnCorrectDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenanceToAdd = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenanceToAdd);

                var fetchedDFMaintenance = manager.GetByCode(dFMaintenanceToAdd.Code.Value);

                Assert.IsNotNull(fetchedDFMaintenance);
                Assert.AreEqual(dFMaintenanceToAdd.Code, fetchedDFMaintenance.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDFMaintenance_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenance = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDFMaintenance);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingDFMaintenance_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenance = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedDFMaintenance);
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
                var manager = new DFMaintenanceManager(context);

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
                var manager = new DFMaintenanceManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultipleDFMaintenances_ShouldReturnAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance1 = await CreateTestDFMaintenanceAsync(context);
                var dFMaintenance2 = await CreateTestDFMaintenanceAsync(context);
                var dFMaintenance3 = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenance1);
                await manager.AddAsync(dFMaintenance2);
                await manager.AddAsync(dFMaintenance3);

                var fetchedDFMaintenances = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDFMaintenances);
                Assert.AreEqual(3, fetchedDFMaintenances.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleDFMaintenances_ShouldReturnAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance1 = CreateTestDFMaintenance(context);
                var dFMaintenance2 = CreateTestDFMaintenance(context);
                var dFMaintenance3 = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenance1);
                manager.Add(dFMaintenance2);
                manager.Add(dFMaintenance3);

                var fetchedDFMaintenances = manager.GetAll();

                Assert.IsNotNull(fetchedDFMaintenances);
                Assert.AreEqual(3, fetchedDFMaintenances.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenances = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedDFMaintenances);
                Assert.AreEqual(0, fetchedDFMaintenances.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var fetchedDFMaintenances = manager.GetAll();

                Assert.IsNotNull(fetchedDFMaintenances);
                Assert.AreEqual(0, fetchedDFMaintenances.Count());
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ValidDFMaintenance_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenance);

                dFMaintenance.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dFMaintenance);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dFMaintenance.Code, context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidDFMaintenance_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenance);

                dFMaintenance.Code = Guid.NewGuid();
                var updateResult = manager.Update(dFMaintenance);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(dFMaintenance.Code, context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Arrange
                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);
                await manager.AddAsync(dFMaintenance);
                var firstInstance = await manager.GetByIdAsync(dFMaintenance.DFMaintenanceID);
                var secondInstance = await manager.GetByIdAsync(dFMaintenance.DFMaintenanceID);

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
                var manager = new DFMaintenanceManager(context);

                // Arrange
                var dFMaintenance = CreateTestDFMaintenance(context);
                manager.Add(dFMaintenance);
                var firstInstance = manager.GetById(dFMaintenance.DFMaintenanceID);
                var secondInstance = manager.GetById(dFMaintenance.DFMaintenanceID);

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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);
                //context.DFMaintenanceSet.Add(dFMaintenance);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dFMaintenance);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dFMaintenance.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dFMaintenance);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFMaintenance = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                    Assert.AreNotEqual(dFMaintenance.Code, freshDFMaintenance.Code); // Because the transaction was not committed.
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);
                //context.DFMaintenanceSet.Add(dFMaintenance);
                //context.SaveChanges();
                manager.Add(dFMaintenance);

                using (var transaction = context.Database.BeginTransaction())
                {
                    dFMaintenance.Code = Guid.NewGuid();
                    var updateResult = manager.Update(dFMaintenance);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFMaintenance = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                    Assert.AreNotEqual(dFMaintenance.Code, freshDFMaintenance.Code); // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenance);

                var deleteResult = await manager.DeleteAsync(dFMaintenance.DFMaintenanceID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteDFMaintenance()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenance);

                var deleteResult = manager.Delete(dFMaintenance.DFMaintenanceID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

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
                var manager = new DFMaintenanceManager(context);

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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);

                await manager.AddAsync(dFMaintenance);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dFMaintenance.DFMaintenanceID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFMaintenance = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                    Assert.IsNotNull(freshDFMaintenance);  // Because the transaction was not committed.
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);

                manager.Add(dFMaintenance);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(dFMaintenance.DFMaintenanceID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDFMaintenance = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                    Assert.IsNotNull(freshDFMaintenance);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidDFMaintenances_ShouldInsertAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                await manager.BulkInsertAsync(dFMaintenances);

                Assert.AreEqual(dFMaintenances.Count, context.DFMaintenanceSet.Count());
                foreach (var dFMaintenance in dFMaintenances)
                {
                    Assert.IsNotNull(context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidDFMaintenances_ShouldInsertAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                manager.BulkInsert(dFMaintenances);

                Assert.AreEqual(dFMaintenances.Count, context.DFMaintenanceSet.Count());
                foreach (var dFMaintenance in dFMaintenances)
                {
                    Assert.IsNotNull(context.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID));
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DFMaintenanceSet.Count());  // Because the transaction was not committed.
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DFMaintenanceSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add initial dFMaintenances
                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                var dFMaintenancesToUpdate = new List<DFMaintenance>();

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenancesToUpdate.Add(await manager.AddAsync(dFMaintenance));
                }

                // Update dFMaintenances
                foreach (var dFMaintenance in dFMaintenancesToUpdate)
                {
                    dFMaintenance.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(dFMaintenancesToUpdate);

                // Verify updates
                foreach (var updatedDFMaintenance in dFMaintenancesToUpdate)
                {
                    var dFMaintenanceFromDb = await manager.GetByIdAsync(updatedDFMaintenance.DFMaintenanceID);// context.DFMaintenanceSet.Find(updatedDFMaintenance.DFMaintenanceID);
                    Assert.AreEqual(updatedDFMaintenance.Code, dFMaintenanceFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add initial dFMaintenances
                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                var dFMaintenancesToUpdate = new List<DFMaintenance>();

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenancesToUpdate.Add(manager.Add(dFMaintenance));
                }

                // Update dFMaintenances
                foreach (var dFMaintenance in dFMaintenancesToUpdate)
                {
                    dFMaintenance.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(dFMaintenancesToUpdate);

                // Verify updates
                foreach (var updatedDFMaintenance in dFMaintenancesToUpdate)
                {
                    var dFMaintenanceFromDb = manager.GetById(updatedDFMaintenance.DFMaintenanceID);// context.DFMaintenanceSet.Find(updatedDFMaintenance.DFMaintenanceID);
                    Assert.AreEqual(updatedDFMaintenance.Code, dFMaintenanceFromDb.Code);
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
        //        var manager = new DFMaintenanceManager(context);

        //        var dFMaintenances = new List<DFMaintenance>
        //        {
        //            await CreateTestDFMaintenanceAsync(context),
        //            await CreateTestDFMaintenanceAsync(context),
        //            await CreateTestDFMaintenanceAsync(context)
        //        };

        //        foreach (var dFMaintenance in dFMaintenances)
        //        {
        //            await manager.AddAsync(dFMaintenance);
        //        }

        //        foreach (var dFMaintenance in dFMaintenances)
        //        {
        //            dFMaintenance.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dFMaintenances);  // This should throw a concurrency exception
        //    }
        //}

        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    await manager.AddAsync(dFMaintenance);
                }

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenance.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFMaintenance in dFMaintenances)
                    {
                        var dFMaintenanceFromDb = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                        Assert.AreNotEqual(dFMaintenance.Code, dFMaintenanceFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    manager.Add(dFMaintenance);
                }

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenance.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFMaintenance in dFMaintenances)
                    {
                        var dFMaintenanceFromDb = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                        Assert.AreNotEqual(dFMaintenance.Code, dFMaintenanceFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add initial dFMaintenances
                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    await manager.AddAsync(dFMaintenance);
                }

                // Delete dFMaintenances
                await manager.BulkDeleteAsync(dFMaintenances);

                // Verify deletions
                foreach (var deletedDFMaintenance in dFMaintenances)
                {
                    var dFMaintenanceFromDb = context.DFMaintenanceSet.Find(deletedDFMaintenance.DFMaintenanceID);
                    Assert.IsNull(dFMaintenanceFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                // Add initial dFMaintenances
                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    manager.Add(dFMaintenance);
                }

                // Delete dFMaintenances
                manager.BulkDelete(dFMaintenances);

                // Verify deletions
                foreach (var deletedDFMaintenance in dFMaintenances)
                {
                    var dFMaintenanceFromDb = context.DFMaintenanceSet.Find(deletedDFMaintenance.DFMaintenanceID);
                    Assert.IsNull(dFMaintenanceFromDb);
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    await manager.AddAsync(dFMaintenance);
                }

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenance.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dFMaintenances);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    manager.Add(dFMaintenance);
                }

                foreach (var dFMaintenance in dFMaintenances)
                {
                    dFMaintenance.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(dFMaintenances);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context),
                    await CreateTestDFMaintenanceAsync(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    await manager.AddAsync(dFMaintenance);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFMaintenance in dFMaintenances)
                    {
                        var dFMaintenanceFromDb = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                        Assert.IsNotNull(dFMaintenanceFromDb);  // DFMaintenance should still exist as the transaction wasn't committed.
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
                var manager = new DFMaintenanceManager(context);

                var dFMaintenances = new List<DFMaintenance>
                {
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context),
                    CreateTestDFMaintenance(context)
                };

                foreach (var dFMaintenance in dFMaintenances)
                {
                    manager.Add(dFMaintenance);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(dFMaintenances);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dFMaintenance in dFMaintenances)
                    {
                        var dFMaintenanceFromDb = freshContext.DFMaintenanceSet.Find(dFMaintenance.DFMaintenanceID);
                        Assert.IsNotNull(dFMaintenanceFromDb);  // DFMaintenance should still exist as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = await CreateTestDFMaintenanceAsync(context);
                //dFMaintenance.PacID = 1;
                //context.DFMaintenanceSet.Add(dFMaintenance);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dFMaintenance);

                var result = await manager.GetByPacIDAsync(dFMaintenance.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dFMaintenance.DFMaintenanceID, result.First().DFMaintenanceID);
            }
        }

        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance = CreateTestDFMaintenance(context);
                //dFMaintenance.PacID = 1;
                //context.DFMaintenanceSet.Add(dFMaintenance);
                //context.SaveChanges();
                manager.Add(dFMaintenance);

                var result = manager.GetByPacID(dFMaintenance.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dFMaintenance.DFMaintenanceID, result.First().DFMaintenanceID);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

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
                var manager = new DFMaintenanceManager(context);

                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDFMaintenancesSamePacId_ShouldReturnAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance1 = await CreateTestDFMaintenanceAsync(context);
                var dFMaintenance2 = await CreateTestDFMaintenanceAsync(context);
                dFMaintenance2.PacID = dFMaintenance1.PacID;

                await manager.AddAsync(dFMaintenance1);
                await manager.AddAsync(dFMaintenance2);

                //context.DFMaintenanceSet.AddRange(dFMaintenance1, dFMaintenance2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByPacIDAsync(dFMaintenance1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //PacID
        public void GetByPacId_MultipleDFMaintenancesSamePacId_ShouldReturnAllDFMaintenances()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DFMaintenanceManager(context);

                var dFMaintenance1 = CreateTestDFMaintenance(context);
                var dFMaintenance2 = CreateTestDFMaintenance(context);
                dFMaintenance2.PacID = dFMaintenance1.PacID;

                manager.Add(dFMaintenance1);
                manager.Add(dFMaintenance2);

                //context.DFMaintenanceSet.AddRange(dFMaintenance1, dFMaintenance2);
                //context.SaveChanges();

                var result = manager.GetByPacID(dFMaintenance1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        private async Task<DFMaintenance> CreateTestDFMaintenanceAsync(FarmDbContext dbContext)
        {
            return await DFMaintenanceFactory.CreateAsync(dbContext);
        }

        private DFMaintenance CreateTestDFMaintenance(FarmDbContext dbContext)
        {
            return DFMaintenanceFactory.Create(dbContext);
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
