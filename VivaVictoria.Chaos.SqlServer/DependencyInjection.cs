using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.SqlServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterChaosSqlServer(this IServiceCollection collection)
        {
            return collection
                .AddTransient<IMetadata, SqlServerMetadata>()
                .AddTransient<IConnectionProvider, SqlServerProvider>()
                .AddTransient<IMigrator, DapperMigrator>();
        }
    }
}