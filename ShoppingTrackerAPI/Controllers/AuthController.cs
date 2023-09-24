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
    public ActionResult<User> Register(UserRegisterDto request)
    {
        // check if the email is already used
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return BadRequest("Email has already been registered!");
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
        
        // add the new user to db
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok($"Successfully registered with email: {request.Email}");
    }
    
    [HttpPost("login")]
    public ActionResult<User> Login(UserLoginDto request)
    {
        // check if user exists
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        
        if (existingUser == null)
        {
            return BadRequest("User not found");
        }
        
        // VERIFY
        if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
        {
            return BadRequest("Wrong password!");
        }
        
        // Create JWT
        string token = CreateToken(existingUser);
        

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email,user.Email),
            // TODO add user role 
            new Claim(ClaimTypes.Role,"Admin"),
            new Claim(ClaimTypes.Role,"User")
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