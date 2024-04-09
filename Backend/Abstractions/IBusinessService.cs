using Backend1.Models;
using System.Security.Claims;
namespace Backend1.Abstractions;

public interface IBusinessService
{
    bool Add(Business business);
    bool Find(uint id);
    bool Find(ClaimsPrincipal claims);
}
