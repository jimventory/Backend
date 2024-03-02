using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using TestHelpers;

namespace IntegrationTests;

public class InventoryControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public InventoryControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetInventoryTest()
    {
        const int hardCodedBusinessId = 10;
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"api/inventory/getInventory/{hardCodedBusinessId}");
        response.EnsureSuccessStatusCode();
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
        var accessToken = await TokenHelper.GetAccessToken();
        
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await client.GetAsync("api/inventory/private");
        response.EnsureSuccessStatusCode();
    }
}