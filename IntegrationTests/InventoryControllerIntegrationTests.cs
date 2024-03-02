using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Backend1.Models;
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
    /*
     * This is one big test for add, update, and delete.
     * The reason is because these tests deal with the actual inventory database.
     * I do not feel like making a separate database to test with, so this is my solution.
     */
    public async Task AddUpdateDeleteTest()
    {
        var item = new Item
        {
            Name = "InappropriateTestItem",
            BusinessId = 42069,
            Id = 80085,
            Quantity = 1,
            Price = 5,
            About = "This is a test item.",
            LowStockNotification = 0,
            Sales = 0
        };

        var client = _factory.CreateClient();

        // Create
        var json = JsonConvert.SerializeObject(item);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/inventory/add", content);

        response.EnsureSuccessStatusCode();
        
        var item2 = new Item
        {
            Name = "InappropriateTestItem",
            BusinessId = 42069,
            Id = 80085,
            Quantity = 10,
            Price = 7,
            About = "This is a test item.",
            LowStockNotification = 0,
            Sales = 5
        };
    
        // Update
        json = JsonConvert.SerializeObject(item2);
        content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        response = await client.PutAsync("api/inventory/update", content);

        response.EnsureSuccessStatusCode();

        // Delete
        response = await client.DeleteAsync($"api/inventory/remove/{item2.Id}");
        response.EnsureSuccessStatusCode();
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