using System;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.ClickHouse.Models;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosClickHouse(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IMetadata, ClickHouseMetadata>()
                .AddSingleton<IMigrator, ClickHouseMigrator>();
        }

        public static IServiceCollection AddChaosClickHouse<TMetadata>(
            this IServiceCollection collection, 
            Func<IServiceProvider, TMetadata> implementationFactory)
            where TMetadata : ClickHouseMetadata
        {
            return collection
                .AddSingleton<IMetadata, ClickHouseMetadata>(implementationFactory)
                .AddSingleton<IMigrator, ClickHouseMigrator>();
        }
    }
}