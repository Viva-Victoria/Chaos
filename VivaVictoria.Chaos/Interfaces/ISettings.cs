using System.Collections.Generic;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface ISettings
    {
        public bool ParallelListeners { get; }
        public IEnumerable<MigrationState> SaveStates { get; }
    }
}