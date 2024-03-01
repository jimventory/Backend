namespace Backend1.Models;

public class Item
{
    public uint BusinessId { get; set; }
    public uint Id { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public string Name { get; set; } = string.Empty;
    public uint Sales {  get; set; } 
    public uint LowStockNotification {  get; set; }
    public string About { get; set; } = string.Empty;
}