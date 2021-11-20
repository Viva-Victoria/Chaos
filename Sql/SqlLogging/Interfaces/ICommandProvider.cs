using System.Data;

namespace VivaVictoria.Chaos.Logging.Interfaces
{
    public interface ICommandProvider
    {
        public CommandWrapper Wrap(IDbCommand command);
    }
}