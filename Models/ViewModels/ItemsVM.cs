namespace tongDe.Models.ViewModels;

public class ItemsVM
{
    public int ShopId { get; set; }
    public string? ShopName { get; set; }
    public List<Item>? Items { get; set; }
}