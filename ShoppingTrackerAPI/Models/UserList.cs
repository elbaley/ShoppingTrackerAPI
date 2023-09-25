namespace ShoppingTrackerAPI.Models;

public class UserList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool StartedShopping { get; set; } = false;
    
    
    public User User { get; set; }
    public List<UserProduct>? UserProducts { get; set; }
}