using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface ISettings
    {
        public string ConnectionString { get; }
        public TransactionMode TransactionMode { get; }
    }
}