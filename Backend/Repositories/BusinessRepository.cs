using Backend1.Abstractions;
using Backend1.Data;
using Backend1.Models;

namespace Backend1.Repositories;

public class BusinessRepository : IBusinessRepository
{
    private readonly BusinessContext _db;
    
    public BusinessRepository(BusinessContext db)
    {
        _db = db;
    }
    
    public Business GetById(uint id)
    {
        return _db.Businesses.Find(id) ?? throw new Exception($"Could not find business with ID {id}.");
    }

    public IEnumerable<Business> GetAll()
    {
        return _db.Businesses.ToList();
    }

    public void Add(Business business)
    {
        _db.Businesses.Add(business);
        _db.SaveChanges();
    }

    public void Update(Business business)
    {
        var old = GetById(business.Id);
        _db.Businesses.Entry(old).CurrentValues.SetValues(business);
        _db.SaveChanges();
    }

    public void Delete(Business business)
    {
        _db.Businesses.Remove(business);
        _db.SaveChanges();
    }
}