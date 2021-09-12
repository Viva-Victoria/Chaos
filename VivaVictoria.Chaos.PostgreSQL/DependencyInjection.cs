﻿using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Dapper;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSql
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterChaosPostgres(this IServiceCollection collection)
        {
            return collection
                .AddTransient<IMetadata, PostgresMetadata>()
                .AddTransient<IConnectionProvider, PostgresProvider>()
                .AddTransient<IMigrator, DapperMigrator>();
        }
    }
}