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
    public class OrgCustomerTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddOrgCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                var result = await manager.AddAsync(orgCustomer);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.OrgCustomerSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddOrgCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var orgCustomer = await CreateTestOrgCustomer(context);
                    var result = await manager.AddAsync(orgCustomer);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.OrgCustomerSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoOrgCustomers_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithOrgCustomers_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                // Add some orgCustomers
                await manager.AddAsync(await CreateTestOrgCustomer(context));
                await manager.AddAsync(await CreateTestOrgCustomer(context));
                await manager.AddAsync(await CreateTestOrgCustomer(context));
                //// Add some orgCustomers
                //context.OrgCustomerSet.AddRange(
                //    CreateTestOrgCustomer(context),
                //    CreateTestOrgCustomer(context),
                //    CreateTestOrgCustomer(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoOrgCustomers_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithOrgCustomers_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                // Add some orgCustomers
                var orgCustomer1 = await CreateTestOrgCustomer(context);
                var orgCustomer2 = await CreateTestOrgCustomer(context);
                var orgCustomer3 = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.AddRange(orgCustomer1, orgCustomer2, orgCustomer3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer1);
                await manager.AddAsync(orgCustomer2);
                await manager.AddAsync(orgCustomer3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { orgCustomer1.OrgCustomerID, orgCustomer2.OrgCustomerID, orgCustomer3.OrgCustomerID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingOrgCustomer_ShouldReturnCorrectOrgCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomerToAdd = await CreateTestOrgCustomer(context);
                await manager.AddAsync(orgCustomerToAdd);
                //context.OrgCustomerSet.Add(orgCustomerToAdd);
                //await context.SaveChangesAsync();
                var fetchedOrgCustomer = await manager.GetByIdAsync(orgCustomerToAdd.OrgCustomerID);
                Assert.IsNotNull(fetchedOrgCustomer);
                Assert.AreEqual(orgCustomerToAdd.OrgCustomerID, fetchedOrgCustomer.OrgCustomerID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingOrgCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var fetchedOrgCustomer = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedOrgCustomer);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingOrgCustomer_ShouldReturnCorrectOrgCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomerToAdd = await CreateTestOrgCustomer(context);
                await manager.AddAsync(orgCustomerToAdd);
                //context.OrgCustomerSet.Add(orgCustomerToAdd);
                //await context.SaveChangesAsync();
                var fetchedOrgCustomer = await manager.GetByCodeAsync(orgCustomerToAdd.Code.Value);
                Assert.IsNotNull(fetchedOrgCustomer);
                Assert.AreEqual(orgCustomerToAdd.Code, fetchedOrgCustomer.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingOrgCustomer_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var fetchedOrgCustomer = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedOrgCustomer);
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
                var manager = new OrgCustomerManager(context);
                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleOrgCustomers_ShouldReturnAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer1 = await CreateTestOrgCustomer(context);
                var orgCustomer2 = await CreateTestOrgCustomer(context);
                var orgCustomer3 = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.AddRange(orgCustomer1, orgCustomer2, orgCustomer3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer1);
                await manager.AddAsync(orgCustomer2);
                await manager.AddAsync(orgCustomer3);
                var fetchedOrgCustomers = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrgCustomers);
                Assert.AreEqual(3, fetchedOrgCustomers.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var fetchedOrgCustomers = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrgCustomers);
                Assert.AreEqual(0, fetchedOrgCustomers.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidOrgCustomer_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                orgCustomer.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(orgCustomer);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(orgCustomer.Code, context.OrgCustomerSet.Find(orgCustomer.OrgCustomerID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                //var orgCustomer = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(orgCustomer).State = EntityState.Modified;
                //orgCustomer.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(orgCustomer);
                //Assert.IsFalse(updateResult);
                // Arrange
                var orgCustomer = await CreateTestOrgCustomer(context);
                await manager.AddAsync(orgCustomer);
                var firstInstance = await manager.GetByIdAsync(orgCustomer.OrgCustomerID);
                var secondInstance = await manager.GetByIdAsync(orgCustomer.OrgCustomerID);
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
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    orgCustomer.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(orgCustomer);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrgCustomer = freshContext.OrgCustomerSet.Find(orgCustomer.OrgCustomerID);
                    Assert.AreNotEqual(orgCustomer.Code, freshOrgCustomer.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteOrgCustomer()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                var deleteResult = await manager.DeleteAsync(orgCustomer.OrgCustomerID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.OrgCustomerSet.Find(orgCustomer.OrgCustomerID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
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
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(orgCustomer.OrgCustomerID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrgCustomer = freshContext.OrgCustomerSet.Find(orgCustomer.OrgCustomerID);
                    Assert.IsNotNull(freshOrgCustomer);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidOrgCustomers_ShouldInsertAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                await manager.BulkInsertAsync(orgCustomers);
                Assert.AreEqual(orgCustomers.Count, context.OrgCustomerSet.Count());
                foreach (var orgCustomer in orgCustomers)
                {
                    Assert.IsNotNull(context.OrgCustomerSet.Find(orgCustomer.OrgCustomerID));
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
                var manager = new OrgCustomerManager(context);
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(orgCustomers);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.OrgCustomerSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                // Add initial orgCustomers
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                var orgCustomersToUpdate = new List<OrgCustomer>();
                foreach (var orgCustomer in orgCustomers)
                {
                    orgCustomersToUpdate.Add(await manager.AddAsync(orgCustomer));
                }
                // Update orgCustomers
                foreach (var orgCustomer in orgCustomersToUpdate)
                {
                    orgCustomer.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(orgCustomersToUpdate);
                // Verify updates
                foreach (var updatedOrgCustomer in orgCustomersToUpdate)
                {
                    var orgCustomerFromDb = await manager.GetByIdAsync(updatedOrgCustomer.OrgCustomerID);// context.OrgCustomerSet.Find(updatedOrgCustomer.OrgCustomerID);
                    Assert.AreEqual(updatedOrgCustomer.Code, orgCustomerFromDb.Code);
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
        //        var manager = new OrgCustomerManager(context);
        //        var orgCustomers = new List<OrgCustomer>
        //        {
        //            await CreateTestOrgCustomer(context),
        //            await CreateTestOrgCustomer(context),
        //            await CreateTestOrgCustomer(context)
        //        };
        //        foreach (var orgCustomer in orgCustomers)
        //        {
        //            await manager.AddAsync(orgCustomer);
        //        }
        //        foreach (var orgCustomer in orgCustomers)
        //        {
        //            orgCustomer.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(orgCustomers);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                foreach (var orgCustomer in orgCustomers)
                {
                    await manager.AddAsync(orgCustomer);
                }
                foreach (var orgCustomer in orgCustomers)
                {
                    orgCustomer.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(orgCustomers);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var orgCustomer in orgCustomers)
                    {
                        var orgCustomerFromDb = freshContext.OrgCustomerSet.Find(orgCustomer.OrgCustomerID);
                        Assert.AreNotEqual(orgCustomer.Code, orgCustomerFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                // Add initial orgCustomers
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                foreach (var orgCustomer in orgCustomers)
                {
                    await manager.AddAsync(orgCustomer);
                }
                // Delete orgCustomers
                await manager.BulkDeleteAsync(orgCustomers);
                // Verify deletions
                foreach (var deletedOrgCustomer in orgCustomers)
                {
                    var orgCustomerFromDb = context.OrgCustomerSet.Find(deletedOrgCustomer.OrgCustomerID);
                    Assert.IsNull(orgCustomerFromDb);
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
                var manager = new OrgCustomerManager(context);
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                foreach (var orgCustomer in orgCustomers)
                {
                    await manager.AddAsync(orgCustomer);
                }
                foreach (var orgCustomer in orgCustomers)
                {
                    orgCustomer.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(orgCustomers);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomers = new List<OrgCustomer>
                {
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context),
                    await CreateTestOrgCustomer(context)
                };
                foreach (var orgCustomer in orgCustomers)
                {
                    await manager.AddAsync(orgCustomer);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(orgCustomers);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var orgCustomer in orgCustomers)
                    {
                        var orgCustomerFromDb = freshContext.OrgCustomerSet.Find(orgCustomer.OrgCustomerID);
                        Assert.IsNotNull(orgCustomerFromDb);  // OrgCustomer should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//CustomerID
        public async Task GetByCustomerAsync_ValidCustomerID_ShouldReturnOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                // orgCustomer.CustomerID = 1;
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                var result = await manager.GetByCustomerAsync(orgCustomer.CustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgCustomer.OrgCustomerID, result.First().OrgCustomerID);
            }
        }
        //ENDSET
        [TestMethod]//OrganizationID
        public async Task GetByOrganizationIdAsync_ValidOrganizationId_ShouldReturnOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer = await CreateTestOrgCustomer(context);
                //orgCustomer.OrganizationID = 1;
                //context.OrgCustomerSet.Add(orgCustomer);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer);
                var result = await manager.GetByOrganizationAsync(orgCustomer.OrganizationID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgCustomer.OrgCustomerID, result.First().OrgCustomerID);
            }
        }
        [TestMethod]//CustomerID
        public async Task GetByCustomerAsync_InvalidCustomerID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var result = await manager.GetByCustomerAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //OrganizationID
        public async Task GetByOrganizationIdAsync_InvalidOrganizationId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var result = await manager.GetByOrganizationAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //CustomerID
        public async Task GetByCustomerAsync_MultipleOrgCustomersSameCustomerID_ShouldReturnAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer1 = await CreateTestOrgCustomer(context);
                //  orgCustomer1.CustomerID = 1;
                var orgCustomer2 = await CreateTestOrgCustomer(context);
                orgCustomer2.CustomerID = orgCustomer1.CustomerID;
                //context.OrgCustomerSet.AddRange(orgCustomer1, orgCustomer2);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgCustomer1);
                await manager.AddAsync(orgCustomer2);
                var result = await manager.GetByCustomerAsync(orgCustomer1.CustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //OrganizationID
        public async Task GetByOrganizationIdAsync_MultipleOrgCustomersSameOrganizationId_ShouldReturnAllOrgCustomers()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgCustomerManager(context);
                var orgCustomer1 = await CreateTestOrgCustomer(context);
                var orgCustomer2 = await CreateTestOrgCustomer(context);
                orgCustomer2.OrganizationID = orgCustomer1.OrganizationID;
                await manager.AddAsync(orgCustomer1);
                await manager.AddAsync(orgCustomer2);
                //context.OrgCustomerSet.AddRange(orgCustomer1, orgCustomer2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByOrganizationAsync(orgCustomer1.OrganizationID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        private async Task<OrgCustomer> CreateTestOrgCustomer(FarmDbContext dbContext)
        {
            return await OrgCustomerFactory.CreateAsync(dbContext);
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
