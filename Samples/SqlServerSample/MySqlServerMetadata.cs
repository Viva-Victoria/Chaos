using VivaVictoria.Chaos.SqlServer.Models;

namespace SqlServerSample
{
    public class MySqlServerMetadata : SqlServerMetadata
    {
        public override string Schema => "sql_server_sample";
    }
}