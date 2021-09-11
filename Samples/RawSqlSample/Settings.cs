using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace WebApplication
{
    public class Settings : ISettings
    {
        /*
         * ConnectionString for your db driver, like Npgsql for PostgreSQL or ODBC for MSSQL 
         */
        public string ConnectionString =>
            "Host=192.168.5.1;Port=49002;Username=postgres;Password=2ytbJ34eaJPP4zYe;SearchPath=public";

        /*
         * Provides default TransactionMode for Chaos.
         * This value will be used if MigrationInfo.TransactionMode is TransactionMode.Default.
         * It also used for every raw-sql migration.
         */
        public TransactionMode TransactionMode => 
            TransactionMode.One;
    }
}