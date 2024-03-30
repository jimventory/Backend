using Backend1;
using Xunit.Abstractions;

namespace UnitTests;

public class EnvVarHelperUnitTests
{
    private readonly ITestOutputHelper _outputHelper;

    public EnvVarHelperUnitTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void TestEnvHelperSuccess()
    {
        try 
        {
            // Create an environment variable to find.
            Environment.SetEnvironmentVariable("UnitTestEnvVar1", "HelloWorld");

            var rv = EnvVarHelper.GetVariable("UnitTestEnvVar1");

            Assert.Equal("HelloWorld", rv);
        }
        catch (Exception e)
        {
            _outputHelper.WriteLine(e.Message);
        }
        finally
        {
            // Remove environment variable.
            Environment.SetEnvironmentVariable("UnitTestEnvVar1", null);
        }

    }

    [Fact]
    public void TestEnvHelperThrowsException()
    {
        // Try to fetch a variable that doesn't exist.
        Assert.Throws<Exception>(() => EnvVarHelper.GetVariable("BadVariable"));
    }
}
