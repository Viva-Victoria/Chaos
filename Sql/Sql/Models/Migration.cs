using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;

namespace VivaVictoria.Chaos.Sql.Models
{
    public class Migration : IMigration
    {
        public long Version { get; set; } = -1;
        public string Name { get; set; }
        public TransactionMode TransactionMode { get; set; }
        public string Up { get; set; }
        public string Down { get; set; }
    }
}