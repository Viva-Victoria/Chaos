using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.ClickHouse.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Extensions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Logging.Db;
using VivaVictoria.Chaos.Models;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ClickHouse
{
    public class ClickHouseMigrator : IMigrator<Migration>
    {
        private IClickHouseSettings settings;
        private ILogger logger;
        private IClickHouseMetadata metadata;

        public ClickHouseMigrator(IEnumerable<ISettings> settings, ILogger logger, IClickHouseMetadata metadata)
        {
            this.settings = settings.RequireService<ISettings, IClickHouseSettings>(false);
            this.logger = logger;
            this.metadata = metadata;
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
        
        public Info GetInfo()
        {
            using var conn = Connect();
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = metadata.SelectStatement;
            var reader = cmd.ExecuteReader();
            if (reader.NextResult() && reader.Read())
            {
                return new Info
                {
                    Version = reader.GetInt64(0),
                    State = (MigrationState) reader.GetInt16(1)
                };
            }

            return null;
        }

        public void SaveState(long version, MigrationState state)
        {
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
                logger.Log(LogLevel.Warning, "ClickHouse does not support transactions, TransactionMode will be ignored");
            }

            using var conn = Connect();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = downgrade ? migration.Down : migration.Up;
            cmd.ExecuteNonQuery();
        }
    }
}