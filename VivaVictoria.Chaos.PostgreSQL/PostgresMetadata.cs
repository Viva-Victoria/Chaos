using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSQL
{
    public class PostgresMetadata : IMetadata
    {
        public string CreateTableQuery => $@"create table {TableName} 
(
    {IdColumnName} {IdColumnType} primary key,
    {VersionColumnName} {VersionColumnType} not null,
    {DateColumnName} {DateColumnType} not null default now()
)";

        public string GetVersionQuery => $@"select {VersionColumnName} as Version 
from {TableName} 
order by {IdColumnName} desc 
limit 1";

        public string SetVersionQuery => $@"insert into {TableName} 
({VersionColumnName}, {DateColumnName}) 
values 
(@version, now())";
        
        public string TableName => "migration_info";
        public string IdColumnName => "id";
        public string IdColumnType => "serial";
        public string VersionColumnName => "version";
        public string VersionColumnType => "integer";
        public string DateColumnName => "applied_at";
        public string DateColumnType => "timestamp";
    }
}