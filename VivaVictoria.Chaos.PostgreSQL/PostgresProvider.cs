using System.Data;
using Npgsql;
using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.PostgreSql
{
    public class PostgresProvider : IConnectionProvider
    {
        public IDbConnection Connect(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}