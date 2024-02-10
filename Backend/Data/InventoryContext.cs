using Backend1.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend1.Data;

public class InventoryContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
            optionsBuilder.UseNpgsql(EnvVarHelper.GetVariable("SQL_CONN_STRING"));
    }
}