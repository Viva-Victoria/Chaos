using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class Chaos<TMigration> : IChaos<TMigration>
        where TMigration : IMigration
    {
        private readonly ILogger<Chaos<TMigration>> logger;
        private readonly ISettings settings;
        private readonly IMigrator<TMigration> migrator;
        private readonly IMigrationReader<TMigration> migrationReader;
        private readonly List<IEventListener> listeners;
        private bool ready;

        public Chaos(
            ILogger<Chaos<TMigration>> logger, 
            ISettings settings, 
            IMigrator<TMigration> migrator, 
            IMigrationReader<TMigration> migrationReader, 
            IEnumerable<IEventListener> listeners)
        {
            this.logger = logger;
            this.settings = settings;
            this.migrator = migrator;
            this.migrationReader = migrationReader;
            this.listeners = listeners.ToList();
        }

        private void PublishEvent(Action<IEventListener> action)
        {
            logger.LogDebug($"Event publishing...");
            if (settings.ParallelListeners)
            {
                listeners.AsParallel().ForAll(action);
            }
            else
            {
                listeners.ForEach(action);
            }
            logger.LogDebug($"Event published");
        }
        
        public IChaos<TMigration> Init(Func<bool> condition = null)
        {
            logger.LogDebug(condition == null 
                ? "Initializing Chaos"
                : "Initializing Chaos - condition needs to be checked");
            
            if (condition == null || condition())
            {
                migrator.Init();
                ready = true;
                logger.LogDebug($"Chaos ready." +
                    $"States to be saved: {string.Join(", ", settings.SaveStates)}, " +
                    $"Parallel publishing {(settings.ParallelListeners ? "enabled" : "disabled")}");
                PublishEvent(l => l.OnChaosReady());
            }

            return this;
        }

        public bool IsReady()
        {
            return ready;
        }

        public void Migrate()
        {
            Migrate(-1);
        }

        public void Migrate(long targetVersion)
        {
            if (!IsReady())
            {
                logger.LogDebug("Chaos is not ready. Migrations skipped");
                return;
            }

            var migrations = migrationReader.Read().AsEnumerable();
            if (!migrations.Any())
            {
                logger.LogWarning("No migrations found");
                PublishEvent(l => l.OnNoMigrations());
                return;
            }
            
            migrations = migrations.OrderBy(m => m.Version);

            var currentVersion = 0L;
            
            logger.LogDebug("Fetching database info...");
            var info = migrator.GetInfo();
            if (info == null)
            {
                logger.LogDebug("No info fetched: database is raw");
            }
            else
            {
                if (info.State != MigrationState.Applied)
                {
                    logger.LogCritical($"Last migration {info.Version} applied with errors. Manual fix required before new migrations will be applied");
                    PublishEvent(l => l.OnCorrupted());
                    return;
                }

                currentVersion = info.Version;
                logger.LogDebug($"Current database version: {currentVersion}");
            }
            
            var minVersion = migrations.First().Version;
            var maxVersion = migrations.Last().Version;
            if (targetVersion < minVersion - 1)
            {
                targetVersion = maxVersion;
            }

            bool downgrade;
            if (targetVersion < currentVersion)
            {
                logger.LogInformation($"Downgrading to {targetVersion}. {migrations.Count()} migrations will be rolled back");
                migrations = migrations.Where(m => m.Version > targetVersion);
                downgrade = true;
            }
            else if (targetVersion > currentVersion)
            {
                migrations = migrations.Where(m => m.Version > currentVersion && m.Version <= targetVersion);
                logger.LogInformation($"Upgrading to {targetVersion}. {migrations.Count()} migrations will be applied");
                downgrade = false;
            }
            else
            {
                logger.LogInformation("Database is up-to-date");
                PublishEvent(l => l.OnUpToDate());
                return;
            }

            foreach (var migration in migrations.ToList())
            {
                currentVersion = Apply(currentVersion, migration, downgrade);
            }
        }

        private void SaveStateIfNeeded(long currentVersion, long applyingVersion, MigrationState state, Exception e = null)
        {
            PublishEvent(l => l.OnStateChanged(currentVersion, applyingVersion, state, e));
            if (settings.SaveStates.Contains(state))
            {
                migrator.SaveState(applyingVersion, state);
            }
        }

        private long Apply(long currentVersion, TMigration migration, bool downgrade)
        {
            var version = downgrade ? migration.Version - 1 : migration.Version;
            SaveStateIfNeeded(currentVersion, version, MigrationState.Started);
            
            try
            {
                migrator.Apply(migration, downgrade);
            }
            catch (Exception e)
            {
                logger.LogError($"Migration {version} failed", e);
                SaveStateIfNeeded(currentVersion, version, MigrationState.Failed, e);
                throw;
            }
            
            SaveStateIfNeeded(currentVersion, version, MigrationState.Applied);
            return version;
        }
    }
}