using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Models;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrator<in TMigration>
        where TMigration : IMigration
    {
        public void Init();
        public Info GetInfo();
        public void SaveState(long version, MigrationState state);
        public void Apply(TMigration migration, bool downgrade);
    }
}