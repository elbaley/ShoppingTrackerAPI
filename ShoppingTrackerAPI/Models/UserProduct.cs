namespace ShoppingTrackerAPI.Models;

public class UserProduct
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int UserListId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsBought { get; set; } = false;
    
    public User User { get; set; }
    public Product Product { get; set; }
    public UserList UserList { get; set; }
    
}