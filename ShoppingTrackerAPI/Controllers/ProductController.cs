using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<List<ProductResponseDto>>> Get()
    {
        var response = new ApiResponse<List<ProductResponseDto>>();
        try
        {
            var products = _context.Products.Include(p => p.Category).Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category.Name
            }).ToList();

            response.Data = products;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the products :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return response;
        }
    }
    
    
    
    [HttpPost(Name = "AddProduct")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<ProductResponseDto>> Add(ProductRequestDto request)
    {
        var response = new ApiResponse<ProductResponseDto>();
        
        try
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId
            };
            
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            var addedProduct = _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == newProduct.Id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category.Name
                })
                .FirstOrDefault();

            response.Data = addedProduct;
            response.StatusCode = StatusCodes.Status201Created;
            return Created("", response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't add new product: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        } 
    }
    
    [HttpPut("{id}", Name = "UpdateProduct")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<ProductResponseDto>> Update(int id, ProductRequestDto request)
    {
        var response = new ApiResponse<ProductResponseDto>();

        try
        {
            var existingProduct = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                response.Message = "Product does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            existingProduct.Name = request.Name;
            existingProduct.Price = request.Price;
            existingProduct.CategoryId = request.CategoryId;

            _context.SaveChanges();

            var updatedProduct = new ProductResponseDto
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                Category = existingProduct.Category.Name
            };

            response.Data = updatedProduct;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't update the product: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
    
    
    [HttpDelete("{id}", Name = "DeleteProduct")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        var response = new ApiResponse<string>();

        try
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                response.Message = "Product doesn't exist";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            _context.Products.Remove(existingProduct);
            _context.SaveChanges();

            response.Data = "Product deleted!";
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't delete the prodcut: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }


}