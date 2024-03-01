using Backend1.Models;

namespace TestHelpers;

public static class ItemHelper
{
    public static Item GetBoilerplateItem()
    {
        return new Item
        {
            Id = 0,
            BusinessId = 0,
            Name = "FakeItem",
            Quantity = 5,
        };
    }
}