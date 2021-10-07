using System;
using System.Collections;
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
        private readonly ILogger logger;
        private readonly ISettings settings;
        private readonly IMigrator<TMigration> migrator;
        private readonly IMigrationReader<TMigration> migrationReader;
        private readonly List<IEventListener> listeners;
        private bool ready;

        public Chaos(
            ILogger logger, 
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
            if (settings.ParallelListeners)
            {
                listeners.AsParallel().ForAll(action);
            }
            else
            {
                listeners.ForEach(action);
            }
        }
        
        public IChaos<TMigration> Init(Func<bool> condition = null)
        {
            if (condition == null || condition())
            {
                migrator.Init();
                ready = true;
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
            if (!IsReady())
            {
                logger.LogDebug("Chaos is not ready. Migrations skipped");
                return;
            }
            
            Migrate(-1);
        }

        public void Migrate(long targetVersion)
        {
            if (!ready)
            {
                logger.LogDebug("Chaos is not ready. Migrations skipped");
                return;
            }

            var migrations = migrationReader.Read().AsEnumerable();
            if (!migrations.Any())
            {
                logger.LogDebug("No migrations found");
                PublishEvent(l => l.OnUpToDate());
                return;
            }
            
            migrations = migrations.OrderBy(m => m.Version);

            var currentVersion = 0L;
            var info = migrator.GetInfo();
            if (info != null)
            {
                if (info.State != MigrationState.Applied)
                {
                    logger.LogCritical(
                        $"Last migration {info.Version} applied with errors. Manual fix required before new migrations will be applied");
                    PublishEvent(l => l.OnCorrupted());
                    return;
                }

                currentVersion = info.Version;
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
                migrations = migrations.Where(m => m.Version > targetVersion);
                downgrade = true;
            }
            else if (targetVersion > currentVersion)
            {
                migrations = migrations.Where(m => m.Version > currentVersion && m.Version <= targetVersion);
                downgrade = false;
            }
            else
            {
                logger.LogDebug("Database is up-to-date");
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
                logger.LogError($"Migration {migration.Version} failed", e);
                SaveStateIfNeeded(currentVersion, version, MigrationState.Failed, e);
                throw;
            }
            
            SaveStateIfNeeded(currentVersion, version, MigrationState.Applied);
            return version;
        }
    }
}