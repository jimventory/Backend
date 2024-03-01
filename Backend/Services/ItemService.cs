using Backend1.Abstractions;
using Backend1.Models;

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

    public bool Add(Item item)
    {
        try
        {
            _repository.Add(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public bool Update(Item item)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            _repository.GetById(item.Id);
            
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
    public bool Delete(Item item)
    {
        try
        {
            _repository.Delete(item);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }
    
    public bool Delete(uint id)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            var item = _repository.GetById(id);
            
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

    // Not allowing quanity to become negative because it is currently unsigned. Can change in the future
    public bool UpdateSales(Item item, uint sales)
    {
        try
        {
            // This call throws an exception if it can't find the item.
            var currentItem = _repository.GetById(item.Id);

            if (currentItem.Quantity >= sales)
            {
                currentItem.Sales += sales;
                currentItem.Quantity -= sales;
                _repository.Update(currentItem);
            }
            else
            {
                throw new Exception("Sale amount exceeds quantity.");
            }
        }
        catch (Exception e) 
        {
            _logger.LogError("Failed to delete item: {ExMessage}", e.Message);
            return false;
        }
        return true;
    }
}