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
        return _db.Items.Find(id) ?? throw new Exception($"Could not find item with ID {id}.");
    }

    public IEnumerable<Item> GetAll()
    {
        return _db.Items.ToList();
    }

    public void Add(Item it)
    {
        _db.Items.Add(it);
        _db.SaveChanges();
    }

    public void Update(Item it)
    {
        var old = GetById(it.Id);
        _db.Items.Entry(old).CurrentValues.SetValues(it);
        _db.SaveChanges();
    }

    public void Delete(Item it)
    {
        _db.Items.Remove(it);
        _db.SaveChanges();
    }
}