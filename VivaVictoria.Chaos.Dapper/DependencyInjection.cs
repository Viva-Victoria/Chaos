using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.Dapper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosDapper(this IServiceCollection collection)
        {
            return collection.AddTransient<IMigrator, DapperMigrator>();
        }
    }
}