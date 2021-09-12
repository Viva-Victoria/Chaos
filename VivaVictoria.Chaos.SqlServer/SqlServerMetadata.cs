using System;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.SqlServer
{
    public class SqlServerMetadata : IMetadata
    {
        public string TableName => "MigrationSchema";
        public string IdColumnName => "Id";
        public string IdColumnType => "int not null identity(1,1)";
        public string VersionColumnName => "Version";
        public string VersionColumnType => "bigint";
        public string DateColumnName => "Date";
        public string DateColumnType => "datetime";
    }
}