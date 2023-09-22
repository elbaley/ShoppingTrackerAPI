using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ShoppingTrackerAPI.Controllers;

namespace ShoppingTrackerAPI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}