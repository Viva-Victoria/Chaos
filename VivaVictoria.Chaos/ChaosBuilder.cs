using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class ChaosBuilder
    {
        private ILogger logger;
        private IMetadata metadata;
        private IMigrator migrator;
        private IMigrationReader reader;
        private ISettings settings;

        public ChaosBuilder Use(IMetadata metadata)
        {
            this.metadata = metadata;
            return this;
        }

        public ChaosBuilder Use(IMigrator migrator)
        {
            this.migrator = migrator;
            return this;
        }

        public ChaosBuilder Use(IMigrationReader reader)
        {
            this.reader = reader;
            return this;
        }

        public ChaosBuilder Use(ISettings settings)
        {
            this.settings = settings;
            return this;
        }

        public ChaosBuilder Use(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public ChaosBuilder Resolve(ServiceProvider provider)
        {
            logger ??= provider.GetRequiredService<ILogger>();
            settings ??= provider.GetRequiredService<ISettings>();
            reader ??= provider.GetRequiredService<IMigrationReader>();

            metadata ??= provider.GetRequiredService<IMetadata>();
            migrator ??= provider.GetRequiredService<IMigrator>();

            return this;
        }

        public Chaos Build()
        {
            return new Chaos(settings, metadata, logger, reader, migrator);
        }
    }
}