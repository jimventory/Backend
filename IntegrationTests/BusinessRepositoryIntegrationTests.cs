using Backend1.Data;
using Backend1.Models;
using Backend1.Repositories;
using Microsoft.EntityFrameworkCore;
using TestHelpers;

namespace IntegrationTests;

public class BusinessRepositoryIntegrationTests
{
    private readonly DbContextOptions<BusinessContext> _options;

    public BusinessRepositoryIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<BusinessContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }
    
    [Fact]
    public void TestAddBusiness()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);
        var business = BusinessHelper.GetBoilerplateBusiness();
            
        sut.Add(business);

        var found = db.Businesses.Find(business.Id);
            
        Assert.NotNull(found);
    }

    [Fact]
    public void TestUpdateBusiness()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);
        var business = BusinessHelper.GetBoilerplateBusiness();
        
        // Add the initial Business and fail the test if we for some reason didn't insert.
        sut.Add(business);
        var found = db.Businesses.Find(business.Id);
        Assert.NotNull(found);
        
        // Create a different object entirely to avoid immediately updating the Business (classes are reference types by default in C#)
        var updateBusiness = BusinessHelper.GetBoilerplateBusiness();
        updateBusiness.Id = business.Id;
        updateBusiness.Name = "UpdatedName";
        
        // Verify that the names is different but the ID is the same.
        Assert.NotEqual(updateBusiness.Name, business.Name);
        Assert.Equal(updateBusiness.Id, business.Id);
        
        sut.Update(updateBusiness);
        
        found = db.Businesses.Find(business.Id);
            
        Assert.NotNull(found);
        Assert.Equal(updateBusiness.Name, found.Name);
    }

    [Fact]
    public void TestDeleteBusiness()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);
        var business = BusinessHelper.GetBoilerplateBusiness();
            
        sut.Add(business);

        var found = db.Businesses.Find(business.Id);
            
        Assert.NotNull(found);
        
        sut.Delete(business);
        
        found = db.Businesses.Find(business.Id);
        
        Assert.Null(found);
    }

    [Fact]
    public void TestGetByIdException()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);

        Assert.Throws<Exception>(() => sut.GetById(0));
    }

    [Fact]
    public void TestGetByIdSuccess()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);
        
        var business = BusinessHelper.GetBoilerplateBusiness();
            
        sut.Add(business);
        
        var found = db.Businesses.Find(business.Id);
            
        Assert.NotNull(found);

        var rv = sut.GetById(business.Id);
        Assert.Same(business, rv);
    }

    [Fact(Skip = "Issue with DBs not being disposed.")]
    public void TestGetAllEmpty()
    {
        using var db = new BusinessContext(_options);
        var sut = new BusinessRepository(db);
        
        var coll = sut.GetAll();
        
        Assert.Empty(coll);
    }

    [Fact(Skip = "Issue with DBs not being disposed.")]
    public void TestGetAllWithBusinesses()
    {
        using var db = new BusinessContext(_options);
        
        var sut = new BusinessRepository(db);

        var businesses = new List<Business>();
        businesses.Add(BusinessHelper.GetBoilerplateBusiness());
        businesses.Add(BusinessHelper.GetBoilerplateBusiness());
        
        sut.Add(businesses[0]);
        sut.Add(businesses[1]);

        var coll = sut.GetAll();
        
        Assert.True(coll.SequenceEqual(businesses));
    }
}