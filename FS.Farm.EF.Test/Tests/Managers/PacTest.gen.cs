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
    public class PacTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddPac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pac = await CreateTestPac(context);
                var result = await manager.AddAsync(pac);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.PacSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddPac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var pac = await CreateTestPac(context);
                    var result = await manager.AddAsync(pac);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.PacSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoPacs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithPacs_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                // Add some pacs
                await manager.AddAsync(await CreateTestPac(context));
                await manager.AddAsync(await CreateTestPac(context));
                await manager.AddAsync(await CreateTestPac(context));
                //// Add some pacs
                //context.PacSet.AddRange(
                //    CreateTestPac(context),
                //    CreateTestPac(context),
                //    CreateTestPac(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoPacs_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithPacs_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                // Add some pacs
                var pac1 = await CreateTestPac(context);
                var pac2 = await CreateTestPac(context);
                var pac3 = await CreateTestPac(context);
                //context.PacSet.AddRange(pac1, pac2, pac3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac1);
                await manager.AddAsync(pac2);
                await manager.AddAsync(pac3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { pac1.PacID, pac2.PacID, pac3.PacID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingPac_ShouldReturnCorrectPac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pacToAdd = await CreateTestPac(context);
                await manager.AddAsync(pacToAdd);
                //context.PacSet.Add(pacToAdd);
                //await context.SaveChangesAsync();
                var fetchedPac = await manager.GetByIdAsync(pacToAdd.PacID);
                Assert.IsNotNull(fetchedPac);
                Assert.AreEqual(pacToAdd.PacID, fetchedPac.PacID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingPac_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var fetchedPac = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedPac);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingPac_ShouldReturnCorrectPac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pacToAdd = await CreateTestPac(context);
                await manager.AddAsync(pacToAdd);
                //context.PacSet.Add(pacToAdd);
                //await context.SaveChangesAsync();
                var fetchedPac = await manager.GetByCodeAsync(pacToAdd.Code.Value);
                Assert.IsNotNull(fetchedPac);
                Assert.AreEqual(pacToAdd.Code, fetchedPac.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingPac_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var fetchedPac = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedPac);
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
                var manager = new PacManager(context);
                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultiplePacs_ShouldReturnAllPacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pac1 = await CreateTestPac(context);
                var pac2 = await CreateTestPac(context);
                var pac3 = await CreateTestPac(context);
                //context.PacSet.AddRange(pac1, pac2, pac3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac1);
                await manager.AddAsync(pac2);
                await manager.AddAsync(pac3);
                var fetchedPacs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedPacs);
                Assert.AreEqual(3, fetchedPacs.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var fetchedPacs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedPacs);
                Assert.AreEqual(0, fetchedPacs.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidPac_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pac = await CreateTestPac(context);
                //context.PacSet.Add(pac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac);
                pac.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(pac);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(pac.Code, context.PacSet.Find(pac.PacID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                //var pac = await CreateTestPac(context);
                //context.PacSet.Add(pac);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(pac).State = EntityState.Modified;
                //pac.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(pac);
                //Assert.IsFalse(updateResult);
                // Arrange
                var pac = await CreateTestPac(context);
                await manager.AddAsync(pac);
                var firstInstance = await manager.GetByIdAsync(pac.PacID);
                var secondInstance = await manager.GetByIdAsync(pac.PacID);
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
                var manager = new PacManager(context);
                var pac = await CreateTestPac(context);
                //context.PacSet.Add(pac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    pac.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(pac);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPac = freshContext.PacSet.Find(pac.PacID);
                    Assert.AreNotEqual(pac.Code, freshPac.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeletePac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pac = await CreateTestPac(context);
                //context.PacSet.Add(pac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac);
                var deleteResult = await manager.DeleteAsync(pac.PacID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.PacSet.Find(pac.PacID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
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
                var manager = new PacManager(context);
                var pac = await CreateTestPac(context);
                //context.PacSet.Add(pac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(pac);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(pac.PacID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPac = freshContext.PacSet.Find(pac.PacID);
                    Assert.IsNotNull(freshPac);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidPacs_ShouldInsertAllPacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                await manager.BulkInsertAsync(pacs);
                Assert.AreEqual(pacs.Count, context.PacSet.Count());
                foreach (var pac in pacs)
                {
                    Assert.IsNotNull(context.PacSet.Find(pac.PacID));
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
                var manager = new PacManager(context);
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(pacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.PacSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllPacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                // Add initial pacs
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                var pacsToUpdate = new List<Pac>();
                foreach (var pac in pacs)
                {
                    pacsToUpdate.Add(await manager.AddAsync(pac));
                }
                // Update pacs
                foreach (var pac in pacsToUpdate)
                {
                    pac.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(pacsToUpdate);
                // Verify updates
                foreach (var updatedPac in pacsToUpdate)
                {
                    var pacFromDb = await manager.GetByIdAsync(updatedPac.PacID);// context.PacSet.Find(updatedPac.PacID);
                    Assert.AreEqual(updatedPac.Code, pacFromDb.Code);
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
        //        var manager = new PacManager(context);
        //        var pacs = new List<Pac>
        //        {
        //            await CreateTestPac(context),
        //            await CreateTestPac(context),
        //            await CreateTestPac(context)
        //        };
        //        foreach (var pac in pacs)
        //        {
        //            await manager.AddAsync(pac);
        //        }
        //        foreach (var pac in pacs)
        //        {
        //            pac.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(pacs);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                foreach (var pac in pacs)
                {
                    await manager.AddAsync(pac);
                }
                foreach (var pac in pacs)
                {
                    pac.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(pacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var pac in pacs)
                    {
                        var pacFromDb = freshContext.PacSet.Find(pac.PacID);
                        Assert.AreNotEqual(pac.Code, pacFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllPacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                // Add initial pacs
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                foreach (var pac in pacs)
                {
                    await manager.AddAsync(pac);
                }
                // Delete pacs
                await manager.BulkDeleteAsync(pacs);
                // Verify deletions
                foreach (var deletedPac in pacs)
                {
                    var pacFromDb = context.PacSet.Find(deletedPac.PacID);
                    Assert.IsNull(pacFromDb);
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
                var manager = new PacManager(context);
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                foreach (var pac in pacs)
                {
                    await manager.AddAsync(pac);
                }
                foreach (var pac in pacs)
                {
                    pac.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(pacs);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PacManager(context);
                var pacs = new List<Pac>
                {
                    await CreateTestPac(context),
                    await CreateTestPac(context),
                    await CreateTestPac(context)
                };
                foreach (var pac in pacs)
                {
                    await manager.AddAsync(pac);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(pacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var pac in pacs)
                    {
                        var pacFromDb = freshContext.PacSet.Find(pac.PacID);
                        Assert.IsNotNull(pacFromDb);  // Pac should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        //ENDSET
        private async Task<Pac> CreateTestPac(FarmDbContext dbContext)
        {
            return await PacFactory.CreateAsync(dbContext);
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
