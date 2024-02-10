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
        var item = GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .Throws(new Exception("AddException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Add(item));
    }

    [Fact]
    public void TestAddSuccess()
    {
        var item = GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Add(item));
    }

    private static Item GetBoilerplateItem()
    {
        return new Item
        {
            Id = 0,
            BusinessId = 0,
            Name = "FakeItem",
            Quantity = 5
        };
    }
    
}