using Backend1;
using Backend1.Abstractions;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;

namespace UnitTests;

public class UserBusinessIdResolverUnitTests
{
    private IBusinessService _fakeBusinessService;

    public UserBusinessIdResolverUnitTests()
    {
        _fakeBusinessService = A.Fake<IBusinessService>();

        A.CallTo(() => _fakeBusinessService.Find(A<uint>.Ignored))
            .Returns(true);

    }

    [Fact]
    public void TestGetBusinessIdFromClaims()
    {
        var sut = new UserBusinessIdResolver(_fakeBusinessService, NullLogger<UserBusinessIdResolver>.Instance);
        var rv = sut.GetBusinessIdFromClaimsPrincipal(AuthHelper.GetClaims());

        // The default ClaimsPrincipal returned by our AuthHelper should have a NameIdentifier that returns 10, for business Id 10.
        Assert.Equal((uint)10, rv);
    }

    [Fact]
    public void TestGetBusinessIdFromClaimsThrowsOnNullNameIdentifier()
    {
        var sut = new UserBusinessIdResolver(_fakeBusinessService, NullLogger<UserBusinessIdResolver>.Instance);
        var claim = AuthHelper.GetClaims(nameIdentifier: null);

        Assert.Throws<Exception>(() => sut.GetBusinessIdFromClaimsPrincipal(claim));
    }

    [Fact]
    public void TestGetBusinessIdFromClaimsThrowsOnBusinessNotFound()
    {
        var emptyBusinessService = A.Fake<IBusinessService>();
        
        A.CallTo(() => emptyBusinessService.Find(A<uint>.Ignored))
            .Returns(false);

        var sut = new UserBusinessIdResolver(emptyBusinessService, NullLogger<UserBusinessIdResolver>.Instance);

        Assert.Throws<Exception>(() => sut.GetBusinessIdFromClaimsPrincipal(AuthHelper.GetClaims()));
    }
}
