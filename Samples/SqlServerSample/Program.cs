using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Console;
using VivaVictoria.Chaos.Logging.Console.Extensions;
using VivaVictoria.Chaos.RawSql;
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
                .AddChaosConsoleLogger()
                .AddChaosRawSql("Migrations")
                .AddChaosCore();

            using (var scope = services.BuildServiceProvider())
            {
                var chaos = scope.GetRequiredService<IChaos>();
                chaos.Init().Up();
            }
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}