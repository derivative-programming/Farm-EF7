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
    public class TacTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddTac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                var result = await manager.AddAsync(tac);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.TacSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddTac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var tac = await CreateTestTac(context);
                    var result = await manager.AddAsync(tac);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.TacSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoTacs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithTacs_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                // Add some tacs
                await manager.AddAsync(await CreateTestTac(context));
                await manager.AddAsync(await CreateTestTac(context));
                await manager.AddAsync(await CreateTestTac(context));
                //// Add some tacs
                //context.TacSet.AddRange(
                //    CreateTestTac(context),
                //    CreateTestTac(context),
                //    CreateTestTac(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoTacs_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithTacs_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                // Add some tacs
                var tac1 = await CreateTestTac(context);
                var tac2 = await CreateTestTac(context);
                var tac3 = await CreateTestTac(context);
                //context.TacSet.AddRange(tac1, tac2, tac3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac1);
                await manager.AddAsync(tac2);
                await manager.AddAsync(tac3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { tac1.TacID, tac2.TacID, tac3.TacID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingTac_ShouldReturnCorrectTac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tacToAdd = await CreateTestTac(context);
                await manager.AddAsync(tacToAdd);
                //context.TacSet.Add(tacToAdd);
                //await context.SaveChangesAsync();
                var fetchedTac = await manager.GetByIdAsync(tacToAdd.TacID);
                Assert.IsNotNull(fetchedTac);
                Assert.AreEqual(tacToAdd.TacID, fetchedTac.TacID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingTac_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var fetchedTac = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedTac);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingTac_ShouldReturnCorrectTac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tacToAdd = await CreateTestTac(context);
                await manager.AddAsync(tacToAdd);
                //context.TacSet.Add(tacToAdd);
                //await context.SaveChangesAsync();
                var fetchedTac = await manager.GetByCodeAsync(tacToAdd.Code.Value);
                Assert.IsNotNull(fetchedTac);
                Assert.AreEqual(tacToAdd.Code, fetchedTac.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingTac_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var fetchedTac = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedTac);
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
                var manager = new TacManager(context);
                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleTacs_ShouldReturnAllTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac1 = await CreateTestTac(context);
                var tac2 = await CreateTestTac(context);
                var tac3 = await CreateTestTac(context);
                //context.TacSet.AddRange(tac1, tac2, tac3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac1);
                await manager.AddAsync(tac2);
                await manager.AddAsync(tac3);
                var fetchedTacs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedTacs);
                Assert.AreEqual(3, fetchedTacs.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var fetchedTacs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedTacs);
                Assert.AreEqual(0, fetchedTacs.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidTac_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac);
                tac.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(tac);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(tac.Code, context.TacSet.Find(tac.TacID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                //var tac = await CreateTestTac(context);
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(tac).State = EntityState.Modified;
                //tac.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(tac);
                //Assert.IsFalse(updateResult);
                // Arrange
                var tac = await CreateTestTac(context);
                await manager.AddAsync(tac);
                var firstInstance = await manager.GetByIdAsync(tac.TacID);
                var secondInstance = await manager.GetByIdAsync(tac.TacID);
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
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    tac.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(tac);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTac = freshContext.TacSet.Find(tac.TacID);
                    Assert.AreNotEqual(tac.Code, freshTac.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteTac()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac);
                var deleteResult = await manager.DeleteAsync(tac.TacID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.TacSet.Find(tac.TacID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
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
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(tac.TacID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshTac = freshContext.TacSet.Find(tac.TacID);
                    Assert.IsNotNull(freshTac);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidTacs_ShouldInsertAllTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                await manager.BulkInsertAsync(tacs);
                Assert.AreEqual(tacs.Count, context.TacSet.Count());
                foreach (var tac in tacs)
                {
                    Assert.IsNotNull(context.TacSet.Find(tac.TacID));
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
                var manager = new TacManager(context);
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(tacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.TacSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                // Add initial tacs
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                var tacsToUpdate = new List<Tac>();
                foreach (var tac in tacs)
                {
                    tacsToUpdate.Add(await manager.AddAsync(tac));
                }
                // Update tacs
                foreach (var tac in tacsToUpdate)
                {
                    tac.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(tacsToUpdate);
                // Verify updates
                foreach (var updatedTac in tacsToUpdate)
                {
                    var tacFromDb = await manager.GetByIdAsync(updatedTac.TacID);// context.TacSet.Find(updatedTac.TacID);
                    Assert.AreEqual(updatedTac.Code, tacFromDb.Code);
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
        //        var manager = new TacManager(context);
        //        var tacs = new List<Tac>
        //        {
        //            await CreateTestTac(context),
        //            await CreateTestTac(context),
        //            await CreateTestTac(context)
        //        };
        //        foreach (var tac in tacs)
        //        {
        //            await manager.AddAsync(tac);
        //        }
        //        foreach (var tac in tacs)
        //        {
        //            tac.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(tacs);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                foreach (var tac in tacs)
                {
                    await manager.AddAsync(tac);
                }
                foreach (var tac in tacs)
                {
                    tac.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(tacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var tac in tacs)
                    {
                        var tacFromDb = freshContext.TacSet.Find(tac.TacID);
                        Assert.AreNotEqual(tac.Code, tacFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                // Add initial tacs
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                foreach (var tac in tacs)
                {
                    await manager.AddAsync(tac);
                }
                // Delete tacs
                await manager.BulkDeleteAsync(tacs);
                // Verify deletions
                foreach (var deletedTac in tacs)
                {
                    var tacFromDb = context.TacSet.Find(deletedTac.TacID);
                    Assert.IsNull(tacFromDb);
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
                var manager = new TacManager(context);
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                foreach (var tac in tacs)
                {
                    await manager.AddAsync(tac);
                }
                foreach (var tac in tacs)
                {
                    tac.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(tacs);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tacs = new List<Tac>
                {
                    await CreateTestTac(context),
                    await CreateTestTac(context),
                    await CreateTestTac(context)
                };
                foreach (var tac in tacs)
                {
                    await manager.AddAsync(tac);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(tacs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var tac in tacs)
                    {
                        var tacFromDb = freshContext.TacSet.Find(tac.TacID);
                        Assert.IsNotNull(tacFromDb);  // Tac should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        //ENDSET
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac = await CreateTestTac(context);
                //tac.PacID = 1;
                //context.TacSet.Add(tac);
                //await context.SaveChangesAsync();
                await manager.AddAsync(tac);
                var result = await manager.GetByPacAsync(tac.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(tac.TacID, result.First().TacID);
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
                var manager = new TacManager(context);
                var result = await manager.GetByPacAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleTacsSamePacId_ShouldReturnAllTacs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new TacManager(context);
                var tac1 = await CreateTestTac(context);
                var tac2 = await CreateTestTac(context);
                tac2.PacID = tac1.PacID;
                await manager.AddAsync(tac1);
                await manager.AddAsync(tac2);
                //context.TacSet.AddRange(tac1, tac2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacAsync(tac1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        private async Task<Tac> CreateTestTac(FarmDbContext dbContext)
        {
            return await TacFactory.CreateAsync(dbContext);
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
