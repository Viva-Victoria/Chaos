using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface IChaosLogger : ILogger
    {
        public void Enable(LogLevel level) => Set(level, true);
        public void Disable(LogLevel level) => Set(level, false);
        
        /*
         * Applies new state to specified LogLevel.
         * If LogLevel setup to disabled state instance should ignores messages with this level.
         */
        public void Set(LogLevel level, bool enabled);
    }
}