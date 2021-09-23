using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse.Models
{
    public class ClickHouseMetadata : IMetadata
    {
        public string Engine => "MergeTree()";
        public string TableName => "migration_info";
        public string IdColumnName => "`Id`";
        public string IdColumnType => "UUID";
        public string IdColumnParams => "codec(ZSTD)";
        public string VersionColumnName => "`Version`";
        public string VersionColumnType => "Int64";
        public string VersionColumnParams => "codec(ZSTD)";
        public string DateColumnName => "`AppliedAt`";
        public string DateColumnType => "DateTime";
        public string DateColumnParams => "codec(ZSTD)";
    }
}