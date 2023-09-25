namespace ShoppingTrackerAPI.Models;

public class UserListResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool StartedShopping { get; set; } = false;

    public List<ProductResponseDto>? Products { get; set; }
}
    