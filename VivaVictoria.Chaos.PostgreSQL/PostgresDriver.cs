using System;
using System.Data;
using Npgsql;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Enums;
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
    {metadata.StateColumnName} {metadata.StateColumnType},
    {metadata.VersionColumnName} {metadata.VersionColumnType},
    {metadata.DateColumnName} {metadata.DateColumnType},
    constraint {metadata.TableName}_pk primary key ({metadata.IdColumnName})
);";

        
        public object InsertParameters(DateTime dateTime, long version, MigrationState state) => new { version, dateTime, state = (int)state };
        public string InsertStatement(PostgresMetadata metadata) =>
$@"insert into {metadata.Schema}.{metadata.TableName}
(
    {metadata.VersionColumnName},
    {metadata.StateColumnName},
    {metadata.DateColumnName}
)
values
(
    @version,
    @state,
    @dateTime
)";

        public string SelectStatement(PostgresMetadata metadata) =>
$@"select 
    m.{metadata.VersionColumnName} as Version,
    m.{metadata.StateColumnName} as State
from {metadata.Schema}.{metadata.TableName} m
order by {metadata.IdColumnName} desc
limit 1";
    }
}