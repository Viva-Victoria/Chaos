using System.Data;
using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Db
{
    public class Connection : IDbConnection
    {
        private ILogger logger;
        private IDbConnection connection;

        public Connection(ILogger logger, IDbConnection connection)
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