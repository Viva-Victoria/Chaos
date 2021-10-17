using System;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IEventListener
    {
        public void OnChaosReady()
        {

        }

        public void OnNoMigrations()
        {

        }

        public void OnUpToDate()
        {

        }

        public void OnCorrupted()
        {
            
        }

        public void OnStateChanged(long currentVersion, long migrationVersion, MigrationState state, Exception exception)
        {
            
        }
    }
}