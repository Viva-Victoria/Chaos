using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Extensions;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.Dapper.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosDapper<TMetadata>(this IServiceCollection collection)
            where TMetadata : IMetadata
        {
            return collection
                .AddChaosConnectionLogging()
                .AddTransient<IMigrator<Migration>, DapperMigrator<TMetadata>>();
        }
    }
}