using System.Collections.Generic;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrationReader<TMigration>
        where TMigration : IMigration
    {
        public List<TMigration> Read();
    }
}