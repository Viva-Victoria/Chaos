using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos
{
    public class ChaosBuilder<TMetadata, TMigrator>
        where TMetadata : IMetadata
        where TMigrator : IMigrator<TMetadata>
    {
        private ILogger logger;
        private TMetadata metadata;
        private TMigrator migrator;
        private IMigrationReader reader;
        private ISettings settings;

        public ChaosBuilder<TMetadata, TMigrator> Use(TMetadata metadata)
        {
            this.metadata = metadata;
            return this;
        }

        public ChaosBuilder<TMetadata, TMigrator> Use(TMigrator migrator)
        {
            this.migrator = migrator;
            return this;
        }

        public ChaosBuilder<TMetadata, TMigrator> Use(IMigrationReader reader)
        {
            this.reader = reader;
            return this;
        }

        public ChaosBuilder<TMetadata, TMigrator> Use(ISettings settings)
        {
            this.settings = settings;
            return this;
        }

        public ChaosBuilder<TMetadata, TMigrator> Use(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public ChaosBuilder<TMetadata, TMigrator> Resolve(ServiceProvider provider)
        {
            logger ??= provider.GetRequiredService<ILogger>();
            settings ??= provider.GetRequiredService<ISettings>();
            reader ??= provider.GetRequiredService<IMigrationReader>();

            metadata ??= (TMetadata) provider.GetServices<IMetadata>()
                .FirstOrDefault(t => t.GetType() == typeof(TMetadata));
            migrator ??= (TMigrator) provider.GetServices<IMigrator<TMetadata>>()
                .FirstOrDefault(t => t.GetType() == typeof(TMigrator));

            return this;
        }

        public Chaos<TMetadata, TMigrator> Build()
        {
            return new Chaos<TMetadata, TMigrator>(settings, metadata, logger, reader, migrator);
        }
    }
}