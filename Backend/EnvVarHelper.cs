namespace Backend1;

public static class EnvVarHelper
{
    public static string GetVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? 
               throw new Exception($"Tried to retrieve env. var `{name}`, but value is null.  Check that your env. vars are set up correctly.");
    }
}