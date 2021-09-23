using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace SqlServerSample
{
    public class Settings : ISettings
    {
        public string ConnectionString =>
            "Data Source=192.168.5.1,49005;Initial Catalog=master;User Id=mssql;Password=vwjNeyCbSQdKE5QT";
        
        public TransactionMode TransactionMode => 
            TransactionMode.One;
    }
}