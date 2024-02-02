using Backend1.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend1.Data;

public class BusinessContext : DbContext
{
    public DbSet<Business> Businesses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(EnvVarHelper.GetVariable("SQL_CONN_STRING"));
    }
}