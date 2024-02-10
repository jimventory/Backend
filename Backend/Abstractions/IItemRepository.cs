using Backend1.Models;

namespace Backend1.Abstractions;

public interface IItemRepository
{
    Item GetById(uint id);
    IEnumerable<Item> GetAll();
    void Add(Item it);
    void Update(uint id, Item it);
    void Delete(Item it);
}