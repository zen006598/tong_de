namespace tongDe.Models.ViewModels;

public class ItemCategoryVM
{
    public int ShopId { get; set; }
    public string? ShopName { get; set; }
    public List<ItemCategory>? ItemCategories { get; set; }
}
