using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Logging.Db;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosConnectionLogging(this IServiceCollection services)
        {
            return services.AddSingleton<IConnectionProvider, ConnectionProvider>();
        }
    }
}