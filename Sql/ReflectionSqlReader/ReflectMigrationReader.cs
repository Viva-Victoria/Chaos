using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.ReflectionSqlReader.Attributes;
using VivaVictoria.Chaos.ReflectionSqlReader.Exceptions;
using VivaVictoria.Chaos.ReflectionSqlReader.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ReflectionSqlReader
{
    public class ReflectMigrationReader : IMigrationReader<Migration>
    {
        private readonly ILogger<ReflectMigrationReader> logger;
        private readonly Assembly assembly;

        public ReflectMigrationReader(ILogger<ReflectMigrationReader> logger, Assembly assembly)
        {
            this.logger = logger;
            this.assembly = assembly;
        }

        public List<Migration> Read()
        {
            var migrationTypes = assembly.GetTypes().Where(type => 
                type.GetInterfaces().Contains(typeof(IReflectMigration)));
            var result = new List<Migration>();

            foreach (var type in migrationTypes)
            {
                logger.LogDebug($"Loading {type.FullName}");
                var info = type.GetCustomAttribute<MigrationAttribute>();
                if (info == null)
                {
                    if (type.GetCustomAttribute<BaseClassAttribute>() == null)
                        throw new AttributeRequiredException(type);
                    continue;
                }

                logger.LogDebug($"Instantiating {type.FullName}");
                var instance = Activator.CreateInstance(type) as IReflectMigration;
                result.Add(new Migration {
                    Version = info.Version, 
                    Name = info.Name, 
                    TransactionMode = info.TransactionMode, 
                    Up = instance.Up(),
                    Down = instance.Down()
                });
            }

            return result;
        }
    }
}