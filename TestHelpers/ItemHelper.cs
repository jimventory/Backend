using Backend1.Models;

namespace TestHelpers;

public static class ItemHelper
{
    public static Item GetBoilerplateItem(uint? busId = null, uint? id = null, uint? quantity = null, uint? price = null, string? name = null,
            uint? sales = null, uint? lowStockNot = null)
    {
        return new Item
        {
            BusinessId = busId ?? 0,
            Id = id ?? 0,
            Quantity = quantity ?? 5,
            Price = price ?? 1,
            Name = name ?? "FakeItem",
            Sales = sales ?? 100,
            LowStockNotification = lowStockNot ?? 5,
        };
    }
}
