using Backend1.Data;
using Backend1.Models;
using Backend1.Repositories;
using Microsoft.EntityFrameworkCore;
using TestHelpers;

namespace IntegrationTests;

public class ItemRepositoryIntegrationTests
{
    private readonly DbContextOptions<InventoryContext> _options;

    public ItemRepositoryIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<InventoryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }
    
    [Fact]
    public void TestAddItem()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);
        var item = ItemHelper.GetBoilerplateItem();
            
        sut.Add(item);

        var found = db.Items.Find(item.Id);
            
        Assert.NotNull(found);
    }

    [Fact]
    public void TestUpdateItem()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);
        var item = ItemHelper.GetBoilerplateItem();
        
        // Add the initial item and fail the test if we for some reason didn't insert.
        sut.Add(item);
        var found = db.Items.Find(item.Id);
        Assert.NotNull(found);
        
        // Create a different object entirely to avoid immediately updating the Item (classes are reference types by default in C#)
        var updateItem = ItemHelper.GetBoilerplateItem();
        updateItem.Id = item.Id;
        updateItem.Name = "UpdatedName";
        
        // Verify that the names is different but the ID is the same.
        Assert.NotEqual(updateItem.Name, item.Name);
        Assert.Equal(updateItem.Id, item.Id);
        
        sut.Update(updateItem);
        
        found = db.Items.Find(item.Id);
            
        Assert.NotNull(found);
        Assert.Equal(updateItem.Name, found.Name);
    }

    [Fact]
    public void TestDeleteItem()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);
        var item = ItemHelper.GetBoilerplateItem();
            
        sut.Add(item);

        var found = db.Items.Find(item.Id);
            
        Assert.NotNull(found);
        
        sut.Delete(item);
        
        found = db.Items.Find(item.Id);
        
        Assert.Null(found);
    }

    [Fact]
    public void TestGetByIdException()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);

        Assert.Throws<Exception>(() => sut.GetById(0));
    }

    [Fact]
    public void TestGetByIdSuccess()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);
        
        var item = ItemHelper.GetBoilerplateItem();
            
        sut.Add(item);
        
        var found = db.Items.Find(item.Id);
            
        Assert.NotNull(found);

        var rv = sut.GetById(item.Id);
        Assert.Same(item, rv);
    }

    [Fact(Skip = "Issue with DBs not being disposed.")]
    public void TestGetAllEmpty()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);

        var coll = sut.GetAll();
        
        Assert.Empty(coll);
    }

    [Fact(Skip = "Issue with DBs not being disposed.")]
    public void TestGetAllWithItems()
    {
        using var db = new InventoryContext(_options);
        var sut = new ItemRepository(db);

        var items = new List<Item>();
        items.Add(ItemHelper.GetBoilerplateItem());
        items.Add(ItemHelper.GetBoilerplateItem());
        
        sut.Add(items[0]);
        sut.Add(items[1]);

        var coll = sut.GetAll();
        
        Assert.True(coll.SequenceEqual(items));
    }
}