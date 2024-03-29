using Microsoft.AspNetCore.Mvc.Testing;
using TestHelpers;
using Xunit.Abstractions;

namespace IntegrationTests;

public class InventoryControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public InventoryControllerIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _output = outputHelper;
    }

    [Fact]
    public async Task UnauthorizedPrivateEndpointTest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("api/inventory/private");
        Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
    }

    [Fact]
    public async Task AuthorizedPrivateEndpointTest()
    {
        var client = await AuthHelper.ConstructAuthorizedClient(_factory);
        var response = await client.GetAsync("api/inventory/private");

        response.EnsureSuccessStatusCode();
    }
}
