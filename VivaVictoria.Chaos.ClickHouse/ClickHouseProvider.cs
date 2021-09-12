using System.Data;
using ClickHouse.Ado;
using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseProvider : IConnectionProvider
    {
        public IDbConnection Connect(string connectionString)
        {
            return new ClickHouseConnection(connectionString);
        }
    }
}