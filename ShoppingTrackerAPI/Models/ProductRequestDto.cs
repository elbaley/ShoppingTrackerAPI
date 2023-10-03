namespace ShoppingTrackerAPI.Models;

public class ProductRequestDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string ProductImg { get; set; }
}