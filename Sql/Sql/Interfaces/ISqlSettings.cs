using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;

namespace VivaVictoria.Chaos.Sql.Interfaces
{
    public interface ISqlSettings : ISettings
    {
        public string ConnectionString { get; }
        public TransactionMode TransactionMode { get; }
    }
}