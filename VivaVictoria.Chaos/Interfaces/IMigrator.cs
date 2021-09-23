using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrator
    {
        public void Init();
        public long GetVersion();
        public void Apply(TransactionMode requestedMode, string migration);
        public void SetVersion(long version);
    }
}