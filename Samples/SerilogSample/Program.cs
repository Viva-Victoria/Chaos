using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawSqlSample;
using Serilog;
using Serilog.Formatting.Compact;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Extensions;
using VivaVictoria.Chaos.Postgres.Extensions;
using VivaVictoria.Chaos.RawSqlReader.Extensions;
using VivaVictoria.Chaos.Sql.Extensions;

namespace SerilogSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var chaos =  host.Services.GetChaos();
            chaos.Init().Migrate();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(new RenderedCompactJsonFormatter())
                    .MinimumLevel.Debug()
                )
                .ConfigureServices((context, services) => {
                    services.AddTransient<ISettings, Settings>()
                        .AddChaosConnectionLogging(b => b.
                            Level(LogLevel.Information).
                            Format("{timestamp}\n{script}"))
                        .AddChaosPostgres()
                        .AddChaosRawSql("Migrations")
                        .AddChaosCore();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}