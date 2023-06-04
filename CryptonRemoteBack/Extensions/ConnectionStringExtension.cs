using Npgsql;

namespace CryptonRemoteBack.Extensions
{
    internal static class ConnectionStringExtension
    {
        public static string AppendApplicationName(string connectionString)
        {
            NpgsqlConnectionStringBuilder builder = new(connectionString);
            if (string.IsNullOrEmpty(builder.ApplicationName))
            {
                string? @namespace = typeof(ConnectionStringExtension).Namespace;
                builder.ApplicationName = @namespace?.Remove(@namespace.LastIndexOf('.'));
            }

            return builder.ConnectionString;
        }
    }
}
