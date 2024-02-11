using Backend1.Models;

namespace TestHelpers;

public static class BusinessHelper
{
    public static Business GetBoilerplateBusiness()
    {
        return new Business
        {
            Name = "FakeBusiness",
            Id = 0,
            Location = "Knoxville"
        };
    }
}