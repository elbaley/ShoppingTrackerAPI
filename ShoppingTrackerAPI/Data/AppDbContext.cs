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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasIndex(c =>c.Name)
            .IsUnique(true);
        modelBuilder.Entity<Product>()
            .HasIndex(p =>p.Name)
            .IsUnique(true);
        modelBuilder.Entity<UserList>()
            .HasIndex(ul => ul.Name)
            .IsUnique(true);
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserList> UserLists { get; set; }
    public DbSet<UserProduct> UserProducts { get; set; }
}