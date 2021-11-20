using System.Data;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// Provides <see cref="CommandWrapper"/> instance via DI
    /// </summary>
    public class CommandProvider : ICommandProvider
    {
        private readonly ILogger<CommandWrapper> logger;
        private readonly IWrapperSettings settings;
        private readonly ICommandFormatter formatter;

        public CommandProvider(
            ILogger<CommandWrapper> logger, 
            IWrapperSettings settings, 
            ICommandFormatter formatter)
        {
            this.logger = logger;
            this.settings = settings;
            this.formatter = formatter;
        }

        public CommandWrapper Wrap(IDbCommand command)
        {
            return new CommandWrapper(logger, settings, formatter, command);
        }
    }
}