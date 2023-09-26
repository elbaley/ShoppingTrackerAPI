using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingTrackerAPI.Data;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserListController: ControllerBase
{
    private AppDbContext _context;

    public UserListController(AppDbContext context)
    {
        _context = context;
    }
    private int FetchUserId()
    {
            int id =int.Parse(User.Claims.First(i => i.Type == "userId").Value);
            return id;
    }

    [HttpGet(Name = "GetUserLists")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<List<UserListResponseDto>>> Get()
    {
        int userId = FetchUserId();
        var response = new ApiResponse<List<UserListResponseDto>>();
        // get user lists from the db
        try
        {
            var userLists = _context.UserLists.Where(ul => ul.UserId == userId).Select(ul => new UserListResponseDto
            {
                Id = ul.Id,
                Name = ul.Name,
                UserId = ul.UserId,
                StartedShopping = ul.StartedShopping
            }).ToList();

            response.Data = userLists;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the user lists :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return response;
        }
        
    }
    
    
    
    [HttpGet("{id}",Name = "GetUserListsWithId")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserListResponseDto>> GetWithId(int id)
    {
        int userId = FetchUserId();
        var response = new ApiResponse<UserListResponseDto>();
        // get user lists from the db
        try
        {
            var userList = _context.UserLists.Where(ul => ul.Id == id && ul.UserId == userId).Include(ul=>ul.UserProducts).ThenInclude(ul =>ul.Product).Select(ul => new UserListResponseDto
            {
                Id = ul.Id,
                Name = ul.Name,
                UserId = ul.UserId,
                StartedShopping = ul.StartedShopping,
                Products = ul.UserProducts.Select(up => new ProductResponseDto
                {
                    Id = up.Id,
                    Name = up.Product.Name,
                    Price= up.Product.Price,
                    Category = up.Product.Category.Name,
                }).ToList()
            }).FirstOrDefault();

            response.Data = userList;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the user list :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return response;
        }
        
    }


    [HttpPost(Name = "AddUserList")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserListResponseDto>> Add(UserListRequestDto request)
    {
        int userId = FetchUserId();
        var response = new ApiResponse<UserListResponseDto>();
        try
        {
            var newUserList = new UserList
            {
                Name = request.Name,
                StartedShopping = request.StartedShopping,
                UserId = userId
            };

            _context.UserLists.Add(newUserList);
            _context.SaveChanges();

            var addedUserList = _context.UserLists.Where(ul => ul.Id == newUserList.Id).Select(ul =>
                new UserListResponseDto
                {
                    Id = ul.Id,
                    Name = ul.Name,
                    UserId = ul.UserId,
                    StartedShopping = ul.StartedShopping

                }).FirstOrDefault();

            response.Data = addedUserList;
            response.StatusCode = StatusCodes.Status201Created;
            return Created("", response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't add new user list: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
    
    
    [HttpPut(Name = "UpdateUserList")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<UserListResponseDto>> Update(int id,UserListRequestDto request)
    {
        int userId = FetchUserId();
        var response = new ApiResponse<UserListResponseDto>();
        try
        {
            var existingUserList = _context.UserLists.FirstOrDefault(ul => ul.Id == id);
            
            if (existingUserList == null)
            {
                response.Message = "User List does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            if (existingUserList.UserId != userId)
            {
                response.Message = "You are not authenticated to update this list!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }

            existingUserList.Name = request.Name;
            existingUserList.StartedShopping = request.StartedShopping;

            _context.SaveChanges();

            var updatedUserList = new UserListResponseDto
            {
                Name = existingUserList.Name,
                StartedShopping = existingUserList.StartedShopping
            };
            
            response.Data = updatedUserList;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't update user list: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }


    [HttpDelete("{id}", Name = "DeleteUserList")]
    [Authorize(Roles = "User")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        int userId = FetchUserId();
        var response = new ApiResponse<string>();
        try
        {
            var existingUserList = _context.UserLists.FirstOrDefault(ul => ul.Id == id);
            
            if (existingUserList == null)
            {
                response.Message = "User List doesn't exist";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            if (existingUserList.UserId != userId)
            {
                response.Message = "You are not authenticated to remove this list!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return BadRequest(response);
            }

            _context.UserLists.Remove(existingUserList);
            _context.SaveChanges();
            
            response.Data = "User List deleted!";
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't delete the user list: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        } 
    }
}