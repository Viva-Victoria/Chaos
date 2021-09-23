using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Models;
using VivaVictoria.Chaos.Reflection.Attributes;
using VivaVictoria.Chaos.Reflection.Exceptions;
using VivaVictoria.Chaos.Reflection.Interfaces;

namespace VivaVictoria.Chaos.Reflection
{
    public class ReflectMigrationReader : IMigrationReader
    {
        private readonly Assembly assembly;

        public ReflectMigrationReader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public List<Migration> Read()
        {
            var migrationTypes = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IMigration)));
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

                var instance = Activator.CreateInstance(type) as IMigration;
                result.Add(new Migration(info.Version, info.Name, info.TransactionMode, instance.Up(),
                    instance.Down()));
            }

            return result;
        }
    }
}