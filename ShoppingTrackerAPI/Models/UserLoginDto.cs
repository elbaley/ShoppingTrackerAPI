using System.ComponentModel.DataAnnotations;

namespace ShoppingTrackerAPI.Models;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Password must contain both upper and lower case letters")]
    public string Password { get; set; } = string.Empty;
}