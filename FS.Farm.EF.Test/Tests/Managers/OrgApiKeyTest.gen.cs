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
    public class OrgApiKeyTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                var result = await manager.AddAsync(orgApiKey);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.OrgApiKeySet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var orgApiKey = await CreateTestOrgApiKey(context);
                    var result = await manager.AddAsync(orgApiKey);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.OrgApiKeySet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoOrgApiKeys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithOrgApiKeys_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add some orgApiKeys
                await manager.AddAsync(await CreateTestOrgApiKey(context));
                await manager.AddAsync(await CreateTestOrgApiKey(context));
                await manager.AddAsync(await CreateTestOrgApiKey(context));
                //// Add some orgApiKeys
                //context.OrgApiKeySet.AddRange(
                //    CreateTestOrgApiKey(context),
                //    CreateTestOrgApiKey(context),
                //    CreateTestOrgApiKey(context));
                //await context.SaveChangesAsync();
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoOrgApiKeys_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithOrgApiKeys_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add some orgApiKeys
                var orgApiKey1 = await CreateTestOrgApiKey(context);
                var orgApiKey2 = await CreateTestOrgApiKey(context);
                var orgApiKey3 = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2, orgApiKey3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                await manager.AddAsync(orgApiKey3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { orgApiKey1.OrgApiKeyID, orgApiKey2.OrgApiKeyID, orgApiKey3.OrgApiKeyID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingOrgApiKey_ShouldReturnCorrectOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeyToAdd = await CreateTestOrgApiKey(context);
                await manager.AddAsync(orgApiKeyToAdd);
                //context.OrgApiKeySet.Add(orgApiKeyToAdd);
                //await context.SaveChangesAsync();
                var fetchedOrgApiKey = await manager.GetByIdAsync(orgApiKeyToAdd.OrgApiKeyID);
                Assert.IsNotNull(fetchedOrgApiKey);
                Assert.AreEqual(orgApiKeyToAdd.OrgApiKeyID, fetchedOrgApiKey.OrgApiKeyID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingOrgApiKey_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKey = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedOrgApiKey);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingOrgApiKey_ShouldReturnCorrectOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeyToAdd = await CreateTestOrgApiKey(context);
                await manager.AddAsync(orgApiKeyToAdd);
                //context.OrgApiKeySet.Add(orgApiKeyToAdd);
                //await context.SaveChangesAsync();
                var fetchedOrgApiKey = await manager.GetByCodeAsync(orgApiKeyToAdd.Code.Value);
                Assert.IsNotNull(fetchedOrgApiKey);
                Assert.AreEqual(orgApiKeyToAdd.Code, fetchedOrgApiKey.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingOrgApiKey_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKey = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedOrgApiKey);
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
                var manager = new OrgApiKeyManager(context);
                await manager.GetByCodeAsync(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleOrgApiKeys_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = await CreateTestOrgApiKey(context);
                var orgApiKey2 = await CreateTestOrgApiKey(context);
                var orgApiKey3 = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2, orgApiKey3);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                await manager.AddAsync(orgApiKey3);
                var fetchedOrgApiKeys = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrgApiKeys);
                Assert.AreEqual(3, fetchedOrgApiKeys.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKeys = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrgApiKeys);
                Assert.AreEqual(0, fetchedOrgApiKeys.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidOrgApiKey_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                orgApiKey.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(orgApiKey);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(orgApiKey.Code, context.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                //var orgApiKey = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                //// Simulate concurrent update by changing entity state without saving
                //context.Entry(orgApiKey).State = EntityState.Modified;
                //orgApiKey.Code = Guid.NewGuid();
                //var updateResult = await manager.UpdateAsync(orgApiKey);
                //Assert.IsFalse(updateResult);
                // Arrange
                var orgApiKey = await CreateTestOrgApiKey(context);
                await manager.AddAsync(orgApiKey);
                var firstInstance = await manager.GetByIdAsync(orgApiKey.OrgApiKeyID);
                var secondInstance = await manager.GetByIdAsync(orgApiKey.OrgApiKeyID);
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    orgApiKey.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(orgApiKey);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrgApiKey = freshContext.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID);
                    Assert.AreNotEqual(orgApiKey.Code, freshOrgApiKey.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                var deleteResult = await manager.DeleteAsync(orgApiKey.OrgApiKeyID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(orgApiKey.OrgApiKeyID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrgApiKey = freshContext.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID);
                    Assert.IsNotNull(freshOrgApiKey);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidOrgApiKeys_ShouldInsertAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                await manager.BulkInsertAsync(orgApiKeys);
                Assert.AreEqual(orgApiKeys.Count, context.OrgApiKeySet.Count());
                foreach (var orgApiKey in orgApiKeys)
                {
                    Assert.IsNotNull(context.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID));
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(orgApiKeys);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.OrgApiKeySet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add initial orgApiKeys
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                var orgApiKeysToUpdate = new List<OrgApiKey>();
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKeysToUpdate.Add(await manager.AddAsync(orgApiKey));
                }
                // Update orgApiKeys
                foreach (var orgApiKey in orgApiKeysToUpdate)
                {
                    orgApiKey.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(orgApiKeysToUpdate);
                // Verify updates
                foreach (var updatedOrgApiKey in orgApiKeysToUpdate)
                {
                    var orgApiKeyFromDb = await manager.GetByIdAsync(updatedOrgApiKey.OrgApiKeyID);// context.OrgApiKeySet.Find(updatedOrgApiKey.OrgApiKeyID);
                    Assert.AreEqual(updatedOrgApiKey.Code, orgApiKeyFromDb.Code);
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
        //        var manager = new OrgApiKeyManager(context);
        //        var orgApiKeys = new List<OrgApiKey>
        //        {
        //            await CreateTestOrgApiKey(context),
        //            await CreateTestOrgApiKey(context),
        //            await CreateTestOrgApiKey(context)
        //        };
        //        foreach (var orgApiKey in orgApiKeys)
        //        {
        //            await manager.AddAsync(orgApiKey);
        //        }
        //        foreach (var orgApiKey in orgApiKeys)
        //        {
        //            orgApiKey.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(orgApiKeys);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    await manager.AddAsync(orgApiKey);
                }
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKey.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(orgApiKeys);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var orgApiKey in orgApiKeys)
                    {
                        var orgApiKeyFromDb = freshContext.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID);
                        Assert.AreNotEqual(orgApiKey.Code, orgApiKeyFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add initial orgApiKeys
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    await manager.AddAsync(orgApiKey);
                }
                // Delete orgApiKeys
                await manager.BulkDeleteAsync(orgApiKeys);
                // Verify deletions
                foreach (var deletedOrgApiKey in orgApiKeys)
                {
                    var orgApiKeyFromDb = context.OrgApiKeySet.Find(deletedOrgApiKey.OrgApiKeyID);
                    Assert.IsNull(orgApiKeyFromDb);
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    await manager.AddAsync(orgApiKey);
                }
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKey.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(orgApiKeys);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context),
                    await CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    await manager.AddAsync(orgApiKey);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(orgApiKeys);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var orgApiKey in orgApiKeys)
                    {
                        var orgApiKeyFromDb = freshContext.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID);
                        Assert.IsNotNull(orgApiKeyFromDb);  // OrgApiKey should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        //ENDSET
        [TestMethod]//OrganizationID
        public async Task GetByOrganizationIdAsync_ValidOrganizationId_ShouldReturnOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                //orgApiKey.OrganizationID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                var result = await manager.GetByOrganizationAsync(orgApiKey.OrganizationID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgApiKey.OrgApiKeyID, result.First().OrgApiKeyID);
            }
        }
        [TestMethod]//OrgCustomerID
        public async Task GetByOrgCustomerAsync_ValidOrgCustomerID_ShouldReturnOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKey(context);
                // orgApiKey.OrgCustomerID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                var result = await manager.GetByOrgCustomerAsync(orgApiKey.OrgCustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgApiKey.OrgApiKeyID, result.First().OrgApiKeyID);
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
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetByOrganizationAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//OrgCustomerID
        public async Task GetByOrgCustomerAsync_InvalidOrgCustomerID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetByOrgCustomerAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        //ENDSET
        [TestMethod] //OrganizationID
        public async Task GetByOrganizationIdAsync_MultipleOrgApiKeysSameOrganizationId_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = await CreateTestOrgApiKey(context);
                var orgApiKey2 = await CreateTestOrgApiKey(context);
                orgApiKey2.OrganizationID = orgApiKey1.OrganizationID;
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByOrganizationAsync(orgApiKey1.OrganizationID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //OrgCustomerID
        public async Task GetByOrgCustomerAsync_MultipleOrgApiKeysSameOrgCustomerID_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = await CreateTestOrgApiKey(context);
                //  orgApiKey1.OrgCustomerID = 1;
                var orgApiKey2 = await CreateTestOrgApiKey(context);
                orgApiKey2.OrgCustomerID = orgApiKey1.OrgCustomerID;
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                var result = await manager.GetByOrgCustomerAsync(orgApiKey1.OrgCustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        //ENDSET
        private async Task<OrgApiKey> CreateTestOrgApiKey(FarmDbContext dbContext)
        {
            return await OrgApiKeyFactory.CreateAsync(dbContext);
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
