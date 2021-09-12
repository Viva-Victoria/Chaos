using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivaVictoria.Chaos;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Console;
using VivaVictoria.Chaos.PostgreSql;
using VivaVictoria.Chaos.RawSql;

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
                .AddChaosRawSql("Migrations"); //relative path to sql scripts directory, see csproj for more info
            
            var chaos = new ChaosBuilder()
                .Resolve(services.BuildServiceProvider())
                .Build();
            chaos.Up();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}