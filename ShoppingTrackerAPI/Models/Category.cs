using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    // A  category can have many products
    public List<Product> Products { get; set; }
}