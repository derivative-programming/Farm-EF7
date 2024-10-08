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
    public class PacConfigDynaFlowTaskSearchTest
    {

        //TODO create test for each filter param.  copy count test. positive and negative

        [TestMethod]
        public async Task GetCountAsync_ShouldReturnSingleCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);

                var manager = new DynaFlowTaskManager(context);

                await manager.AddAsync(dynaFlowTask1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);

                var manager = new DynaFlowTaskManager(context);

                manager.Add(dynaFlowTask1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var resultCount = reportGenerator.GetCount(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);

                var manager = new DynaFlowTaskManager(context);

                await manager.AddAsync(dynaFlowTask1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var resultCount = await reportGenerator.GetCountAsync(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);

                var manager = new DynaFlowTaskManager(context);

                manager.Add(dynaFlowTask1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var resultCount = reportGenerator.GetCount(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);

                var manager = new DynaFlowTaskManager(context);

                await manager.AddAsync(dynaFlowTask1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var result = await reportGenerator.GetAsync(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);

                var manager = new DynaFlowTaskManager(context);

                manager.Add(dynaFlowTask1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var result = reportGenerator.Get(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = await CreateTestDynaFlowTaskAsync(context);

                var manager = new DynaFlowTaskManager(context);

                await manager.AddAsync(dynaFlowTask1);

                var count = await manager.GetTotalCountAsync();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var result = await reportGenerator.GetAsync(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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

                var reportGenerator = new PacConfigDynaFlowTaskSearch(context);

                var dynaFlowTask1 = CreateTestDynaFlowTask(context);

                var manager = new DynaFlowTaskManager(context);

                manager.Add(dynaFlowTask1);

                var count = manager.GetTotalCount();

                Guid contextCode = GetPacCode(context, dynaFlowTask1);

                var result = reportGenerator.Get(
                    null,   //startedDateGreaterThanFilterCode
                    null,   //processorIdentifier
                    null,   //isStartedTriStateFilterCode
                    null,   //isCompletedTriStateFilterCode
                    null,   //isSuccessfulTriStateFilterCode
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
        private Guid GetPacCode(FarmDbContext dbContext, FS.Farm.EF.Models.DynaFlowTask dynaFlowTask)
        {
            Dictionary<string,string> lineage = DynaFlowTaskFactory.GetCodeLineage(dbContext, dynaFlowTask.Code.Value);

            if(lineage.ContainsKey("PacCode"))
            {
                return new Guid(lineage["PacCode"]);
            }
            else
            {
                return Guid.Empty;
            }
        }
        private async Task<FS.Farm.EF.Models.DynaFlowTask> CreateTestDynaFlowTaskAsync(FarmDbContext dbContext)
        {
            return await DynaFlowTaskFactory.CreateAsync(dbContext);
        }
        private FS.Farm.EF.Models.DynaFlowTask CreateTestDynaFlowTask(FarmDbContext dbContext)
        {
            return   DynaFlowTaskFactory.Create(dbContext);
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

