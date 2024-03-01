using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;
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
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .Throws(new Exception("AddException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Add(item));
    }

    [Fact]
    public void TestAddSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Add(item));
    }

    [Fact]
    public void TestUpdateGetItemException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.GetById(item.Id))
            .Throws(new Exception("GetItemById Exception"));

        A.CallTo(() => fakeItemRepo.Update(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Update(item));      
    }
    
    [Fact]
    public void TestUpdateException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Update(item))
            .Throws(new Exception("UpdateException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Update(item));  
    }

    [Fact]
    public void TestUpdateSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Update(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Update(item));
    }

    [Fact]
    public void TestDeleteByItemException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .Throws(new Exception("DeleteItemException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item));  
    }

    [Fact]
    public void TestDeleteByIdGetItemException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.GetById(item.Id))
            .Throws(new Exception("GetById Exception"));

        A.CallTo(() => fakeItemRepo.Delete(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item.Id));  
    }
    
    [Fact]
    public void TestDeleteByIdException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.GetById(item.Id))
            .Returns(item);
        
        A.CallTo(() => fakeItemRepo.Delete(item))
            .Throws(new Exception("DeleteItemException"));

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item.Id));  
    }

    [Fact]
    public void TestDeleteItemSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Delete(item));
    }
    
    [Fact]
    public void TestDeleteByIdSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Delete(0));
    }

    [Fact]
    public void TestUpdateSalesSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();
        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        var saleAmount = (uint) 1;

        var result = sut.UpdateSales(item, saleAmount);
        Assert.True(result);
    }

    [Fact]
    public void TestUpdateSalesExeption()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();
        var sut = new ItemService(fakeItemRepo, NullLogger<ItemService>.Instance);

        var saleAmount = (uint) 100;

        var result = sut.UpdateSales(item, saleAmount);
        Assert.False(result);
    }

}