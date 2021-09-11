using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Console
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterChaosConsoleLogger(this IServiceCollection collection, Action<IChaosLogger> configure = null)
        {
            return collection.AddSingleton<ILogger, ConsoleLogger>(p =>
            {
                var logger = new ConsoleLogger();
                configure?.Invoke(logger);
                return logger;
            });
        }
    }
}