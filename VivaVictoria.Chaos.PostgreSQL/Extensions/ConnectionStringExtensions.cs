namespace VivaVictoria.Chaos.PostgreSQL.Extensions
{
    public static class ConnectionStringExtensions
    {
        public static string GetSearchPath(this string connectionString, string defaultValue)
        {
            var parts = connectionString.Split("SearchPath=");
            if (parts.Length < 2)
            {
                return defaultValue;
            }

            return parts[1].Split(";")[0];
        }
    }
}