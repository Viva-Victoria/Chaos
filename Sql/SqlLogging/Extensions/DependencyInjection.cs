using System;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChaosConnectionLogging(this IServiceCollection services, Action<WrapperBuilder> configure)
        {
            return services
                .AddSingleton(p =>
                {
                    var builder = new WrapperBuilder();
                    configure(builder);
                    return builder.Build();
                })
                .AddSingleton<ICommandFormatter, CommandFormatter>()
                .AddSingleton<ICommandProvider, CommandProvider>()
                .AddSingleton<IConnectionProvider, ConnectionProvider>();
        }
    }
}