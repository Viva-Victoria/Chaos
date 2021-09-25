using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class Chaos : IChaos
    {
        private readonly ILogger logger;
        private readonly IMigrationReader migrationReader;
        private readonly IMigrator migrator;
        private bool ready;

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
            ready = false;
        }

        public Chaos Init(Func<bool> condition = null)
        {
            if (condition?.Invoke() ?? false)
            {
                migrator.Init();
                ready = true;
            }

            return this;
        }

        public void Up()
        {
            if (!ready)
            {
                logger.LogDebug("Chaos is not ready. Migrations skipped");
                return;
            }
            
            var currentVersion = migrator.GetVersion();
            Migrate(currentVersion, true);
        }

        public void Down(long targetVersion)
        {
            if (!ready)
            {
                logger.LogDebug("Chaos is not ready. Migrations skipped");
                return;
            }
            
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