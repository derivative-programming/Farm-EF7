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
    public class OrganizationTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                var result = await manager.AddAsync(organization);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.OrganizationSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                var result = manager.Add(organization);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.OrganizationSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var organization = await CreateTestOrganizationAsync(context);
                    var result = await manager.AddAsync(organization);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.OrganizationSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var organization = CreateTestOrganization(context);
                    var result = manager.Add(organization);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.OrganizationSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoOrganizations_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoOrganizations_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithOrganizations_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add some organizations
                await manager.AddAsync(await CreateTestOrganizationAsync(context));
                await manager.AddAsync(await CreateTestOrganizationAsync(context));
                await manager.AddAsync(await CreateTestOrganizationAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithOrganizations_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add some organizations
                manager.Add(CreateTestOrganization(context));
                manager.Add(CreateTestOrganization(context));
                manager.Add(CreateTestOrganization(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoOrganizations_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoOrganizations_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = manager.GetMaxId();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithOrganizations_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add some organizations
                var organization1 = await CreateTestOrganizationAsync(context);
                var organization2 = await CreateTestOrganizationAsync(context);
                var organization3 = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization1);
                await manager.AddAsync(organization2);
                await manager.AddAsync(organization3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { organization1.OrganizationID, organization2.OrganizationID, organization3.OrganizationID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithOrganizations_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add some organizations
                var organization1 = CreateTestOrganization(context);
                var organization2 = CreateTestOrganization(context);
                var organization3 = CreateTestOrganization(context);
                manager.Add(organization1);
                manager.Add(organization2);
                manager.Add(organization3);
                var result = manager.GetMaxId();
                var maxId = new[] { organization1.OrganizationID, organization2.OrganizationID, organization3.OrganizationID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingOrganization_ShouldReturnCorrectOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizationToAdd = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organizationToAdd);
                var fetchedOrganization = await manager.GetByIdAsync(organizationToAdd.OrganizationID);
                Assert.IsNotNull(fetchedOrganization);
                Assert.AreEqual(organizationToAdd.OrganizationID, fetchedOrganization.OrganizationID);
            }
        }
        [TestMethod]
        public void GetById_ExistingOrganization_ShouldReturnCorrectOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizationToAdd = CreateTestOrganization(context);
                manager.Add(organizationToAdd);
                var fetchedOrganization = manager.GetById(organizationToAdd.OrganizationID);
                Assert.IsNotNull(fetchedOrganization);
                Assert.AreEqual(organizationToAdd.OrganizationID, fetchedOrganization.OrganizationID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingOrganization_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganization = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedOrganization);
            }
        }
        [TestMethod]
        public void GetById_NonExistingOrganization_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganization = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedOrganization);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingOrganization_ShouldReturnCorrectOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizationToAdd = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organizationToAdd);
                var fetchedOrganization = await manager.GetByCodeAsync(organizationToAdd.Code.Value);
                Assert.IsNotNull(fetchedOrganization);
                Assert.AreEqual(organizationToAdd.Code, fetchedOrganization.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingOrganization_ShouldReturnCorrectOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizationToAdd = CreateTestOrganization(context);
                manager.Add(organizationToAdd);
                var fetchedOrganization = manager.GetByCode(organizationToAdd.Code.Value);
                Assert.IsNotNull(fetchedOrganization);
                Assert.AreEqual(organizationToAdd.Code, fetchedOrganization.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingOrganization_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganization = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedOrganization);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingOrganization_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganization = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedOrganization);
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
                var manager = new OrganizationManager(context);
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
                var manager = new OrganizationManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleOrganizations_ShouldReturnAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization1 = await CreateTestOrganizationAsync(context);
                var organization2 = await CreateTestOrganizationAsync(context);
                var organization3 = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization1);
                await manager.AddAsync(organization2);
                await manager.AddAsync(organization3);
                var fetchedOrganizations = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrganizations);
                Assert.AreEqual(3, fetchedOrganizations.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleOrganizations_ShouldReturnAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization1 = CreateTestOrganization(context);
                var organization2 = CreateTestOrganization(context);
                var organization3 = CreateTestOrganization(context);
                manager.Add(organization1);
                manager.Add(organization2);
                manager.Add(organization3);
                var fetchedOrganizations = manager.GetAll();
                Assert.IsNotNull(fetchedOrganizations);
                Assert.AreEqual(3, fetchedOrganizations.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganizations = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedOrganizations);
                Assert.AreEqual(0, fetchedOrganizations.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var fetchedOrganizations = manager.GetAll();
                Assert.IsNotNull(fetchedOrganizations);
                Assert.AreEqual(0, fetchedOrganizations.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidOrganization_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization);
                organization.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(organization);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(organization.Code, context.OrganizationSet.Find(organization.OrganizationID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidOrganization_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                manager.Add(organization);
                organization.Code = Guid.NewGuid();
                var updateResult = manager.Update(organization);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(organization.Code, context.OrganizationSet.Find(organization.OrganizationID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Arrange
                var organization = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization);
                var firstInstance = await manager.GetByIdAsync(organization.OrganizationID);
                var secondInstance = await manager.GetByIdAsync(organization.OrganizationID);
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
                var manager = new OrganizationManager(context);
                // Arrange
                var organization = CreateTestOrganization(context);
                manager.Add(organization);
                var firstInstance = manager.GetById(organization.OrganizationID);
                var secondInstance = manager.GetById(organization.OrganizationID);
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
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                //context.OrganizationSet.Add(organization);
                //await context.SaveChangesAsync();
                await manager.AddAsync(organization);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    organization.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(organization);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrganization = freshContext.OrganizationSet.Find(organization.OrganizationID);
                    Assert.AreNotEqual(organization.Code, freshOrganization.Code); // Because the transaction was not committed.
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
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                //context.OrganizationSet.Add(organization);
                //context.SaveChanges();
                manager.Add(organization);
                using (var transaction = context.Database.BeginTransaction())
                {
                    organization.Code = Guid.NewGuid();
                    var updateResult = manager.Update(organization);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrganization = freshContext.OrganizationSet.Find(organization.OrganizationID);
                    Assert.AreNotEqual(organization.Code, freshOrganization.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization);
                var deleteResult = await manager.DeleteAsync(organization.OrganizationID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.OrganizationSet.Find(organization.OrganizationID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteOrganization()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                manager.Add(organization);
                var deleteResult = manager.Delete(organization.OrganizationID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.OrganizationSet.Find(organization.OrganizationID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
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
                var manager = new OrganizationManager(context);
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
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                await manager.AddAsync(organization);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(organization.OrganizationID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrganization = freshContext.OrganizationSet.Find(organization.OrganizationID);
                    Assert.IsNotNull(freshOrganization);  // Because the transaction was not committed.
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
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                manager.Add(organization);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(organization.OrganizationID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshOrganization = freshContext.OrganizationSet.Find(organization.OrganizationID);
                    Assert.IsNotNull(freshOrganization);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidOrganizations_ShouldInsertAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                await manager.BulkInsertAsync(organizations);
                Assert.AreEqual(organizations.Count, context.OrganizationSet.Count());
                foreach (var organization in organizations)
                {
                    Assert.IsNotNull(context.OrganizationSet.Find(organization.OrganizationID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidOrganizations_ShouldInsertAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                manager.BulkInsert(organizations);
                Assert.AreEqual(organizations.Count, context.OrganizationSet.Count());
                foreach (var organization in organizations)
                {
                    Assert.IsNotNull(context.OrganizationSet.Find(organization.OrganizationID));
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.OrganizationSet.Count());  // Because the transaction was not committed.
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.OrganizationSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add initial organizations
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                var organizationsToUpdate = new List<Organization>();
                foreach (var organization in organizations)
                {
                    organizationsToUpdate.Add(await manager.AddAsync(organization));
                }
                // Update organizations
                foreach (var organization in organizationsToUpdate)
                {
                    organization.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(organizationsToUpdate);
                // Verify updates
                foreach (var updatedOrganization in organizationsToUpdate)
                {
                    var organizationFromDb = await manager.GetByIdAsync(updatedOrganization.OrganizationID);// context.OrganizationSet.Find(updatedOrganization.OrganizationID);
                    Assert.AreEqual(updatedOrganization.Code, organizationFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add initial organizations
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                var organizationsToUpdate = new List<Organization>();
                foreach (var organization in organizations)
                {
                    organizationsToUpdate.Add(manager.Add(organization));
                }
                // Update organizations
                foreach (var organization in organizationsToUpdate)
                {
                    organization.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(organizationsToUpdate);
                // Verify updates
                foreach (var updatedOrganization in organizationsToUpdate)
                {
                    var organizationFromDb = manager.GetById(updatedOrganization.OrganizationID);// context.OrganizationSet.Find(updatedOrganization.OrganizationID);
                    Assert.AreEqual(updatedOrganization.Code, organizationFromDb.Code);
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
        //        var manager = new OrganizationManager(context);
        //        var organizations = new List<Organization>
        //        {
        //            await CreateTestOrganizationAsync(context),
        //            await CreateTestOrganizationAsync(context),
        //            await CreateTestOrganizationAsync(context)
        //        };
        //        foreach (var organization in organizations)
        //        {
        //            await manager.AddAsync(organization);
        //        }
        //        foreach (var organization in organizations)
        //        {
        //            organization.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(organizations);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                foreach (var organization in organizations)
                {
                    await manager.AddAsync(organization);
                }
                foreach (var organization in organizations)
                {
                    organization.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var organization in organizations)
                    {
                        var organizationFromDb = freshContext.OrganizationSet.Find(organization.OrganizationID);
                        Assert.AreNotEqual(organization.Code, organizationFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                foreach (var organization in organizations)
                {
                    manager.Add(organization);
                }
                foreach (var organization in organizations)
                {
                    organization.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var organization in organizations)
                    {
                        var organizationFromDb = freshContext.OrganizationSet.Find(organization.OrganizationID);
                        Assert.AreNotEqual(organization.Code, organizationFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add initial organizations
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                foreach (var organization in organizations)
                {
                    await manager.AddAsync(organization);
                }
                // Delete organizations
                await manager.BulkDeleteAsync(organizations);
                // Verify deletions
                foreach (var deletedOrganization in organizations)
                {
                    var organizationFromDb = context.OrganizationSet.Find(deletedOrganization.OrganizationID);
                    Assert.IsNull(organizationFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                // Add initial organizations
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                foreach (var organization in organizations)
                {
                    manager.Add(organization);
                }
                // Delete organizations
                manager.BulkDelete(organizations);
                // Verify deletions
                foreach (var deletedOrganization in organizations)
                {
                    var organizationFromDb = context.OrganizationSet.Find(deletedOrganization.OrganizationID);
                    Assert.IsNull(organizationFromDb);
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                foreach (var organization in organizations)
                {
                    await manager.AddAsync(organization);
                }
                foreach (var organization in organizations)
                {
                    organization.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(organizations);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                foreach (var organization in organizations)
                {
                    manager.Add(organization);
                }
                foreach (var organization in organizations)
                {
                    organization.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(organizations);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context),
                    await CreateTestOrganizationAsync(context)
                };
                foreach (var organization in organizations)
                {
                    await manager.AddAsync(organization);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var organization in organizations)
                    {
                        var organizationFromDb = freshContext.OrganizationSet.Find(organization.OrganizationID);
                        Assert.IsNotNull(organizationFromDb);  // Organization should still exist as the transaction wasn't committed.
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
                var manager = new OrganizationManager(context);
                var organizations = new List<Organization>
                {
                    CreateTestOrganization(context),
                    CreateTestOrganization(context),
                    CreateTestOrganization(context)
                };
                foreach (var organization in organizations)
                {
                    manager.Add(organization);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(organizations);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var organization in organizations)
                    {
                        var organizationFromDb = freshContext.OrganizationSet.Find(organization.OrganizationID);
                        Assert.IsNotNull(organizationFromDb);  // Organization should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//TacID
        public async Task GetByTacIdAsync_ValidTacId_ShouldReturnOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = await CreateTestOrganizationAsync(context);
                //organization.TacID = 1;
                //context.OrganizationSet.Add(organization);
                //await context.SaveChangesAsync();
                await manager.AddAsync(organization);
                var result = await manager.GetByTacIDAsync(organization.TacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(organization.OrganizationID, result.First().OrganizationID);
            }
        }
        [TestMethod]//TacID
        public void GetByTacId_ValidTacId_ShouldReturnOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization = CreateTestOrganization(context);
                //organization.TacID = 1;
                //context.OrganizationSet.Add(organization);
                //context.SaveChanges();
                manager.Add(organization);
                var result = manager.GetByTacID(organization.TacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(organization.OrganizationID, result.First().OrganizationID);
            }
        }
        [TestMethod] //TacID
        public async Task GetByTacIdAsync_InvalidTacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = await manager.GetByTacIDAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //TacID
        public void GetByTacId_InvalidTacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var result = manager.GetByTacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //TacID
        public async Task GetByTacIdAsync_MultipleOrganizationsSameTacId_ShouldReturnAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization1 = await CreateTestOrganizationAsync(context);
                var organization2 = await CreateTestOrganizationAsync(context);
                organization2.TacID = organization1.TacID;
                await manager.AddAsync(organization1);
                await manager.AddAsync(organization2);
                //context.OrganizationSet.AddRange(organization1, organization2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByTacIDAsync(organization1.TacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //TacID
        public void GetByTacId_MultipleOrganizationsSameTacId_ShouldReturnAllOrganizations()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new OrganizationManager(context);
                var organization1 = CreateTestOrganization(context);
                var organization2 = CreateTestOrganization(context);
                organization2.TacID = organization1.TacID;
                manager.Add(organization1);
                manager.Add(organization2);
                //context.OrganizationSet.AddRange(organization1, organization2);
                //context.SaveChanges();
                var result = manager.GetByTacID(organization1.TacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<Organization> CreateTestOrganizationAsync(FarmDbContext dbContext)
        {
            return await OrganizationFactory.CreateAsync(dbContext);
        }
        private Organization CreateTestOrganization(FarmDbContext dbContext)
        {
            return OrganizationFactory.Create(dbContext);
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
