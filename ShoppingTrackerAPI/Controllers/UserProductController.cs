using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingTrackerAPI.Data;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserProductController:ControllerBase
{
    private AppDbContext _context;

    public UserProductController(AppDbContext context)
    {
        _context = context;
    }
    
    private int FetchUserId()
    {
            int id =int.Parse(User.Claims.First(i => i.Type == "userId").Value);
            return id;
    }
    
    
    [HttpGet(Name = "GetUserProducts")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<List<UserProductResponseDto>>> Get()
    {
        int userId = FetchUserId();
        var response = new ApiResponse<List<UserProductResponseDto>>();
        // get user products from the db
        try
        {
            var userProducts = _context.UserProducts.Include(ul=> ul.Product).Where(up => up.UserId == userId).Select(up => new UserProductResponseDto
            {
                Id = up.Id,
                    Product = new ProductResponseDto
                    {
                        Id= up.Product.Id,
                        Category = up.Product.Category.Name,
                        Name= up.Product.Name,
                        Price=up.Product.Price,
                        ProductImg = up.Product.ProductImg
                        
                    },
                UserListId = up.UserListId,
                Description = up.Description,
                IsBought = up.IsBought,
                UserId = up.UserId,
            }).ToList();

            response.Data = userProducts;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the user products :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return response;
        }
        
    }

    [HttpGet("{id}", Name = "GetUserProductsWithId")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserProductResponseDto>> GetWithId(int id)
    {
        
        int userId = FetchUserId();
        var response = new ApiResponse<UserProductResponseDto>();
        try
        {
            var userProduct = _context.UserProducts.Where(up => up.Id == id && up.UserId == userId).Include(up=>up.Product).Select(up => new UserProductResponseDto
            {
                Id = up.Id,
                Product = new ProductResponseDto
                {
                    Id= up.Product.Id,
                    Category = up.Product.Category.Name,
                    Name= up.Product.Name,
                    Price=up.Product.Price,
                    ProductImg = up.Product.ProductImg
                    
                },
                UserId = up.UserId,
                IsBought = up.IsBought,
                
            }).FirstOrDefault();

            if (userProduct is null)
            { 
                response.Message = "User product does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            response.Data = userProduct;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);

        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the user product :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return BadRequest(response);
        }
    }


    [HttpPost(Name = "AddUserProduct")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserProductResponseDto>> Add(UserProductRequestDto request)
    {
        
        var response = new ApiResponse<UserProductResponseDto>();
        int userId = FetchUserId();
        // check if it is already on the list
        var existingUserProduct = _context.UserProducts.Where(up => up.ProductId == request.ProductId && up.UserListId == request.UserListId).FirstOrDefault();
        if (existingUserProduct is not null)
        {
            response.Message = "You already added this product to this list!";
            response.StatusCode = StatusCodes.Status400BadRequest;
            return BadRequest(response);
        }
        
        try
        {
            var newUserProduct = new UserProduct
            {
                ProductId = request.ProductId,
                UserId = userId,
                UserListId = request.UserListId ,
                Description = request.Description,
                IsBought = request.IsBought,
            };

            _context.UserProducts.Add(newUserProduct);
            _context.SaveChanges();

            var addedUserProduct = _context.UserProducts.Include(up => up.Product)
                .Where(up => up.Id == newUserProduct.Id).Select(up => new UserProductResponseDto
                {
                    Id = up.Id,
                    Product = new ProductResponseDto
                    {
                        Id= up.Product.Id,
                        Category = up.Product.Category.Name,
                        Name= up.Product.Name,
                        Price=up.Product.Price,
                        ProductImg = up.Product.ProductImg
                    },
                    UserId = userId,
                    UserListId = up.UserListId,
                    Description = up.Description,
                    IsBought = up.IsBought
                }).FirstOrDefault();
            
            response.Data = addedUserProduct;
            response.StatusCode = StatusCodes.Status201Created;
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't add new user product: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
        
    }


    [HttpPut(Name = "UpdateUserProduct")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserProductResponseDto>> Update(int id, UserProductRequestDto request)
    {
        
        int userId = FetchUserId();
        var response = new ApiResponse<UserProductResponseDto>();

        try
        {
            var existingUserProduct = _context.UserProducts.Include(up => up.Product)
                .ThenInclude(up=> up.Category).Where(up => up.Id == id && up.UserId == userId).FirstOrDefault();
            if (existingUserProduct is null)
            {
                response.Message = "User product does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }
            if (existingUserProduct.UserId != userId)
            {
                response.Message = "You are not allowed to update this list!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }

            existingUserProduct.Description = request.Description;
            existingUserProduct.IsBought = request.IsBought;

            _context.SaveChanges();

            var updatedUserProduct = new UserProductResponseDto()
            {
                Id = existingUserProduct.Id,
                Description = existingUserProduct.Description,
                IsBought = existingUserProduct.IsBought,
                Status = existingUserProduct.IsBought ? "Done" :"Pending",
                UserId = existingUserProduct.UserId,
                UserListId = existingUserProduct.UserListId,
                Product = new ProductResponseDto
                {
                    Id= existingUserProduct.Product.Id,
                    Category = existingUserProduct.Product.Category.Name,
                    Name= existingUserProduct.Product.Name,
                    Price=existingUserProduct.Product.Price,
                    ProductImg = existingUserProduct.Product.ProductImg
                },
            };
            
            response.Data = updatedUserProduct;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't update user product: " + ex.Message;
            Console.WriteLine(ex);
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }


    [HttpDelete("{id}", Name = "DeleteUserProduct")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        
        int userId = FetchUserId();
        var response = new ApiResponse<string>();
        
        try
        {
            var existingUserProduct = _context.UserProducts.FirstOrDefault(up => up.Id == id);
            
            if (existingUserProduct == null)
            {
                response.Message = "User Product doesn't exist";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            if (existingUserProduct.UserId != userId)
            {
                response.Message = "You are not authenticated to remove this user product!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }

            _context.UserProducts.Remove(existingUserProduct);
            _context.SaveChanges();
            
            response.Data = "User Product deleted!";
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't delete the user product: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        } 
    }
    
    
}