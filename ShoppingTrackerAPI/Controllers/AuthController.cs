using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;
using ShoppingTrackerAPI.Data;
using ShoppingTrackerAPI.Models;

namespace ShoppingTrackerAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private AppDbContext _context;
    
    public AuthController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status400BadRequest)]
    public ActionResult<ApiResponse<User>> Register(UserRegisterRequestDto request)
    {
        var response = new ApiResponse<User>();
        // check if the email is already used
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (existingUser != null)
        {
            response.Message = "Email has already been registered!";
            response.StatusCode = StatusCodes.Status400BadRequest;
            return BadRequest(response);
        }
        
        // Hash the password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        // Make the user's role ADMIN if the email is admin@admin.com 
        if (newUser.Email == "admin@admin.com")
        {
            newUser.Role = UserRole.Admin;
        }
        
        
        // add the new user to db
        _context.Users.Add(newUser);
        _context.SaveChanges();

        response.Data = newUser;
        response.StatusCode = StatusCodes.Status201Created;
        return Created("",response);
    }
    
    [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status400BadRequest)]
    [HttpPost("login")]
    public ActionResult<ApiResponse<UserResponseDto>> Login(UserLoginRequestDto request)
    {
        var response = new ApiResponse<UserResponseDto>();
        // check if user exists
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        
        if (existingUser == null)
        {
            response.Message = "User not found";
            response.StatusCode = StatusCodes.Status400BadRequest;
            return BadRequest(response);
        }
        
        // VERIFY
        if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
        {
            response.Message = "Wrong password";
            response.StatusCode = StatusCodes.Status400BadRequest;
            return BadRequest(response);
        }
        
        // Create JWT
        string token = CreateToken(existingUser);

        // add jwt to response
        UserResponseDto data = new UserResponseDto
        {
            Token = token
        };
        response.Data = data;
        response.StatusCode = StatusCodes.Status200OK;
        return Ok(response);
    }

    private string CreateToken(User user)
    {
        string role = user.Role == UserRole.Admin ? "Admin" : "User";
        List<Claim> claims = new List<Claim>
        {
            new Claim("userId",user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SignKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}