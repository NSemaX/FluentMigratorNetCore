using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using StructureMap;
using FluentMigratorTest.Migrations;

namespace FluentMigratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // add the framework services
            var services = new ServiceCollection()
                .AddFluentMigratorCore()
                // Configure the runner
                .ConfigureRunner(
                builder => builder
                // Use SQL
                .AddSqlServer2012()
                // The SQL connection string
                .WithGlobalConnectionString("Server=.;Database=JokerGames;Trusted_Connection=True;MultipleActiveResultSets=true")
                // Specify the assembly with the migrations
                .WithMigrationsIn(typeof(InitialMigration).Assembly))
                .BuildServiceProvider();



            // add StructureMap
            var container = new Container();
            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Program));
                    _.WithDefaultConventions();
                    _.AssemblyContainingType<IMigrationRunnerBuilder>();
                });

            });



            try
            {
                using (var scope = services.CreateScope())
                {
                    // Instantiate the runner
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                    // Execute the migrations
                    runner.MigrateUp();

                    //Execute the down scripts
                    //runner.RollbackToVersion(0);

                    Console.WriteLine("Migration has successfully executed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();

        }
    }
}
