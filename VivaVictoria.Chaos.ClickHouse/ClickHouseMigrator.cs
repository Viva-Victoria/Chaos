using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.ClickHouse.Models;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Db;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseMigrator : IMigrator
    {
        private ISettings settings;
        private ILogger logger;
        private ClickHouseMetadata metadata;

        public ClickHouseMigrator(ISettings settings, ILogger logger, IEnumerable<IMetadata> metadataList)
        {
            this.settings = settings ?? 
                            throw new NullReferenceException("Settings is null");
            this.logger = logger ?? 
                          throw new NullReferenceException("Logger is null");
            metadata = metadataList.GetService<IMetadata, ClickHouseMetadata>() 
                       ?? throw new NullReferenceException($"Metadata of type {typeof(ClickHouseMetadata)} required");
        }

        private IDbConnection Connect()
        {
            return new Connection(logger, new ClickHouseConnection(settings.ConnectionString));
        }

        public void Init()
        {
            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.CreateStatement;
            cmd.ExecuteNonQuery();
        }
        
        public long GetVersion()
        {
            using var conn = Connect();
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.SelectStatement;
            var reader = cmd.ExecuteReader();
            if (!reader.NextResult() || !reader.Read())
                return 0L;
            
            return reader.GetInt64(0);
        }

        public void SetVersion(long version)
        {
            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.InsertStatement;

            foreach (var param in metadata.InsertParameters(DateTime.UtcNow, version))
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
        }

        public void Apply(TransactionMode transactionMode, string migration)
        {
            if (transactionMode == TransactionMode.Default)
            {
                transactionMode = settings.TransactionMode;
            }
            if (transactionMode == TransactionMode.One)
            {
                logger.Log(LogLevel.Warning, "ClickHouse does not support transactions. Migration will be applied without a transaction");
            }

            Apply(migration);
        }

        private void Apply(string migration)
        {
            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = migration;
            cmd.ExecuteNonQuery();
        }
    }
}