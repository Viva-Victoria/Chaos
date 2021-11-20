using System.Collections.Generic;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface ISettings
    {
        /// <summary>Allows parallel listener event broadcasting</summary>
        public bool ParallelListeners { get; }
        /// <summary>Specifies states to be saved by <see cref="IMigrator{TMigration}"/></summary>
        public IEnumerable<MigrationState> SaveStates { get; }
    }
}