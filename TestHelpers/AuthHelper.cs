using Backend1;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging.Abstractions;
namespace TestHelpers;

[ExcludeFromCodeCoverage]
public static class AuthHelper
{
    public static async Task<string> GetAccessToken()
    {
        // Construct JSON data needed to get token.
        var jsonObject = new JObject();
        jsonObject["client_id"] = EnvVarHelper.GetVariable("AUTH0_CLIENTID");
        jsonObject["client_secret"] = EnvVarHelper.GetVariable("AUTH0_CLIENT_SECRET");
        jsonObject["audience"] = EnvVarHelper.GetVariable("AUTH0_AUDIENCE");
        jsonObject["grant_type"] = "client_credentials";

        // Serialize the object into a string for the request.
        var jsonString = JsonConvert.SerializeObject(jsonObject);

        // Create and execute the request.
        var client = new RestClient("https://dev-85peonfew8syv3it.us.auth0.com/oauth/token");
        var request = new RestRequest();
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
        var response = await client.PostAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Post request was not successful.");

        var jsonContentString = response.Content;

        if (jsonContentString is null)
            throw new Exception("Failed to retrieve token content.");

        var responseObject = JsonConvert.DeserializeObject<JObject>(jsonContentString) ?? throw new Exception("Failed to deserialze JSON content.");

        if (!responseObject.ContainsKey("access_token"))
            throw new Exception("DeserialzedObject `responseObject` does not contain `access_token`.");

        string? accessToken = responseObject["access_token"]?.ToString();

        if (accessToken is null)
            throw new Exception("Could not parse access token.");
        
        return accessToken;
    }

    public static ClaimsPrincipal GetClaims(string identityString = "TestUser", string? nameIdentifier = "auth0|11110000000A")
    {
        var identity = new GenericIdentity(identityString);
        var claims = new List<Claim>();

        if (nameIdentifier is not null)
            claims.Add(new Claim(ClaimTypes.NameIdentifier.ToString(), nameIdentifier));

        var claimsIdentity = new ClaimsIdentity(identity, claims);

        var rv = new ClaimsPrincipal(claimsIdentity);
        return rv;
    }

    public static async Task<HttpClient> ConstructAuthorizedClient(WebApplicationFactory<Program> factory, string? token = null)
    {
        // Get token if one was not manually passed in.
        if (token is null)
            token = await GetAccessToken();

        // Construct client and set auth.
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    // I wrote this helper but decided to instead just unit test most controller endpoints.
    // I'm going to keep this here for now, though.
    // I am commenting it out due to changes that are preventing it from compiling.
    /*
    public static async Task<uint> GetBusinessIdFromToken(string? token = null)
    {
        if (token is null)
            token = await GetAccessToken();

        var tokenHandler = new JwtSecurityTokenHandler();

        var accessToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (accessToken is null)
            throw new Exception("Failed to parse access token string.");

        var nameIdentifier = accessToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (nameIdentifier is null)
            throw new Exception("Access token did not have ClaimType matching NameIdentifier");

        var claimsPrincial = GetClaims(nameIdentifier: nameIdentifier);

        var resolver = new UserBusinessIdResolver(NullLogger<UserBusinessIdResolver>.Instance);

        return resolver.GetBusinessIdFromClaimsPrincipal(claimsPrincial);
    }
    */
}
