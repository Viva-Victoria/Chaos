using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Dapper.Interfaces;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class Chaos : IChaos
    {
        private readonly ILogger logger;
        private readonly IMigrationReader migrationReader;
        private readonly IMigrator migrator;

        public Chaos(
            ILogger logger, 
            IMigrationReader migrationReader,
            IMigrator migrator)
        {
            this.logger = logger ?? 
                          throw new NullReferenceException("Logger is null");
            this.migrationReader = migrationReader ?? 
                                   throw new NullReferenceException("MigrationReader is null");
            this.migrator = migrator ?? 
                            throw new NullReferenceException("Migrator is null");
        }

        public Chaos Init()
        {
            migrator.Init();
            return this;
        }

        public void Up()
        {
            var currentVersion = migrator.GetVersion();
            Migrate(currentVersion, true);
        }

        public void Down(long targetVersion)
        {
            Migrate(targetVersion, false);
        }

        private void Migrate(long version, bool up)
        {
            var migrations = migrationReader.Read()
                .Where(m => m.Version > version)
                .ToList();

            if (!migrations.Any())
            {
                logger.Log(LogLevel.Debug, "Database is up-to-date");
                return;
            }

            migrations.Sort();

            foreach (var migration in migrations)
            {
                try
                {
                    logger.Log(LogLevel.Debug, $"Migrating to {migration.Version}");
                    migrator.Apply(migration.TransactionMode, up ? migration.UpScript : migration.DownScript);
                    
                    logger.Log(LogLevel.Debug, $"Migration applied successfully");
                    migrator.SetVersion(up ? migration.Version : migration.Version - 1);
                    logger.Log(LogLevel.Debug, $"Metadata saved");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Migration failed");
                    throw;
                }
            }
        }
    }
}