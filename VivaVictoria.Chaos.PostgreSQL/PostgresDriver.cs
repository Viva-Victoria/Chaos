using System;
using System.Data;
using Npgsql;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.PostgreSQL.Extensions;
using VivaVictoria.Chaos.PostgreSQL.Models;

namespace VivaVictoria.Chaos.PostgreSql
{
    public class PostgresDriver : IDatabaseDriver<PostgresMetadata>
    {
        public IDbConnection Connect(string connectionString, PostgresMetadata metadata) {
            if (metadata.Schema == null)
            {
                metadata.Schema = connectionString.GetSearchPath("public");
            }
            else if (!connectionString.Contains("SearchPath"))
            {
                connectionString = $"{connectionString};SearchPath={metadata.Schema}";
            }
            return new NpgsqlConnection(connectionString);
        }

        public bool IsTransactionSupported() => true;

        public string CreateStatement(PostgresMetadata metadata) => 
@$"create schema if not exists {metadata.Schema};
create table if not exists {metadata.Schema}.{metadata.TableName}
(
    {metadata.IdColumnName} {metadata.IdColumnType},
    {metadata.VersionColumnName} {metadata.VersionColumnType},
    {metadata.DateColumnName} {metadata.DateColumnType},
    constraint {metadata.TableName}_pk primary key ({metadata.IdColumnName})
);";

        public object InsertParameters(DateTime dateTime, long version) => new { version, dateTime };
        public string InsertStatement(PostgresMetadata metadata) =>
$@"insert into {metadata.Schema}.{metadata.TableName}
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
$@"select m.{metadata.VersionColumnName} as Version
from {metadata.Schema}.{metadata.TableName} m
order by {metadata.IdColumnName} desc 
limit 1";
    }
}