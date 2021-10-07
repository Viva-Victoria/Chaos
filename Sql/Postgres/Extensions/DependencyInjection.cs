using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper.Extensions;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Postgres.Models;

namespace VivaVictoria.Chaos.Postgres.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosPostgres(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IMetadata, PostgresMetadata>()
                .AddSingleton<IDatabaseDriver<PostgresMetadata>, PostgresDriver>()
                .AddChaosDapper<PostgresMetadata>();
        }

        public static IServiceCollection AddChaosPostgres<TMetadata>(
            this IServiceCollection collection)
            where TMetadata : PostgresMetadata
        {
            return collection
                .AddSingleton<IMetadata, TMetadata>()
                .AddSingleton<IDatabaseDriver<PostgresMetadata>, PostgresDriver>()
                .AddChaosDapper<PostgresMetadata>();
        }
    }
}