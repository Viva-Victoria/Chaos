using System;
using System.Data;
using Npgsql;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.PostgreSQL.Models;

namespace VivaVictoria.Chaos.PostgreSql
{
    public class NpgsqlDriver : IDatabaseDriver<PostgresMetadata>
    {
        public IDbConnection Connect(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public bool IsTransactionSupported() => true;

        public string CreateStatement(PostgresMetadata metadata) => 
@$"create table if not exists {metadata.TableName}
(
    {metadata.IdColumnName} {metadata.IdColumnType},
    {metadata.VersionColumnName} {metadata.VersionColumnType},
    {metadata.DateColumnName} {metadata.DateColumnType},
    constraint {metadata.TableName}_pk primary key ({metadata.IdColumnName})
);";

        public object InsertParameters(DateTime dateTime, long version) => new { version, dateTime };
        public string InsertStatement(PostgresMetadata metadata) =>
$@"insert into {metadata.TableName}
(
    {metadata.VersionColumnName},
    {metadata.DateColumnName}
)
values
(
    @version,
    @dateTime
)";

        public string SelectStatement(PostgresMetadata metadata) =>
$@"select {metadata.VersionColumnName} as Version
from {metadata.TableName} 
order by {metadata.IdColumnName} desc 
limit 1";
    }
}