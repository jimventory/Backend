using Backend1.Models;

namespace Backend1.Abstractions;

public interface IBusinessRepository
{
    Business GetById(uint id);
    IEnumerable<Business> GetAll();
    void Add(Business business);
    void Update(Business business);
    void Delete(Business business);
}