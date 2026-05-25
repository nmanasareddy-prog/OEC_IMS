using Microsoft.Data.Sqlite;

namespace OEC.IMS.Infrastructure.Persistence;

/// <summary>
/// Resolves SQLite paths relative to the API working directory (backend/src/OEC.IMS.Api)
/// so the database file lives at backend/data/oec-ims.db.
/// </summary>
internal static class SqliteConnectionStringResolver
{
    private const string DefaultRelativePath = "../../../data/oec-ims.db";

    public static string Resolve(string? configuredConnectionString)
    {
        var connectionString = string.IsNullOrWhiteSpace(configuredConnectionString)
            ? $"Data Source={DefaultRelativePath}"
            : configuredConnectionString;

        var builder = new SqliteConnectionStringBuilder(connectionString);
        var dataSource = builder.DataSource;

        if (!Path.IsPathRooted(dataSource))
        {
            dataSource = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), dataSource));
            builder.DataSource = dataSource;
        }

        var directory = Path.GetDirectoryName(dataSource);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return builder.ConnectionString;
    }
}
