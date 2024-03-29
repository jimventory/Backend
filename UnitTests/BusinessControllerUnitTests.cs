using Xunit.Abstractions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Controllers;
using TestHelpers;
namespace UnitTests;

public class BusinessControllerUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    private IBusinessService _goodBusinessService;
    private IBusinessService _badBusinessService;
    private BusinessController _goodController;
    private BusinessController _badController;

    public BusinessControllerUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;

        SetupFakes();

        _goodController = new BusinessController(_goodBusinessService);
        _badController = new BusinessController(_badBusinessService);
    }

    [Fact]
    public void TestAddBusinessSuccess()
    {
        var business = BusinessHelper.GetBoilerplateBusiness(name: "UniqueName", id: 23);
        var response = _goodController.Register(business);

        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(business, okResult.Value);
    }

    [Fact]
    public void TestAddBusinessStatusCode_500()
    {
        var response = _badController.Register(BusinessHelper.GetBoilerplateBusiness());
        var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    private void SetupFakes()
    {
        _goodBusinessService = A.Fake<IBusinessService>();
        _badBusinessService = A.Fake<IBusinessService>();

        // Set up good calls.
        A.CallTo(() => _goodBusinessService.Add(A<Business>.Ignored))
            .Returns(true);
        
        // Set up bad calls.
        A.CallTo(() => _badBusinessService.Add(A<Business>.Ignored))
            .Returns(false);
    }
}
