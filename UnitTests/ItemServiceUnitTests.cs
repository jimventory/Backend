using Backend1.Abstractions;
using Backend1.Models;
using Backend1.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;
using Xunit.Abstractions;
using System.Security.Claims;

namespace UnitTests;

public class ItemServiceUnitTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly ClaimsPrincipal _claims;
    private IUserBusinessIdValidator _fakeResolver;

    public ItemServiceUnitTests(ITestOutputHelper testOutputHelper)
    {
        _outputHelper = testOutputHelper;
        _claims = AuthHelper.GetClaims();
       
        // Create a ClaimsResolver that assumes the businessId is 10.
        _fakeResolver = A.Fake<IUserBusinessIdValidator>();
        A.CallTo(() => _fakeResolver.GetBusinessIdFromClaimsPrincipal(A<ClaimsPrincipal>.Ignored))
            .Returns((uint)10);
    }
    
    [Fact]
    public void TestAddException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .Throws(new Exception("AddException"));

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Add(item, _claims));
    }

    [Fact]
    public void TestAddSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Add(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Add(item, _claims));
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

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Update(item, _claims));      
    }
    
    [Fact]
    public void TestUpdateException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Update(item))
            .Throws(new Exception("UpdateException"));

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Update(item, _claims));  
    }

    [Fact]
    public void TestUpdateSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem(busId: 10);
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Update(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);
       
        Assert.True(sut.Update(item, _claims));
    }

    [Fact]
    public void TestDeleteByItemException()
    {
        var item = ItemHelper.GetBoilerplateItem();
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .Throws(new Exception("DeleteItemException"));

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item, _claims));  
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

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item.Id, _claims));  
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

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        Assert.False(sut.Delete(item.Id, _claims));  
    }

    [Fact]
    public void TestDeleteItemSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem(busId: 10);
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .DoesNothing();

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Delete(item, _claims));
    }
    
    [Fact]
    public void TestDeleteByIdSuccess()
    {
        var item = ItemHelper.GetBoilerplateItem(busId: 10);
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.Delete(item))
            .DoesNothing();

        A.CallTo(() => fakeItemRepo.GetById(item.Id))
            .Returns(item);

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);
        
        Assert.True(sut.Delete(item.Id, _claims));
    }

    [Fact]
    public void TestGetBusinessInventorySuccess()
    {
        var nonBusinessItem = ItemHelper.GetBoilerplateItem();
        nonBusinessItem.BusinessId = 1;
        
        var items = new List<Item>()
        {
            ItemHelper.GetBoilerplateItem(busId: 10),
            ItemHelper.GetBoilerplateItem(busId: 10),
            nonBusinessItem
        };
      
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.GetAll())
            .Returns(items);

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);

        var rv = sut.GetBusinessInventoryItems(_claims);
        
        Assert.Equal(items.Count - 1, rv.Count());
    }

    [Fact]
    public void TestGetBusinessInventoryException()
    {
        var fakeItemRepo = A.Fake<IItemRepository>();

        A.CallTo(() => fakeItemRepo.GetAll())
            .Throws(new Exception("Failed to get all items."));

        var sut = new ItemService(fakeItemRepo, _fakeResolver, NullLogger<ItemService>.Instance);
        var rv = sut.GetBusinessInventoryItems(_claims);
        Assert.Empty(rv);
    }

}
