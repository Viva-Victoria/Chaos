using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Console.Extensions;
using VivaVictoria.Chaos.Postgres.Extensions;
using VivaVictoria.Chaos.RawSqlReader.Extensions;
using VivaVictoria.Chaos.Sql.Models;
using VivaVictoria.Chaos.Sql.Extensions;

namespace RawSqlSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .AddChaosPostgres()
                .AddChaosConsoleLogger()
                .AddChaosRawSql("Migrations")
                .AddChaosCore();

            using (var scope = services.BuildServiceProvider())
            {
                var chaos = scope.GetChaos<Migration>();
                chaos.Init().Migrate();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}