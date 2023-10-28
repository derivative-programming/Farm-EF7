using Microsoft.EntityFrameworkCore;
using FS.Farm;
using FS.Farm.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.BulkExtensions;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FS.Farm.EF.Managers
{
    public partial class PlantTest
    {

        //GENINCLUDESTART
        //GENLOOPPropStart
        //GENIF[isQueryByAvailable=true]Start 
        [TestMethod] 
        public async Task GetByGENVALPropNameAsync_ValidGENVALPropName_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = await CreateTestPlantAsync(context); 
                await manager.AddAsync(plant);
                 
                //GENCASE[calculatedCSharpDataType]Start
                //GENWHEN[String]Start 
                var result = await manager.GetByGENVALPropNameAsync(plant.GENVALPropName);
                //GENWHEN[String]End 
                //GENElseStart  
                var result = await manager.GetByGENVALPropNameAsync(plant.GENVALPropName.Value);
                //GENElseEnd 
                //GENCASE[calculatedCSharpDataType]End 

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        } 

        [TestMethod] 
        public void GetByGENVALPropName_ValidGENVALPropName_ShouldReturnPlants()
        {
            var options = CreateSQLiteInMemoryDbContextOptions();

            using (var context = new FarmDbContext(options))
            {
                context.Database.EnsureCreated();
                var manager = new PlantManager(context);

                var plant = CreateTestPlant(context); 
                manager.Add(plant);
                 
                //GENCASE[calculatedCSharpDataType]Start
                //GENWHEN[String]Start 
                var result = manager.GetByGENVALPropName(plant.GENVALPropName);
                //GENWHEN[String]End 
                //GENElseStart  
                var result = manager.GetByGENVALPropName(plant.GENVALPropName.Value);
                //GENElseEnd 
                //GENCASE[calculatedCSharpDataType]End 

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(plant.PlantID, result.First().PlantID);
            }
        }
        //GENIF[isQueryByAvailable=true]End
        //GENLOOPPropEnd 
        //GENINCLUDEEND


    }
}