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
    public class DateGreaterThanFilterTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddDateGreaterThanFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                var result = await manager.AddAsync(dateGreaterThanFilter);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.DateGreaterThanFilterSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddDateGreaterThanFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                    var result = await manager.AddAsync(dateGreaterThanFilter);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.DateGreaterThanFilterSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoDateGreaterThanFilters_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithDateGreaterThanFilters_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                // Add some dateGreaterThanFilters
                await manager.AddAsync(await CreateTestDateGreaterThanFilter(context));
                await manager.AddAsync(await CreateTestDateGreaterThanFilter(context));
                await manager.AddAsync(await CreateTestDateGreaterThanFilter(context));
                //// Add some dateGreaterThanFilters
                //context.DateGreaterThanFilterSet.AddRange(
                //    CreateTestDateGreaterThanFilter(context),
                //    CreateTestDateGreaterThanFilter(context),
                //    CreateTestDateGreaterThanFilter(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoDateGreaterThanFilters_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithDateGreaterThanFilters_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                // Add some dateGreaterThanFilters
                var dateGreaterThanFilter1 = await CreateTestDateGreaterThanFilter(context);
                var dateGreaterThanFilter2 = await CreateTestDateGreaterThanFilter(context);
                var dateGreaterThanFilter3 = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.AddRange(dateGreaterThanFilter1, dateGreaterThanFilter2, dateGreaterThanFilter3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter1);
                await manager.AddAsync(dateGreaterThanFilter2);
                await manager.AddAsync(dateGreaterThanFilter3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { dateGreaterThanFilter1.DateGreaterThanFilterID, dateGreaterThanFilter2.DateGreaterThanFilterID, dateGreaterThanFilter3.DateGreaterThanFilterID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingDateGreaterThanFilter_ShouldReturnCorrectDateGreaterThanFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilterToAdd = await CreateTestDateGreaterThanFilter(context);
                await manager.AddAsync(dateGreaterThanFilterToAdd);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilterToAdd);
                //await context.SaveChangesAsync();
                var fetchedDateGreaterThanFilter = await manager.GetByIdAsync(dateGreaterThanFilterToAdd.DateGreaterThanFilterID);
                Assert.IsNotNull(fetchedDateGreaterThanFilter);
                Assert.AreEqual(dateGreaterThanFilterToAdd.DateGreaterThanFilterID, fetchedDateGreaterThanFilter.DateGreaterThanFilterID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingDateGreaterThanFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var fetchedDateGreaterThanFilter = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedDateGreaterThanFilter);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingDateGreaterThanFilter_ShouldReturnCorrectDateGreaterThanFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilterToAdd = await CreateTestDateGreaterThanFilter(context);
                await manager.AddAsync(dateGreaterThanFilterToAdd);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilterToAdd);
                //await context.SaveChangesAsync();
                var fetchedDateGreaterThanFilter = await manager.GetByCodeAsync(dateGreaterThanFilterToAdd.Code.Value);
                Assert.IsNotNull(fetchedDateGreaterThanFilter);
                Assert.AreEqual(dateGreaterThanFilterToAdd.Code, fetchedDateGreaterThanFilter.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingDateGreaterThanFilter_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var fetchedDateGreaterThanFilter = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedDateGreaterThanFilter);
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
                var manager = new DateGreaterThanFilterManager(context);
                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleDateGreaterThanFilters_ShouldReturnAllDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter1 = await CreateTestDateGreaterThanFilter(context);
                var dateGreaterThanFilter2 = await CreateTestDateGreaterThanFilter(context);
                var dateGreaterThanFilter3 = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.AddRange(dateGreaterThanFilter1, dateGreaterThanFilter2, dateGreaterThanFilter3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter1);
                await manager.AddAsync(dateGreaterThanFilter2);
                await manager.AddAsync(dateGreaterThanFilter3);
                var fetchedDateGreaterThanFilters = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedDateGreaterThanFilters);
                Assert.AreEqual(3, fetchedDateGreaterThanFilters.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var fetchedDateGreaterThanFilters = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedDateGreaterThanFilters);
                Assert.AreEqual(0, fetchedDateGreaterThanFilters.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidDateGreaterThanFilter_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter);
                dateGreaterThanFilter.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(dateGreaterThanFilter);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(dateGreaterThanFilter.Code, context.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                //var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(dateGreaterThanFilter).State = EntityState.Modified;
                //dateGreaterThanFilter.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(dateGreaterThanFilter);
                //Assert.IsFalse(updateResult);
                // Arrange
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                await manager.AddAsync(dateGreaterThanFilter);
                var firstInstance = await manager.GetByIdAsync(dateGreaterThanFilter.DateGreaterThanFilterID);
                var secondInstance = await manager.GetByIdAsync(dateGreaterThanFilter.DateGreaterThanFilterID);
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
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    dateGreaterThanFilter.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(dateGreaterThanFilter);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDateGreaterThanFilter = freshContext.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID);
                    Assert.AreNotEqual(dateGreaterThanFilter.Code, freshDateGreaterThanFilter.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteDateGreaterThanFilter()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter);
                var deleteResult = await manager.DeleteAsync(dateGreaterThanFilter.DateGreaterThanFilterID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
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
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(dateGreaterThanFilter.DateGreaterThanFilterID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshDateGreaterThanFilter = freshContext.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID);
                    Assert.IsNotNull(freshDateGreaterThanFilter);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidDateGreaterThanFilters_ShouldInsertAllDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                await manager.BulkInsertAsync(dateGreaterThanFilters);
                Assert.AreEqual(dateGreaterThanFilters.Count, context.DateGreaterThanFilterSet.Count());
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    Assert.IsNotNull(context.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID));
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
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(dateGreaterThanFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.DateGreaterThanFilterSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                // Add initial dateGreaterThanFilters
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                var dateGreaterThanFiltersToUpdate = new List<DateGreaterThanFilter>();
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    dateGreaterThanFiltersToUpdate.Add(await manager.AddAsync(dateGreaterThanFilter));
                }
                // Update dateGreaterThanFilters
                foreach (var dateGreaterThanFilter in dateGreaterThanFiltersToUpdate)
                {
                    dateGreaterThanFilter.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(dateGreaterThanFiltersToUpdate);
                // Verify updates
                foreach (var updatedDateGreaterThanFilter in dateGreaterThanFiltersToUpdate)
                {
                    var dateGreaterThanFilterFromDb = await manager.GetByIdAsync(updatedDateGreaterThanFilter.DateGreaterThanFilterID);// context.DateGreaterThanFilterSet.Find(updatedDateGreaterThanFilter.DateGreaterThanFilterID);
                    Assert.AreEqual(updatedDateGreaterThanFilter.Code, dateGreaterThanFilterFromDb.Code);
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
        //        var manager = new DateGreaterThanFilterManager(context);
        //        var dateGreaterThanFilters = new List<DateGreaterThanFilter>
        //        {
        //            await CreateTestDateGreaterThanFilter(context),
        //            await CreateTestDateGreaterThanFilter(context),
        //            await CreateTestDateGreaterThanFilter(context)
        //        };
        //        foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
        //        {
        //            await manager.AddAsync(dateGreaterThanFilter);
        //        }
        //        foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
        //        {
        //            dateGreaterThanFilter.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(dateGreaterThanFilters);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    await manager.AddAsync(dateGreaterThanFilter);
                }
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    dateGreaterThanFilter.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(dateGreaterThanFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                    {
                        var dateGreaterThanFilterFromDb = freshContext.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID);
                        Assert.AreNotEqual(dateGreaterThanFilter.Code, dateGreaterThanFilterFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                // Add initial dateGreaterThanFilters
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    await manager.AddAsync(dateGreaterThanFilter);
                }
                // Delete dateGreaterThanFilters
                await manager.BulkDeleteAsync(dateGreaterThanFilters);
                // Verify deletions
                foreach (var deletedDateGreaterThanFilter in dateGreaterThanFilters)
                {
                    var dateGreaterThanFilterFromDb = context.DateGreaterThanFilterSet.Find(deletedDateGreaterThanFilter.DateGreaterThanFilterID);
                    Assert.IsNull(dateGreaterThanFilterFromDb);
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
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    await manager.AddAsync(dateGreaterThanFilter);
                }
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    dateGreaterThanFilter.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(dateGreaterThanFilters);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilters = new List<DateGreaterThanFilter>
                {
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context),
                    await CreateTestDateGreaterThanFilter(context)
                };
                foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                {
                    await manager.AddAsync(dateGreaterThanFilter);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(dateGreaterThanFilters);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var dateGreaterThanFilter in dateGreaterThanFilters)
                    {
                        var dateGreaterThanFilterFromDb = freshContext.DateGreaterThanFilterSet.Find(dateGreaterThanFilter.DateGreaterThanFilterID);
                        Assert.IsNotNull(dateGreaterThanFilterFromDb);  // DateGreaterThanFilter should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        //ENDSET
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter = await CreateTestDateGreaterThanFilter(context);
                //dateGreaterThanFilter.PacID = 1;
                //context.DateGreaterThanFilterSet.Add(dateGreaterThanFilter);
                //await context.SaveChangesAsync();
                await manager.AddAsync(dateGreaterThanFilter);
                var result = await manager.GetByPacAsync(dateGreaterThanFilter.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(dateGreaterThanFilter.DateGreaterThanFilterID, result.First().DateGreaterThanFilterID);
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
                var manager = new DateGreaterThanFilterManager(context);
                var result = await manager.GetByPacAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleDateGreaterThanFiltersSamePacId_ShouldReturnAllDateGreaterThanFilters()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new DateGreaterThanFilterManager(context);
                var dateGreaterThanFilter1 = await CreateTestDateGreaterThanFilter(context);
                var dateGreaterThanFilter2 = await CreateTestDateGreaterThanFilter(context);
                dateGreaterThanFilter2.PacID = dateGreaterThanFilter1.PacID;
                await manager.AddAsync(dateGreaterThanFilter1);
                await manager.AddAsync(dateGreaterThanFilter2);
                //context.DateGreaterThanFilterSet.AddRange(dateGreaterThanFilter1, dateGreaterThanFilter2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacAsync(dateGreaterThanFilter1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        private async Task<DateGreaterThanFilter> CreateTestDateGreaterThanFilter(FarmDbContext dbContext)
        {
            return await DateGreaterThanFilterFactory.CreateAsync(dbContext);
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
