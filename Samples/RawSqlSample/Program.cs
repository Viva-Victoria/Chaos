using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivaVictoria.Chaos;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Console;
using VivaVictoria.Chaos.PostgreSql;
using VivaVictoria.Chaos.RawSql;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * Register Chaos components via DependencyInjection mechanism.
             * All Chaos subprojects provide Register* method for registering all dependencies.
             * Check Chaos.PostgreSQL and Chaos.Logging.Console README for more information.
             * RegisterChaosRawSql requires path to your migrations directory, described in csproj file.
             */
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .RegisterChaosPostgres()
                .RegisterChaosConsoleLogger()
                .RegisterChaosRawSql("Migrations");
            
            /*
             * Now you can build Chaos via ChaosBuilder
             */
            var chaos = new ChaosBuilder()
                .Resolve(services.BuildServiceProvider())
                .Build();
            
            /*
             * Start migration process. If current database version is less than your last migration's version,
             * all new migrations will be applied. Check Chaos README for more information.
             */
            chaos.Up();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}