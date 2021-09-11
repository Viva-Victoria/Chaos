using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.PostgreSQL
{
    public class PostgresMigrator : IMigrator<PostgresMetadata>
    {
        private Postgres postgres;
        private ILogger logger;
        private PostgresMetadata metadata;

        public void Prepare(string connectionString, PostgresMetadata metadata, ILogger logger)
        {
            this.metadata = metadata;
            this.logger = logger;
            postgres = new Postgres(connectionString, logger);
        }

        public long GetVersion()
        {
            if (postgres.CheckTableExists(metadata.TableName))
            {
                return postgres.Query<long>(metadata.GetVersionQuery);
            }

            logger.Log(LogLevel.Debug, $"New {metadata.TableName} metadata table will be created.");
            postgres.Execute(metadata.CreateTableQuery);
            logger.Log(LogLevel.Debug, "Metadata table created.");

            return 0L;
        }

        public void SetVersion(long version)
        {
            postgres.Execute(metadata.SetVersionQuery, new { version });
            logger.Log(LogLevel.Debug, $"Migration metadata saved");
        }

        public void Apply(string migration)
        {
            postgres.Execute(migration);
        }

        public void ApplyInTransaction(string migration)
        {
            postgres.ExecuteWithTransaction(migration);
        }
    }
}