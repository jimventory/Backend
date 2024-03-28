using Backend1.Abstractions;
using Backend1.Models;
using System.Security.Claims;

namespace Backend1.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;
    private readonly ILogger _logger;
    
    public ItemService(IItemRepository repo, ILogger<ItemService> logger)
    {
        _repository = repo;
        _logger = logger;
    }

    public bool Add(Item item, ClaimsPrincipal user)
    {
        try
        {
            item.BusinessId = GetBusinessIdFromClaims(user);
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
            
            var businessId = GetBusinessIdFromClaims(user);

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
            var businessId = GetBusinessIdFromClaims(user);
            
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
            
            var businessId = GetBusinessIdFromClaims(user);
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
            var businessId = GetBusinessIdFromClaims(user);
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

    // Probably should just keep the approach we had prior and use roles/scopes to check if a user can access the business they want.
    // But for ease of implementation now, I'm opting for this approach.
    // We can always change in the future.  Since we're not actually going to release this, I don't see any reason to make this more complicated than we actually need it to be.
    // For ease assuring that this method works as expected, I'm marking it public so I can unit test it.
    public uint GetBusinessIdFromClaims(ClaimsPrincipal user)
    {
        // Get NameIdentifer.
        var nameIdent = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (nameIdent is null)
            throw new Exception("Could not find NameIdentifier within claims.");

        // Auth0 NameIdentifier should be formatted as auth0|number
        var identNumString = nameIdent.Split("|")[1];
        var identNumSubString = identNumString.Substring(identNumString.Length - 8);
        
        _logger.LogInformation($"Identity sub lenth is {identNumSubString.Length}");

        var identNum = Convert.ToUInt32(identNumSubString, 16);

        _logger.LogInformation($"Converted Id is: {identNum}");

        return identNum;
    }
}
