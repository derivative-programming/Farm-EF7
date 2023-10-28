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
    public partial class TriStateFilterTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                var result = await manager.AddAsync(triStateFilter);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.TriStateFilterSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                var result = manager.Add(triStateFilter);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.TriStateFilterSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var triStateFilter = await CreateTestTriStateFilterAsync(context);
                    var result = await manager.AddAsync(triStateFilter);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.TriStateFilterSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var triStateFilter = CreateTestTriStateFilter(context);
                    var result = manager.Add(triStateFilter);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.TriStateFilterSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoTriStateFilters_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoTriStateFilters_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithTriStateFilters_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add some triStateFilters
                await manager.AddAsync(await CreateTestTriStateFilterAsync(context));
                await manager.AddAsync(await CreateTestTriStateFilterAsync(context));
                await manager.AddAsync(await CreateTestTriStateFilterAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithTriStateFilters_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add some triStateFilters
                manager.Add(CreateTestTriStateFilter(context));
                manager.Add(CreateTestTriStateFilter(context));
                manager.Add(CreateTestTriStateFilter(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoTriStateFilters_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoTriStateFilters_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = manager.GetMaxId();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithTriStateFilters_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add some triStateFilters
                var triStateFilter1 = await CreateTestTriStateFilterAsync(context);
                var triStateFilter2 = await CreateTestTriStateFilterAsync(context);
                var triStateFilter3 = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                await manager.AddAsync(triStateFilter3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { triStateFilter1.TriStateFilterID, triStateFilter2.TriStateFilterID, triStateFilter3.TriStateFilterID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithTriStateFilters_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add some triStateFilters
                var triStateFilter1 = CreateTestTriStateFilter(context);
                var triStateFilter2 = CreateTestTriStateFilter(context);
                var triStateFilter3 = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter1);
                manager.Add(triStateFilter2);
                manager.Add(triStateFilter3);
                var result = manager.GetMaxId();
                var maxId = new[] { triStateFilter1.TriStateFilterID, triStateFilter2.TriStateFilterID, triStateFilter3.TriStateFilterID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingTriStateFilter_ShouldReturnCorrectTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilterToAdd = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilterToAdd);
                var fetchedTriStateFilter = await manager.GetByIdAsync(triStateFilterToAdd.TriStateFilterID);
                Assert.IsNotNull(fetchedTriStateFilter);
                Assert.AreEqual(triStateFilterToAdd.TriStateFilterID, fetchedTriStateFilter.TriStateFilterID);
            }
        }
        [TestMethod]
        public void GetById_ExistingTriStateFilter_ShouldReturnCorrectTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilterToAdd = CreateTestTriStateFilter(context);
                manager.Add(triStateFilterToAdd);
                var fetchedTriStateFilter = manager.GetById(triStateFilterToAdd.TriStateFilterID);
                Assert.IsNotNull(fetchedTriStateFilter);
                Assert.AreEqual(triStateFilterToAdd.TriStateFilterID, fetchedTriStateFilter.TriStateFilterID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingTriStateFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilter = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedTriStateFilter);
            }
        }
        [TestMethod]
        public void GetById_NonExistingTriStateFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilter = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedTriStateFilter);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingTriStateFilter_ShouldReturnCorrectTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilterToAdd = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilterToAdd);
                var fetchedTriStateFilter = await manager.GetByCodeAsync(triStateFilterToAdd.Code.Value);
                Assert.IsNotNull(fetchedTriStateFilter);
                Assert.AreEqual(triStateFilterToAdd.Code, fetchedTriStateFilter.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingTriStateFilter_ShouldReturnCorrectTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilterToAdd = CreateTestTriStateFilter(context);
                manager.Add(triStateFilterToAdd);
                var fetchedTriStateFilter = manager.GetByCode(triStateFilterToAdd.Code.Value);
                Assert.IsNotNull(fetchedTriStateFilter);
                Assert.AreEqual(triStateFilterToAdd.Code, fetchedTriStateFilter.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingTriStateFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilter = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedTriStateFilter);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingTriStateFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilter = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedTriStateFilter);
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
                var manager = new TriStateFilterManager(context);
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
                var manager = new TriStateFilterManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleTriStateFilters_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = await CreateTestTriStateFilterAsync(context);
                var triStateFilter2 = await CreateTestTriStateFilterAsync(context);
                var triStateFilter3 = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                await manager.AddAsync(triStateFilter3);
                var fetchedTriStateFilters = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedTriStateFilters);
                Assert.AreEqual(3, fetchedTriStateFilters.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleTriStateFilters_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = CreateTestTriStateFilter(context);
                var triStateFilter2 = CreateTestTriStateFilter(context);
                var triStateFilter3 = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter1);
                manager.Add(triStateFilter2);
                manager.Add(triStateFilter3);
                var fetchedTriStateFilters = manager.GetAll();
                Assert.IsNotNull(fetchedTriStateFilters);
                Assert.AreEqual(3, fetchedTriStateFilters.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilters = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedTriStateFilters);
                Assert.AreEqual(0, fetchedTriStateFilters.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var fetchedTriStateFilters = manager.GetAll();
                Assert.IsNotNull(fetchedTriStateFilters);
                Assert.AreEqual(0, fetchedTriStateFilters.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidTriStateFilter_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter);
                triStateFilter.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(triStateFilter);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(triStateFilter.Code, context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidTriStateFilter_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter);
                triStateFilter.Code = Guid.NewGuid();
                var updateResult = manager.Update(triStateFilter);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(triStateFilter.Code, context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Arrange
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter);
                var firstInstance = await manager.GetByIdAsync(triStateFilter.TriStateFilterID);
                var secondInstance = await manager.GetByIdAsync(triStateFilter.TriStateFilterID);
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
                var manager = new TriStateFilterManager(context);
                // Arrange
                var triStateFilter = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter);
                var firstInstance = manager.GetById(triStateFilter.TriStateFilterID);
                var secondInstance = manager.GetById(triStateFilter.TriStateFilterID);
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
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    triStateFilter.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(triStateFilter);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTriStateFilter = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                    Assert.AreNotEqual(triStateFilter.Code, freshTriStateFilter.Code); // Because the transaction was not committed.
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
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //context.SaveChanges();
                manager.Add(triStateFilter);
                using (var transaction = context.Database.BeginTransaction())
                {
                    triStateFilter.Code = Guid.NewGuid();
                    var updateResult = manager.Update(triStateFilter);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTriStateFilter = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                    Assert.AreNotEqual(triStateFilter.Code, freshTriStateFilter.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter);
                var deleteResult = await manager.DeleteAsync(triStateFilter.TriStateFilterID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter);
                var deleteResult = manager.Delete(triStateFilter.TriStateFilterID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
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
                var manager = new TriStateFilterManager(context);
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
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                await manager.AddAsync(triStateFilter);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(triStateFilter.TriStateFilterID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTriStateFilter = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                    Assert.IsNotNull(freshTriStateFilter);  // Because the transaction was not committed.
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
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                manager.Add(triStateFilter);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(triStateFilter.TriStateFilterID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTriStateFilter = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                    Assert.IsNotNull(freshTriStateFilter);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidTriStateFilters_ShouldInsertAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                await manager.BulkInsertAsync(triStateFilters);
                Assert.AreEqual(triStateFilters.Count, context.TriStateFilterSet.Count());
                foreach (var triStateFilter in triStateFilters)
                {
                    Assert.IsNotNull(context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidTriStateFilters_ShouldInsertAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                manager.BulkInsert(triStateFilters);
                Assert.AreEqual(triStateFilters.Count, context.TriStateFilterSet.Count());
                foreach (var triStateFilter in triStateFilters)
                {
                    Assert.IsNotNull(context.TriStateFilterSet.Find(triStateFilter.TriStateFilterID));
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.TriStateFilterSet.Count());  // Because the transaction was not committed.
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.TriStateFilterSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add initial triStateFilters
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                var triStateFiltersToUpdate = new List<TriStateFilter>();
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFiltersToUpdate.Add(await manager.AddAsync(triStateFilter));
                }
                // Update triStateFilters
                foreach (var triStateFilter in triStateFiltersToUpdate)
                {
                    triStateFilter.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(triStateFiltersToUpdate);
                // Verify updates
                foreach (var updatedTriStateFilter in triStateFiltersToUpdate)
                {
                    var triStateFilterFromDb = await manager.GetByIdAsync(updatedTriStateFilter.TriStateFilterID);// context.TriStateFilterSet.Find(updatedTriStateFilter.TriStateFilterID);
                    Assert.AreEqual(updatedTriStateFilter.Code, triStateFilterFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add initial triStateFilters
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                var triStateFiltersToUpdate = new List<TriStateFilter>();
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFiltersToUpdate.Add(manager.Add(triStateFilter));
                }
                // Update triStateFilters
                foreach (var triStateFilter in triStateFiltersToUpdate)
                {
                    triStateFilter.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(triStateFiltersToUpdate);
                // Verify updates
                foreach (var updatedTriStateFilter in triStateFiltersToUpdate)
                {
                    var triStateFilterFromDb = manager.GetById(updatedTriStateFilter.TriStateFilterID);// context.TriStateFilterSet.Find(updatedTriStateFilter.TriStateFilterID);
                    Assert.AreEqual(updatedTriStateFilter.Code, triStateFilterFromDb.Code);
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
        //        var manager = new TriStateFilterManager(context);
        //        var triStateFilters = new List<TriStateFilter>
        //        {
        //            await CreateTestTriStateFilterAsync(context),
        //            await CreateTestTriStateFilterAsync(context),
        //            await CreateTestTriStateFilterAsync(context)
        //        };
        //        foreach (var triStateFilter in triStateFilters)
        //        {
        //            await manager.AddAsync(triStateFilter);
        //        }
        //        foreach (var triStateFilter in triStateFilters)
        //        {
        //            triStateFilter.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(triStateFilters);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    await manager.AddAsync(triStateFilter);
                }
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFilter.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var triStateFilter in triStateFilters)
                    {
                        var triStateFilterFromDb = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                        Assert.AreNotEqual(triStateFilter.Code, triStateFilterFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    manager.Add(triStateFilter);
                }
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFilter.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var triStateFilter in triStateFilters)
                    {
                        var triStateFilterFromDb = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                        Assert.AreNotEqual(triStateFilter.Code, triStateFilterFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add initial triStateFilters
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    await manager.AddAsync(triStateFilter);
                }
                // Delete triStateFilters
                await manager.BulkDeleteAsync(triStateFilters);
                // Verify deletions
                foreach (var deletedTriStateFilter in triStateFilters)
                {
                    var triStateFilterFromDb = context.TriStateFilterSet.Find(deletedTriStateFilter.TriStateFilterID);
                    Assert.IsNull(triStateFilterFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add initial triStateFilters
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    manager.Add(triStateFilter);
                }
                // Delete triStateFilters
                manager.BulkDelete(triStateFilters);
                // Verify deletions
                foreach (var deletedTriStateFilter in triStateFilters)
                {
                    var triStateFilterFromDb = context.TriStateFilterSet.Find(deletedTriStateFilter.TriStateFilterID);
                    Assert.IsNull(triStateFilterFromDb);
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    await manager.AddAsync(triStateFilter);
                }
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFilter.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(triStateFilters);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    manager.Add(triStateFilter);
                }
                foreach (var triStateFilter in triStateFilters)
                {
                    triStateFilter.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(triStateFilters);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context),
                    await CreateTestTriStateFilterAsync(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    await manager.AddAsync(triStateFilter);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var triStateFilter in triStateFilters)
                    {
                        var triStateFilterFromDb = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                        Assert.IsNotNull(triStateFilterFromDb);  // TriStateFilter should still exist as the transaction wasn't committed.
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
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context),
                    CreateTestTriStateFilter(context)
                };
                foreach (var triStateFilter in triStateFilters)
                {
                    manager.Add(triStateFilter);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(triStateFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var triStateFilter in triStateFilters)
                    {
                        var triStateFilterFromDb = freshContext.TriStateFilterSet.Find(triStateFilter.TriStateFilterID);
                        Assert.IsNotNull(triStateFilterFromDb);  // TriStateFilter should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilterAsync(context);
                //triStateFilter.PacID = 1;
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter);
                var result = await manager.GetByPacIDAsync(triStateFilter.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(triStateFilter.TriStateFilterID, result.First().TriStateFilterID);
            }
        }
        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = CreateTestTriStateFilter(context);
                //triStateFilter.PacID = 1;
                //context.TriStateFilterSet.Add(triStateFilter);
                //context.SaveChanges();
                manager.Add(triStateFilter);
                var result = manager.GetByPacID(triStateFilter.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(triStateFilter.TriStateFilterID, result.First().TriStateFilterID);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
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
                var manager = new TriStateFilterManager(context);
                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleTriStateFiltersSamePacId_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = await CreateTestTriStateFilterAsync(context);
                var triStateFilter2 = await CreateTestTriStateFilterAsync(context);
                triStateFilter2.PacID = triStateFilter1.PacID;
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                //context.TriStateFilterSet.AddRange(triStateFilter1, triStateFilter2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacIDAsync(triStateFilter1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_MultipleTriStateFiltersSamePacId_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = CreateTestTriStateFilter(context);
                var triStateFilter2 = CreateTestTriStateFilter(context);
                triStateFilter2.PacID = triStateFilter1.PacID;
                manager.Add(triStateFilter1);
                manager.Add(triStateFilter2);
                //context.TriStateFilterSet.AddRange(triStateFilter1, triStateFilter2);
                //context.SaveChanges();
                var result = manager.GetByPacID(triStateFilter1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<TriStateFilter> CreateTestTriStateFilterAsync(FarmDbContext dbContext)
        {
            return await TriStateFilterFactory.CreateAsync(dbContext);
        }
        private TriStateFilter CreateTestTriStateFilter(FarmDbContext dbContext)
        {
            return TriStateFilterFactory.Create(dbContext);
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
