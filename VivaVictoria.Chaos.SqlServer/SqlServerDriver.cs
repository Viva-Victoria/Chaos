using System;
using System.Data;
using System.Data.SqlClient;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.SqlServer.Models;

namespace VivaVictoria.Chaos.SqlServer
{
    public class SqlServerDriver : IDatabaseDriver<SqlServerMetadata>
    { 
        public IDbConnection Connect(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public bool IsTransactionSupported() => true;

        public string CreateStatement(SqlServerMetadata metadata) =>
@$"if object_id('{metadata.TableName}') is null
create table {metadata.Schema}.{metadata.TableName}
(
    {metadata.IdColumnName} {metadata.IdColumnType} not null identity(1,1),
    {metadata.VersionColumnName} {metadata.VersionColumnType},
    {metadata.DateColumnName} {metadata.DateColumnType},
    constraint PK_{metadata.TableName} primary key ({metadata.IdColumnName})
)";

        public object InsertParameters(DateTime dateTime, long version) => new { version, dateTime };
        public string InsertStatement(SqlServerMetadata metadata) =>
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

        public string SelectStatement(SqlServerMetadata metadata) =>
$@"select top 1 {metadata.VersionColumnName} as Version
from {metadata.Schema}.{metadata.TableName} 
order by {metadata.IdColumnName} desc";
    }
}