using System.Data;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface IConnectionProvider
    {
        public ConnectionWrapper Wrap(IDbConnection connection);
    }
}