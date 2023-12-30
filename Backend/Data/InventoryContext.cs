using Backend1.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend1.Data;

public class InventoryContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection information should not be stored in code.
        // TODO: Care about best practices.
        optionsBuilder.UseNpgsql(ConnectionInformation.PostgresConnString);
    }
}