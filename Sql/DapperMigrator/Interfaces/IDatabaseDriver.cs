using System;
using System.Data;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Dapper.Interfaces
{
    public interface IDatabaseDriver<in TMetadata> where TMetadata : IMetadata
    {
        public string Name { get; }
        public IDbConnection Connect(string connectionString, TMetadata metadata);
        public bool IsTransactionSupported();
        public string CreateStatement(TMetadata metadata);
        public string InsertStatement(TMetadata metadata);
        public string SelectStatement(TMetadata metadata);
        public object InsertParameters(DateTime dateTime, long version, MigrationState state);
    }
}