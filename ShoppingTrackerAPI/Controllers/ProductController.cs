using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingTrackerAPI.Data;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController: ControllerBase
{
    private AppDbContext _context;
    public ProductController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet(Name = "GetProducts")]
    public string Get()
    {
        Product product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == 1);
        return product.Category.Name;
    }
}