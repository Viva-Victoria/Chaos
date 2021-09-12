using System.Data;

namespace VivaVictoria.Chaos.Dapper.Interfaces
{
    public interface IConnectionProvider
    {
        public IDbConnection Connect(string connectionString);
    }
}