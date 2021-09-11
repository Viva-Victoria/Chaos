using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Models
{
    public class MigrationInfo
    {
        public MigrationInfo(long version, string name, TransactionMode transactionMode, string upScript,
            string downScript)
        {
            Version = version;
            Name = name;
            TransactionMode = transactionMode;
            UpScript = upScript;
            DownScript = downScript;
        }

        public long Version { get; }
        public string Name { get; }
        public TransactionMode TransactionMode { get; }
        public string UpScript { get; }
        public string DownScript { get; }
    }
}