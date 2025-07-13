using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DbContex
{
    public class DbConnectionDapper
    {
        private readonly IConfiguration _configuration;

        public DbConnectionDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection(string connectionName="DefaultConnection")
        {
            string connectionString = _configuration.GetConnectionString(connectionName);
            return new SqlConnection(connectionString);
        }

        public IDbConnection GetMainDbConnection()
        {
            return GetConnection("DefaultConnection");
        }

        public IDbConnection GetDbConnection()
        {
            //using (var mainConnection = GetMainDbConnection())
            //{

            //    string secondaryConnectionString = ConstructSecondaryConnectionString(mainConnection);
            //    return new SqlConnection(secondaryConnectionString);
            //}

            return GetConnection("DefaultConnection");
        }

        private string ConstructSecondaryConnectionString(IDbConnection mainConnection)
        {
            // Example: Query the main database for information needed to construct the secondary connection string

            string ServerName = "";
            string DatabaseName = "";
            string Username = "";
            string Password = "";

            // Example query
            using (var command = mainConnection.CreateCommand())
            {
                command.CommandText = "SELECT [ServerName],[DatabaseName],[Username],[Password]  FROM  [stt].[MainDbConfig]";
                mainConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        ServerName = reader.GetString(0);
                        DatabaseName = reader.GetString(1);
                        Username = reader.GetString(2);
                        Password = reader.GetString(3);
                    }
                }
            }

            // Construct the secondary connection string
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName,
                InitialCatalog = DatabaseName,
                UserID = Username,
                Password = Password,
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                Encrypt = false,
                MultipleActiveResultSets = true,

            };

            return builder.ConnectionString;
        }
    }
}
