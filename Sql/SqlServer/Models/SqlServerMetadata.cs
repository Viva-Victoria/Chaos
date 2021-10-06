using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.SqlServer.Models
{
    public class SqlServerMetadata : IMetadata
    {
        public virtual string Schema => "dbo";
        public virtual string TableName => "MigrationSchema";
        public virtual string IdColumnName => "Id";
        public virtual string IdColumnType => "int";
        public string StateColumnName => "State";
        public string StateColumnType => "int";
        public virtual string VersionColumnName => "Version";
        public virtual string VersionColumnType => "bigint";
        public virtual string DateColumnName => "Date";
        public virtual string DateColumnType => "datetime";
    }
}