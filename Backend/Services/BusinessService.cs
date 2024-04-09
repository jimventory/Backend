using Backend1.Abstractions;
using Backend1.Models;
using System.Security.Claims;

namespace Backend1.Services;

public class BusinessService : IBusinessService
{
    private IUserBusinessIdResolver _resolver;
    private readonly IBusinessRepository _repository;
    private readonly ILogger _logger;
    
    public BusinessService(IBusinessRepository repo, IUserBusinessIdResolver resolver, ILogger<BusinessService> logger)
    {
        _repository = repo;
        _resolver = resolver;
        _logger = logger;
    }
    
    public bool Add(Business business)
    {
        try
        {
            _logger.LogInformation($"Business Id is {business.Id}");
            _repository.Add(business);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to add business: {ExMessage}", e.Message);
            return false;
        }
        
        return true;
    }

    public bool Find(uint id)
    {
        try
        {
            var business = _repository.GetAll().Any( business => business.Id == id);
            return business;
        }
        catch (Exception e)
        {
            _logger.LogDebug("Exception occured when trying to find business : {ExMessage}", e.Message);
            return false;
        }
    }

    public bool Find(ClaimsPrincipal claims)
    {
        try
        {
            var id = _resolver.GetBusinessIdFromClaimsPrincipal(claims);
            
            return Find(id);
        }
        catch (Exception e)
        {
            _logger.LogDebug("Exception occurred when trying to find business : {ExMessage}", e.Message);
            return false;
        }
    }
}
