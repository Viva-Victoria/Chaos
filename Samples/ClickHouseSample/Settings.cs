using System.Collections.Generic;
using VivaVictoria.Chaos.ClickHouse.Interfaces;
using VivaVictoria.Chaos.Enums;

namespace ClickHouseSample
{
    public class Settings : IClickHouseSettings
    {
        public string ConnectionString =>
            "Compress=True;CheckCompressedHash=False;Compressor=lz4;Host=192.168.5.1;Port=9000;User=click_house;Password=eCUL22G6WcpnMkU7;SocketTimeout=6000;Database=default;";

        public bool ParallelListeners => false;
        public IEnumerable<MigrationState> SaveStates => new[] { MigrationState.Applied, MigrationState.Started, MigrationState.Failed };
    }
}