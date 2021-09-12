using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivaVictoria.Chaos;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Console;
using VivaVictoria.Chaos.PostgreSql;
using VivaVictoria.Chaos.Reflection;

namespace ReflectionSample
{
    public class Program
    {
        private static Chaos BuildChaosManual()
        {
            return new ChaosBuilder()
                .Use(new Settings())
                .Use(new PostgresMetadata())
                .Use(new DapperMigrator(new PostgresProvider()))
                .Use(new ConsoleLogger())
                .Use(new ReflectMigrationReader(typeof(Program).Assembly))
                .Build();
        }

        private static Chaos BuildChaosDI()
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .AddChaosPostgres()
                .AddChaosConsoleLogger()
                .AddChaosReflection(typeof(Program).Assembly);
            
            return new ChaosBuilder()
                .Resolve(services.BuildServiceProvider())
                .Build();
        }
        
        public static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>() //add Settings
                .AddChaosPostgres()             //add PostgreSQL support
                .AddChaosConsoleLogger();       //add ConsoleLogger
            
            //add Reflection support with project Assembly
            //typeof(Program).Assembly provides reflect Assembly item, which Migrator will be used
            //to find migration classes 
            services.AddChaosReflection(typeof(Program).Assembly);
            
            var chaos = new ChaosBuilder()
                .Resolve(services.BuildServiceProvider()) //just resolve all services from DI container
                .Build();                                 //build Chaos from ChaosBuilder
            chaos.Up(); //migrate database to newest version
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}