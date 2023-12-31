using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Reports;
using FS.Farm.EF.Test.Factory;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using FS.Common.Diagnostics.Loggers;
namespace FS.Farm.EF.Test.Tests.Reports.Grid
{
    [TestClass]
    public class PacUserRoleListTest
    {
        //TODO create test for each filter param.  copy count test. positive and negative
        [TestMethod]
        public async Task GetCountAsync_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = await CreateTestRoleAsync(context);
                var manager = new RoleManager(context);
                await manager.AddAsync(role1);
                var count = await manager.GetTotalCountAsync();
                var resultCount = await reportGenerator.GetCountAsync(
                    Guid.Empty,
                    role1.PacCodePeek);
                Assert.AreEqual(1, resultCount);
            }
        }
        [TestMethod]
        public void GetCount_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = CreateTestRole(context);
                var manager = new RoleManager(context);
                manager.Add(role1);
                var count = manager.GetTotalCount();
                var resultCount = reportGenerator.GetCount(
                    Guid.Empty,
                    role1.PacCodePeek);
                Assert.AreEqual(1, resultCount);
            }
        }
        [TestMethod]
        public async Task GetCountAsync_ShouldReturnZeroCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = await CreateTestRoleAsync(context);
                var manager = new RoleManager(context);
                await manager.AddAsync(role1);
                var count = await manager.GetTotalCountAsync();
                var resultCount = await reportGenerator.GetCountAsync(
                    Guid.Empty,
                    Guid.NewGuid());
                Assert.AreEqual(0, resultCount);
            }
        }
        [TestMethod]
        public void GetCount_ShouldReturnZeroCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = CreateTestRole(context);
                var manager = new RoleManager(context);
                manager.Add(role1);
                var count = manager.GetTotalCount();
                var resultCount = reportGenerator.GetCount(
                    Guid.Empty,
                    Guid.NewGuid());
                Assert.AreEqual(0, resultCount);
            }
        }
        [TestMethod]
        public async Task GetAsync_ShouldReturnSingleItem()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = await CreateTestRoleAsync(context);
                var manager = new RoleManager(context);
                await manager.AddAsync(role1);
                var count = await manager.GetTotalCountAsync();
                var result = await reportGenerator.GetAsync(
                    Guid.Empty,
                    role1.PacCodePeek,
                    1,
                    100,
                    string.Empty,
                    false
                    );
                Assert.AreEqual(1, result.Count);
            }
        }
        [TestMethod]
        public void Get_ShouldReturnSingleItem()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = CreateTestRole(context);
                var manager = new RoleManager(context);
                manager.Add(role1);
                var count = manager.GetTotalCount();
                var result = reportGenerator.Get(
                    Guid.Empty,
                    role1.PacCodePeek,
                    1,
                    100,
                    string.Empty,
                    false
                    );
                Assert.AreEqual(1, result.Count);
            }
        }
        [TestMethod]
        public async Task GetAsync_ShouldReturnNoItem()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = await CreateTestRoleAsync(context);
                var manager = new RoleManager(context);
                await manager.AddAsync(role1);
                var count = await manager.GetTotalCountAsync();
                var result = await reportGenerator.GetAsync(
                    Guid.Empty,
                    Guid.NewGuid(),
                    1,
                    100,
                    string.Empty,
                    false
                    );
                Assert.AreEqual(0, result.Count);
            }
        }
        [TestMethod]
        public void Get_ShouldReturnNoItem()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PacUserRoleList(context);
                var role1 = CreateTestRole(context);
                var manager = new RoleManager(context);
                manager.Add(role1);
                var count = manager.GetTotalCount();
                var result = reportGenerator.Get(
                    Guid.Empty,
                    Guid.NewGuid(),
                    1,
                    100,
                    string.Empty,
                    false
                    );
                Assert.AreEqual(0, result.Count);
            }
        }
        private async Task<Role> CreateTestRoleAsync(FarmDbContext dbContext)
        {
            return await RoleFactory.CreateAsync(dbContext);
        }
        private Role CreateTestRole(FarmDbContext dbContext)
        {
            return   RoleFactory.Create(dbContext);
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
