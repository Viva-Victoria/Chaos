using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.RawSqlReader.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosRawSql(this IServiceCollection collection, string path)
        {
            return collection.AddTransient<IMigrationReader<Migration>, RawSqlMigrationReader>(p => new RawSqlMigrationReader(path));
        }
    }
}