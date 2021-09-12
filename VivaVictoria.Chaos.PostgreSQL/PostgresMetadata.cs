using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSql
{
    public class PostgresMetadata : IMetadata
    {
        public string TableName => "migration_info";
        public string IdColumnName => "id";
        public string IdColumnType => "serial";
        public string VersionColumnName => "version";
        public string VersionColumnType => "integer";
        public string DateColumnName => "applied_at";
        public string DateColumnType => "timestamp";
    }
}