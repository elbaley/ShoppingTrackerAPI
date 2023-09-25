namespace ShoppingTrackerAPI.Models;

public class UserListRequestDto
{
    public string Name { get; set; } = string.Empty;
    public bool StartedShopping { get; set; } = false;
}