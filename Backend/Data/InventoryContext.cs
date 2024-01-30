using Backend1.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend1.Data;

public class InventoryContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(EnvVarHelper.GetVariable("SQL_CONN_STRING"));
    }
}