using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Postgres.Extensions;
using VivaVictoria.Chaos.ResxReader.Extensions;
using VivaVictoria.Chaos.Sql.Models;
using VivaVictoria.Chaos.Sql.Extensions;

namespace ResxSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .AddLogging(logging => 
                    logging
                        .SetMinimumLevel(LogLevel.Debug)
                        .AddConsole())
                .AddChaosPostgres()
                .AddChaosResxReader(typeof(Migrations.Resources))
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