using Backend1.Abstractions;
using System.Security.Claims;

namespace Backend1;

public class UserBusinessIdResolverValidator : UserBusinessIdResolver, IUserBusinessIdValidator
{
    private readonly IBusinessService _businessService;

    public UserBusinessIdResolverValidator(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    public override uint GetBusinessIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
    {
       var id = base.GetBusinessIdFromClaimsPrincipal(claimsPrincipal);

       var result = Validate(id);

       if (result == false)
           throw new Exception("Failed to validate this ID, it does not exist.");

       return id;
    }

    public bool Validate(uint id)
    {
        var exists = _businessService.Find(id);

        return exists;
    }
}
