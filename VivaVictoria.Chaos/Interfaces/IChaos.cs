using System;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IChaos<TMigration>
        where TMigration : IMigration
    {
        /// <summary>Initializes Chaos</summary>
        public IChaos<TMigration> Init(Func<bool> condition = null);
        /// <returns>true if <code>Init</code> condition executed and returns true</returns>
        public bool IsReady();
        /// <summary>Migrate database to newest version</summary>
        public void Migrate();
        /// <summary>Migrate database to specified version, can upgrade or downgrade structure</summary>
        public void Migrate(long targetVersion);
    }
}