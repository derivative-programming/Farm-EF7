using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using FS.Common.Objects;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FS.Farm.Reports.Providers.SqlServer
{
    partial class EF7FarmReportProvider : FS.Farm.Reports.Providers.FarmReportProvider
    {
        string _connectionString = "";
        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = configValue["connectionString"].ToString();
        }
        public override string Name
        {
            get
            {
                return null;
            }
        }
        #endregion
        #region Error Handling
        void HandleError(Exception x, string sprocName)
        {
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            Log(x); 
            throw new Exception(sException, x);
        }
        async Task HandleErrorAsyc(SessionContext sessionContext,  Exception x, string sprocName)
        {
            string sException = "Error Executing " + sprocName + ": " + x.Message + " \r\n";
            await LogAsync(sessionContext, x); 
            throw new Exception(sException, x);
        }
        #endregion

        private async Task<EF.FarmDbContext> BuildDbContextAsync(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                await dbContext.Database.UseTransactionAsync(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }

        private EF.FarmDbContext BuildDbContext(SessionContext context)
        {
            EF.FarmDbContext dbContext = null;
            SqlConnection connection = null;
            if (context.UseTransactions)
            {
                if (!context.SqlConnectionExists(_connectionString))
                {
                    if (_connectionString == null || _connectionString.Length == 0) throw new ArgumentNullException("connectionString");

                    connection = new SqlConnection(_connectionString);
                     connection.Open();
                    context.AddConnection(_connectionString, connection, connection.BeginTransaction());
                }
                else
                {
                    connection = context.GetSqlConnection(_connectionString);
                }

                dbContext = EF.FarmDbContextFactory.Create(connection);
                dbContext.Database.UseTransaction(context.GetSqlTransaction(_connectionString));
            }
            else
            {
                dbContext = EF.FarmDbContextFactory.Create(_connectionString);
            }

            return dbContext;
        }
    }
}
