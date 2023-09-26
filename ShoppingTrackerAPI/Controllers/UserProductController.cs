using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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
                Name = up.Product.Name,
                ProductId = up.Product.Id,
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
                Name = up.Product.Name,
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
        int userId = FetchUserId();
        var response = new ApiResponse<UserProductResponseDto>();
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
                    ProductId = up.ProductId,
                    Name = up.Product.Name,
                    UserId = userId,
                    UserListId = up.UserListId,
                    Description = up.Description,
                    IsBought = up.IsBought
                }).FirstOrDefault();
            
            response.Data = addedUserProduct;
            response.StatusCode = StatusCodes.Status201Created;
            return Created("", response);
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
                .Where(up => up.Id == id && up.UserId == userId).FirstOrDefault();
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
                Name = existingUserProduct.Product.Name,
                UserId = existingUserProduct.UserId,
                UserListId = existingUserProduct.UserListId,
            };
            
            response.Data = updatedUserProduct;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't update user product: " + ex.Message;
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