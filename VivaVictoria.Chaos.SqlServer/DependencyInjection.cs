using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.SqlServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosSqlServer(this IServiceCollection collection)
        {
            return collection.AddChaosSqlServer<SqlServerMetadata, SqlServerProvider>();
        }

        public static IServiceCollection AddChaosSqlServer<TMetadata>(this IServiceCollection collection)
            where TMetadata : SqlServerMetadata
        {
            return collection.AddChaosSqlServer<TMetadata, SqlServerProvider>();
        }
        
        public static IServiceCollection AddChaosSqlServer<TMetadata, TConnectionProvider>(this IServiceCollection collection)
            where TMetadata : SqlServerMetadata
            where TConnectionProvider : SqlServerProvider
        {
            return collection
                .AddTransient<IMetadata, TMetadata>()
                .AddTransient<IConnectionProvider, TConnectionProvider>()
                .AddChaosDapper();
        }
    }
}