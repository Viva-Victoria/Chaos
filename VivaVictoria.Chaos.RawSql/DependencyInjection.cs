using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.RawSql
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosRawSql(this IServiceCollection collection, string path)
        {
            return collection.AddTransient<IMigrationReader, RawSqlMigrationReader>(p => new RawSqlMigrationReader(path));
        }
    }
}