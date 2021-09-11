namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMetadata
    {
        public string TableName { get; }
        public string IdColumnName { get; }
        public string IdColumnType { get; }
        public string VersionColumnName { get; }
        public string VersionColumnType { get; }
        public string DateColumnName { get; }
        public string DateColumnType { get; }
    }
}