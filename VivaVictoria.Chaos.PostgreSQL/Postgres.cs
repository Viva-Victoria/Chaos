using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using VivaVictoria.Chaos.Logging.Db;

namespace VivaVictoria.Chaos.PostgreSQL
{
    public class Postgres
    {
        private string connectionString;
        private ILogger logger;

        public Postgres(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        private IDbConnection Connect()
        {
            return new Connection(logger, new NpgsqlConnection(connectionString));
        }

        public bool CheckTableExists(string tableName)
        {
            return Query<bool>("select true from information_schema.tables where table_name = @tableName",
                new {tableName});
        }

        public T Query<T>(string format, object param = null)
        {
            using var conn = Connect();
            conn.Open();
            return conn.Query<T>(format, param).FirstOrDefault();
        }

        public void Execute(string format, object param = null)
        {
            using var conn = Connect();
            conn.Open();
            conn.Execute(format, param);
        }

        public void ExecuteWithTransaction(string format, object param = null)
        {
            using var conn = Connect();
            conn.Open();

            using var transaction = conn.BeginTransaction();
            conn.Execute(format, param, transaction);
            transaction.Commit();
        }
    }
}