using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Db;
using VivaVictoria.Chaos.Models;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.Dapper
{
    public class DapperMigrator<TMetadata> : IMigrator<Migration>
        where TMetadata : IMetadata
    {
        private ISqlSettings settings;
        private ILogger logger;
        private TMetadata metadata;
        private IDatabaseDriver<TMetadata> driver;

        public DapperMigrator(IEnumerable<ISettings> settings, ILogger logger, IEnumerable<IMetadata> metadataList, IDatabaseDriver<TMetadata> driver)
        {
            this.settings = settings.RequireService<ISettings, ISqlSettings>(false);
            this.logger = logger;
            metadata = metadataList.RequireService<IMetadata, TMetadata>(false);
            this.driver = driver;
        }

        private IDbConnection Connect()
        {
            return new Connection(logger, driver.Connect(settings.ConnectionString, metadata));
        }

        public void Init()
        {
            using var conn = Connect();
            conn.Execute(driver.CreateStatement(metadata));
        }
        
        public Info GetInfo()
        {
            using var conn = Connect();
            return conn.QueryFirstOrDefault<Info>(driver.SelectStatement(metadata));
        }

        public void SaveState(long version, MigrationState state)
        {
            using var conn = Connect();
            conn.Execute(driver.InsertStatement(metadata), driver.InsertParameters(DateTime.UtcNow, version, state));
        }

        public void Apply(Migration migration, bool downgrade)
        {
            if (migration.TransactionMode == TransactionMode.Default)
            {
                migration.TransactionMode = settings.TransactionMode;
            }
            if (migration.TransactionMode == TransactionMode.One && !driver.IsTransactionSupported())
            {
                logger.Log(LogLevel.Warning, $"DatabaseDriver does not support transactions. Migration will be applied without a transaction");
                migration.TransactionMode = TransactionMode.None;
            }

            var script = downgrade ? migration.Down : migration.Up;
            switch (migration.TransactionMode)
            {
                case TransactionMode.None:
                {
                    using var conn = Connect();
                    conn.Execute(script);
                    break;
                }
                case TransactionMode.One:
                {
                    using var conn = Connect();
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        conn.Execute(script, transaction);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    transaction.Commit();
                    break;
                }
            }
        }
    }
}