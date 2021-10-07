using System.Collections.Generic;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Interfaces;

namespace SqlServerSample
{
    public class Settings : ISqlSettings
    {
        public string ConnectionString =>
            "Data Source=192.168.5.1,49005;Initial Catalog=master;User Id=mssql;Password=vwjNeyCbSQdKE5QT";
        
        public TransactionMode TransactionMode => 
            TransactionMode.One;

        public bool ParallelListeners => false;
        public IEnumerable<MigrationState> SaveStates => new[] { MigrationState.Applied };
    }
}