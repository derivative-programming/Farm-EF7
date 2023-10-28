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
    public partial class OrgApiKeyTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                var result = await manager.AddAsync(orgApiKey);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.OrgApiKeySet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                var result = manager.Add(orgApiKey);
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
                    var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                    var result = await manager.AddAsync(orgApiKey);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.OrgApiKeySet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var orgApiKey = CreateTestOrgApiKey(context);
                    var result = manager.Add(orgApiKey);
                    transaction.Commit();
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
        public void GetTotalCount_NoOrgApiKeys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = manager.GetTotalCount();
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
                await manager.AddAsync(await CreateTestOrgApiKeyAsync(context));
                await manager.AddAsync(await CreateTestOrgApiKeyAsync(context));
                await manager.AddAsync(await CreateTestOrgApiKeyAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithOrgApiKeys_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add some orgApiKeys
                manager.Add(CreateTestOrgApiKey(context));
                manager.Add(CreateTestOrgApiKey(context));
                manager.Add(CreateTestOrgApiKey(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoOrgApiKeys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoOrgApiKeys_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = manager.GetMaxId();
                Assert.AreEqual(0, result);
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
                var orgApiKey1 = await CreateTestOrgApiKeyAsync(context);
                var orgApiKey2 = await CreateTestOrgApiKeyAsync(context);
                var orgApiKey3 = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                await manager.AddAsync(orgApiKey3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { orgApiKey1.OrgApiKeyID, orgApiKey2.OrgApiKeyID, orgApiKey3.OrgApiKeyID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithOrgApiKeys_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add some orgApiKeys
                var orgApiKey1 = CreateTestOrgApiKey(context);
                var orgApiKey2 = CreateTestOrgApiKey(context);
                var orgApiKey3 = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey1);
                manager.Add(orgApiKey2);
                manager.Add(orgApiKey3);
                var result = manager.GetMaxId();
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
                var orgApiKeyToAdd = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKeyToAdd);
                var fetchedOrgApiKey = await manager.GetByIdAsync(orgApiKeyToAdd.OrgApiKeyID);
                Assert.IsNotNull(fetchedOrgApiKey);
                Assert.AreEqual(orgApiKeyToAdd.OrgApiKeyID, fetchedOrgApiKey.OrgApiKeyID);
            }
        }
        [TestMethod]
        public void GetById_ExistingOrgApiKey_ShouldReturnCorrectOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeyToAdd = CreateTestOrgApiKey(context);
                manager.Add(orgApiKeyToAdd);
                var fetchedOrgApiKey = manager.GetById(orgApiKeyToAdd.OrgApiKeyID);
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
        public void GetById_NonExistingOrgApiKey_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKey = manager.GetById(999); // Assuming 999 is a non-existing ID
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
                var orgApiKeyToAdd = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKeyToAdd);
                var fetchedOrgApiKey = await manager.GetByCodeAsync(orgApiKeyToAdd.Code.Value);
                Assert.IsNotNull(fetchedOrgApiKey);
                Assert.AreEqual(orgApiKeyToAdd.Code, fetchedOrgApiKey.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingOrgApiKey_ShouldReturnCorrectOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeyToAdd = CreateTestOrgApiKey(context);
                manager.Add(orgApiKeyToAdd);
                var fetchedOrgApiKey = manager.GetByCode(orgApiKeyToAdd.Code.Value);
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
        public void GetByCode_NonExistingOrgApiKey_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKey = manager.GetByCode(Guid.NewGuid()); // Random new GUID
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
        [ExpectedException(typeof(ArgumentException))]
        public void GetByCode_EmptyGuid_ShouldThrowArgumentException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                manager.GetByCode(Guid.Empty);
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
                var orgApiKey1 = await CreateTestOrgApiKeyAsync(context);
                var orgApiKey2 = await CreateTestOrgApiKeyAsync(context);
                var orgApiKey3 = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                await manager.AddAsync(orgApiKey3);
                var fetchedOrgApiKeys = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrgApiKeys);
                Assert.AreEqual(3, fetchedOrgApiKeys.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleOrgApiKeys_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = CreateTestOrgApiKey(context);
                var orgApiKey2 = CreateTestOrgApiKey(context);
                var orgApiKey3 = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey1);
                manager.Add(orgApiKey2);
                manager.Add(orgApiKey3);
                var fetchedOrgApiKeys = manager.GetAll();
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
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var fetchedOrgApiKeys = manager.GetAll();
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
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKey);
                orgApiKey.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(orgApiKey);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(orgApiKey.Code, context.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidOrgApiKey_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey);
                orgApiKey.Code = Guid.NewGuid();
                var updateResult = manager.Update(orgApiKey);
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
                // Arrange
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
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
        public void Update_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Arrange
                var orgApiKey = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey);
                var firstInstance = manager.GetById(orgApiKey.OrgApiKeyID);
                var secondInstance = manager.GetById(orgApiKey.OrgApiKeyID);
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
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
        public void Update_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                //context.OrgApiKeySet.Add(orgApiKey);
                //context.SaveChanges();
                manager.Add(orgApiKey);
                using (var transaction = context.Database.BeginTransaction())
                {
                    orgApiKey.Code = Guid.NewGuid();
                    var updateResult = manager.Update(orgApiKey);
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
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                await manager.AddAsync(orgApiKey);
                var deleteResult = await manager.DeleteAsync(orgApiKey.OrgApiKeyID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.OrgApiKeySet.Find(orgApiKey.OrgApiKeyID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteOrgApiKey()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey);
                var deleteResult = manager.Delete(orgApiKey.OrgApiKeyID);
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
        public void Delete_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
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
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
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
        public void Delete_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                manager.Add(orgApiKey);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(orgApiKey.OrgApiKeyID);
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        public void BulkInsert_ValidOrgApiKeys_ShouldInsertAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                manager.BulkInsert(orgApiKeys);
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        public void BulkInsert_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(orgApiKeys);
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add initial orgApiKeys
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                var orgApiKeysToUpdate = new List<OrgApiKey>();
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKeysToUpdate.Add(manager.Add(orgApiKey));
                }
                // Update orgApiKeys
                foreach (var orgApiKey in orgApiKeysToUpdate)
                {
                    orgApiKey.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(orgApiKeysToUpdate);
                // Verify updates
                foreach (var updatedOrgApiKey in orgApiKeysToUpdate)
                {
                    var orgApiKeyFromDb = manager.GetById(updatedOrgApiKey.OrgApiKeyID);// context.OrgApiKeySet.Find(updatedOrgApiKey.OrgApiKeyID);
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
        //            await CreateTestOrgApiKeyAsync(context),
        //            await CreateTestOrgApiKeyAsync(context),
        //            await CreateTestOrgApiKeyAsync(context)
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        public void BulkUpdate_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    manager.Add(orgApiKey);
                }
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKey.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(orgApiKeys);
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        public void BulkDelete_ValidDeletes_ShouldDeleteAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                // Add initial orgApiKeys
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    manager.Add(orgApiKey);
                }
                // Delete orgApiKeys
                manager.BulkDelete(orgApiKeys);
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void BulkDelete_ConcurrencyMismatch_ShouldThrowConcurrencyException()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    manager.Add(orgApiKey);
                }
                foreach (var orgApiKey in orgApiKeys)
                {
                    orgApiKey.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(orgApiKeys);  // This should throw a concurrency exception due to token mismatch
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
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context),
                    await CreateTestOrgApiKeyAsync(context)
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
        [TestMethod]
        public void BulkDelete_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKeys = new List<OrgApiKey>
                {
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context),
                    CreateTestOrgApiKey(context)
                };
                foreach (var orgApiKey in orgApiKeys)
                {
                    manager.Add(orgApiKey);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(orgApiKeys);
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
        [TestMethod]//OrganizationID
        public async Task GetByOrganizationIdAsync_ValidOrganizationId_ShouldReturnOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                //orgApiKey.OrganizationID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                var result = await manager.GetByOrganizationIDAsync(orgApiKey.OrganizationID.Value);
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
                var orgApiKey = await CreateTestOrgApiKeyAsync(context);
                // orgApiKey.OrgCustomerID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey);
                var result = await manager.GetByOrgCustomerIDAsync(orgApiKey.OrgCustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgApiKey.OrgApiKeyID, result.First().OrgApiKeyID);
            }
        }
        [TestMethod]//OrganizationID
        public void GetByOrganizationId_ValidOrganizationId_ShouldReturnOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                //orgApiKey.OrganizationID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //context.SaveChanges();
                manager.Add(orgApiKey);
                var result = manager.GetByOrganizationID(orgApiKey.OrganizationID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgApiKey.OrgApiKeyID, result.First().OrgApiKeyID);
            }
        }
        [TestMethod]//OrgCustomerID
        public void GetByOrgCustomer_ValidOrgCustomerID_ShouldReturnOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey = CreateTestOrgApiKey(context);
                // orgApiKey.OrgCustomerID = 1;
                //context.OrgApiKeySet.Add(orgApiKey);
                //context.SaveChanges();
                manager.Add(orgApiKey);
                var result = manager.GetByOrgCustomerID(orgApiKey.OrgCustomerID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(orgApiKey.OrgApiKeyID, result.First().OrgApiKeyID);
            }
        }
        [TestMethod] //OrganizationID
        public async Task GetByOrganizationIdAsync_InvalidOrganizationId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = await manager.GetByOrganizationIDAsync(100);  // ID 100 is not added to the database
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
                var result = await manager.GetByOrgCustomerIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //OrganizationID
        public void GetByOrganizationId_InvalidOrganizationId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = manager.GetByOrganizationID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]//OrgCustomerID
        public void GetByOrgCustomer_InvalidOrgCustomerID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var result = manager.GetByOrgCustomerID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //OrganizationID
        public async Task GetByOrganizationIdAsync_MultipleOrgApiKeysSameOrganizationId_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = await CreateTestOrgApiKeyAsync(context);
                var orgApiKey2 = await CreateTestOrgApiKeyAsync(context);
                orgApiKey2.OrganizationID = orgApiKey1.OrganizationID;
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByOrganizationIDAsync(orgApiKey1.OrganizationID.Value);
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
                var orgApiKey1 = await CreateTestOrgApiKeyAsync(context);
                //  orgApiKey1.OrgCustomerID = 1;
                var orgApiKey2 = await CreateTestOrgApiKeyAsync(context);
                orgApiKey2.OrgCustomerID = orgApiKey1.OrgCustomerID;
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //await context.SaveChangesAsync();
                await manager.AddAsync(orgApiKey1);
                await manager.AddAsync(orgApiKey2);
                var result = await manager.GetByOrgCustomerIDAsync(orgApiKey1.OrgCustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //OrganizationID
        public void GetByOrganizationId_MultipleOrgApiKeysSameOrganizationId_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = CreateTestOrgApiKey(context);
                var orgApiKey2 = CreateTestOrgApiKey(context);
                orgApiKey2.OrganizationID = orgApiKey1.OrganizationID;
                manager.Add(orgApiKey1);
                manager.Add(orgApiKey2);
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //context.SaveChanges();
                var result = manager.GetByOrganizationID(orgApiKey1.OrganizationID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //OrgCustomerID
        public void GetByOrgCustomer_MultipleOrgApiKeysSameOrgCustomerID_ShouldReturnAllOrgApiKeys()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrgApiKeyManager(context);
                var orgApiKey1 = CreateTestOrgApiKey(context);
                //  orgApiKey1.OrgCustomerID = 1;
                var orgApiKey2 = CreateTestOrgApiKey(context);
                orgApiKey2.OrgCustomerID = orgApiKey1.OrgCustomerID;
                //context.OrgApiKeySet.AddRange(orgApiKey1, orgApiKey2);
                //context.SaveChanges();
                manager.Add(orgApiKey1);
                manager.Add(orgApiKey2);
                var result = manager.GetByOrgCustomerID(orgApiKey1.OrgCustomerID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<OrgApiKey> CreateTestOrgApiKeyAsync(FarmDbContext dbContext)
        {
            return await OrgApiKeyFactory.CreateAsync(dbContext);
        }
        private OrgApiKey CreateTestOrgApiKey(FarmDbContext dbContext)
        {
            return OrgApiKeyFactory.Create(dbContext);
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
