using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
using VivaVictoria.Chaos.ClickHouse.Interfaces;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.ClickHouse.Models
{
    public class ClickHouseMetadata : IClickHouseMetadata
    {
        public string Engine => "MergeTree()";
        public string TableName => "migration_info";
        
        public string IdColumnName => "Id";
        public string IdColumnType => "UUID";
        
        public string IdColumnParams => "codec(ZSTD)";
        
        public string StateColumnName => "State";
        public string StateColumnType => "Int16";
        public string StateColumnParams => "codec(ZSTD)";
        
        public string VersionColumnName => "Version";
        public string VersionColumnType => "Int64";
        public string VersionColumnParams => "codec(ZSTD)";
        
        public string DateColumnName => "AppliedAt";
        public string DateColumnType => "DateTime";
        public string DateColumnParams => "codec(ZSTD)";
        
        public string CreateStatement => 
$@"create table if not exists {TableName}
(
    `{IdColumnName}` {IdColumnType} not null {IdColumnParams},
    `{StateColumnName}` {StateColumnType} {StateColumnParams},
    `{VersionColumnName}` {VersionColumnType} {VersionColumnParams},
    `{DateColumnName}` {DateColumnType} {DateColumnParams}
)
engine = {Engine}
primary key({IdColumnName})";
        
        public IEnumerable<ClickHouseParameter> InsertParameters(DateTime dateTime, long version, MigrationState state) =>
        new List<ClickHouseParameter>
        {
            new ClickHouseParameter{ ParameterName = "Id", Value = Guid.NewGuid(), DbType = DbType.Guid },
            new ClickHouseParameter{ ParameterName = "State", Value = (int)state, DbType = DbType.Int16 },
            new ClickHouseParameter{ ParameterName = "Version", Value = version, DbType = DbType.Int64 },
            new ClickHouseParameter{ ParameterName = "DateTime", Value = dateTime, DbType = DbType.DateTime }
        };

        public string InsertStatement =>
$@"insert into {TableName}
(
    `{IdColumnName}`,
    `{StateColumnName}`,
    `{VersionColumnName}`,
    `{DateColumnName}`
)
values
(
    @Id,
    @State,
    @Version,
    @DateTime
)";

        public string SelectStatement =>
$@"select 
    `{VersionColumnName}` as Version,
    `{StateColumnName}` as State
from {TableName} 
order by {IdColumnName} desc 
limit 1";
    }
}