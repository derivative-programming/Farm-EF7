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
    public class TriStateFilterTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
                var result = await manager.AddAsync(triStateFilter);
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
                    var triStateFilter = await CreateTestTriStateFilter(context);
                    var result = await manager.AddAsync(triStateFilter);
                    await transaction.CommitAsync();
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
        public async Task GetTotalCountAsync_WithTriStateFilters_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                // Add some triStateFilters
                await manager.AddAsync(await CreateTestTriStateFilter(context));
                await manager.AddAsync(await CreateTestTriStateFilter(context));
                await manager.AddAsync(await CreateTestTriStateFilter(context));
                //// Add some triStateFilters
                //context.TriStateFilterSet.AddRange(
                //    CreateTestTriStateFilter(context),
                //    CreateTestTriStateFilter(context),
                //    CreateTestTriStateFilter(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoTriStateFilters_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
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
                var triStateFilter1 = await CreateTestTriStateFilter(context);
                var triStateFilter2 = await CreateTestTriStateFilter(context);
                var triStateFilter3 = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.AddRange(triStateFilter1, triStateFilter2, triStateFilter3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                await manager.AddAsync(triStateFilter3);
                var result = await manager.GetMaxIdAsync();
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
                var triStateFilterToAdd = await CreateTestTriStateFilter(context);
                await manager.AddAsync(triStateFilterToAdd);
                //context.TriStateFilterSet.Add(triStateFilterToAdd);
                //await context.SaveChangesAsync();
                var fetchedTriStateFilter = await manager.GetByIdAsync(triStateFilterToAdd.TriStateFilterID);
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
        public async Task GetByCodeAsync_ExistingTriStateFilter_ShouldReturnCorrectTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilterToAdd = await CreateTestTriStateFilter(context);
                await manager.AddAsync(triStateFilterToAdd);
                //context.TriStateFilterSet.Add(triStateFilterToAdd);
                //await context.SaveChangesAsync();
                var fetchedTriStateFilter = await manager.GetByCodeAsync(triStateFilterToAdd.Code.Value);
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
        public async Task GetAllAsync_MultipleTriStateFilters_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = await CreateTestTriStateFilter(context);
                var triStateFilter2 = await CreateTestTriStateFilter(context);
                var triStateFilter3 = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.AddRange(triStateFilter1, triStateFilter2, triStateFilter3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                await manager.AddAsync(triStateFilter3);
                var fetchedTriStateFilters = await manager.GetAllAsync();
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
        public async Task UpdateAsync_ValidTriStateFilter_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter);
                triStateFilter.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(triStateFilter);
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
                //var triStateFilter = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(triStateFilter).State = EntityState.Modified;
                //triStateFilter.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(triStateFilter);
                //Assert.IsFalse(updateResult);
                // Arrange
                var triStateFilter = await CreateTestTriStateFilter(context);
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
        public async Task UpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
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
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteTriStateFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter);
                var deleteResult = await manager.DeleteAsync(triStateFilter.TriStateFilterID);
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
        public async Task DeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
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
        public async Task BulkInsertAsync_ValidTriStateFilters_ShouldInsertAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
        public async Task BulkInsertAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
        //            await CreateTestTriStateFilter(context),
        //            await CreateTestTriStateFilter(context),
        //            await CreateTestTriStateFilter(context)
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
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilters = new List<TriStateFilter>
                {
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context),
                    await CreateTestTriStateFilter(context)
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
        //ENDSET
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter = await CreateTestTriStateFilter(context);
                //triStateFilter.PacID = 1;
                //context.TriStateFilterSet.Add(triStateFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(triStateFilter);
                var result = await manager.GetByPacAsync(triStateFilter.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(triStateFilter.TriStateFilterID, result.First().TriStateFilterID);
            }
        }
        //ENDSET
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var result = await manager.GetByPacAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleTriStateFiltersSamePacId_ShouldReturnAllTriStateFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TriStateFilterManager(context);
                var triStateFilter1 = await CreateTestTriStateFilter(context);
                var triStateFilter2 = await CreateTestTriStateFilter(context);
                triStateFilter2.PacID = triStateFilter1.PacID;
                await manager.AddAsync(triStateFilter1);
                await manager.AddAsync(triStateFilter2);
                //context.TriStateFilterSet.AddRange(triStateFilter1, triStateFilter2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacAsync(triStateFilter1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        private async Task<TriStateFilter> CreateTestTriStateFilter(FarmDbContext dbContext)
        {
            return await TriStateFilterFactory.CreateAsync(dbContext);
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
