using System;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.ClickHouse.Interfaces;
using VivaVictoria.Chaos.ClickHouse.Models;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ClickHouse.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosClickHouse(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IClickHouseMetadata, ClickHouseMetadata>()
                .AddSingleton<IMigrator<Migration>, ClickHouseMigrator>();
        }

        public static IServiceCollection AddChaosClickHouse<TMetadata>(
            this IServiceCollection collection, 
            Func<IServiceProvider, TMetadata> implementationFactory)
            where TMetadata : ClickHouseMetadata
        {
            return collection
                .AddSingleton<IClickHouseMetadata, TMetadata>(implementationFactory)
                .AddSingleton<IMigrator<Migration>, ClickHouseMigrator>();
        }
    }
}