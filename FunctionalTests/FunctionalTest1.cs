using System.Diagnostics;
using System.Net.Http.Headers;
using Backend1;
using Xunit.Abstractions;

namespace FunctionalTests;

public class FunctionalTest1
{
    private readonly ITestOutputHelper _testOutputHelper;
    private const string BackendPath = "../../../../Backend";
    private const string Host = "localhost";
    private const string Port = "5161";
    private const string ApiPath = $"http://{Host}:{Port}/api";
    private Process? _proc = null;
    
    public FunctionalTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task TestApiTest()
    {
        const string endpoint = $"{ApiPath}/test/helloWorld";
        const string expected = "Hello, world!\n";
        _testOutputHelper.WriteLine(endpoint);

        StartBackend();
        
        // Allow 5 seconds for backend to start.
        await Task.Delay(10_000);

        await HitEndpoint(endpoint, expected);
    }

    [Fact]
    public async Task InventorySizeTest()
    {
        const string endpoint = $"{ApiPath}/inventory/size";
        const string expected = "The global inventory currently contains 0 items.\n";
        _testOutputHelper.WriteLine(endpoint);

        StartBackend();
        
        // Allow 5 seconds for backend to start.
        await Task.Delay(10_000);

        // Test endpoint.
        await HitEndpoint(endpoint, expected);
    }

    [Fact]
    public async Task PrivateEndpointNoAuth()
    {
        const string endpoint = $"{ApiPath}/inventory/private";
        const string expected = "";
        _testOutputHelper.WriteLine(endpoint);

        StartBackend();

        await Task.Delay(10_000);
        
        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(endpoint);

            Assert.False(response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
            Assert.Fail(e.ToString());
        }
        finally
        {
            KillBackend();
        }
    }

    [Fact]
    public async Task PrivateEndpointAuth()
    {
        const string endpoint = $"{ApiPath}/inventory/private";
        const string expected = "This is a private endpoint in the inventory controller.\n";
        _testOutputHelper.WriteLine(endpoint);

        StartBackend();

        await Task.Delay(10_000);
        
        using var client = new HttpClient();
        try
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", EnvVarHelper.GetVariable("AUTH0_PRIVATE_INV_TOKEN"));

            var response = await client.GetAsync(endpoint);

            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(expected, content);
        }
        catch (Exception e)
        {
            Assert.Fail(e.ToString());
        }
        finally
        {
            KillBackend();
        }
    }
    
    private async Task HitEndpoint(string endpoint, string expected)
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(endpoint);

            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(expected, content);

        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail(e.ToString());
        }
        finally
        {
            KillBackend();
        }
    }
    
    private void KillBackend()
    {
        _proc?.Kill(true);
        _proc = null;
    }
    
    private void StartBackend()
    {
        KillBackend();
        
        // Execute backend project using dotnet.
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {BackendPath}",
                UseShellExecute = true,
                CreateNoWindow = true,
            }
        };

        var started = proc.Start();

        if (started is false)
            throw new Exception("Could not start backend.");

        _proc = proc;
    }
}
