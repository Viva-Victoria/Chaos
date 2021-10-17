using System.Data;
using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Db
{
    /*
     * Simple IDbConnection wrapper for logging commands to logger.
     * Requires real IDbConnection implementation and ILogger. Uses Command wrapper. 
     */
    public class Connection : IDbConnection
    {
        private readonly ILogger<ConnectionProvider> logger;
        private readonly IDbConnection connection;

        public Connection(ILogger<ConnectionProvider> logger, IDbConnection connection)
        {
            this.logger = logger;
            this.connection = connection;
        }

        public IDbCommand CreateCommand() => new Command(logger, connection.CreateCommand());

        public void Open() => connection.Open();

        public void Close() => connection.Close();

        public void Dispose() => connection.Dispose();

        public int ConnectionTimeout => connection.ConnectionTimeout;
        
        public string Database => connection.Database;
        
        public ConnectionState State => connection.State;

        public IDbTransaction BeginTransaction() => connection.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => connection.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => connection.ChangeDatabase(databaseName);

        public string ConnectionString
        {
            get => connection.ConnectionString; 
            set => connection.ConnectionString = value;
        }
    }
}