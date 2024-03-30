using Backend1.Abstractions;
using System.Security.Claims;

namespace Backend1;

public class UserBusinessIdResolver : IUserBusinessIdResolver
{
    private readonly ILogger _logger;

    public UserBusinessIdResolver(ILogger<UserBusinessIdResolver> logger)
    {
        _logger = logger;
    }

    // Probably should just keep the approach we had prior and use roles/scopes to check if a user can access the business they want.
    // But for ease of implementation now, I'm opting for this approach.
    // We can always change in the future.  Since we're not actually going to release this, I don't see any reason to make this more complicated than we actually need it to be.
    // For ease assuring that this method works as expected, I'm marking it public so I can unit test it.
    public uint GetBusinessIdFromClaimsPrincipal(ClaimsPrincipal user)
    {
        // Get NameIdentifer.
        var nameIdent = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (nameIdent is null)
            throw new Exception("Could not find NameIdentifier within claims.");

        // Auth0 NameIdentifier should be formatted as auth0|number
        var identNumString = nameIdent.Split("|")[1];
        var identNumSubString = identNumString.Substring(identNumString.Length - 8);
        
        _logger.LogInformation($"Identity sub lenth is {identNumSubString.Length}");

        var identNum = Convert.ToUInt32(identNumSubString, 16);

        _logger.LogInformation($"Converted Id is: {identNum}");

        return identNum;
    }
}
