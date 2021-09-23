using System;
using System.Windows.Input;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Models
{
    public class Migration : IComparable<Migration>
    {
        public Migration(long version, string name, TransactionMode transactionMode, string upScript,
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
        public TransactionMode TransactionMode { get; set; }
        public string UpScript { get; set; }
        public string DownScript { get; set; }

        public int CompareTo(Migration other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Version.CompareTo(other.Version);
        }
    }
}