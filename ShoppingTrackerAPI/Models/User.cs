namespace ShoppingTrackerAPI.Models;

public enum UserRole
{
    User,
    Admin
}
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    
    public List<UserList> UserLists { get; set; }
}