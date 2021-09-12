using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseMetadata : IMetadata
    {
        public string TableName => "migration_info";
        public string IdColumnName => "`Id`";
        public string IdColumnType => "UUID";
        public string VersionColumnName => "`Version`";
        public string VersionColumnType => "UInt32";
        public string DateColumnName => "`AppliedAt`";
        public string DateColumnType => "DateTime";
    }
}