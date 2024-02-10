using Backend1.Abstractions;
using Backend1.Data;
using Backend1.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend1.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly InventoryContext _db;
    
    public ItemRepository(InventoryContext db)
    {
        _db = db;
    }
    
    public Item GetById(uint id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Item> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(Item it)
    {
        throw new NotImplementedException();
    }

    public void Update(Item it)
    {
        throw new NotImplementedException();
    }

    public void Delete(Item it)
    {
        throw new NotImplementedException();
    }
}