using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Models
{
    public class WrapperSettings : IWrapperSettings
    {
        public string Format { get; set; }
        public bool FormatParameters { get; set; }
        public string Null { get; set; }
        public LogLevel Level { get; set; }
    }
}