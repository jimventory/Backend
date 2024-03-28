using Backend1;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;

namespace UnitTests;

public class UserBusinessIdResolverTests
{
    [Fact]
    public void TestGetBusinessIdFromClaims()
    {
        var sut = new UserBusinessIdResolver(NullLogger<UserBusinessIdResolver>.Instance);
        var rv = sut.GetBusinessIdFromClaimsPrincipal(AuthHelper.GetClaims());

        // The default ClaimsPrincipal returned by our AuthHelper should have a NameIdentifier that returns 10, for business Id 10.
        Assert.Equal((uint)10, rv);
    }

    [Fact]
    public void TestGetBusinessIdFromClaimsThrowsOnNullNameIdentifier()
    {
        var sut = new UserBusinessIdResolver(NullLogger<UserBusinessIdResolver>.Instance);
        var claim = AuthHelper.GetClaims(nameIdentifier: null);

        Assert.Throws<Exception>(() => sut.GetBusinessIdFromClaimsPrincipal(claim));
    }
}