using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;
using Xunit.Abstractions;

namespace UnitTests;

public class BusinessServiceUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    
    public BusinessServiceUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;
    }

    [Fact]
    public void TestAddException()
    {
        var business = BusinessHelper.GetBoilerplateBusiness();
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();

        A.CallTo(() => fakeBusinessRepo.Add(business))
            .Throws(new Exception("AddException"));

        var sut = new BusinessService(fakeBusinessRepo, NullLogger<BusinessService>.Instance);
        
        Assert.False(sut.Add(business));
    }

    [Fact]
    public void TestAddSuccess()
    {
        var business = BusinessHelper.GetBoilerplateBusiness();
        var fakeBusinessRepo = A.Fake<IBusinessRepository>();

        A.CallTo(() => fakeBusinessRepo.Add(business))
            .DoesNothing();

        var sut = new BusinessService(fakeBusinessRepo, NullLogger<BusinessService>.Instance);
        
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

        var sut = new BusinessService(fakeBusinessRepo, NullLogger<BusinessService>.Instance);

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

        var sut = new BusinessService(fakeBusinessRepo, NullLogger<BusinessService>.Instance);

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

        var sut = new BusinessService(fakeBusinessRepo, NullLogger<BusinessService>.Instance);

        Assert.False(sut.Find(testId));
    }
}
