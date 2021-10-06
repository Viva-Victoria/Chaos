using System;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IChaos<TMigration>
        where TMigration : IMigration
    {
        public IChaos<TMigration> Init(Func<bool> condition = null);
        public bool IsReady();
        public void Migrate();
        public void Migrate(long targetVersion);
    }
}