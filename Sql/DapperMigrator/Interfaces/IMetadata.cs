namespace VivaVictoria.Chaos.Dapper.Interfaces
{
    public interface IMetadata
    {
        public string TableName { get; }
        public string IdColumnName { get; }
        public string IdColumnType { get; }
        public string StateColumnName { get; }
        public string StateColumnType { get; }
        public string VersionColumnName { get; }
        public string VersionColumnType { get; }
        public string DateColumnName { get; }
        public string DateColumnType { get; }
    }
}