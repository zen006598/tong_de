using System.ComponentModel.DataAnnotations;
using tongDe.Models.Validator;

namespace tongDe.Models;

public class Item
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? PinyIn { get; set; }
    [Required]
    public string? Unit { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;
    public ICollection<ItemAlias> ItemAliases { get; } = new List<ItemAlias>();
    public int? ItemCategoryId { get; set; }
    public ItemCategory? ItemCategory { get; set; }

}