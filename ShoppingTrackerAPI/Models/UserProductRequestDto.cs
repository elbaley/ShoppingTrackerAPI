namespace ShoppingTrackerAPI.Models;

public class UserProductRequestDto
{
    // When updating UserProduct we don't need ProductId and UserListId props 
    public int ProductId { get; set; }
    public int UserListId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsBought { get; set; } = false;
}
