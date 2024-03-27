using Backend1.Models;
using System.Security.Claims;

namespace Backend1.Abstractions;

public interface IItemService
{
    bool Add(Item item);
    bool Update(Item item);
    bool Delete(Item item);
    bool Delete(uint id);
    IEnumerable<Item> GetBusinessInventoryItems(ClaimsPrincipal user);
}
