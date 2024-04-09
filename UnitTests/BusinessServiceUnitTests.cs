using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;
using Xunit.Abstractions;
using System.Security.Claims;

namespace UnitTests;

public class BusinessServiceUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IUserBusinessIdResolver _fakeResolver;
    private readonly ClaimsPrincipal _claims;

    public BusinessServiceUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;
        _claims = AuthHelper.GetClaims();

        _fakeResolver = A.Fake<IUserBusinessIdResolver>();
        A.CallTo(() => _fakeResolver.GetBusinessIdFromClaimsPrincipal(A<ClaimsPrincipal>.Ignored))
            .Returns((uint)10);
    }

    [Fact]
    public void TestAddException()
    {
        var business = BusinessHelper.GetBoilerplateBusiness();
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();

        A.CallTo(() => fakeBusinessRepo.Add(business))
            .Throws(new Exception("AddException"));

        var sut = new BusinessService(fakeBusinessRepo, _fakeResolver, NullLogger<BusinessService>.Instance);
        
        Assert.False(sut.Add(business));
    }

    [Fact]
    public void TestAddSuccess()
    {
        var business = BusinessHelper.GetBoilerplateBusiness();
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();

        A.CallTo(() => fakeBusinessRepo.Add(business))
            .DoesNothing();

        var sut = new BusinessService(fakeBusinessRepo, _fakeResolver, NullLogger<BusinessService>.Instance);
        
        Assert.True(sut.Add(business));
    }

    [Fact]
    public void TestFindFailure()
    {
        uint testId = 10;
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();
        var businesses = new List<Business> { BusinessHelper.GetBoilerplateBusiness(id: 5) };

        A.CallTo(() => fakeBusinessRepo.GetAll())
            .Returns(businesses);

        var sut = new BusinessService(fakeBusinessRepo, _fakeResolver, NullLogger<BusinessService>.Instance);

        Assert.False(sut.Find(testId));
    }
    
    [Fact]
    public void TestFindSuccess()
    {
        uint testId = 10;
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();
        var businesses = new List<Business> { BusinessHelper.GetBoilerplateBusiness(id: 10) };

        A.CallTo(() => fakeBusinessRepo.GetAll())
            .Returns(businesses);

        var sut = new BusinessService(fakeBusinessRepo, _fakeResolver, NullLogger<BusinessService>.Instance);

        Assert.True(sut.Find(testId));
    }

    [Fact]
    public void TestFindException()
    {
        uint testId = 10;
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();
        var businesses = new List<Business> { BusinessHelper.GetBoilerplateBusiness(id: 5) };

        A.CallTo(() => fakeBusinessRepo.GetAll())
            .Throws(new Exception("IBusinessRepository.GetAll Exception"));

        var sut = new BusinessService(fakeBusinessRepo, _fakeResolver, NullLogger<BusinessService>.Instance);

        Assert.False(sut.Find(testId));
    }

    // We could unit test the Find(ClaimsPrincipal) method, but meh, it is just a wrapper around the Find(uint) method currently.
    // I see no need to test that.
}
