namespace ShoppingTrackerAPI.Models;

public class ProductResponseDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string ProductImg { get; set; } 

}