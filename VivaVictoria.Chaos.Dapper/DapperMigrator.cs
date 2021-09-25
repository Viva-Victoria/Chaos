using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Dapper.Extensions;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Db;

namespace VivaVictoria.Chaos.Dapper
{
    public class DapperMigrator<TMetadata> : IMigrator
        where TMetadata : IMetadata
    {
        private ISettings settings;
        private ILogger logger;
        private TMetadata metadata;
        private IDatabaseDriver<TMetadata> driver;

        public DapperMigrator(ISettings settings, ILogger logger, IEnumerable<IMetadata> metadataList, IDatabaseDriver<TMetadata> driver)
        {
            this.settings = settings ?? 
                            throw new NullReferenceException("Settings is null");
            this.logger = logger ?? 
                          throw new NullReferenceException("Logger is null");
            metadata = metadataList.GetService<IMetadata, TMetadata>(false) 
                       ?? throw new NullReferenceException($"Metadata of type {typeof(TMetadata)} required");
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
        
        public long GetVersion()
        {
            using var conn = Connect();
            return conn.QueryFirstOrDefault<long>(driver.SelectStatement(metadata));
        }

        public void SetVersion(long version)
        {
            using var conn = Connect();
            conn.Execute(driver.InsertStatement(metadata), driver.InsertParameters(DateTime.UtcNow, version));
        }

        public void Apply(TransactionMode transactionMode, string migration)
        {
            if (transactionMode == TransactionMode.Default)
            {
                transactionMode = settings.TransactionMode;
            }
            if (transactionMode == TransactionMode.One && !driver.IsTransactionSupported())
            {
                logger.Log(LogLevel.Warning, $"DatabaseDriver does not support transactions. Migration will be applied without a transaction");
                transactionMode = TransactionMode.None;
            }

            switch (transactionMode)
            {
                case TransactionMode.None:
                    Apply(migration);
                    break;
                case TransactionMode.One:
                {
                    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    Apply(migration);
                    scope.Complete();
                    break;
                }
            }
        }

        public void Apply(string migration)
        {
            using var conn = Connect();
            conn.Execute(migration);
        }
    }
}