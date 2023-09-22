using Microsoft.EntityFrameworkCore;
using ShoppingTrackerAPI.Controllers;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Data;

public class AppDbContext: DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // connect to postgres with connection string from app settings
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}