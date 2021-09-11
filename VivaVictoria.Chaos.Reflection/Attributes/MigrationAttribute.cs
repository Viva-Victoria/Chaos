using System;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MigrationAttribute : Attribute
    {
        public long Version { get; set; }
        public string Name { get; set; }
        public TransactionMode TransactionMode { get; set; } = TransactionMode.Default;
    }
}