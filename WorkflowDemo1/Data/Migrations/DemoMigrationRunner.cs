using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace WorkflowDemo1.Data.Migrations
{
    public class DemoMigrationRunner
    {
        private const string ConnectionStringName = "appConString";

        /// <summary>
        /// Load web.config from a diferent context (like a script)
        /// </summary>
        /// <returns></returns>
        private static Configuration GetConfiguration()
        {
            var path = Path.GetFullPath(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "..\\Web.config"));
            var configFile = new FileInfo(path);
            var vdm = new VirtualDirectoryMapping(configFile.DirectoryName, true, configFile.Name);
            var wcfm = new WebConfigurationFileMap();
            wcfm.VirtualDirectories.Add("/", vdm);
            return WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");
        }

        private static IServiceProvider CreateServices()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];

            if (connectionString == null)
            {
                var config = GetConfiguration();
                connectionString = config.ConnectionStrings.ConnectionStrings[ConnectionStringName];
            }

            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer2014()
                    .WithGlobalConnectionString(connectionString.ConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(DemoMigrationRunner).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void Run(Action<IMigrationRunner> action)
        {
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                // Instantiate the runner
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                action(runner);
            }
        }

        public static void Up()
        {
            Run(runner => runner.MigrateUp());
        }

        public static void RollBack()
        {
            Run(runner => runner.Rollback(1));
        }

        public static void AllDown()
        {
            Run(runner => runner.MigrateDown(0));
        }
    }
}