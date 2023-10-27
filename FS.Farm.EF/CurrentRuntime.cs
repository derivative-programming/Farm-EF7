using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.Data.SqlClient;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;

namespace FS.Farm.EF
{
    public class CurrentRuntime 
    { 

        public static void ClearTestObjects(FarmDbContext dbContext)
        {
            int delCount = 1;

            while(delCount > 0)
            {
                delCount = 0;
                delCount = delCount + (new Managers.PlantManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.CustomerManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.CustomerRoleManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.DateGreaterThanFilterManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.ErrorLogManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.FlavorManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.LandManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.OrganizationManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.OrgApiKeyManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.OrgCustomerManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.PacManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.RoleManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.TacManager(dbContext)).ClearTestObjects();
                delCount = delCount + (new Managers.TriStateFilterManager(dbContext)).ClearTestObjects();
            }

        }

        public static void ClearTestChildObjects(FarmDbContext dbContext)
        {
            int delCount = 1;

            while (delCount > 0)
            {
                delCount = 0;
                delCount = delCount + (new Managers.PlantManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.CustomerManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.CustomerRoleManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.DateGreaterThanFilterManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.ErrorLogManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.FlavorManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.LandManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.OrganizationManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.OrgApiKeyManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.OrgCustomerManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.PacManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.RoleManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.TacManager(dbContext)).ClearTestChildObjects();
                delCount = delCount + (new Managers.TriStateFilterManager(dbContext)).ClearTestChildObjects();
            }

        }
    }
}