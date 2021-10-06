using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.Postgres.Models
{
    public class PostgresMetadata : IMetadata
    {
        public virtual string Schema { get; set; }
        public virtual string TableName => "migration_info";
        public virtual string IdColumnName => "id";
        public virtual string IdColumnType => "serial";
        public string StateColumnName => "state";
        public string StateColumnType => "int2";
        public virtual string VersionColumnName => "version";
        public virtual string VersionColumnType => "integer";
        public virtual string DateColumnName => "applied_at";
        public virtual string DateColumnType => "timestamp";
    }
}