using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;

namespace ClickHouseSample
{
    public class Settings : ISettings
    {
        public string ConnectionString =>
            "Compress=True;CheckCompressedHash=False;Compressor=lz4;Host=192.168.5.1;Port=9000;User=click_house;Password=eCUL22G6WcpnMkU7;SocketTimeout=6000;Database=default;";
        
        public TransactionMode TransactionMode => TransactionMode.None;
    }
}