using System.Collections.Generic;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface ICommandFormatter
    {
        public string Format(string sql, Dictionary<string, object> parameters);
    }
}