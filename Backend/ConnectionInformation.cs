namespace Backend1;

// At first glance, making classes that are just holders of information seems wrong, but it's useful!
public static class ConnectionInformation
{
    // A bunch of wasteful variables.
    private const string Host = "75.136.42.133";
    private const string Database = "jimventory-test";
    private const string User = "postgres";
    private const string Password = "jimjim";
    
    // A useful variable made up of wasteful ones.
    public const string PostgresConnString = $"Host={Host};Database={Database};Username={User};Password={Password}";
}