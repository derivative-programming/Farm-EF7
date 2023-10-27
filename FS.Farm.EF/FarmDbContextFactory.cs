using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Data.SqlClient;

namespace FS.Farm.EF
{
    public static class FarmDbContextFactory 
    { 
        public static FarmDbContext Create(string connectionString)
        {  
            var options = new DbContextOptionsBuilder<FarmDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new FarmDbContext(options);
        }
        public static FarmDbContext Create(SqlConnection connection)
        { 
            var options = new DbContextOptionsBuilder<FarmDbContext>()
                .UseSqlServer(connection)
                .Options; 

            return new FarmDbContext(options);
        }

    }
}