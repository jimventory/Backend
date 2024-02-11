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
}