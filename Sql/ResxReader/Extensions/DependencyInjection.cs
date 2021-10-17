using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ResxReader.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosResxReader(this IServiceCollection services, BindingFlags bindingFlags, params Type[] types)
        {
            return services
                .AddTransient<IMigrationReader<Migration>, ResxReader>(_ => new ResxReader(bindingFlags, types));
        }

        public static IServiceCollection AddChaosResxReader(this IServiceCollection services, params Type[] types)
        {
            return services
                .AddTransient<IMigrationReader<Migration>, ResxReader>(_ => new ResxReader(types));
        }

        public static IServiceCollection AddChaosResxReader(this IServiceCollection services, Func<IServiceProvider, IEnumerable<Type>> typesProvider)
        {
            return services
                .AddTransient<IMigrationReader<Migration>, ResxReader>(p => new ResxReader(typesProvider(p).ToArray()));
        }
    }
}