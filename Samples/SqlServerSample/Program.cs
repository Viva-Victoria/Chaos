using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.RawSqlReader.Extensions;
using VivaVictoria.Chaos.Sql.Models;
using VivaVictoria.Chaos.SqlServer.Extensions;

namespace SqlServerSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .AddChaosSqlServer<MySqlServerMetadata>()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddChaosRawSql("Migrations")
                .AddChaosCore<Migration>();

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