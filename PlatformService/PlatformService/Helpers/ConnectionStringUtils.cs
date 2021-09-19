namespace PlatformService.Helpers
{
    public static class ConnectionStringUtils
    {
        public static string Prepare(string connectionString, string password) => 
            connectionString.Replace("{DB_PASSWORD}", password);
    }
}
