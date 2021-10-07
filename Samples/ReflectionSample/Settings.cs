using System.Collections.Generic;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Interfaces;

namespace ReflectionSample
{
    public class Settings : ISqlSettings
    {
        public string ConnectionString =>
            "Host=192.168.5.1;Port=49002;Username=postgres;Password=2ytbJ34eaJPP4zYe;SearchPath=public";

        public TransactionMode TransactionMode => 
            TransactionMode.One;

        public bool ParallelListeners => false;
        public IEnumerable<MigrationState> SaveStates => new[] { MigrationState.Started, MigrationState.Failed, MigrationState.Applied };
    }
}