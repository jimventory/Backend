using Backend1.Models;

namespace Backend1.Abstractions;

public interface IBusinessService
{
    bool Add(Business business);
    bool Find(uint id);
}
