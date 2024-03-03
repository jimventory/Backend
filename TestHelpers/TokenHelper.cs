using Backend1;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TestHelpers;

public static class TokenHelper
{
    public static async Task<string> GetAccessToken()
    {
        var clientId = EnvVarHelper.GetVariable("AUTH0_CLIENTID");
        var clientSecret = EnvVarHelper.GetVariable("AUTH0_CLIENT_SECRET");
        var audience = EnvVarHelper.GetVariable("AUTH0_AUDIENCE");
        
        var client = new HttpClient();
        var content = new StringContent($"{{\"client_id\":\"{clientId}\",\"client_secret\":\"{clientSecret}\",\"audience\":\"{audience}\",\"grant_type\":\"client_credentials\"}}");

        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await client.PostAsync("https://dev-85peonfew8syv3it.us.auth0.com/oauth/token", content);

        response.EnsureSuccessStatusCode();

        var jsonContentString = await response.Content.ReadAsStringAsync();
        

        var jsonObject = JsonDocument.Parse(jsonContentString).RootElement;
        var accessToken = jsonObject.GetProperty("access_token").GetString();

        if (accessToken is null)
            throw new Exception("Could not get access token.");
        
        return accessToken;
    }
}