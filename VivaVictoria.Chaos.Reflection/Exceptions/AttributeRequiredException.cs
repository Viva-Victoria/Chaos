using System;
using VivaVictoria.Chaos.Reflection.Attributes;

namespace VivaVictoria.Chaos.Reflection.Exceptions
{
    public class AttributeRequiredException : Exception
    {
        public AttributeRequiredException(Type type)
            : base(
                $"Type {type.FullName} should have {typeof(MigrationAttribute).FullName} attribute with migration info like version, name and transaction mode")
        {
        }
    }
}