using Backend1;
using Backend1.Abstractions;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;

namespace UnitTests;

public class UserBusinessIdResolverValidatorUnitTests
{
    private IBusinessService _fakeBusinessService;

    public UserBusinessIdResolverValidatorUnitTests()
    {
        _fakeBusinessService = A.Fake<IBusinessService>();

        A.CallTo(() => _fakeBusinessService.Find(A<uint>.Ignored))
            .Returns(true);

    }

    [Fact]
    public void TestGetBusinessIdFromClaims()
    {
        var sut = new UserBusinessIdResolverValidator(_fakeBusinessService);
        var rv = sut.GetBusinessIdFromClaimsPrincipal(AuthHelper.GetClaims());

        // The default ClaimsPrincipal returned by our AuthHelper should have a NameIdentifier that returns 10, for business Id 10.
        Assert.Equal((uint)10, rv);
    }

    [Fact]
    public void TestGetBusinessIdFromClaimsThrowsOnNullNameIdentifier()
    {
        var sut = new UserBusinessIdResolverValidator(_fakeBusinessService);
        var claim = AuthHelper.GetClaims(nameIdentifier: null);

        Assert.Throws<Exception>(() => sut.GetBusinessIdFromClaimsPrincipal(claim));
    }

    [Fact]
    public void TestGetBusinessIdFromClaimsThrowsOnBusinessNotFound()
    {
        var emptyBusinessService = A.Fake<IBusinessService>();
        
        A.CallTo(() => emptyBusinessService.Find(A<uint>.Ignored))
            .Returns(false);

        var sut = new UserBusinessIdResolverValidator(emptyBusinessService);

        Assert.Throws<Exception>(() => sut.GetBusinessIdFromClaimsPrincipal(AuthHelper.GetClaims()));
    }
}
