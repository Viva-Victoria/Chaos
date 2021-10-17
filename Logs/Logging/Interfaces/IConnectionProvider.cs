using System.Data;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface IConnectionProvider
    {
        public IDbConnection Wrap(IDbConnection connection);
    }
}