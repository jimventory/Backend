using Backend1.Data;
using Backend1.Repositories;
using Microsoft.EntityFrameworkCore;

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
        using (var db = new InventoryContext(_options))
        {
            var sut = new ItemRepository(db);
            
            
        }
    }
}