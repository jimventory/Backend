using Xunit.Abstractions;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Controllers;
using System.Security.Claims;
using TestHelpers;
namespace UnitTests;

public class InventoryControllerUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    private IItemService? _goodItemService;
    private IItemService? _badItemService;
    private NullLogger<InventoryController>? _fakeLogger;
    private InventoryController _goodController;
    private InventoryController _badController;

    public InventoryControllerUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;

        SetupFakes();

        _goodController = new InventoryController(_goodItemService!, _fakeLogger!);
        _badController = new InventoryController(_badItemService!, _fakeLogger!);
    }

    [Fact]
    public void TestAddItemSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem(name: "UniqueName", id: 23);
        var response = _goodController.AddItem(item);

        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(item, okResult.Value);
    }

    [Fact]
    public void TestAddItemStatusCode_500()
    {
        var response = _badController.AddItem(ItemHelper.GetBoilerplateItem());
        var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void TestUpdateItemSuccess()
    {
        // We're just testing the status codes, no actual updating occurs in the mocks.
        var response = _goodController.UpdateItem(ItemHelper.GetBoilerplateItem());
        Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void TestUpdateItemStatusCode_500()
    {
        var response = _badController.UpdateItem(ItemHelper.GetBoilerplateItem());
        var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void TestDeleteItemSuccess()
    {
        var response = _goodController.DeleteItem((uint) 5);
        Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void TestDeleteItemStatusCode_500()
    {
        var response = _badController.DeleteItem((uint) 5);
        var statusCodeResult = Assert.IsType<StatusCodeResult>(response);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void TestGetInventorySuccess()
    {
        var response = _goodController.GetInventory();
        Assert.IsType<OkObjectResult>(response);
    }

    private void SetupFakes()
    {
        _goodItemService = A.Fake<IItemService>();
        _badItemService = A.Fake<IItemService>();

        // Set up good calls.
        A.CallTo(() => _goodItemService.Add(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(true);

        A.CallTo(() => _goodItemService.Update(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(true);

        A.CallTo(() => _goodItemService.Delete(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(true);

        A.CallTo(() => _goodItemService.Delete(A<uint>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(true);

        A.CallTo(() => _goodItemService.GetBusinessInventoryItems(A<ClaimsPrincipal>.Ignored))
            .Returns(new List<Item> { ItemHelper.GetBoilerplateItem() });
        
        // Set up bad calls.
        A.CallTo(() => _badItemService.Add(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(false);

        A.CallTo(() => _badItemService.Update(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(false);

        A.CallTo(() => _badItemService.Delete(A<Item>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(false);

        A.CallTo(() => _badItemService.Delete(A<uint>.Ignored, A<ClaimsPrincipal>.Ignored))
            .Returns(false);

        A.CallTo(() => _badItemService.GetBusinessInventoryItems(A<ClaimsPrincipal>.Ignored))
            .Returns(Enumerable.Empty<Item>());
        
        _fakeLogger = NullLogger<InventoryController>.Instance;
    }
}
