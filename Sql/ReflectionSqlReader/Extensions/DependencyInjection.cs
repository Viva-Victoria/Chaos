using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ReflectionSqlReader.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosReflection(this IServiceCollection collection, Assembly assembly)
        {
            return collection.AddTransient<IMigrationReader<Migration>, ReflectMigrationReader>(p =>
                new ReflectMigrationReader(assembly));
        }
    }
}