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
}