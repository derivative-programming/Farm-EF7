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
    public class RoleTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                var result = await manager.AddAsync(role);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.RoleSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                var result = manager.Add(role);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.RoleSet.Count());
            }
        }
        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var role = await CreateTestRoleAsync(context);
                    var result = await manager.AddAsync(role);
                    await transaction.CommitAsync();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.RoleSet.Count());
                }
            }
        }
        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var role = CreateTestRole(context);
                    var result = manager.Add(role);
                    transaction.Commit();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.RoleSet.Count());
                }
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_NoRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoRoles_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var result = manager.GetTotalCount();
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public async Task GetTotalCountAsync_WithRoles_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add some roles
                await manager.AddAsync(await CreateTestRoleAsync(context));
                await manager.AddAsync(await CreateTestRoleAsync(context));
                await manager.AddAsync(await CreateTestRoleAsync(context));
                var result = await manager.GetTotalCountAsync();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithRoles_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add some roles
                manager.Add(CreateTestRole(context));
                manager.Add(CreateTestRole(context));
                manager.Add(CreateTestRole(context));
                var result = manager.GetTotalCount();
                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_NoRoles_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var result = await manager.GetMaxIdAsync();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoRoles_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var result = manager.GetMaxId();
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public async Task GetMaxIdAsync_WithRoles_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add some roles
                var role1 = await CreateTestRoleAsync(context);
                var role2 = await CreateTestRoleAsync(context);
                var role3 = await CreateTestRoleAsync(context);
                await manager.AddAsync(role1);
                await manager.AddAsync(role2);
                await manager.AddAsync(role3);
                var result = await manager.GetMaxIdAsync();
                var maxId = new[] { role1.RoleID, role2.RoleID, role3.RoleID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public void GetMaxId_WithRoles_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add some roles
                var role1 = CreateTestRole(context);
                var role2 = CreateTestRole(context);
                var role3 = CreateTestRole(context);
                manager.Add(role1);
                manager.Add(role2);
                manager.Add(role3);
                var result = manager.GetMaxId();
                var maxId = new[] { role1.RoleID, role2.RoleID, role3.RoleID }.Max();
                Assert.AreEqual(maxId, result);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_ExistingRole_ShouldReturnCorrectRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roleToAdd = await CreateTestRoleAsync(context);
                await manager.AddAsync(roleToAdd);
                var fetchedRole = await manager.GetByIdAsync(roleToAdd.RoleID);
                Assert.IsNotNull(fetchedRole);
                Assert.AreEqual(roleToAdd.RoleID, fetchedRole.RoleID);
            }
        }
        [TestMethod]
        public void GetById_ExistingRole_ShouldReturnCorrectRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roleToAdd = CreateTestRole(context);
                manager.Add(roleToAdd);
                var fetchedRole = manager.GetById(roleToAdd.RoleID);
                Assert.IsNotNull(fetchedRole);
                Assert.AreEqual(roleToAdd.RoleID, fetchedRole.RoleID);
            }
        }
        [TestMethod]
        public async Task GetByIdAsync_NonExistingRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRole = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedRole);
            }
        }
        [TestMethod]
        public void GetById_NonExistingRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRole = manager.GetById(999); // Assuming 999 is a non-existing ID
                Assert.IsNull(fetchedRole);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_ExistingRole_ShouldReturnCorrectRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roleToAdd = await CreateTestRoleAsync(context);
                await manager.AddAsync(roleToAdd);
                var fetchedRole = await manager.GetByCodeAsync(roleToAdd.Code.Value);
                Assert.IsNotNull(fetchedRole);
                Assert.AreEqual(roleToAdd.Code, fetchedRole.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingRole_ShouldReturnCorrectRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roleToAdd = CreateTestRole(context);
                manager.Add(roleToAdd);
                var fetchedRole = manager.GetByCode(roleToAdd.Code.Value);
                Assert.IsNotNull(fetchedRole);
                Assert.AreEqual(roleToAdd.Code, fetchedRole.Code);
            }
        }
        [TestMethod]
        public async Task GetByCodeAsync_NonExistingRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRole = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedRole);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingRole_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRole = manager.GetByCode(Guid.NewGuid()); // Random new GUID
                Assert.IsNull(fetchedRole);
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
                var manager = new RoleManager(context);
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
                var manager = new RoleManager(context);
                manager.GetByCode(Guid.Empty);
            }
        }
        [TestMethod]
        public async Task GetAllAsync_MultipleRoles_ShouldReturnAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role1 = await CreateTestRoleAsync(context);
                var role2 = await CreateTestRoleAsync(context);
                var role3 = await CreateTestRoleAsync(context);
                await manager.AddAsync(role1);
                await manager.AddAsync(role2);
                await manager.AddAsync(role3);
                var fetchedRoles = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedRoles);
                Assert.AreEqual(3, fetchedRoles.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultipleRoles_ShouldReturnAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role1 = CreateTestRole(context);
                var role2 = CreateTestRole(context);
                var role3 = CreateTestRole(context);
                manager.Add(role1);
                manager.Add(role2);
                manager.Add(role3);
                var fetchedRoles = manager.GetAll();
                Assert.IsNotNull(fetchedRoles);
                Assert.AreEqual(3, fetchedRoles.Count());
            }
        }
        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRoles = await manager.GetAllAsync();
                Assert.IsNotNull(fetchedRoles);
                Assert.AreEqual(0, fetchedRoles.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var fetchedRoles = manager.GetAll();
                Assert.IsNotNull(fetchedRoles);
                Assert.AreEqual(0, fetchedRoles.Count());
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ValidRole_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                await manager.AddAsync(role);
                role.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(role);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(role.Code, context.RoleSet.Find(role.RoleID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidRole_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                manager.Add(role);
                role.Code = Guid.NewGuid();
                var updateResult = manager.Update(role);
                Assert.IsTrue(updateResult);
                Assert.AreEqual(role.Code, context.RoleSet.Find(role.RoleID).Code);
            }
        }
        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Arrange
                var role = await CreateTestRoleAsync(context);
                await manager.AddAsync(role);
                var firstInstance = await manager.GetByIdAsync(role.RoleID);
                var secondInstance = await manager.GetByIdAsync(role.RoleID);
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
                var manager = new RoleManager(context);
                // Arrange
                var role = CreateTestRole(context);
                manager.Add(role);
                var firstInstance = manager.GetById(role.RoleID);
                var secondInstance = manager.GetById(role.RoleID);
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
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                //context.RoleSet.Add(role);
                //await context.SaveChangesAsync();
                await manager.AddAsync(role);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    role.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(role);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshRole = freshContext.RoleSet.Find(role.RoleID);
                    Assert.AreNotEqual(role.Code, freshRole.Code); // Because the transaction was not committed.
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
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                //context.RoleSet.Add(role);
                //context.SaveChanges();
                manager.Add(role);
                using (var transaction = context.Database.BeginTransaction())
                {
                    role.Code = Guid.NewGuid();
                    var updateResult = manager.Update(role);
                    Assert.IsTrue(updateResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshRole = freshContext.RoleSet.Find(role.RoleID);
                    Assert.AreNotEqual(role.Code, freshRole.Code); // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeleteRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                await manager.AddAsync(role);
                var deleteResult = await manager.DeleteAsync(role.RoleID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.RoleSet.Find(role.RoleID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeleteRole()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                manager.Add(role);
                var deleteResult = manager.Delete(role.RoleID);
                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.RoleSet.Find(role.RoleID));
            }
        }
        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
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
                var manager = new RoleManager(context);
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
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                await manager.AddAsync(role);
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(role.RoleID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshRole = freshContext.RoleSet.Find(role.RoleID);
                    Assert.IsNotNull(freshRole);  // Because the transaction was not committed.
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
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                manager.Add(role);
                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(role.RoleID);
                    Assert.IsTrue(deleteResult);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshRole = freshContext.RoleSet.Find(role.RoleID);
                    Assert.IsNotNull(freshRole);  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkInsertAsync_ValidRoles_ShouldInsertAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                await manager.BulkInsertAsync(roles);
                Assert.AreEqual(roles.Count, context.RoleSet.Count());
                foreach (var role in roles)
                {
                    Assert.IsNotNull(context.RoleSet.Find(role.RoleID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidRoles_ShouldInsertAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                manager.BulkInsert(roles);
                Assert.AreEqual(roles.Count, context.RoleSet.Count());
                foreach (var role in roles)
                {
                    Assert.IsNotNull(context.RoleSet.Find(role.RoleID));
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.RoleSet.Count());  // Because the transaction was not committed.
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.RoleSet.Count());  // Because the transaction was not committed.
                }
            }
        }
        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add initial roles
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                var rolesToUpdate = new List<Role>();
                foreach (var role in roles)
                {
                    rolesToUpdate.Add(await manager.AddAsync(role));
                }
                // Update roles
                foreach (var role in rolesToUpdate)
                {
                    role.Code = Guid.NewGuid();
                }
                await manager.BulkUpdateAsync(rolesToUpdate);
                // Verify updates
                foreach (var updatedRole in rolesToUpdate)
                {
                    var roleFromDb = await manager.GetByIdAsync(updatedRole.RoleID);// context.RoleSet.Find(updatedRole.RoleID);
                    Assert.AreEqual(updatedRole.Code, roleFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add initial roles
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                var rolesToUpdate = new List<Role>();
                foreach (var role in roles)
                {
                    rolesToUpdate.Add(manager.Add(role));
                }
                // Update roles
                foreach (var role in rolesToUpdate)
                {
                    role.Code = Guid.NewGuid();
                }
                manager.BulkUpdate(rolesToUpdate);
                // Verify updates
                foreach (var updatedRole in rolesToUpdate)
                {
                    var roleFromDb = manager.GetById(updatedRole.RoleID);// context.RoleSet.Find(updatedRole.RoleID);
                    Assert.AreEqual(updatedRole.Code, roleFromDb.Code);
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
        //        var manager = new RoleManager(context);
        //        var roles = new List<Role>
        //        {
        //            await CreateTestRoleAsync(context),
        //            await CreateTestRoleAsync(context),
        //            await CreateTestRoleAsync(context)
        //        };
        //        foreach (var role in roles)
        //        {
        //            await manager.AddAsync(role);
        //        }
        //        foreach (var role in roles)
        //        {
        //            role.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(roles);  // This should throw a concurrency exception
        //    }
        //}
        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                foreach (var role in roles)
                {
                    await manager.AddAsync(role);
                }
                foreach (var role in roles)
                {
                    role.Code = Guid.NewGuid();
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var role in roles)
                    {
                        var roleFromDb = freshContext.RoleSet.Find(role.RoleID);
                        Assert.AreNotEqual(role.Code, roleFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                foreach (var role in roles)
                {
                    manager.Add(role);
                }
                foreach (var role in roles)
                {
                    role.Code = Guid.NewGuid();
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var role in roles)
                    {
                        var roleFromDb = freshContext.RoleSet.Find(role.RoleID);
                        Assert.AreNotEqual(role.Code, roleFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add initial roles
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                foreach (var role in roles)
                {
                    await manager.AddAsync(role);
                }
                // Delete roles
                await manager.BulkDeleteAsync(roles);
                // Verify deletions
                foreach (var deletedRole in roles)
                {
                    var roleFromDb = context.RoleSet.Find(deletedRole.RoleID);
                    Assert.IsNull(roleFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                // Add initial roles
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                foreach (var role in roles)
                {
                    manager.Add(role);
                }
                // Delete roles
                manager.BulkDelete(roles);
                // Verify deletions
                foreach (var deletedRole in roles)
                {
                    var roleFromDb = context.RoleSet.Find(deletedRole.RoleID);
                    Assert.IsNull(roleFromDb);
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                foreach (var role in roles)
                {
                    await manager.AddAsync(role);
                }
                foreach (var role in roles)
                {
                    role.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(roles);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                foreach (var role in roles)
                {
                    manager.Add(role);
                }
                foreach (var role in roles)
                {
                    role.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(roles);  // This should throw a concurrency exception due to token mismatch
            }
        }
        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context),
                    await CreateTestRoleAsync(context)
                };
                foreach (var role in roles)
                {
                    await manager.AddAsync(role);
                }
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var role in roles)
                    {
                        var roleFromDb = freshContext.RoleSet.Find(role.RoleID);
                        Assert.IsNotNull(roleFromDb);  // Role should still exist as the transaction wasn't committed.
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
                var manager = new RoleManager(context);
                var roles = new List<Role>
                {
                    CreateTestRole(context),
                    CreateTestRole(context),
                    CreateTestRole(context)
                };
                foreach (var role in roles)
                {
                    manager.Add(role);
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(roles);
                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }
                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var role in roles)
                    {
                        var roleFromDb = freshContext.RoleSet.Find(role.RoleID);
                        Assert.IsNotNull(roleFromDb);  // Role should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        [TestMethod]//PacID
        public async Task GetByPacIdAsync_ValidPacId_ShouldReturnRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = await CreateTestRoleAsync(context);
                //role.PacID = 1;
                //context.RoleSet.Add(role);
                //await context.SaveChangesAsync();
                await manager.AddAsync(role);
                var result = await manager.GetByPacIDAsync(role.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(role.RoleID, result.First().RoleID);
            }
        }
        [TestMethod]//PacID
        public void GetByPacId_ValidPacId_ShouldReturnRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role = CreateTestRole(context);
                //role.PacID = 1;
                //context.RoleSet.Add(role);
                //context.SaveChanges();
                manager.Add(role);
                var result = manager.GetByPacID(role.PacID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(role.RoleID, result.First().RoleID);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_InvalidPacId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
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
                var manager = new RoleManager(context);
                var result = manager.GetByPacID(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod] //PacID
        public async Task GetByPacIdAsync_MultipleRolesSamePacId_ShouldReturnAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role1 = await CreateTestRoleAsync(context);
                var role2 = await CreateTestRoleAsync(context);
                role2.PacID = role1.PacID;
                await manager.AddAsync(role1);
                await manager.AddAsync(role2);
                //context.RoleSet.AddRange(role1, role2);
                //await context.SaveChangesAsync();
                var result = await manager.GetByPacIDAsync(role1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        [TestMethod] //PacID
        public void GetByPacId_MultipleRolesSamePacId_ShouldReturnAllRoles()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new RoleManager(context);
                var role1 = CreateTestRole(context);
                var role2 = CreateTestRole(context);
                role2.PacID = role1.PacID;
                manager.Add(role1);
                manager.Add(role2);
                //context.RoleSet.AddRange(role1, role2);
                //context.SaveChanges();
                var result = manager.GetByPacID(role1.PacID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }
        private async Task<Role> CreateTestRoleAsync(FarmDbContext dbContext)
        {
            return await RoleFactory.CreateAsync(dbContext);
        }
        private Role CreateTestRole(FarmDbContext dbContext)
        {
            return RoleFactory.Create(dbContext);
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
