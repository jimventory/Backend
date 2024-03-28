using Backend1.Abstractions;
using System.Security.Claims;

namespace Backend1;

public class UserBusinessIdResolver : IUserBusinessIdResolver
{
    public uint GetBusinessIdFromClaimsPrincipal(ClaimsPrincipal claims)
    {
        return 0;
    }
}
