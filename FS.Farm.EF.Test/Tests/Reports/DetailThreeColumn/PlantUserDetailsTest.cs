using Microsoft.EntityFrameworkCore;
using FS.Farm.EF;
using FS.Farm.EF.Models;
using FS.Farm.EF.Managers;
using FS.Farm.EF.Reports;
using FS.Farm.EF.Test.Factory;
using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using FS.Common.Diagnostics.Loggers;
namespace FS.Farm.EF.Test.Tests.Reports.DetailThreeColumn
{
    [TestClass]
    public class PlantUserDetailsTest
    {
        //TODO create test for each filter param.  copy count test. positive and negative
        [TestMethod]
        public async Task GetCountAsync_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();
            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var reportGenerator = new PlantUserDetails(context);
                var plant1 = await CreateTestPlant(context);
                var manager = new PlantManager(context);
                await manager.AddAsync(plant1);
                var count = await manager.GetTotalCountAsync();
                var resultCount = await reportGenerator.GetCountAsync(
                    Guid.Empty,
                    plant1.Code.Value);
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
                var reportGenerator = new PlantUserDetails(context);
                var plant1 = await CreateTestPlant(context);
                var manager = new PlantManager(context);
                await manager.AddAsync(plant1);
                var count = await manager.GetTotalCountAsync();
                var resultCount = await reportGenerator.GetCountAsync(
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
                var reportGenerator = new PlantUserDetails(context);
                var plant1 = await CreateTestPlant(context);
                var manager = new PlantManager(context);
                await manager.AddAsync(plant1);
                var count = await manager.GetTotalCountAsync();
                var result = await reportGenerator.GetAsync(
                    Guid.Empty,
                    plant1.Code.Value,
                    1,
                    100,
                    String.Empty,
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
                var reportGenerator = new PlantUserDetails(context);
                var plant1 = await CreateTestPlant(context);
                var manager = new PlantManager(context);
                await manager.AddAsync(plant1);
                var count = await manager.GetTotalCountAsync();
                var result = await reportGenerator.GetAsync(
                    Guid.Empty,
                    Guid.NewGuid(),
                    1,
                    100,
                    String.Empty,
                    false
                    );
                Assert.AreEqual(0, result.Count);
            }
        }
        private async Task<Plant> CreateTestPlant(FarmDbContext dbContext)
        {
            return await Factory.PlantFactory.CreateAsync(dbContext);
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
