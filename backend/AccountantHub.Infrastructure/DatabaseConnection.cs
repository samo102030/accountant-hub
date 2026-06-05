namespace AccountantHub.Infrastructure;

public static class DatabaseConnection
{
    public static string? FromEnvironment()
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        return string.IsNullOrWhiteSpace(databaseUrl) ? null : ParseDatabaseUrl(databaseUrl);
    }

    public static string ParseDatabaseUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;

        return $"Host={uri.Host};Port={uri.Port};" +
               $"Username={username};" +
               $"Password={password};" +
               $"Database={uri.AbsolutePath.TrimStart('/')};" +
               "SSL Mode=Require;Trust Server Certificate=true";
    }
}
