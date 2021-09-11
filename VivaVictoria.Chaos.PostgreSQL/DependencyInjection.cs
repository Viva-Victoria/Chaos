using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterChaosPostgres(this IServiceCollection collection)
        {
            return collection
                .AddTransient<IMetadata, PostgresMetadata>()
                .AddTransient<IMigrator<PostgresMetadata>, PostgresMigrator>();
        }
    }
}