using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Postgres.Extensions;
using VivaVictoria.Chaos.ReflectionSqlReader.Extensions;
using VivaVictoria.Chaos.Sql.Models;

namespace ReflectionSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<ISettings, Settings>()
                .AddChaosPostgres()
                .AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddChaosCore<Migration>()
                .AddChaosReflection(typeof(Program).Assembly);
            
            using (var scope = services.BuildServiceProvider())
            {
                var chaos = scope.GetChaos<Migration>();
                chaos
                    .Init(() => !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("REPLICA_MASTER")))
                    .Migrate();
            }
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}