using System.Data.Common;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using texo.data.Exceptions;
using texo.data.Interfaces;


namespace texo.data.Services
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly string _dbConnectionString;
        private readonly ILogger<DatabaseBootstrap> _logger;
        public readonly string SetupDatabaseFile;

        public DatabaseBootstrap(ILogger<DatabaseBootstrap> logger, IConfiguration configuration)
        {
            _logger = logger;
            _dbConnectionString = configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(_dbConnectionString))
                throw new MissingConfigurationException("Connection String");
            SetupDatabaseFile = Path.Join(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "SQL",
                "setup_database.sql");
            _logger.LogInformation("INIT");
        }

        public void Setup()
        {
            using var connection = GetConnection();
            var sql = File.ReadAllText(SetupDatabaseFile);
            try
            {
                connection.Execute(sql);
                _logger.LogInformation("Setup database is done");
            }
            catch (SqliteException exc)
            {
                _logger.LogError(exc, "Failed to setup database");
                throw;
            }
        }

        public DbConnection GetConnection()
        {
            return new SqliteConnection(_dbConnectionString);
        }
    }
}