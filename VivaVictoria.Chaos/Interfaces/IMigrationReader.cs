using System.Collections.Generic;
using VivaVictoria.Chaos.Models;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IMigrationReader
    {
        public List<MigrationInfo> Read();
    }
}