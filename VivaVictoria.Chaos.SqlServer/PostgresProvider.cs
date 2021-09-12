using System.Data;
using System.Data.SqlClient;
using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.SqlServer
{
    public class SqlServerProvider : IConnectionProvider
    {
        public IDbConnection Connect(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}