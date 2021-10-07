using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.Sql.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosCore(this IServiceCollection services)
        {
            return services.AddSingleton<IChaos<Migration>, Chaos<Migration>>();
        }
    }
}