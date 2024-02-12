using Backend1.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend1.Data;

public class BusinessContext : DbContext
{
    public DbSet<Business> Businesses { get; set; }

    public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
            optionsBuilder.UseNpgsql(EnvVarHelper.GetVariable("SQL_CONN_STRING"));
    }
}