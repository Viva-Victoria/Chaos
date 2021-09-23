using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.SqlServer.Models
{
    public class SqlServerMetadata : IMetadata
    {
        public string Schema => "dbo";
        public string TableName => "MigrationSchema";
        public string IdColumnName => "Id";
        public string IdColumnType => "int";
        public string VersionColumnName => "Version";
        public string VersionColumnType => "bigint";
        public string DateColumnName => "Date";
        public string DateColumnType => "datetime";
    }
}