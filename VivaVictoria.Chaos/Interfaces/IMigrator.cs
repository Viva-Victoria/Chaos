using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrator
    {
        public void Prepare(string connectionString, IMetadata metadata, ILogger logger);
        public long GetVersion();
        public void Apply(string migration);
        public void ApplyInTransaction(string migration);
        public void SetVersion(long version);
    }
}