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
    public class PacConfigDynaFlowDFTBuildToDoListTest
    {

        //TODO create test for each filter param.  copy count test. positive and negative

        [TestMethod]
        public async Task GetCountAsync_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);

                var manager = new DynaFlowManager(context);

                await manager.AddAsync(dynaFlow1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = CreateTestDynaFlow(context);

                var manager = new DynaFlowManager(context);

                manager.Add(dynaFlow1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var resultCount = reportGenerator.GetCount(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);

                var manager = new DynaFlowManager(context);

                await manager.AddAsync(dynaFlow1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = CreateTestDynaFlow(context);

                var manager = new DynaFlowManager(context);

                manager.Add(dynaFlow1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var resultCount = reportGenerator.GetCount(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);

                var manager = new DynaFlowManager(context);

                await manager.AddAsync(dynaFlow1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var result = await reportGenerator.GetAsync(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = CreateTestDynaFlow(context);

                var manager = new DynaFlowManager(context);

                manager.Add(dynaFlow1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var result = reportGenerator.Get(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = await CreateTestDynaFlowAsync(context);

                var manager = new DynaFlowManager(context);

                await manager.AddAsync(dynaFlow1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var result = await reportGenerator.GetAsync(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowDFTBuildToDoList(context);

                var dynaFlow1 = CreateTestDynaFlow(context);

                var manager = new DynaFlowManager(context);

                manager.Add(dynaFlow1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlow1);

                var result = reportGenerator.Get(
                    null,   //isBuildTaskDebugRequiredTriStateFilterCode
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
        private Guid GetPacCode(FarmDbContext dbContext, FS.Farm.EF.Models.DynaFlow dynaFlow)
        {
            Dictionary<string,string> lineage = DynaFlowFactory.GetCodeLineage(dbContext, dynaFlow.Code.Value);

            if(lineage.ContainsKey("PacCode"))
            {
                return new Guid(lineage["PacCode"]);
            }
            else
            {
                return Guid.Empty;
            }
        }
        private async Task<FS.Farm.EF.Models.DynaFlow> CreateTestDynaFlowAsync(FarmDbContext dbContext)
        {
            return await DynaFlowFactory.CreateAsync(dbContext);
        }
        private FS.Farm.EF.Models.DynaFlow CreateTestDynaFlow(FarmDbContext dbContext)
        {
            return   DynaFlowFactory.Create(dbContext);
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

