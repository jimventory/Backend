using Backend1.Models;
using System.Security.Claims;

namespace Backend1.Abstractions;

public interface IItemService
{
    bool Add(Item item, ClaimsPrincipal user);
    bool Update(Item item, ClaimsPrincipal user);
    bool Delete(Item item, ClaimsPrincipal user);
    bool Delete(uint id, ClaimsPrincipal user);
    IEnumerable<Item> GetBusinessInventoryItems(ClaimsPrincipal user);
}
