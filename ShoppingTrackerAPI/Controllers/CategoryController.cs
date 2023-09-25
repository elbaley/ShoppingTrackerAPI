using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingTrackerAPI.Data;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController:ControllerBase
{
    private AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetCategories")]
    [Authorize]
    public ActionResult<ApiResponse<List<CategoryResponseDto>>> Get()
    {
        var response = new ApiResponse<List<CategoryResponseDto>>();
        try
        {
            var categories = _context.Categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name= c.Name
            }).ToList();
            
            
            response.Data = categories;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't get the categories :" + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return response;
        }
    }


    [HttpPost(Name = "AddCategory")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<CategoryResponseDto>> Add(CategoryRequestDto request)
    {
        var response = new ApiResponse<CategoryResponseDto>();
        try
        {
            var newCategory = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            var addedCategory = _context.Categories.Where(c => c.Id == newCategory.Id).Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefault();
            
            
            response.Data = addedCategory;
            response.StatusCode = StatusCodes.Status201Created;
            return Created("", response);

        }
        catch (Exception ex)
        {
            response.Message = "Couldn't add new category: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }


    [HttpPut("{id}", Name = "UpdateCategory")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<CategoryResponseDto>> Update(int id, CategoryRequestDto request)
    {
        var response = new ApiResponse<CategoryResponseDto>();
        try
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            
            if (existingCategory == null)
            {
                response.Message = "Category does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }
            existingCategory.Name = request.Name;
            
            _context.SaveChanges();

            var updatedCategory = new CategoryResponseDto
            {
                Name = existingCategory.Name
            };
            
            response.Data = updatedCategory;
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't update the category: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }



    [HttpDelete("{id}", Name = "DeleteCategory")]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        var response = new ApiResponse<string>();
        try
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            
            if (existingCategory == null)
            {
                response.Message = "Category does not exist!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound(response);
            }

            _context.Categories.Remove(existingCategory);
            _context.SaveChanges();
            
            response.Data = "Category deleted!";
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = "Couldn't delete the category: " + ex.Message;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
        
    }
}
