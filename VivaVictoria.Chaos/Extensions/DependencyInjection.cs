using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosCore(this IServiceCollection services)
        {
            return services.AddSingleton<IChaos, Chaos>();
        }
    }
}