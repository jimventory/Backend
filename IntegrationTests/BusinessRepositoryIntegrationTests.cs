using Backend1.Data;
using Microsoft.EntityFrameworkCore;

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
}