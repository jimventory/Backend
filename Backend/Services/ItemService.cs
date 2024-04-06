using Backend1.Abstractions;
using Backend1.Models;
using System.Security.Claims;

namespace Backend1.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;
    private readonly ILogger _logger;
    private readonly IUserBusinessIdResolver _claimsResolver;
    
    public ItemService(IItemRepository repo, IUserBusinessIdResolver claimsResolver, ILogger<ItemService> logger)
    {
        _repository = repo;
        _claimsResolver = claimsResolver;
        _logger = logger;
    }

    public bool Add(Item item, ClaimsPrincipal user)
    {
        try
        {
            var businessId = _claimsResolver.GetBusinessIdFromClaimsPrincipal(user);

            // Override user provided business Id.
            item.BusinessId = businessId;

            _repository.Add(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public bool Update(Item item, ClaimsPrincipal user)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            _repository.GetById(item.Id);
            
            var businessId = _claimsResolver.GetBusinessIdFromClaimsPrincipal(user);

            if (item.BusinessId != businessId)
                throw new Exception("You do not have permission to modify items for this business.");

            // Item exists, can update.
            
            _repository.Update(item);
        }
        catch (Exception e)
        {
            // Item did not exist.  Can't update it.
            // Should we just add it..?
            // For now, no.
            _logger.LogError("Failed to update item: {ExMessage}", e.Message);
            return false;
        }

        return true;
    }

    // No reason for this overload to exist, probably will delete in future refactoring if we don't use.
    public bool Delete(Item item, ClaimsPrincipal user)
    {
        try
        {
            var businessId = _claimsResolver.GetBusinessIdFromClaimsPrincipal(user);
            
            if (businessId != item.BusinessId)
                throw new Exception("You do not have permission to delete items for this business.");

            _repository.Delete(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }
    
    public bool Delete(uint id, ClaimsPrincipal user)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            var item = _repository.GetById(id);
            
            var businessId = _claimsResolver.GetBusinessIdFromClaimsPrincipal(user);
            if (businessId != item.BusinessId)
                throw new Exception("You do not have permission to delete items for this business.");
            
            // We found the item, delete it.
            _repository.Delete(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public IEnumerable<Item> GetBusinessInventoryItems(ClaimsPrincipal user)
    {
        try
        {
            var businessId = _claimsResolver.GetBusinessIdFromClaimsPrincipal(user);
            var globalInventory = _repository.GetAll();
            var businessItems = globalInventory.Where((item) => item.BusinessId == businessId);

            return businessItems;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to get inventory: {ExMessage}", e.Message);
        }

        return Enumerable.Empty<Item>();
    }
}
