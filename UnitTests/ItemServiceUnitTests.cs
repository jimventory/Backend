using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;

namespace UnitTests;

public class ItemServiceUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    
    public ItemServiceUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;
    }
    
    [Fact]
    public void TestAddException()
    {
        var item = new Item
        {
            Id = 0,
            BusinessId = 0,
            Name = "FakeItemException",
            Quantity = 5
        };

        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .Throws(new Exception("AddException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Add(item));
    }
    
}