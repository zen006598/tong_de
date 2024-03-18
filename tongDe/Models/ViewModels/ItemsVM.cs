namespace tongDe.Models.ViewModels;

public class ItemsVM
{
    public int ShopId { get; set; }
    public List<ItemWithItemCategoryVM>? Items { get; set; }
    public List<ItemCategory>? ItemCategories { get; set; }
}