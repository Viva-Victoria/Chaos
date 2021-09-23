using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper.Extensions;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.SqlServer.Models;

namespace VivaVictoria.Chaos.SqlServer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosSqlServer(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IMetadata, SqlServerMetadata>()
                .AddSingleton<IDatabaseDriver<SqlServerMetadata>, SqlServerDriver>()
                .AddChaosDapper<SqlServerMetadata>();
        }
    }
}