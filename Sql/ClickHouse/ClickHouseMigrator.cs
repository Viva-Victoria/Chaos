using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.ClickHouse.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Interfaces;
using VivaVictoria.Chaos.Models;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseMigrator : IMigrator<Migration>
    {
        private readonly IClickHouseSettings settings;
        private readonly ILogger<ClickHouseMigrator> logger;
        private readonly IClickHouseMetadata metadata;
        private readonly IConnectionProvider connectionProvider;

        public ClickHouseMigrator(
            IEnumerable<ISettings> settings, 
            ILogger<ClickHouseMigrator> logger, 
            IClickHouseMetadata metadata, 
            IConnectionProvider connectionProvider)
        {
            this.settings = settings.RequireService<ISettings, IClickHouseSettings>(false);
            this.logger = logger;
            this.metadata = metadata;
            this.connectionProvider = connectionProvider;
        }

        private IDbConnection Connect()
        {
            return connectionProvider.Wrap(new ClickHouseConnection(settings.ConnectionString));
        }

        public void Init()
        {
            using var conn = Connect();
            conn.Open();
            
            logger.LogDebug("Creating metadata table...");
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.CreateStatement;
            cmd.ExecuteNonQuery();
        }
        
        public Info GetInfo()
        {
            using var conn = Connect();
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.SelectStatement;
            var reader = cmd.ExecuteReader();
            logger.LogDebug("Reading ClickHouse response");
            if (reader.NextResult() && reader.Read())
            {
                return new Info
                {
                    Version = reader.GetInt64(0),
                    State = (MigrationState) reader.GetInt16(1)
                };
            }

            logger.LogDebug("No results from ClickHouse");
            return null;
        }

        public void SaveState(long version, MigrationState state)
        {
            logger.LogDebug("Saving state to metadata table");
            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.InsertStatement;

            foreach (var param in metadata.InsertParameters(DateTime.UtcNow, version, state))
            {
                cmd.Parameters.Add(param);
            }

            cmd.ExecuteNonQuery();
        }

        public void Apply(Migration migration, bool downgrade)
        {
            if (migration.TransactionMode == TransactionMode.One)
            {
                logger.LogWarning("ClickHouse does not support transactions, TransactionMode will be ignored");
            }

            logger.LogInformation($"{(downgrade ? "Reverting" : "Applying")} migration {migration.Version}...");
            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = downgrade ? migration.Down : migration.Up;
            cmd.ExecuteNonQuery();
        }
    }
}