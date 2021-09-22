using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using texo.data.Exceptions;
using texo.data.Services;
using Xunit;

namespace texo.tests.data.Services
{
    [ExcludeFromCodeCoverage]
    public class DatabaseBootstrapTests
    {
        private static DatabaseBootstrap getBootstrap(string connectionString)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger = logFactory.CreateLogger<DatabaseBootstrap>();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", connectionString },
                })
                .Build();

            return new DatabaseBootstrap(logger, configuration);
        }

        [Fact]
        public void TestSetup()
        {
            var dbBootStrap = getBootstrap("Data Source=:memory:");
            dbBootStrap.Setup();
            Assert.NotNull(dbBootStrap.GetConnection());
        }

        [Fact]
        public void TestEmptyConnectionString()
        {
            Assert.Throws<MissingConfigurationException>(() => { getBootstrap(""); });
        }

        [Fact]
        public void TestBadSQL()
        {
            var dbBootStrap = getBootstrap("Data Source=:memory:");
            var previousContent = File.ReadAllText(dbBootStrap.SetupDatabaseFile);
            File.WriteAllText(dbBootStrap.SetupDatabaseFile, "BAD SQL;");
            Assert.Throws<SqliteException>(() => { dbBootStrap.Setup(); });
            File.WriteAllText(dbBootStrap.SetupDatabaseFile, previousContent);
        }
    }
}