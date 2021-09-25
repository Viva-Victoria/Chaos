using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.PostgreSQL.Models
{
    public class PostgresMetadata : IMetadata
    {
        public virtual string Schema { get; set; } = null;
        public virtual string TableName => "migration_info";
        public virtual string IdColumnName => "id";
        public virtual string IdColumnType => "serial";
        public virtual string VersionColumnName => "version";
        public virtual string VersionColumnType => "integer";
        public virtual string DateColumnName => "applied_at";
        public virtual string DateColumnType => "timestamp";
    }
}