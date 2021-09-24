using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
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
        
        public string CreateStatement => 
$@"create table if not exists {TableName}
(
    {IdColumnName} {IdColumnType} not null {IdColumnParams},
    {VersionColumnName} {VersionColumnType} {VersionColumnParams},
    {DateColumnName} {DateColumnType} {DateColumnParams}
)
engine = {Engine}
primary key({IdColumnName})";
        
        public IEnumerable<IDbDataParameter> InsertParameters(DateTime dateTime, long version) =>
        new List<IDbDataParameter>
        {
            new ClickHouseParameter{ ParameterName = "Id", Value = Guid.NewGuid(), DbType = DbType.Guid },
            new ClickHouseParameter{ ParameterName = "Version", Value = version, DbType = DbType.Int64 },
            new ClickHouseParameter{ ParameterName = "DateTime", Value = dateTime, DbType = DbType.DateTime }
        };

        public string InsertStatement =>
$@"insert into {TableName}
(
    {IdColumnName},
    {VersionColumnName},
    {DateColumnName}
)
values
(
    @Id,
    @Version,
    @DateTime
)";

        public string SelectStatement =>
$@"select {VersionColumnName} as Version
from {TableName} 
order by {IdColumnName} desc 
limit 1";
    }
}