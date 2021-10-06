using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Reflection.Attributes;
using VivaVictoria.Chaos.Reflection.Exceptions;
using VivaVictoria.Chaos.Reflection.Interfaces;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.Reflection
{
    public class ReflectMigrationReader : IMigrationReader<Migration>
    {
        private readonly Assembly assembly;

        public ReflectMigrationReader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public List<Migration> Read()
        {
            var migrationTypes = assembly.GetTypes().Where(type => 
                type.GetInterfaces().Contains(typeof(IReflectMigration)));
            var result = new List<Migration>();

            foreach (var type in migrationTypes)
            {
                var info = type.GetCustomAttribute<MigrationAttribute>();
                if (info == null)
                {
                    if (type.GetCustomAttribute<BaseClassAttribute>() == null)
                        throw new AttributeRequiredException(type);
                    continue;
                }

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