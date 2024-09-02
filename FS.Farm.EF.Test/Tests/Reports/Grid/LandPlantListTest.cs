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
    public class LandPlantListTest
    {

        //TODO create test for each filter param.  copy count test. positive and negative

        [TestMethod]
        public async Task GetCountAsync_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();

                var reportGenerator = new LandPlantList(context);

                var plant1 = await CreateTestPlantAsync(context);

                var manager = new PlantManager(context);

                await manager.AddAsync(plant1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetLandCode(context, plant1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
                    Guid.Empty,
                    contextCode);

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

                var reportGenerator = new LandPlantList(context);

                var plant1 = CreateTestPlant(context);

                var manager = new PlantManager(context);

                manager.Add(plant1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetLandCode(context, plant1);

                var resultCount = reportGenerator.GetCount(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
                    Guid.Empty,
                    contextCode);

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

                var reportGenerator = new LandPlantList(context);

                var plant1 = await CreateTestPlantAsync(context);

                var manager = new PlantManager(context);

                await manager.AddAsync(plant1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetLandCode(context, plant1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
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

                var reportGenerator = new LandPlantList(context);

                var plant1 = CreateTestPlant(context);

                var manager = new PlantManager(context);

                manager.Add(plant1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetLandCode(context, plant1);

                var resultCount = reportGenerator.GetCount(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
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

                var reportGenerator = new LandPlantList(context);

                var plant1 = await CreateTestPlantAsync(context);

                var manager = new PlantManager(context);

                await manager.AddAsync(plant1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetLandCode(context, plant1);

                var result = await reportGenerator.GetAsync(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
                    Guid.Empty,
                    contextCode,
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

                var reportGenerator = new LandPlantList(context);

                var plant1 = CreateTestPlant(context);

                var manager = new PlantManager(context);

                manager.Add(plant1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetLandCode(context, plant1);

                var result = reportGenerator.Get(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
                    Guid.Empty,
                    contextCode,
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

                var reportGenerator = new LandPlantList(context);

                var plant1 = await CreateTestPlantAsync(context);

                var manager = new PlantManager(context);

                await manager.AddAsync(plant1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetLandCode(context, plant1);

                var result = await reportGenerator.GetAsync(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
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

                var reportGenerator = new LandPlantList(context);

                var plant1 = CreateTestPlant(context);

                var manager = new PlantManager(context);

                manager.Add(plant1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetLandCode(context, plant1);

                var result = reportGenerator.Get(
                    null,   //flavorFilterCode
                    null,   //someFilterIntVal
                    null,   //someFilterBigIntVal
                    null,   //someFilterFloatVal
                    null,   //someFilterBitVal
                    null,   //isFilterEditAllowed
                    null,   //isFilterDeleteAllowed
                    null,   //someFilterDecimalVal
                    null,   //someMinUTCDateTimeVal
                    null,   //someMinDateVal
                    null,   //someFilterMoneyVal
                    null,   //someFilterNVarCharVal
                    null,   //someFilterVarCharVal
                    null,   //someFilterTextVal
                    null,   //someFilterPhoneNumber
                    null,   //someFilterEmailAddress
                    null,   //someFilterUniqueIdentifier
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
        private Guid GetLandCode(FarmDbContext dbContext, FS.Farm.EF.Models.Plant plant)
        {
            Dictionary<string,string> lineage = PlantFactory.GetCodeLineage(dbContext, plant.Code.Value);

            if(lineage.ContainsKey("LandCode"))
            {
                return new Guid(lineage["LandCode"]);
            }
            else
            {
                return Guid.Empty;
            }
        }
        private async Task<FS.Farm.EF.Models.Plant> CreateTestPlantAsync(FarmDbContext dbContext)
        {
            return await PlantFactory.CreateAsync(dbContext);
        }
        private FS.Farm.EF.Models.Plant CreateTestPlant(FarmDbContext dbContext)
        {
            return   PlantFactory.Create(dbContext);
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