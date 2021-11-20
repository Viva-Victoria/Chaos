using System.Data;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// Provides <see cref="ConnectionWrapper"/> instance via DI
    /// </summary>
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ICommandProvider commandProvider;

        public ConnectionProvider(ICommandProvider commandProvider)
        {
            this.commandProvider = commandProvider;
        }

        public ConnectionWrapper Wrap(IDbConnection connection)
        {
            return new ConnectionWrapper(commandProvider, connection);
        }
    }
}