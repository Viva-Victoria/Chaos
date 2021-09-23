using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper.Extensions;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.PostgreSql;
using VivaVictoria.Chaos.PostgreSQL.Models;

namespace VivaVictoria.Chaos.PostgreSQL.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosPostgres(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IMetadata, PostgresMetadata>()
                .AddSingleton<IDatabaseDriver<PostgresMetadata>, NpgsqlDriver>()
                .AddChaosDapper<PostgresMetadata>();
        }
    }
}