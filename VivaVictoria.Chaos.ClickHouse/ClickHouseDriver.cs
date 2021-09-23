using System;
using System.Data;
using ClickHouse.Ado;
using VivaVictoria.Chaos.ClickHouse.Models;
using VivaVictoria.Chaos.Dapper.Interfaces;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseProvider : IDatabaseDriver<ClickHouseMetadata>
    {
        public IDbConnection Connect(string connectionString)
        {
            return new ClickHouseConnection(connectionString);
        }

        public bool IsTransactionSupported() => false;

        public string CreateStatement(ClickHouseMetadata metadata) => 
$@"create table if not exists {metadata.TableName}
(
    {metadata.IdColumnName} {metadata.IdColumnType} {metadata.IdColumnParams},
    {metadata.VersionColumnName} {metadata.VersionColumnType} {metadata.VersionColumnParams},
    {metadata.DateColumnName} {metadata.DateColumnType} {metadata.DateColumnParams}
)
engine = {metadata.Engine}
primary key({metadata.IdColumnName})";

        public object InsertParameters(DateTime dateTime, long version) => new { id = Guid.NewGuid(), version, dateTime };
        public string InsertStatement(ClickHouseMetadata metadata) =>
$@"insert into {metadata.TableName}
(
    {metadata.IdColumnName},
    {metadata.VersionColumnName},
    {metadata.DateColumnName}
)
values
(
    @id,
    @version,
    @dateTime
)";

        public string SelectStatement(ClickHouseMetadata metadata) =>
$@"select {metadata.VersionColumnName} as Version
from {metadata.TableName} 
order by {metadata.IdColumnName} desc 
limit 1";
    }
}