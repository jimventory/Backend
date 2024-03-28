using System.Security.Claims;

namespace Backend1.Abstractions;

public interface IUserBusinessIdResolver
{
    uint GetBusinessIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
}
