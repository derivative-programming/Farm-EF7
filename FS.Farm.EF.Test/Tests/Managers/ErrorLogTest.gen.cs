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
    public partial class ErrorLogTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                var result = await manager.AddAsync(errorLog);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.ErrorLogSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                var result = manager.Add(errorLog);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.ErrorLogSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var errorLog = await CreateTestErrorLogAsync(context);
                    var result = await manager.AddAsync(errorLog);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.ErrorLogSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var errorLog = CreateTestErrorLog(context);
                    var result = manager.Add(errorLog);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.ErrorLogSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoErrorLogs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoErrorLogs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithErrorLogs_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add some errorLogs
                await manager.AddAsync(await CreateTestErrorLogAsync(context));
                await manager.AddAsync(await CreateTestErrorLogAsync(context));
                await manager.AddAsync(await CreateTestErrorLogAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithErrorLogs_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add some errorLogs
                manager.Add(CreateTestErrorLog(context));
                manager.Add(CreateTestErrorLog(context));
                manager.Add(CreateTestErrorLog(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoErrorLogs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoErrorLogs_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var result = manager.GetMaxId();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithErrorLogs_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add some errorLogs
                var errorLog1 = await CreateTestErrorLogAsync(context);
                var errorLog2 = await CreateTestErrorLogAsync(context);
                var errorLog3 = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog1);
                await manager.AddAsync(errorLog2);
                await manager.AddAsync(errorLog3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { errorLog1.ErrorLogID, errorLog2.ErrorLogID, errorLog3.ErrorLogID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithErrorLogs_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add some errorLogs
                var errorLog1 = CreateTestErrorLog(context);
                var errorLog2 = CreateTestErrorLog(context);
                var errorLog3 = CreateTestErrorLog(context);
                manager.Add(errorLog1);
                manager.Add(errorLog2);
                manager.Add(errorLog3);
                var result = manager.GetMaxId();
                var maxId = new[] { errorLog1.ErrorLogID, errorLog2.ErrorLogID, errorLog3.ErrorLogID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingErrorLog_ShouldReturnCorrectErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogToAdd = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLogToAdd);
                var fetchedErrorLog = await manager.GetByIdAsync(errorLogToAdd.ErrorLogID);
                Assert.IsNotNull(fetchedErrorLog);
                Assert.AreEqual(errorLogToAdd.ErrorLogID, fetchedErrorLog.ErrorLogID);
            }
        }
        [TestMethod]
        public void GetById_ExistingErrorLog_ShouldReturnCorrectErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogToAdd = CreateTestErrorLog(context);
                manager.Add(errorLogToAdd);
                var fetchedErrorLog = manager.GetById(errorLogToAdd.ErrorLogID);
                Assert.IsNotNull(fetchedErrorLog);
                Assert.AreEqual(errorLogToAdd.ErrorLogID, fetchedErrorLog.ErrorLogID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingErrorLog_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLog = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedErrorLog);
            }
        }
        [TestMethod]
        public void GetById_NonExistingErrorLog_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLog = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedErrorLog);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingErrorLog_ShouldReturnCorrectErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogToAdd = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLogToAdd);
                var fetchedErrorLog = await manager.GetByCodeAsync(errorLogToAdd.Code.Value);
                Assert.IsNotNull(fetchedErrorLog);
                Assert.AreEqual(errorLogToAdd.Code, fetchedErrorLog.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingErrorLog_ShouldReturnCorrectErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogToAdd = CreateTestErrorLog(context);
                manager.Add(errorLogToAdd);
                var fetchedErrorLog = manager.GetByCode(errorLogToAdd.Code.Value);
                Assert.IsNotNull(fetchedErrorLog);
                Assert.AreEqual(errorLogToAdd.Code, fetchedErrorLog.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingErrorLog_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLog = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedErrorLog);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingErrorLog_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLog = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedErrorLog);
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
                var manager = new ErrorLogManager(context);
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
                var manager = new ErrorLogManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleErrorLogs_ShouldReturnAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog1 = await CreateTestErrorLogAsync(context);
                var errorLog2 = await CreateTestErrorLogAsync(context);
                var errorLog3 = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog1);
                await manager.AddAsync(errorLog2);
                await manager.AddAsync(errorLog3);
                var fetchedErrorLogs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedErrorLogs);
                Assert.AreEqual(3, fetchedErrorLogs.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleErrorLogs_ShouldReturnAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog1 = CreateTestErrorLog(context);
                var errorLog2 = CreateTestErrorLog(context);
                var errorLog3 = CreateTestErrorLog(context);
                manager.Add(errorLog1);
                manager.Add(errorLog2);
                manager.Add(errorLog3);
                var fetchedErrorLogs = manager.GetAll();
                Assert.IsNotNull(fetchedErrorLogs);
                Assert.AreEqual(3, fetchedErrorLogs.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLogs = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedErrorLogs);
                Assert.AreEqual(0, fetchedErrorLogs.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var fetchedErrorLogs = manager.GetAll();
                Assert.IsNotNull(fetchedErrorLogs);
                Assert.AreEqual(0, fetchedErrorLogs.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidErrorLog_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog);
                errorLog.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(errorLog);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(errorLog.Code, context.ErrorLogSet.Find(errorLog.ErrorLogID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidErrorLog_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                manager.Add(errorLog);
                errorLog.Code = Guid.NewGuid();
                var updateResult = manager.Update(errorLog);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(errorLog.Code, context.ErrorLogSet.Find(errorLog.ErrorLogID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Arrange
                var errorLog = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog);
                var firstInstance = await manager.GetByIdAsync(errorLog.ErrorLogID);
                var secondInstance = await manager.GetByIdAsync(errorLog.ErrorLogID);
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
                var manager = new ErrorLogManager(context);
                // Arrange
                var errorLog = CreateTestErrorLog(context);
                manager.Add(errorLog);
                var firstInstance = manager.GetById(errorLog.ErrorLogID);
                var secondInstance = manager.GetById(errorLog.ErrorLogID);
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
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                //context.ErrorLogSet.Add(errorLog);
                //await context.SaveChangesAsync();
                await manager.AddAsync(errorLog);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    errorLog.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(errorLog);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshErrorLog = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                    Assert.AreNotEqual(errorLog.Code, freshErrorLog.Code); // Because the transaction was not committed.
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
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                //context.ErrorLogSet.Add(errorLog);
                //context.SaveChanges();
                manager.Add(errorLog);
                using (var transaction = context.Database.BeginTransaction())
                {
                    errorLog.Code = Guid.NewGuid();
                    var updateResult = manager.Update(errorLog);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshErrorLog = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                    Assert.AreNotEqual(errorLog.Code, freshErrorLog.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog);
                var deleteResult = await manager.DeleteAsync(errorLog.ErrorLogID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.ErrorLogSet.Find(errorLog.ErrorLogID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteErrorLog()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                manager.Add(errorLog);
                var deleteResult = manager.Delete(errorLog.ErrorLogID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.ErrorLogSet.Find(errorLog.ErrorLogID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
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
                var manager = new ErrorLogManager(context);
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
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                await manager.AddAsync(errorLog);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(errorLog.ErrorLogID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshErrorLog = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                    Assert.IsNotNull(freshErrorLog);  // Because the transaction was not committed.
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
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                manager.Add(errorLog);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(errorLog.ErrorLogID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshErrorLog = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                    Assert.IsNotNull(freshErrorLog);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidErrorLogs_ShouldInsertAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                await manager.BulkInsertAsync(errorLogs);
                Assert.AreEqual(errorLogs.Count, context.ErrorLogSet.Count());
                foreach (var errorLog in errorLogs)
                {
                    Assert.IsNotNull(context.ErrorLogSet.Find(errorLog.ErrorLogID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidErrorLogs_ShouldInsertAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                manager.BulkInsert(errorLogs);
                Assert.AreEqual(errorLogs.Count, context.ErrorLogSet.Count());
                foreach (var errorLog in errorLogs)
                {
                    Assert.IsNotNull(context.ErrorLogSet.Find(errorLog.ErrorLogID));
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.ErrorLogSet.Count());  // Because the transaction was not committed.
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.ErrorLogSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add initial errorLogs
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                var errorLogsToUpdate = new List<ErrorLog>();
                foreach (var errorLog in errorLogs)
                {
                    errorLogsToUpdate.Add(await manager.AddAsync(errorLog));
                }
                // Update errorLogs
                foreach (var errorLog in errorLogsToUpdate)
                {
                    errorLog.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(errorLogsToUpdate);
                // Verify updates
                foreach (var updatedErrorLog in errorLogsToUpdate)
                {
                    var errorLogFromDb = await manager.GetByIdAsync(updatedErrorLog.ErrorLogID);// context.ErrorLogSet.Find(updatedErrorLog.ErrorLogID);
                    Assert.AreEqual(updatedErrorLog.Code, errorLogFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add initial errorLogs
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                var errorLogsToUpdate = new List<ErrorLog>();
                foreach (var errorLog in errorLogs)
                {
                    errorLogsToUpdate.Add(manager.Add(errorLog));
                }
                // Update errorLogs
                foreach (var errorLog in errorLogsToUpdate)
                {
                    errorLog.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(errorLogsToUpdate);
                // Verify updates
                foreach (var updatedErrorLog in errorLogsToUpdate)
                {
                    var errorLogFromDb = manager.GetById(updatedErrorLog.ErrorLogID);// context.ErrorLogSet.Find(updatedErrorLog.ErrorLogID);
                    Assert.AreEqual(updatedErrorLog.Code, errorLogFromDb.Code);
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
        //        var manager = new ErrorLogManager(context);
        //        var errorLogs = new List<ErrorLog>
        //        {
        //            await CreateTestErrorLogAsync(context),
        //            await CreateTestErrorLogAsync(context),
        //            await CreateTestErrorLogAsync(context)
        //        };
        //        foreach (var errorLog in errorLogs)
        //        {
        //            await manager.AddAsync(errorLog);
        //        }
        //        foreach (var errorLog in errorLogs)
        //        {
        //            errorLog.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(errorLogs);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    await manager.AddAsync(errorLog);
                }
                foreach (var errorLog in errorLogs)
                {
                    errorLog.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var errorLog in errorLogs)
                    {
                        var errorLogFromDb = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                        Assert.AreNotEqual(errorLog.Code, errorLogFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    manager.Add(errorLog);
                }
                foreach (var errorLog in errorLogs)
                {
                    errorLog.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var errorLog in errorLogs)
                    {
                        var errorLogFromDb = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                        Assert.AreNotEqual(errorLog.Code, errorLogFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add initial errorLogs
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    await manager.AddAsync(errorLog);
                }
                // Delete errorLogs
                await manager.BulkDeleteAsync(errorLogs);
                // Verify deletions
                foreach (var deletedErrorLog in errorLogs)
                {
                    var errorLogFromDb = context.ErrorLogSet.Find(deletedErrorLog.ErrorLogID);
                    Assert.IsNull(errorLogFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                // Add initial errorLogs
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    manager.Add(errorLog);
                }
                // Delete errorLogs
                manager.BulkDelete(errorLogs);
                // Verify deletions
                foreach (var deletedErrorLog in errorLogs)
                {
                    var errorLogFromDb = context.ErrorLogSet.Find(deletedErrorLog.ErrorLogID);
                    Assert.IsNull(errorLogFromDb);
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    await manager.AddAsync(errorLog);
                }
                foreach (var errorLog in errorLogs)
                {
                    errorLog.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(errorLogs);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    manager.Add(errorLog);
                }
                foreach (var errorLog in errorLogs)
                {
                    errorLog.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(errorLogs);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context),
                    await CreateTestErrorLogAsync(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    await manager.AddAsync(errorLog);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var errorLog in errorLogs)
                    {
                        var errorLogFromDb = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                        Assert.IsNotNull(errorLogFromDb);  // ErrorLog should still exist as the transaction wasn't committed.
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
                var manager = new ErrorLogManager(context);
                var errorLogs = new List<ErrorLog>
                {
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context),
                    CreateTestErrorLog(context)
                };
                foreach (var errorLog in errorLogs)
                {
                    manager.Add(errorLog);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(errorLogs);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var errorLog in errorLogs)
                    {
                        var errorLogFromDb = freshContext.ErrorLogSet.Find(errorLog.ErrorLogID);
                        Assert.IsNotNull(errorLogFromDb);  // ErrorLog should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = await CreateTestErrorLogAsync(context);
                //errorLog.PacID = 1;
                //context.ErrorLogSet.Add(errorLog);
                //await context.SaveChangesAsync();
                await manager.AddAsync(errorLog);
                var result = await manager.GetByPacIDAsync(errorLog.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(errorLog.ErrorLogID, result.First().ErrorLogID);
            }
        }
        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog = CreateTestErrorLog(context);
                //errorLog.PacID = 1;
                //context.ErrorLogSet.Add(errorLog);
                //context.SaveChanges();
                manager.Add(errorLog);
                var result = manager.GetByPacID(errorLog.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(errorLog.ErrorLogID, result.First().ErrorLogID);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
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
                var manager = new ErrorLogManager(context);
                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleErrorLogsSamePacId_ShouldReturnAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog1 = await CreateTestErrorLogAsync(context);
                var errorLog2 = await CreateTestErrorLogAsync(context);
                errorLog2.PacID = errorLog1.PacID;
                await manager.AddAsync(errorLog1);
                await manager.AddAsync(errorLog2);
                //context.ErrorLogSet.AddRange(errorLog1, errorLog2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacIDAsync(errorLog1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_MultipleErrorLogsSamePacId_ShouldReturnAllErrorLogs()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new ErrorLogManager(context);
                var errorLog1 = CreateTestErrorLog(context);
                var errorLog2 = CreateTestErrorLog(context);
                errorLog2.PacID = errorLog1.PacID;
                manager.Add(errorLog1);
                manager.Add(errorLog2);
                //context.ErrorLogSet.AddRange(errorLog1, errorLog2);
                //context.SaveChanges();
                var result = manager.GetByPacID(errorLog1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<ErrorLog> CreateTestErrorLogAsync(FarmDbContext dbContext)
        {
            return await ErrorLogFactory.CreateAsync(dbContext);
        }
        private ErrorLog CreateTestErrorLog(FarmDbContext dbContext)
        {
            return ErrorLogFactory.Create(dbContext);
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
