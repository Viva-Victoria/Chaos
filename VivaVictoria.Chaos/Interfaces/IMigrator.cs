﻿using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrator<TMetadata> where TMetadata : IMetadata
    {
        public void Prepare(string connectionString, TMetadata metadata, ILogger logger);
        public long GetVersion();
        public void Apply(string migration);
        public void ApplyInTransaction(string migration);
        public void SetVersion(long version);
    }
}