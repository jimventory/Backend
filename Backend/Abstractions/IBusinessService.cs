using Backend1.Models;

namespace Backend1.Abstractions;

public interface IBusinessService
{
    bool Add(Business business);
    bool Update(Business business);
    bool Delete(Business business);
    bool Delete(uint id);
}