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
    public class PlantTest
    {
        [TestMethod]
        public async Task AddAsync_NoExistingTransaction_ShouldAddPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);
                var result = await manager.AddAsync(plant);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.PlantSet.Count());
            }
        }
        [TestMethod]
        public void Add_NoExistingTransaction_ShouldAddPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);
                var result = manager.Add(plant);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, context.PlantSet.Count());
            }
        }

        [TestMethod]
        public async Task AddAsync_WithExistingTransaction_ShouldAddPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var plant = await CreateTestPlantAsync(context);
                    var result = await manager.AddAsync(plant);
                    await transaction.CommitAsync();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.PlantSet.Count());
                }
            }
        }

        [TestMethod]
        public void Add_WithExistingTransaction_ShouldAddPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var plant = CreateTestPlant(context);
                    var result = manager.Add(plant);
                    transaction.Commit();

                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, context.PlantSet.Count());
                }
            }
        }

        [TestMethod]
        public async Task GetTotalCountAsync_NoPlants_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_NoPlants_ShouldReturnZero()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = manager.GetTotalCount();

                Assert.AreEqual(0, result);
            }
        }


        [TestMethod]
        public async Task GetTotalCountAsync_WithPlants_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add some plants
                await manager.AddAsync(await CreateTestPlantAsync(context));
                await manager.AddAsync(await CreateTestPlantAsync(context));
                await manager.AddAsync(await CreateTestPlantAsync(context));
                  
                var result = await manager.GetTotalCountAsync();

                Assert.AreEqual(3, result);
            }
        }
        [TestMethod]
        public void GetTotalCount_WithPlants_ShouldReturnCorrectCount()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add some plants
                manager.Add(CreateTestPlant(context));
                manager.Add(CreateTestPlant(context));
                manager.Add(CreateTestPlant(context));

                var result = manager.GetTotalCount();

                Assert.AreEqual(3, result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_NoPlants_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = await manager.GetMaxIdAsync();

                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public void GetMaxId_NoPlants_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = manager.GetMaxId();

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task GetMaxIdAsync_WithPlants_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add some plants
                var plant1 = await CreateTestPlantAsync(context);
                var plant2 = await CreateTestPlantAsync(context);
                var plant3 = await CreateTestPlantAsync(context);
                 
                await manager.AddAsync(plant1);
                await manager.AddAsync(plant2);
                await manager.AddAsync(plant3);

                var result = await manager.GetMaxIdAsync();

                var maxId = new[] { plant1.PlantID, plant2.PlantID, plant3.PlantID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }

        [TestMethod]
        public void GetMaxId_WithPlants_ShouldReturnMaxId()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add some plants
                var plant1 = CreateTestPlant(context);
                var plant2 = CreateTestPlant(context);
                var plant3 = CreateTestPlant(context);

                manager.Add(plant1);
                manager.Add(plant2);
                manager.Add(plant3);

                var result = manager.GetMaxId();

                var maxId = new[] { plant1.PlantID, plant2.PlantID, plant3.PlantID }.Max();

                Assert.AreEqual(maxId, result);
            }
        }


        [TestMethod]
        public async Task GetByIdAsync_ExistingPlant_ShouldReturnCorrectPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plantToAdd = await CreateTestPlantAsync(context);

                await manager.AddAsync(plantToAdd);

                var fetchedPlant = await manager.GetByIdAsync(plantToAdd.PlantID);

                Assert.IsNotNull(fetchedPlant);
                Assert.AreEqual(plantToAdd.PlantID, fetchedPlant.PlantID);
            }
        }

        [TestMethod]
        public void GetById_ExistingPlant_ShouldReturnCorrectPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plantToAdd = CreateTestPlant(context);

                manager.Add(plantToAdd);

                var fetchedPlant = manager.GetById(plantToAdd.PlantID);

                Assert.IsNotNull(fetchedPlant);
                Assert.AreEqual(plantToAdd.PlantID, fetchedPlant.PlantID);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingPlant_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlant = await manager.GetByIdAsync(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedPlant);
            }
        }
        [TestMethod]
        public void GetById_NonExistingPlant_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlant = manager.GetById(999); // Assuming 999 is a non-existing ID

                Assert.IsNull(fetchedPlant);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_ExistingPlant_ShouldReturnCorrectPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plantToAdd = await CreateTestPlantAsync(context);

                await manager.AddAsync(plantToAdd);

                var fetchedPlant = await manager.GetByCodeAsync(plantToAdd.Code.Value);

                Assert.IsNotNull(fetchedPlant);
                Assert.AreEqual(plantToAdd.Code, fetchedPlant.Code);
            }
        }
        [TestMethod]
        public void GetByCode_ExistingPlant_ShouldReturnCorrectPlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plantToAdd = CreateTestPlant(context);

                manager.Add(plantToAdd);

                var fetchedPlant = manager.GetByCode(plantToAdd.Code.Value);

                Assert.IsNotNull(fetchedPlant);
                Assert.AreEqual(plantToAdd.Code, fetchedPlant.Code);
            }
        }

        [TestMethod]
        public async Task GetByCodeAsync_NonExistingPlant_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlant = await manager.GetByCodeAsync(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedPlant);
            }
        }
        [TestMethod]
        public void GetByCode_NonExistingPlant_ShouldReturnNull()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlant = manager.GetByCode(Guid.NewGuid()); // Random new GUID

                Assert.IsNull(fetchedPlant);
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
                var manager = new PlantManager(context);

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
                var manager = new PlantManager(context);

                manager.GetByCode(Guid.Empty);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_MultiplePlants_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = await CreateTestPlantAsync(context);
                var plant2 = await CreateTestPlantAsync(context);
                var plant3 = await CreateTestPlantAsync(context);

                await manager.AddAsync(plant1);
                await manager.AddAsync(plant2);
                await manager.AddAsync(plant3);

                var fetchedPlants = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedPlants);
                Assert.AreEqual(3, fetchedPlants.Count());
            }
        }
        [TestMethod]
        public void GetAll_MultiplePlants_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = CreateTestPlant(context);
                var plant2 = CreateTestPlant(context);
                var plant3 = CreateTestPlant(context);

                manager.Add(plant1);
                manager.Add(plant2);
                manager.Add(plant3);

                var fetchedPlants = manager.GetAll();

                Assert.IsNotNull(fetchedPlants);
                Assert.AreEqual(3, fetchedPlants.Count());
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlants = await manager.GetAllAsync();

                Assert.IsNotNull(fetchedPlants);
                Assert.AreEqual(0, fetchedPlants.Count());
            }
        }
        [TestMethod]
        public void GetAll_EmptyDatabase_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var fetchedPlants = manager.GetAll();

                Assert.IsNotNull(fetchedPlants);
                Assert.AreEqual(0, fetchedPlants.Count());
            }
        }


        [TestMethod]
        public async Task UpdateAsync_ValidPlant_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);

                await manager.AddAsync(plant);

                plant.Code = Guid.NewGuid();
                var updateResult = await manager.UpdateAsync(plant);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(plant.Code, context.PlantSet.Find(plant.PlantID).Code);
            }
        }
        [TestMethod]
        public void Update_ValidPlant_ShouldReturnTrue()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);

                manager.Add(plant);

                plant.Code = Guid.NewGuid();
                var updateResult = manager.Update(plant);

                Assert.IsTrue(updateResult);
                Assert.AreEqual(plant.Code, context.PlantSet.Find(plant.PlantID).Code);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ConcurrentUpdate_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context); 

                // Arrange
                var plant = await CreateTestPlantAsync(context);
                await manager.AddAsync(plant);
                var firstInstance = await manager.GetByIdAsync(plant.PlantID);
                var secondInstance = await manager.GetByIdAsync(plant.PlantID);

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
                var manager = new PlantManager(context);

                // Arrange
                var plant = CreateTestPlant(context);
                manager.Add(plant);
                var firstInstance = manager.GetById(plant.PlantID);
                var secondInstance = manager.GetById(plant.PlantID);

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
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);
                //context.PlantSet.Add(plant);
                //await context.SaveChangesAsync();
                await manager.AddAsync(plant);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    plant.Code = Guid.NewGuid();
                    var updateResult = await manager.UpdateAsync(plant);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPlant = freshContext.PlantSet.Find(plant.PlantID);
                    Assert.AreNotEqual(plant.Code, freshPlant.Code); // Because the transaction was not committed.
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
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);
                //context.PlantSet.Add(plant);
                //context.SaveChanges();
                manager.Add(plant);

                using (var transaction = context.Database.BeginTransaction())
                {
                    plant.Code = Guid.NewGuid();
                    var updateResult = manager.Update(plant);

                    Assert.IsTrue(updateResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPlant = freshContext.PlantSet.Find(plant.PlantID);
                    Assert.AreNotEqual(plant.Code, freshPlant.Code); // Because the transaction was not committed.
                }
            }
        }


        [TestMethod]
        public async Task DeleteAsync_ValidId_ShouldReturnTrueAndDeletePlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);

                await manager.AddAsync(plant);

                var deleteResult = await manager.DeleteAsync(plant.PlantID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.PlantSet.Find(plant.PlantID));
            }
        }
        [TestMethod]
        public void Delete_ValidId_ShouldReturnTrueAndDeletePlant()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);

                manager.Add(plant);

                var deleteResult = manager.Delete(plant.PlantID);

                Assert.IsTrue(deleteResult);
                Assert.IsNull(context.PlantSet.Find(plant.PlantID));
            }
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidId_ShouldReturnFalse()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

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
                var manager = new PlantManager(context);

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
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);

                await manager.AddAsync(plant);

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var deleteResult = await manager.DeleteAsync(plant.PlantID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPlant = freshContext.PlantSet.Find(plant.PlantID);
                    Assert.IsNotNull(freshPlant);  // Because the transaction was not committed.
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
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);

                manager.Add(plant);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var deleteResult = manager.Delete(plant.PlantID);

                    Assert.IsTrue(deleteResult);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    var freshPlant = freshContext.PlantSet.Find(plant.PlantID);
                    Assert.IsNotNull(freshPlant);  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkInsertAsync_ValidPlants_ShouldInsertAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                await manager.BulkInsertAsync(plants);

                Assert.AreEqual(plants.Count, context.PlantSet.Count());
                foreach (var plant in plants)
                {
                    Assert.IsNotNull(context.PlantSet.Find(plant.PlantID));
                }
            }
        }
        [TestMethod]
        public void BulkInsert_ValidPlants_ShouldInsertAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                manager.BulkInsert(plants);

                Assert.AreEqual(plants.Count, context.PlantSet.Count());
                foreach (var plant in plants)
                {
                    Assert.IsNotNull(context.PlantSet.Find(plant.PlantID));
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkInsertAsync(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.PlantSet.Count());  // Because the transaction was not committed.
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkInsert(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    Assert.AreEqual(0, freshContext.PlantSet.Count());  // Because the transaction was not committed.
                }
            }
        }

        [TestMethod]
        public async Task BulkUpdateAsync_ValidUpdates_ShouldUpdateAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add initial plants
                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                var plantsToUpdate = new List<Plant>();

                foreach (var plant in plants)
                {
                    plantsToUpdate.Add(await manager.AddAsync(plant));
                }

                // Update plants
                foreach (var plant in plantsToUpdate)
                {
                    plant.Code = Guid.NewGuid();
                }

                await manager.BulkUpdateAsync(plantsToUpdate);

                // Verify updates
                foreach (var updatedPlant in plantsToUpdate)
                {
                    var plantFromDb = await manager.GetByIdAsync(updatedPlant.PlantID);// context.PlantSet.Find(updatedPlant.PlantID);
                    Assert.AreEqual(updatedPlant.Code, plantFromDb.Code);
                }
            }
        }
        [TestMethod]
        public void BulkUpdate_ValidUpdates_ShouldUpdateAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add initial plants
                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                var plantsToUpdate = new List<Plant>();

                foreach (var plant in plants)
                {
                    plantsToUpdate.Add(manager.Add(plant));
                }

                // Update plants
                foreach (var plant in plantsToUpdate)
                {
                    plant.Code = Guid.NewGuid();
                }

                manager.BulkUpdate(plantsToUpdate);

                // Verify updates
                foreach (var updatedPlant in plantsToUpdate)
                {
                    var plantFromDb = manager.GetById(updatedPlant.PlantID);// context.PlantSet.Find(updatedPlant.PlantID);
                    Assert.AreEqual(updatedPlant.Code, plantFromDb.Code);
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
        //        var manager = new PlantManager(context);

        //        var plants = new List<Plant>
        //        {
        //            await CreateTestPlantAsync(context),
        //            await CreateTestPlantAsync(context),
        //            await CreateTestPlantAsync(context)
        //        };

        //        foreach (var plant in plants)
        //        {
        //            await manager.AddAsync(plant);
        //        }

        //        foreach (var plant in plants)
        //        {
        //            plant.LastChangeCode = Guid.NewGuid();
        //        }
        //        await manager.BulkUpdateAsync(plants);  // This should throw a concurrency exception
        //    }
        //}



        [TestMethod]
        public async Task BulkUpdateAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                foreach (var plant in plants)
                {
                    await manager.AddAsync(plant);
                }

                foreach (var plant in plants)
                {
                    plant.Code = Guid.NewGuid();
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkUpdateAsync(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var plant in plants)
                    {
                        var plantFromDb = freshContext.PlantSet.Find(plant.PlantID);
                        Assert.AreNotEqual(plant.Code, plantFromDb.Code);  // Names should not match as the transaction wasn't committed.
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                foreach (var plant in plants)
                {
                    manager.Add(plant);
                }

                foreach (var plant in plants)
                {
                    plant.Code = Guid.NewGuid();
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkUpdate(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if changes persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var plant in plants)
                    {
                        var plantFromDb = freshContext.PlantSet.Find(plant.PlantID);
                        Assert.AreNotEqual(plant.Code, plantFromDb.Code);  // Names should not match as the transaction wasn't committed.
                    }
                }
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_ValidDeletes_ShouldDeleteAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add initial plants
                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                foreach (var plant in plants)
                {
                    await manager.AddAsync(plant);
                }

                // Delete plants
                await manager.BulkDeleteAsync(plants);

                // Verify deletions
                foreach (var deletedPlant in plants)
                {
                    var plantFromDb = context.PlantSet.Find(deletedPlant.PlantID);
                    Assert.IsNull(plantFromDb);
                }
            }
        }
        [TestMethod]
        public void BulkDelete_ValidDeletes_ShouldDeleteAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                // Add initial plants
                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                foreach (var plant in plants)
                {
                    manager.Add(plant);
                }

                // Delete plants
                manager.BulkDelete(plants);

                // Verify deletions
                foreach (var deletedPlant in plants)
                {
                    var plantFromDb = context.PlantSet.Find(deletedPlant.PlantID);
                    Assert.IsNull(plantFromDb);
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                foreach (var plant in plants)
                {
                    await manager.AddAsync(plant);
                }

                foreach (var plant in plants)
                {
                    plant.LastChangeCode = Guid.NewGuid();
                }
                await manager.BulkDeleteAsync(plants);  // This should throw a concurrency exception due to token mismatch
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                foreach (var plant in plants)
                {
                    manager.Add(plant);
                }

                foreach (var plant in plants)
                {
                    plant.LastChangeCode = Guid.NewGuid();
                }
                manager.BulkDelete(plants);  // This should throw a concurrency exception due to token mismatch
            }
        }

        [TestMethod]
        public async Task BulkDeleteAsync_WithExistingTransaction_ShouldUseExistingTransaction()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context),
                    await CreateTestPlantAsync(context)
                };

                foreach (var plant in plants)
                {
                    await manager.AddAsync(plant);
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    await manager.BulkDeleteAsync(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var plant in plants)
                    {
                        var plantFromDb = freshContext.PlantSet.Find(plant.PlantID);
                        Assert.IsNotNull(plantFromDb);  // Plant should still exist as the transaction wasn't committed.
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
                var manager = new PlantManager(context);

                var plants = new List<Plant>
                {
                    CreateTestPlant(context),
                    CreateTestPlant(context),
                    CreateTestPlant(context)
                };

                foreach (var plant in plants)
                {
                    manager.Add(plant);
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    manager.BulkDelete(plants);

                    // Intentionally do not commit or rollback the transaction to ensure manager does not commit it.
                }

                // Fetch a fresh instance of the context to verify if deletions persisted.
                using (var freshContext = new FarmDbContext(options))
                {
                    foreach (var plant in plants)
                    {
                        var plantFromDb = freshContext.PlantSet.Find(plant.PlantID);
                        Assert.IsNotNull(plantFromDb);  // Plant should still exist as the transaction wasn't committed.
                    }
                }
            }
        }
        //ENDSET


        [TestMethod]//LandID
        public async Task GetByLandIdAsync_ValidLandId_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);
                //plant.LandID = 1;
                //context.PlantSet.Add(plant);
                //await context.SaveChangesAsync();
                await manager.AddAsync(plant);

                var result = await manager.GetByLandAsync(plant.LandID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        }

        [TestMethod]//FlvrForeignKeyID
        public async Task GetByFlvrForeignKeyAsync_ValidFlvrForeignKeyID_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context);
                // plant.FlvrForeignKeyID = 1;
                //context.PlantSet.Add(plant);
                //await context.SaveChangesAsync();
                await manager.AddAsync(plant);

                var result = await manager.GetByFlvrForeignKeyAsync(plant.FlvrForeignKeyID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        }

        //ENDSET

        [TestMethod]//LandID
        public void GetByLandId_ValidLandId_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);
                //plant.LandID = 1;
                //context.PlantSet.Add(plant);
                //context.SaveChanges();
                manager.Add(plant);

                var result = manager.GetByLand(plant.LandID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        }

        [TestMethod]//FlvrForeignKeyID
        public void GetByFlvrForeignKey_ValidFlvrForeignKeyID_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context);
                // plant.FlvrForeignKeyID = 1;
                //context.PlantSet.Add(plant);
                //context.SaveChanges();
                manager.Add(plant);

                var result = manager.GetByFlvrForeignKey(plant.FlvrForeignKeyID.Value);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        }

        //ENDSET


        [TestMethod] //LandID
        public async Task GetByLandIdAsync_InvalidLandId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = await manager.GetByLandAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]//FlvrForeignKeyID
        public async Task GetByFlvrForeignKeyAsync_InvalidFlvrForeignKeyID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = await manager.GetByFlvrForeignKeyAsync(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        //ENDSET


        [TestMethod] //LandID
        public void GetByLandId_InvalidLandId_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = manager.GetByLand(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]//FlvrForeignKeyID
        public void GetByFlvrForeignKey_InvalidFlvrForeignKeyID_ShouldReturnEmptyList()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var result = manager.GetByFlvrForeignKey(100);  // ID 100 is not added to the database
                Assert.AreEqual(0, result.Count);
            }
        }

        //ENDSET


        [TestMethod] //LandID
        public async Task GetByLandIdAsync_MultiplePlantsSameLandId_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = await CreateTestPlantAsync(context);
                var plant2 = await CreateTestPlantAsync(context);
                plant2.LandID = plant1.LandID;

                await manager.AddAsync(plant1);
                await manager.AddAsync(plant2);

                //context.PlantSet.AddRange(plant1, plant2);
                //await context.SaveChangesAsync();

                var result = await manager.GetByLandAsync(plant1.LandID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //FlvrForeignKeyID
        public async Task GetByFlvrForeignKeyAsync_MultiplePlantsSameFlvrForeignKeyID_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = await CreateTestPlantAsync(context);
                //  plant1.FlvrForeignKeyID = 1;
                var plant2 = await CreateTestPlantAsync(context);
                plant2.FlvrForeignKeyID = plant1.FlvrForeignKeyID;

                //context.PlantSet.AddRange(plant1, plant2);
                //await context.SaveChangesAsync();

                await manager.AddAsync(plant1);
                await manager.AddAsync(plant2);

                var result = await manager.GetByFlvrForeignKeyAsync(plant1.FlvrForeignKeyID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        //ENDSET

        [TestMethod] //LandID
        public void GetByLandId_MultiplePlantsSameLandId_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = CreateTestPlant(context);
                var plant2 = CreateTestPlant(context);
                plant2.LandID = plant1.LandID;

                manager.Add(plant1);
                manager.Add(plant2);

                //context.PlantSet.AddRange(plant1, plant2);
                //context.SaveChanges();

                var result = manager.GetByLand(plant1.LandID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod] //FlvrForeignKeyID
        public void GetByFlvrForeignKey_MultiplePlantsSameFlvrForeignKeyID_ShouldReturnAllPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant1 = CreateTestPlant(context);
                //  plant1.FlvrForeignKeyID = 1;
                var plant2 = CreateTestPlant(context);
                plant2.FlvrForeignKeyID = plant1.FlvrForeignKeyID;

                //context.PlantSet.AddRange(plant1, plant2);
                //context.SaveChanges();

                manager.Add(plant1);
                manager.Add(plant2);

                var result = manager.GetByFlvrForeignKey(plant1.FlvrForeignKeyID.Value);
                Assert.AreEqual(2, result.Count);
            }
        }

        //ENDSET

        private async Task<Plant> CreateTestPlantAsync(FarmDbContext dbContext)
        {
            return await PlantFactory.CreateAsync(dbContext);
        }

        private Plant CreateTestPlant(FarmDbContext dbContext)
        {
            return PlantFactory.Create(dbContext);
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