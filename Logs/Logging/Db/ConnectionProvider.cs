using System.Data;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Db
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ILogger<ConnectionProvider> logger;

        public ConnectionProvider(ILogger<ConnectionProvider> logger)
        {
            this.logger = logger;
        }

        public IDbConnection Wrap(IDbConnection connection)
        {
            return new Connection(logger, connection);
        }
    }
}