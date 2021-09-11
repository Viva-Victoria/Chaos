using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class Chaos<TMetadata, TMigrator> : IChaos
        where TMetadata : IMetadata
        where TMigrator : IMigrator<TMetadata>
    {
        private readonly ISettings settings;
        private readonly ILogger logger;
        private readonly IMigrationReader migrationReader;
        private TMigrator migrator;

        public Chaos(ISettings settings, TMetadata metadata, ILogger logger, IMigrationReader migrationReader,
            TMigrator migrator)
        {
            this.settings = settings ?? throw new NullReferenceException("Settings is null");
            this.logger = logger ?? throw new NullReferenceException("Logger is null");
            this.migrationReader = migrationReader ?? throw new NullReferenceException("MigrationReader is null");
            this.migrator = migrator ?? throw new NullReferenceException("Migrator is null");

            metadata = metadata ?? throw new NullReferenceException("Metadata is null");
            migrator.Prepare(settings.ConnectionString, metadata, logger);
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
                logger.Log(LogLevel.Debug, "Database is already up-to-date");
                return;
            }


            migrations.Sort((m1, m2) => m1.Version.CompareTo(m2.Version));

            foreach (var migration in migrations)
            {
                var transactionMode = migration.TransactionMode == TransactionMode.Default
                    ? settings.TransactionMode
                    : migration.TransactionMode;

                var script = up ? migration.UpScript : migration.DownScript;
                if (transactionMode == TransactionMode.One)
                {
                    logger.Log(LogLevel.Debug,
                        $"Applying migration {migration.Version} in transaction with script\n{script}");
                    migrator.ApplyInTransaction(script);
                }
                else
                {
                    logger.Log(LogLevel.Debug,
                        $"Applying migration {migration.Version} without transaction with script\n{script}");
                    migrator.Apply(script);
                }

                logger.Log(LogLevel.Debug, $"Version {migration.Version} migrated successful");
                migrator.SetVersion(up ? migration.Version : migration.Version - 1);
            }
        }
    }
}