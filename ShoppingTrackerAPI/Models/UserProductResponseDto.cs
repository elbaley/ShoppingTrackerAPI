namespace ShoppingTrackerAPI.Models;

public class UserProductResponseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public int UserListId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsBought { get; set; } = false;
}