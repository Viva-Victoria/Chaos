using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface IWrapperSettings
    {
        public string Format { get; }
        public bool FormatParameters { get; }
        public string Null { get; }
        public LogLevel Level { get; }
    }
}