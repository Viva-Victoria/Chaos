using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterChaosClickHouse(this IServiceCollection collection)
        {
            return collection
                .AddTransient<IMetadata, ClickHouseMetadata>()
                .AddTransient<IConnectionProvider, ClickHouseProvider>()
                .AddTransient<IMigrator, DapperMigrator>();
        }
    }
}