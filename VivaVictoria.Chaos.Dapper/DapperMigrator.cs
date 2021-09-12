using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Db;

namespace VivaVictoria.Chaos.Dapper
{
    public class DapperMigrator : IMigrator
    {
        private string connectionString;
        private IMetadata metadata;
        private ILogger logger;
        private IConnectionProvider provider;

        public DapperMigrator(IConnectionProvider provider)
        {
            this.provider = provider;
        }

        public void Prepare(string connectionString, IMetadata metadata, ILogger logger)
        {
            this.connectionString = connectionString;
            this.metadata = metadata;
            this.logger = logger;
        }

        private IDbConnection Connect()
        {
            return new Connection(logger, provider.Connect(connectionString));
        }
        
        public long GetVersion()
        {
            using var conn = Connect();
            conn.Execute($@"create table if not exists {metadata.TableName}
(
    {metadata.IdColumnName} {metadata.IdColumnType},
    {metadata.VersionColumnName} {metadata.VersionColumnType},
    {metadata.DateColumnName} {metadata.DateColumnType},
    constraint {metadata.TableName}_pk primary key ({metadata.IdColumnName})
)");
            return conn.Query<long>($@"select {metadata.VersionColumnName} 
from {metadata.TableName} 
order by {metadata.IdColumnName} desc 
limit 1").FirstOrDefault();
        }

        public void SetVersion(long version)
        {
            using var conn = Connect();
            conn.Execute($@"insert into {metadata.TableName} 
({metadata.VersionColumnName}, {metadata.DateColumnName})
values
({version}, {DateTime.UtcNow})");
        }

        public void Apply(string migration)
        {
            using var conn = Connect();
            conn.Execute(migration);
        }

        public void ApplyInTransaction(string migration)
        {
            using var conn = Connect();
            using var transaction = conn.BeginTransaction();
            try
            {
                conn.Execute(migration);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}