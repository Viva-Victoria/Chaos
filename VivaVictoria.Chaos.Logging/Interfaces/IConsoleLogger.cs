using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface IChaosLogger : ILogger
    {
        public void Enable(LogLevel level) => Set(level, true);
        public void Disable(LogLevel level) => Set(level, false);
        public void Set(LogLevel level, bool enabled);
    }
}