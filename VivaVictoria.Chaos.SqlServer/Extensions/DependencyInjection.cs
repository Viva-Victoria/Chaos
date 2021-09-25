using System;
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

        public static IServiceCollection AddChaosSqlServer<TMetadata>(this IServiceCollection collection)
            where TMetadata : SqlServerMetadata
        {
            return collection
                .AddSingleton<IMetadata, TMetadata>()
                .AddSingleton<IDatabaseDriver<SqlServerMetadata>, SqlServerDriver>()
                .AddChaosDapper<SqlServerMetadata>();
        }
    }
}