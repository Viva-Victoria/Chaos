using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.Reflection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosReflection(this IServiceCollection collection, Assembly assembly)
        {
            return collection.AddTransient<IMigrationReader, ReflectMigrationReader>(p =>
                new ReflectMigrationReader(assembly));
        }
    }
}