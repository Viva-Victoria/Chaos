using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSql
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosPostgres(this IServiceCollection collection)
        {
            return collection.AddChaosPostgres<PostgresMetadata, PostgresProvider>();
        }

        public static IServiceCollection AddChaosPostgres<TMetadata>(this IServiceCollection collection)
            where TMetadata : PostgresMetadata
        {
            return collection.AddChaosPostgres<TMetadata, PostgresProvider>();
        }

        public static IServiceCollection AddChaosPostgres<TMetadata, TConnectionProvider>(this IServiceCollection collection)
            where TMetadata : PostgresMetadata
            where TConnectionProvider : PostgresProvider
        {
            return collection
                .AddTransient<IMetadata, TMetadata>()
                .AddTransient<IConnectionProvider, TConnectionProvider>()
                .AddChaosDapper();
        }
    }
}