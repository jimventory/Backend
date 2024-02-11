using Backend1.Abstractions;
using Backend1.Models;

namespace Backend1.Services;

public class BusinessService : IBusinessService
{
    private readonly IBusinessRepository _repository;
    private readonly ILogger _logger;
    
    public BusinessService(IBusinessRepository repo, ILogger<BusinessService> logger)
    {
        _repository = repo;
        _logger = logger;
    }
    
    public bool Add(Business business)
    {
        try
        {
            _repository.Add(business);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add business: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    // I'm not going to bother implementing this for now, as all we currently support is registration.
    // We'll likely add updates in the future, but currently there isn't much to update.
    // And there really isn't any reason for anyone to delete their business.
    // So, in conclusion: maybe do later.
    
    public bool Update(Business business)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Business business)
    {
        throw new NotImplementedException();
    }

    public bool Delete(uint id)
    {
        throw new NotImplementedException();
    }
}