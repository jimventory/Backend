using Backend1.Abstractions;
using System.Security.Claims;

namespace Backend1;

public class UserBusinessIdResolver : IUserBusinessIdResolver
{
    public virtual uint GetBusinessIdFromClaimsPrincipal(ClaimsPrincipal user)
    {
        // Get NameIdentifer.
        var nameIdent = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (nameIdent is null)
            throw new Exception("Could not find NameIdentifier within claims.");

        // Auth0 NameIdentifier should be formatted as auth0|number
        var identNumString = nameIdent.Split("|")[1];
        var identNumSubString = identNumString.Substring(identNumString.Length - 8);
        
        var identNum = Convert.ToUInt32(identNumSubString, 16);

        return identNum;
    }
}
