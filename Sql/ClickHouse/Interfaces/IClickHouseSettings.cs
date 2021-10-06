using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;

namespace VivaVictoria.Chaos.ClickHouse.Interfaces
{
    public interface IClickHouseSettings : ISettings
    {
        public string ConnectionString { get; }
    }
}